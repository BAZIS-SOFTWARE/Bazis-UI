using System.Drawing;
using BazisGUI.SceneCore.Primitives;
using BazisGUI.SceneCore.Rendering;
using BazisGUI.SceneCore.VBO;
using OpenTK.Graphics.OpenGL;

namespace BazisGUI.SceneCore.Layers
{
    /// <summary>
    /// Перенос BaseForm.DisplayBasis(): три оси + сфера в центре. Вместо gluCylinder/gluSphere
    /// используются меши, построенные один раз через IQuadricMeshFactory (см. scene.avalonia.md,
    /// раздел "Попутная находка" — заодно снимается утечка gluNewQuadric без gluDeleteQuadric).
    /// </summary>
    public class BasisLayer : ISceneLayer, System.IDisposable
    {
        public string Name => "Basis";
        public int Order { get; set; } = 0;
        public bool IsVisible { get; set; } = true;

        private readonly VBObject axisX;
        private readonly VBObject axisY;
        private readonly VBObject axisZ;
        private readonly VBObject tipX;
        private readonly VBObject tipY;
        private readonly VBObject tipZ;
        private readonly VBObject centre;

        public BasisLayer(IQuadricMeshFactory meshFactory)
        {
            axisX = meshFactory.CreateCylinder(0.0015, 0.0015, 0.025, Color.FromArgb(255, 128, 0));
            axisY = meshFactory.CreateCylinder(0.0015, 0.0015, 0.025, Color.FromArgb(0, 255, 0));
            axisZ = meshFactory.CreateCylinder(0.0015, 0.0015, 0.025, Color.FromArgb(0, 0, 255));

            tipX = meshFactory.CreateCone(0.0025, 0.01, Color.FromArgb(255, 128, 0));
            tipY = meshFactory.CreateCone(0.0025, 0.01, Color.FromArgb(0, 255, 0));
            tipZ = meshFactory.CreateCone(0.0025, 0.01, Color.FromArgb(0, 0, 255));

            centre = meshFactory.CreateSphere(0.002, Color.FromArgb(255, 255, 0));
        }

        public void Draw(IRenderContext context)
        {
            var position = context.Camera.Position;
            var scale = 1f / context.ScaleFactor;

            // "Z line"
            GL.PushMatrix();
            GL.Translate(-position.X, -position.Y, -position.Z);
            GL.Scale(scale, scale, scale);
            axisZ.Load();
            GL.PopMatrix();

            // "Y line"
            GL.PushMatrix();
            GL.Translate(-position.X, -position.Y, -position.Z);
            GL.Scale(scale, scale, scale);
            GL.Rotate(-90, 1, 0, 0);
            axisY.Load();
            GL.PopMatrix();

            // "X line"
            GL.PushMatrix();
            GL.Translate(-position.X, -position.Y, -position.Z);
            GL.Scale(scale, scale, scale);
            GL.Rotate(90, 0, 1, 0);
            axisX.Load();
            GL.PopMatrix();

            // "X tip"
            GL.PushMatrix();
            GL.Translate(-position.X, -position.Y, -position.Z);
            GL.Scale(scale, scale, scale);
            GL.Translate(0.025f, 0, 0);
            GL.Rotate(90, 0, 1, 0);
            tipX.Load();
            GL.PopMatrix();

            // "Y tip"
            GL.PushMatrix();
            GL.Translate(-position.X, -position.Y, -position.Z);
            GL.Scale(scale, scale, scale);
            GL.Translate(0, 0.025f, 0);
            GL.Rotate(-90, 1, 0, 0);
            tipY.Load();
            GL.PopMatrix();

            // "Z tip"
            GL.PushMatrix();
            GL.Translate(-position.X, -position.Y, -position.Z);
            GL.Scale(scale, scale, scale);
            GL.Translate(0, 0, 0.025f);
            tipZ.Load();
            GL.PopMatrix();

            // центр
            GL.PushMatrix();
            GL.Translate(-position.X, -position.Y, -position.Z);
            GL.Scale(scale, scale, scale);
            centre.Load();
            GL.PopMatrix();
        }

        /// <summary>Освобождает GL-буферы мешей слоя (вызывается при текущем GL-контексте).</summary>
        public void Dispose()
        {
            foreach (var mesh in new[] { axisX, axisY, axisZ, tipX, tipY, tipZ, centre })
                BazisGUI.SceneCore.VBO.VBO.DeleteAllBuffers(mesh);
        }
    }
}
