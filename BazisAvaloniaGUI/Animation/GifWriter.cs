using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;

namespace BazisAvaloniaGUI.Animation
{
    /// <summary>
    /// Создаёт анимированный GIF из кадров сцены (PNG). Замена BazisGUI.Animation.GifWriter,
    /// который кодировал кадры через System.Drawing (только Windows): здесь кадры декодируются
    /// SkiaSharp, приводятся к фиксированной палитре 6×7×6 и сжимаются LZW.
    /// </summary>
    internal sealed class GifWriter : IDisposable
    {
        private const int RedLevels = 6, GreenLevels = 7, BlueLevels = 6;

        private readonly BinaryWriter writer;
        private readonly object syncLock = new();
        private bool firstFrame = true;
        private int width;
        private int height;

        /// <param name="outStream">Поток, в который пишется GIF.</param>
        /// <param name="defaultFrameDelay">Задержка между кадрами по умолчанию, мс.</param>
        /// <param name="repeat">Число повторов: -1 — без повтора, 0 — бесконечно.</param>
        public GifWriter(Stream outStream, int defaultFrameDelay = 500, int repeat = -1)
        {
            ArgumentNullException.ThrowIfNull(outStream);
            if (defaultFrameDelay <= 0)
                throw new ArgumentOutOfRangeException(nameof(defaultFrameDelay));
            if (repeat < -1)
                throw new ArgumentOutOfRangeException(nameof(repeat));

            writer = new BinaryWriter(outStream);
            DefaultFrameDelay = defaultFrameDelay;
            Repeat = repeat;
        }

        /// <summary>Задержка по умолчанию, мс.</summary>
        public int DefaultFrameDelay { get; set; }

        /// <summary>Число повторов анимации: -1 — без повтора, 0 — бесконечно.</summary>
        public int Repeat { get; }

        /// <summary>Добавляет кадр (PNG) с задержкой <paramref name="delay"/> мс (0 — <see cref="DefaultFrameDelay"/>).</summary>
        public void WriteFrame(byte[] png, int delay = 0)
        {
            using var bitmap = SKBitmap.Decode(png);
            if (bitmap == null)
                throw new InvalidDataException("Frame image cannot be decoded.");

            lock (syncLock)
            {
                if (firstFrame)
                {
                    width = bitmap.Width;
                    height = bitmap.Height;
                    WriteHeader();
                    firstFrame = false;
                }

                WriteGraphicControlBlock(delay == 0 ? DefaultFrameDelay : delay);
                WriteImageBlock(Quantize(bitmap));
            }
        }

        private void WriteHeader()
        {
            writer.Write("GIF89a".ToCharArray());
            writer.Write((short)width);
            writer.Write((short)height);
            writer.Write((byte)0xF7); // глобальная палитра, 8 бит на цвет, 256 записей
            writer.Write((byte)0);    // индекс цвета фона
            writer.Write((byte)0);    // соотношение сторон пикселя

            for (var i = 0; i < 256; i++)
            {
                var (r, g, b) = PaletteColor(i);
                writer.Write(r);
                writer.Write(g);
                writer.Write(b);
            }

            if (Repeat != -1)
            {
                writer.Write((byte)0x21);
                writer.Write((byte)0xFF);
                writer.Write((byte)0x0B);
                writer.Write("NETSCAPE2.0".ToCharArray());
                writer.Write((byte)3);
                writer.Write((byte)1);
                writer.Write((short)Repeat);
                writer.Write((byte)0);
            }
        }

        private static (byte R, byte G, byte B) PaletteColor(int index)
        {
            if (index >= RedLevels * GreenLevels * BlueLevels)
                return (0, 0, 0);

            var r = index / (GreenLevels * BlueLevels);
            var g = index / BlueLevels % GreenLevels;
            var b = index % BlueLevels;
            return ((byte)(r * 255 / (RedLevels - 1)), (byte)(g * 255 / (GreenLevels - 1)), (byte)(b * 255 / (BlueLevels - 1)));
        }

        private byte[] Quantize(SKBitmap bitmap)
        {
            var indices = new byte[width * height];
            var source = bitmap.Pixels;
            for (var y = 0; y < height; y++)
            {
                var sourceY = Math.Min(bitmap.Height - 1, y * bitmap.Height / height);
                for (var x = 0; x < width; x++)
                {
                    var color = source[sourceY * bitmap.Width + Math.Min(bitmap.Width - 1, x * bitmap.Width / width)];
                    // Прозрачный фон кадра OpenGL считается белым, как на экране.
                    var alpha = color.Alpha / 255f;
                    var red = color.Red * alpha + 255 * (1 - alpha);
                    var green = color.Green * alpha + 255 * (1 - alpha);
                    var blue = color.Blue * alpha + 255 * (1 - alpha);
                    var r = (int)Math.Round(red * (RedLevels - 1) / 255f);
                    var g = (int)Math.Round(green * (GreenLevels - 1) / 255f);
                    var b = (int)Math.Round(blue * (BlueLevels - 1) / 255f);
                    indices[y * width + x] = (byte)((r * GreenLevels + g) * BlueLevels + b);
                }
            }
            return indices;
        }

        private void WriteGraphicControlBlock(int delayMs)
        {
            writer.Write((byte)0x21);
            writer.Write((byte)0xF9);
            writer.Write((byte)4);
            writer.Write((byte)0x04); // способ удаления: оставить кадр
            writer.Write((short)Math.Max(1, delayMs / 10));
            writer.Write((byte)0);
            writer.Write((byte)0);
        }

        private void WriteImageBlock(byte[] indices)
        {
            writer.Write((byte)0x2C);
            writer.Write((short)0);
            writer.Write((short)0);
            writer.Write((short)width);
            writer.Write((short)height);
            writer.Write((byte)0);

            new LzwEncoder(indices).Encode(writer);
        }

        public void Dispose()
        {
            lock (syncLock)
            {
                if (!firstFrame)
                    writer.Write((byte)0x3B);
                writer.Flush();
            }
        }

        /// <summary>LZW-сжатие индексов пикселей с переменной длиной кода (как в GIF-кодировщике К. Вайнера).</summary>
        private sealed class LzwEncoder(byte[] pixels)
        {
            private const int MinCodeSize = 8;
            private const int MaxBits = 12;
            private const int MaxMaxCode = 1 << MaxBits;

            private readonly Dictionary<int, int> codes = new();
            private readonly List<byte> block = new(255);
            private BinaryWriter output;
            private int bitBuffer;
            private int bitCount;
            private int bits;
            private int maxCode;
            private int freeEntry;
            private bool clearFlag;

            private int ClearCode => 1 << MinCodeSize;
            private int EndCode => ClearCode + 1;

            public void Encode(BinaryWriter writer)
            {
                output = writer;
                output.Write((byte)MinCodeSize);

                bits = MinCodeSize + 1;
                maxCode = (1 << bits) - 1;
                freeEntry = ClearCode + 2;

                Output(ClearCode);
                var prefix = (int)pixels[0];
                for (var i = 1; i < pixels.Length; i++)
                {
                    var pixel = pixels[i];
                    var key = (prefix << 8) | pixel;
                    if (codes.TryGetValue(key, out var code))
                    {
                        prefix = code;
                        continue;
                    }

                    Output(prefix);
                    prefix = pixel;
                    if (freeEntry < MaxMaxCode)
                        codes[key] = freeEntry++;
                    else
                    {
                        codes.Clear();
                        freeEntry = ClearCode + 2;
                        clearFlag = true;
                        Output(ClearCode);
                    }
                }

                Output(prefix);
                Output(EndCode);
                FlushBits();
                output.Write((byte)0);
            }

            private void Output(int code)
            {
                bitBuffer |= code << bitCount;
                bitCount += bits;
                while (bitCount >= 8)
                {
                    WriteByte((byte)(bitBuffer & 0xFF));
                    bitBuffer >>= 8;
                    bitCount -= 8;
                }

                if (freeEntry > maxCode || clearFlag)
                {
                    if (clearFlag)
                    {
                        bits = MinCodeSize + 1;
                        maxCode = (1 << bits) - 1;
                        clearFlag = false;
                    }
                    else
                    {
                        ++bits;
                        maxCode = bits == MaxBits ? MaxMaxCode : (1 << bits) - 1;
                    }
                }
            }

            private void FlushBits()
            {
                while (bitCount > 0)
                {
                    WriteByte((byte)(bitBuffer & 0xFF));
                    bitBuffer >>= 8;
                    bitCount -= 8;
                }
                bitCount = 0;
                if (block.Count > 0)
                    WriteBlock();
            }

            private void WriteByte(byte value)
            {
                block.Add(value);
                if (block.Count == 255)
                    WriteBlock();
            }

            private void WriteBlock()
            {
                output.Write((byte)block.Count);
                output.Write(block.ToArray());
                block.Clear();
            }
        }
    }
}
