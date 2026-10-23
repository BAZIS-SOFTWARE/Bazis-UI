using System.Drawing;
using SceneCore.Primitives;
using SceneCore.Rendering;
using SceneCore.Text;
using WinGUI.Scene.VBO;
using Geometry;
using MathNet.Numerics.LinearAlgebra;
using OpenTK.Graphics.OpenGL;

namespace SceneCore.Layers
{
    /// <summary>Перенос BaseForm.DisplayCompass().</summary>
    public class CompassLayer : ISceneLayer, System.IDisposable
    {
        public string Name => "Compass";
        public int Order { get; set; } = 50;
        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// Размер знакоместа в единицах системы координат компаса (1 — один пиксель атласа шрифта).
        /// Компас рисуется в пиксельной ортопроекции с домножением 1/ScaleFactor, поэтому «мировой»
        /// масштаб текста (GlyphWorldScale = 0.02) давал знакоместо 0.4 единицы — доли пикселя.
        /// Знакоместо 20 px (шрифт 12) даёт букву ~12 px — как растровый шрифт в WinForms.
        /// </summary>
        public float LabelScale { get; set; } = 1f;

        private readonly VBObject tipX;
        private readonly VBObject tipY;
        private readonly VBObject tipZ;
        private readonly ISceneTextRenderer textRenderer;

        public CompassLayer(IQuadricMeshFactory meshFactory, ISceneTextRenderer textRenderer)
        {
            tipX = meshFactory.CreateCone(4, 10, Color.FromArgb(255, 128, 0));
            tipY = meshFactory.CreateCone(4, 10, Color.FromArgb(0, 255, 0));
            tipZ = meshFactory.CreateCone(4, 10, Color.FromArgb(0, 0, 255));
            this.textRenderer = textRenderer;
        }

        public void Draw(IRenderContext context)
        {
            var viewport = context.Viewport;

            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Ortho(0, viewport.Width, 0, viewport.Height, 0.1, 200);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.LoadIdentity();

            var view = context.Camera.GetViewMatrix();
            var matrix = Matrix<float>.Build.DenseIdentity(4);
            for (var r = 0; r < 3; r++)
                for (var c = 0; c < 3; c++)
                    matrix[r, c] = view[r, c];

            matrix[0, 3] = viewport.Width - 80;
            matrix[1, 3] = 70;
            matrix[2, 3] = -60;

            GL.LoadMatrix(matrix.AsColumnMajorArray());
            GL.Scale(1f / context.ScaleFactor, 1f / context.ScaleFactor, 1f / context.ScaleFactor);

            GL.PushMatrix();
            GL.LineWidth(3.0f);
            GL.Begin(PrimitiveType.Lines);

            GL.Color3(0, 0, 1f);
            GL.Vertex3(0f, 0f, 0f);
            GL.Color3(0, 0, 1f);
            GL.Vertex3(0f, 0f, 50f);

            GL.Color3(0, 1f, 0);
            GL.Vertex3(0f, 0f, 0f);
            GL.Color3(0, 1f, 0);
            GL.Vertex3(0f, 50f, 0f);

            GL.Color3(1, 0.5f, 0);
            GL.Vertex3(0f, 0f, 0f);
            GL.Color3(1, 0.5f, 0);
            GL.Vertex3(50f, 0f, 0f);

            GL.End();
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(40, 0, 0);
            GL.Rotate(90, 0, 1, 0);
            tipX.Load();
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(0, 40, 0);
            GL.Rotate(-90, 1, 0, 0);
            tipY.Load();
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(0, 0, 40);
            tipZ.Load();
            GL.PopMatrix();

            var black = Color.Black;
            DrawLabel("X", black, new Point3D(60, 0, 0), context);
            DrawLabel("Y", black, new Point3D(0, 60, 0), context);
            DrawLabel("Z", black, new Point3D(0, 0, 60), context);

            GL.MatrixMode(MatrixMode.Projection);
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PopMatrix();
        }

        /// <summary>Освобождает GL-буферы мешей слоя (вызывается при текущем GL-контексте).</summary>
        public void Dispose()
        {
            foreach (var mesh in new[] { tipX, tipY, tipZ })
                VBO.DeleteAllBuffers(mesh);
        }

        private void DrawLabel(string text, Color color, Point3D position, IRenderContext context)
        {
            textRenderer.DrawText3D(new TextLabel { Text = text, Color = color, Position3D = position, WorldScale = LabelScale }, context);
        }
    }
}
