
using Geometry;
using OpenTK.Graphics.OpenGL;

namespace SceneCore
{
    public class ScenePath
    {
        public double Length { get; }

        Point3D [] coords { get; }

        public Point3D this[int ind]
        { get { return coords[ind]; } }

        public int PointsQuantity { get { return coords.Length; } }

        public ScenePath(Point3D[] Coords)
        {
            var path = 0.0;
            coords = new Point3D[Coords.Length];
            for (int i = 0; i < Coords.Length - 1; i++)
            {
                coords[i] = Coords[i];
                coords[i + 1] = Coords[i + 1];
                var p0 = Coords[i];
                var p1 = Coords[i + 1];

                path = path + new Segment3D(p0, p1).GetLength();
            }
            Length = path;
        }
        public void Display(Point3D position)
        {
            GL.PushMatrix();
            GL.Translate(-position._x, -position._y, -position._z);

            GL.LineWidth(5.0f);
            GL.Begin(PrimitiveType.Lines);

            for (int i = 0; i < coords.Length - 1; i++)
            {
                var p0 = coords[i];
                var p1 = coords[i + 1];


                GL.Color3(1f, 0, 0);
                GL.Vertex3(p0._x, p0._y, p0._z);
                GL.Vertex3(p1._x, p1._y, p1._z);
            }
            GL.End();

            GL.PointSize(12.5f);
            GL.Begin(PrimitiveType.Points);
            GL.Vertex3(coords[0]._x, coords[0]._y, coords[0]._z);
            GL.Vertex3(coords[coords.Length - 1]._x, coords[coords.Length - 1]._y, coords[coords.Length - 1]._z);
            GL.End();
            GL.PopMatrix();
        }
    }
}
