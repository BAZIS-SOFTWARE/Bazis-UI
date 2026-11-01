using BazisGUI.SceneCore.Rendering;
using BazisGUI.SceneCore.Coordinates;
using OpenTK.Graphics.OpenGL;

namespace BazisGUI.SceneCore.Layers
{
    /// <summary>Перенос BaseForm.DisplayReflectionPlane(float[] coeff).</summary>
    public class ReflectionPlaneLayer : ISceneLayer
    {
        public string Name => "ReflectionPlane";
        public int Order { get; set; } = 25;
        public bool IsVisible { get; set; } = true;

        private ModelPlane3D plane;

        public void SetPlane(ModelPlane3D plane) => this.plane = plane;

        public void Clear() => plane = null;

        public void Draw(IRenderContext context)
        {
            if (plane == null)
                return;

            var position = context.Camera.Position;
            var scale = 1f / context.ScaleFactor;

            GL.PushMatrix();
            GL.Translate(-position.X, -position.Y, -position.Z);
            GL.Scale(scale, scale, scale);

            var source = plane.Normal;
            var normalLength = System.Math.Sqrt(source.X * source.X + source.Y * source.Y + source.Z * source.Z);
            var nx = source.X / normalLength;
            var ny = source.Y / normalLength;
            var nz = source.Z / normalLength;
            var centreX = nx * plane.D;
            var centreY = ny * plane.D;
            var centreZ = nz * plane.D;
            GL.Translate(centreX, centreY, centreZ);

            var xX = ny;
            var xY = -nx;
            var xLength = System.Math.Sqrt(xX * xX + xY * xY);
            var yX = xY * nz;
            var yY = -xX * nz;
            var yZ = xX * ny - xY * nx;
            var yLength = System.Math.Sqrt(yX * yX + yY * yY + yZ * yZ);
            var xpX = centreX + 0.1 * xX / xLength;
            var xpY = centreY + 0.1 * xY / xLength;
            var xpZ = centreZ;
            var ypX = centreX + 0.1 * yX / yLength;
            var ypY = centreY + 0.1 * yY / yLength;
            var ypZ = centreZ + 0.1 * yZ / yLength;

            GL.Color3(1f, 0, 0);
            GL.Begin(PrimitiveType.LineStrip);
            GL.Vertex3(centreX, centreY, centreZ);
            GL.Vertex3(xpX, xpY, xpZ);
            GL.Vertex3(xpX + ypX, xpY + ypY, xpZ + ypZ);
            GL.Vertex3(ypX, ypY, ypZ);
            GL.Vertex3(centreX, centreY, centreZ);
            GL.End();

            GL.PopMatrix();
        }
    }
}
