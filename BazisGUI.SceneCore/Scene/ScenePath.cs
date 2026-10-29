
using System;
using OpenTK.Graphics.OpenGL;
using BazisGUI.SceneCore.Coordinates;

namespace BazisGUI.SceneCore
{
    public class ScenePath
    {
        public double Length { get; }

        ModelPoint3D[] coords { get; }

        public ModelPoint3D this[int ind]
        { get { return coords[ind]; } }

        public int PointsQuantity { get { return coords.Length; } }

        /// <summary>Сохраняет точки пути и вычисляет его длину в координатах модели.</summary>
        public ScenePath(ModelPoint3D[] Coords)
        {
            var path = 0.0;
            coords = new ModelPoint3D[Coords.Length];
            for (int i = 0; i < Coords.Length - 1; i++)
            {
                coords[i] = Coords[i];
                coords[i + 1] = Coords[i + 1];
                var p0 = Coords[i];
                var p1 = Coords[i + 1];

                var dx = p1.X - p0.X;
                var dy = p1.Y - p0.Y;
                var dz = p1.Z - p0.Z;
                path += Math.Sqrt(dx * dx + dy * dy + dz * dz);
            }
            Length = path;
        }
        /// <summary>Отображает путь относительно центра камеры в координатах модели.</summary>
        public void Display(ModelPoint3D position)
        {
            GL.PushMatrix();
            GL.Translate(-position.X, -position.Y, -position.Z);

            GL.LineWidth(5.0f);
            GL.Begin(PrimitiveType.Lines);

            for (int i = 0; i < coords.Length - 1; i++)
            {
                var p0 = coords[i];
                var p1 = coords[i + 1];


                GL.Color3(1f, 0, 0);
                GL.Vertex3(p0.X, p0.Y, p0.Z);
                GL.Vertex3(p1.X, p1.Y, p1.Z);
            }
            GL.End();

            GL.PointSize(12.5f);
            GL.Begin(PrimitiveType.Points);
            GL.Vertex3(coords[0].X, coords[0].Y, coords[0].Z);
            GL.Vertex3(coords[coords.Length - 1].X, coords[coords.Length - 1].Y, coords[coords.Length - 1].Z);
            GL.End();
            GL.PopMatrix();
        }
    }
}
