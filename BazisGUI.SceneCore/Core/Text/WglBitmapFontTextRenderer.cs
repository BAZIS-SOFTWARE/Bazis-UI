using System;
using System.Runtime.InteropServices;
using System.Security;
using BazisGUI.SceneCore.Rendering;
using OpenTK.Graphics.OpenGL;

namespace BazisGUI.SceneCore.Text
{
    /// <summary>
    /// Перенос текущего кода (wglUseFontBitmapsW, GL.GenLists, GL.ListBase, GL.CallLists) —
    /// Windows-only и временный, нужен только чтобы не сломать отображение текста на шагах 2-6
    /// (см. scene.avalonia.md, раздел 6). Целевая реализация — SkiaGlyphAtlasTextRenderer.
    /// Не использует System.Drawing.Font/ToHfont (System.Drawing.Common запрещён в этой сборке) —
    /// поэтому, в отличие от старого BaseForm.ChangeTextFont, не поддерживает подмену шрифта,
    /// а всегда растеризует тот, что уже выбран в контексте устройства.
    /// </summary>
    public class WglBitmapFontTextRenderer : ISceneTextRenderer, IDisposable
    {
        private const int GlyphCount = 1150; // 256 — только латиница, 1150 — плюс кириллица

        private int fontBase;
        private IntPtr deviceContext;
        private bool initialized;

        public void AttachToCurrentContext(IntPtr hdc)
        {
            deviceContext = hdc;
            fontBase = GL.GenLists(GlyphCount);

            if (!UseFontBitmapsW(deviceContext, 0, GlyphCount, fontBase))
                throw new InvalidOperationException("Не удалось загрузить глифы шрифта (wglUseFontBitmapsW)");

            initialized = true;
        }

        public void SetFont(string family, float size)
        {
            if (!initialized)
                return;

            if (!UseFontBitmapsW(deviceContext, 0, GlyphCount, fontBase))
                throw new InvalidOperationException("Не удалось загрузить глифы шрифта (wglUseFontBitmapsW)");
        }

        public void DrawText3D(TextLabel label, IRenderContext context)
        {
            GL.PushMatrix();
            GL.Color3(label.Color.R, label.Color.G, label.Color.B);
            GL.RasterPos3(label.Position3D._x, label.Position3D._y, label.Position3D._z);
            CallLists(label.Text);
            GL.PopMatrix();
        }

        public void DrawText2D(TextLabel label, IRenderContext context)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Ortho(0, context.Viewport.Width, 0, context.Viewport.Height, 0.1, 200);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.LoadIdentity();

            GL.Color3(label.Color.R, label.Color.G, label.Color.B);
            GL.RasterPos3(label.Position2D._x, label.Position2D._y, -5);
            CallLists(label.Text);

            GL.MatrixMode(MatrixMode.Projection);
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PopMatrix();
        }

        private void CallLists(string text)
        {
            GL.PushAttrib(AttribMask.ListBit);
            GL.ListBase(fontBase);
            var handle = GCHandle.Alloc(text, GCHandleType.Pinned);
            try
            {
                GL.CallLists(text.Length, ListNameType.UnsignedShort, handle.AddrOfPinnedObject());
            }
            finally
            {
                handle.Free();
            }
            GL.PopAttrib();
        }

        public void Dispose()
        {
            if (initialized)
                GL.DeleteLists(fontBase, GlyphCount);
        }

        [DllImport("opengl32.dll", CharSet = CharSet.Auto, EntryPoint = "wglUseFontBitmapsW")]
        [SuppressUnmanagedCodeSecurity]
        private static extern bool UseFontBitmapsW(IntPtr hDC, int first, int count, int listBase);
    }
}
