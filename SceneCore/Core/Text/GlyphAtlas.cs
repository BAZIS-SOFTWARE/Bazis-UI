using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using SkiaSharp;

namespace BazisGUI.Scene.Core.Text
{
    /// <summary>
    /// Растеризует глифы SkiaSharp'ом в текстурный атлас (вместо wglUseFontBitmapsW),
    /// снимая ограничение в 1150 глифов из GL.GenLists(1150) — см. scene.avalonia.md, раздел 6.
    /// SkiaSharp уже в дереве зависимостей Avalonia, отдельного пакета не требуется.
    /// </summary>
    public class GlyphAtlas : IDisposable
    {
        private readonly Dictionary<char, GlyphMetrics> glyphs = new Dictionary<char, GlyphMetrics>();

        public int Texture { get; private set; }

        public void Build(string family, float size, bool bold = false)
        {
            var chars = BuildCharset();
            var cols = (int)Math.Ceiling(Math.Sqrt(chars.Count));
            var rows = (int)Math.Ceiling(chars.Count / (double)cols);
            var cell = (int)Math.Ceiling(size * 1.6);
            var atlasWidth = cols * cell;
            var atlasHeight = rows * cell;

            var familyName = string.IsNullOrEmpty(family) ? SKTypeface.Default.FamilyName : family;
            var weight = bold ? SKFontStyleWeight.Bold : SKFontStyleWeight.Normal;
            using var typeface = SKTypeface.FromFamilyName(familyName, weight, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright)
                                 ?? SKTypeface.Default;
            using var font = new SKFont(typeface, size) { Edging = SKFontEdging.Antialias };
            using var paint = new SKPaint { Color = SKColors.White, IsAntialias = true };
            using var bitmap = new SKBitmap(atlasWidth, atlasHeight, SKColorType.Rgba8888, SKAlphaType.Premul);
            using var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.Transparent);

            glyphs.Clear();
            for (var i = 0; i < chars.Count; i++)
            {
                var ch = chars[i];
                var col = i % cols;
                var row = i / cols;
                var x = col * cell;
                var y = row * cell;

                var text = ch.ToString();
                var advance = font.MeasureText(text, out var bounds);
                var baseline = y + cell - font.Metrics.Descent - 2;
                canvas.DrawText(text, x - bounds.Left, baseline, font, paint);

                glyphs[ch] = new GlyphMetrics
                {
                    U0 = (float)x / atlasWidth,
                    V0 = (float)y / atlasHeight,
                    U1 = (float)(x + cell) / atlasWidth,
                    V1 = (float)(y + cell) / atlasHeight,
                    Width = cell,
                    Height = cell,
                    Advance = advance > 0 ? advance : cell * 0.5f
                };
            }

            UploadTexture(bitmap, atlasWidth, atlasHeight);
        }

        public GlyphMetrics GetGlyph(char symbol)
        {
            if (glyphs.TryGetValue(symbol, out var metrics))
                return metrics;

            return glyphs.TryGetValue(' ', out var space) ? space : default;
        }

        private void UploadTexture(SKBitmap bitmap, int width, int height)
        {
            if (Texture != 0)
                GL.DeleteTexture(Texture);

            Texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, Texture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, bitmap.GetPixels());
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        private static List<char> BuildCharset()
        {
            var chars = Enumerable.Range(0x20, 0x7F - 0x20).Select(c => (char)c).ToList();
            chars.AddRange(Enumerable.Range(0x0410, 0x0450 - 0x0410).Select(c => (char)c)); // А-я
            chars.Add('Ё');
            chars.Add('ё');
            return chars;
        }

        public void Dispose()
        {
            if (Texture != 0)
                GL.DeleteTexture(Texture);
        }
    }
}
