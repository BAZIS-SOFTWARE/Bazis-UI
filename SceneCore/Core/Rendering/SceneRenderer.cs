using System;
using BazisGUI.Scene.Core.Layers;
using BazisGUI.Scene.Interfaces;
using BazisGUI.Scene; // AverageColorRenderer, Advanced3DClipper (существующий переиспользуемый GL-слой)
using OpenTK.Graphics.OpenGL;

namespace BazisGUI.Scene.Core.Rendering
{
    /// <summary>
    /// Замена BaseForm.DisplayObjects(): не вызывает SwapBuffers — буферами владеет
    /// композитор Avalonia (см. scene.avalonia.md, раздел 6). Гасит buffer clear/reset
    /// один раз за кадр и затем проходит по SceneLayerCollection в порядке Order —
    /// вместо явной последовательности вызовов DisplayXxxEvent из старого кода.
    /// Оборачивание AverageColorRenderer.DoActionsBeforeDrawing/AfterDrawing в старом коде
    /// шло тремя отдельными участками (базис, геометрия, текст); здесь упрощено до одного
    /// прохода на весь кадр — по объектам VBOController ActiveDrawingObject сейчас и так
    /// не назначается (закомментировано в VBOController.CreateXxxVBObjects), так что это
    /// не меняет наблюдаемое поведение. Условие прозрачности читается напрямую из
    /// transparency.IsEnable (а не из отдельного флага настроек) — в старом коде оба
    /// значения всегда выставлялись синхронно.
    /// </summary>
    public class SceneRenderer : ISceneRenderer, IDisposable
    {
        private readonly SceneLayerCollection layers;
        private readonly AverageColorRenderer transparency;
        private readonly Advanced3DClipper clipper;

        public SceneRenderer(SceneLayerCollection layers, AverageColorRenderer transparency, Advanced3DClipper clipper)
        {
            this.layers = layers;
            this.transparency = transparency;
            this.clipper = clipper;
        }

        public void Render(IRenderContext context)
        {
            transparency.TargetFramebuffer = context.TargetFramebuffer;
            GL.BindFramebuffer(FramebufferTarget.FramebufferExt, context.TargetFramebuffer);
            GL.DrawBuffer(context.TargetFramebuffer == 0 ? DrawBufferMode.Back : DrawBufferMode.ColorAttachment0);
            ClearBuffers(context);
            GL.DrawBuffer(context.TargetFramebuffer == 0 ? DrawBufferMode.Back : DrawBufferMode.ColorAttachment0);
            ResetViewMatrix(context);

            if (!transparency.IsEnable || clipper.IsEnable)
            {
                foreach (var layer in layers.GetOrdered())
                    if (layer.IsVisible)
                        layer.Draw(context);
            }
            else
                RenderWithTransparency(context);

            MakeFrameOpaque();

            GL.Finish();
        }

        /// <summary>
        /// Отрисовка с прозрачностью. Смешивание (BlendFramebuffers) перекрывает весь кадр, поэтому, как в
        /// BaseForm.DisplayObjects, объекты в координатах модели, рисуемые до модели (базис, вспомогательная
        /// геометрия, плоскости), попадают в буфер геометрии рендера прозрачности и смешиваются с моделью
        /// по глубине, а экранные слои после модели (компас, подписи, шкала, рамка выбора) рисуются поверх
        /// уже смешанного кадра.
        /// </summary>
        private void RenderWithTransparency(IRenderContext context)
        {
            var blended = false;
            foreach (var layer in layers.GetOrdered())
            {
                if (!layer.IsVisible)
                    continue;

                if (blended)
                    layer.Draw(context);
                else if (layer is ModelObjectsLayer)
                {
                    layer.Draw(context);
                    transparency.BlendFramebuffers();
                    blended = true;
                }
                else
                {
                    transparency.DoActionsBeforeDrawing(null, DrawElements.GeometryObjects);
                    layer.Draw(context);
                    transparency.DoActionsAfterDrawing(null, DrawElements.GeometryObjects);
                }
            }

            if (!blended)
                transparency.BlendFramebuffers();
        }

        /// <summary>
        /// Делает готовый кадр непрозрачным. Avalonia смешивает кадр OpenGL с окном по альфа-каналу
        /// (GLControl в WinForms его игнорировал): фон очищается с альфой 0, а грани с прозрачностью
        /// ModelView записывают альфу меньше 1 — без этого вместо цвета фона виден цвет окна,
        /// а модель просвечивает.
        /// </summary>
        private static void MakeFrameOpaque()
        {
            GL.ColorMask(false, false, false, true);
            GL.ClearColor(0, 0, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ColorMask(true, true, true, true);
        }

        private void ClearBuffers(IRenderContext context)
        {
            var bg = context.Settings.BackGroundColor;
            GL.ClearColor(bg.R / 255f, bg.G / 255f, bg.B / 255f, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (transparency.IsEnable && !clipper.IsEnable)
                transparency.ClearColors();
        }

        private static void ResetViewMatrix(IRenderContext context)
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.LoadMatrix(context.Camera.GetViewMatrix().AsColumnMajorArray());
        }

        public void Reshape(int width, int height) => transparency.Reshape(width, height);

        public void Dispose()
        {
            transparency.Dispose();
            clipper.Dispose();
        }
    }
}
