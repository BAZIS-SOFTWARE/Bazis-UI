using System;
using System.Runtime.InteropServices;
using BazisGUI.SceneCore.Camera;
using OpenTK.Graphics.OpenGL;
using SkiaSharp;

namespace BazisGUI.SceneCore.Capture
{
    /// <summary>
    /// Заменяет System.Drawing.Image.Save(path, ImageFormat.Bmp) из btnMakeScreenShot_Click:
    /// кадр читается через glReadPixels и кодируется SkiaSharp'ом в PNG вместо BMP
    /// (см. scene.avalonia.md, раздел 6).
    /// </summary>
    public class GlFrameGrabber : IFrameGrabber
    {
        public byte[] Capture(Viewport viewport)
        {
            var width = viewport.Width;
            var height = viewport.Height;
            var rowBytes = width * 4;
            var pixels = new byte[height * rowBytes];

            GL.ReadPixels(0, 0, width, height, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);

            using var bitmap = new SKBitmap(width, height, SKColorType.Rgba8888, SKAlphaType.Unpremul);
            var destBase = bitmap.GetPixels();

            // glReadPixels отдаёт строки снизу вверх, PNG ожидает сверху вниз.
            for (var y = 0; y < height; y++)
            {
                var srcOffset = (height - 1 - y) * rowBytes;
                var destPtr = IntPtr.Add(destBase, y * rowBytes);
                Marshal.Copy(pixels, srcOffset, destPtr, rowBytes);
            }

            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            return data.ToArray();
        }
    }
}
