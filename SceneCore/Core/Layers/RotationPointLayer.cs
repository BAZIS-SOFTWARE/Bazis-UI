using System.Drawing;
using SceneCore.Primitives;
using SceneCore.Rendering;
using WinGUI.Scene.VBO;
using OpenTK.Graphics.OpenGL;

namespace SceneCore.Layers
{
    /// <summary>
    /// Перенос BaseForm.CreateRotationPoint(). Видимость переключается снаружи
    /// (нажатие/отпускание средней кнопки мыши) — см. SceneInputController.
    /// </summary>
    public class RotationPointLayer : ISceneLayer, System.IDisposable
    {
        public string Name => "RotationPoint";
        public int Order { get; set; } = 10;
        public bool IsVisible { get; set; }

        private readonly VBObject sphere;

        public RotationPointLayer(IQuadricMeshFactory meshFactory)
        {
            sphere = meshFactory.CreateSphere(0.003, Color.FromArgb(255, 191, 0));
        }

        public void Draw(IRenderContext context)
        {
            var scale = 1f / context.ScaleFactor;

            GL.PushMatrix();
            GL.Scale(scale, scale, scale);
            sphere.Load();
            GL.PopMatrix();
        }

        /// <summary>Освобождает GL-буферы меша слоя (вызывается при текущем GL-контексте).</summary>
        public void Dispose() => VBO.DeleteAllBuffers(sphere);
    }
}
