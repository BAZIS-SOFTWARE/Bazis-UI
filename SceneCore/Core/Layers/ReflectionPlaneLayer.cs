using SceneCore.Rendering;
using Geometry;
using OpenTK.Graphics.OpenGL;

namespace SceneCore.Layers
{
    /// <summary>Перенос BaseForm.DisplayReflectionPlane(float[] coeff).</summary>
    public class ReflectionPlaneLayer : ISceneLayer
    {
        public string Name => "ReflectionPlane";
        public int Order { get; set; } = 25;
        public bool IsVisible { get; set; } = true;

        private Plane plane;

        public void SetPlane(Plane plane) => this.plane = plane;

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

            var normal = Vector.GetVectorNorm(plane.Normal);
            var centre = normal.Mult(plane.Shifting);
            GL.Translate(centre._x, centre._y, centre._z);

            var z = new Point3D(0, 0, -1);
            var x = Vector.CrossProd(z, normal);
            var y = Vector.CrossProd(x, normal);

            var xn = Vector.GetVectorNorm(x);
            var yn = Vector.GetVectorNorm(y);

            var xp = xn.Mult(0.1f).Sum(centre);
            var yp = yn.Mult(0.1f).Sum(centre);
            var xyp = xp.Sum(yp);

            GL.Color3(1f, 0, 0);
            GL.Begin(PrimitiveType.LineStrip);
            GL.Vertex3(centre._x, centre._y, centre._z);
            GL.Vertex3(xp._x, xp._y, xp._z);
            GL.Vertex3(xyp._x, xyp._y, xyp._z);
            GL.Vertex3(yp._x, yp._y, yp._z);
            GL.Vertex3(centre._x, centre._y, centre._z);
            GL.End();

            GL.PopMatrix();
        }
    }
}
