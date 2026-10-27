using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazisGUI.SceneCore
{
    /// <summary>
    /// Структура хранения координат ограничивающего параллелепипеда
    /// </summary>
    public class BoundingBox : IComparable<BoundingBox>
    {
        List<Point3D[]> surfaces;

        Point3D[] corners; 
        /// <summary>
        /// Левая верхняя ближняя точка ограничивающего параллелепипеда
        /// </summary>
        public Point3D LeftUpNear { get; private set; }
        /// <summary>
        /// Правая нижняя дальняя точка ограничивающего параллелепипеда
        /// </summary>
        public Point3D RightDownFar { get; private set; }
        /// <summary>
        /// Конструктор BoundingBoxCoords
        /// </summary>
        /// <param name="leftUpNear">Левая верхняя ближняя точка</param>
        /// <param name="rightDownNear">Правая нижняя дальняя точка</param>
        public BoundingBox(Point3D leftUpNear, Point3D rightDownNear)
        {
            LeftUpNear = leftUpNear;
            RightDownFar = rightDownNear;
        }
        public BoundingBox(float[] coords)
        {
            if (coords.Length != 0)
            {
                var xMin = coords[0];
                var xMax = coords[0];
                var yMin = coords[1];
                var yMax = coords[1];
                var zMin = coords[2];
                var zMax = coords[2];
                for (var i = 0; i < coords.Length; i += 3)
                {
                    xMin = Math.Min(xMin, coords[i]);
                    xMax = Math.Max(xMax, coords[i]);
                    yMin = Math.Min(yMin, coords[i + 1]);
                    yMax = Math.Max(yMax, coords[i + 1]);
                    zMin = Math.Min(zMin, coords[i + 2]);
                    zMax = Math.Max(zMax, coords[i + 2]);
                }

                CreateCornerPoints(xMin, xMax, yMin, yMax, zMin, zMax);

                LeftUpNear = corners[0];
                RightDownFar = corners[4];
                CreateSides();
            }
        }

        private void CreateCornerPoints(float xMin, float xMax, float yMin, float yMax, float zMin, float zMax)
        {
            corners = new Point3D[8];
            corners[0] = new Point3D(xMin, yMax, zMax);
            corners[1] = new Point3D(xMin, yMax, zMin);
            corners[2] = new Point3D(xMin, yMin, zMin);
            corners[3] = new Point3D(xMin, yMin, zMax);
            corners[4] = new Point3D(xMax, yMin, zMin);
            corners[5] = new Point3D(xMax, yMin, zMax);
            corners[6] = new Point3D(xMax, yMax, zMax);
            corners[7] = new Point3D(xMax, yMax, zMin);
        }

        private void CreateSides()
        {
            surfaces = new List<Point3D[]>()
                {
                    new Point3D[]{ LeftUpNear, corners[1], corners[2], corners[3] },
                    new Point3D[]{corners[2], RightDownFar, corners[7], corners[1] },
                    new Point3D[]{corners[6], corners[5], RightDownFar, corners[7] },
                    new Point3D[]{ LeftUpNear, corners[3], corners[5], corners[6] },
                    new Point3D[]{ LeftUpNear, corners[6], corners[7], corners[1] },
                    new Point3D[]{ corners[2], RightDownFar, corners[5], corners[3] },
                };
        }

        /// <summary>
        /// GetCornerPoints
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Point3D[]> GetSidesPoints()
        {
            foreach (var surface in surfaces)
            {
                yield return surface;
            }
        }

        /// <summary>
        /// GetCornerPoints
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Point3D> GetCornerPoints()
        {
            foreach (var item in corners)
            {
                yield return item;
            }
        }

        /// <summary>
        /// GetVolume
        /// </summary>
        /// <returns></returns>
        public double GetSqrCoordsSum()
        {
            var dx = LeftUpNear._x - RightDownFar._x;
            var dy = LeftUpNear._y - RightDownFar._y;
            var dz = LeftUpNear._z - RightDownFar._z;

            return dx * dx + dy * dy + dz * dz;
        }
/// <inheritdoc/>

        public int CompareTo(BoundingBox other)
        {
            if (GetSqrCoordsSum().CompareTo(other.GetSqrCoordsSum()) < 0)
                return -1;
            else if (GetSqrCoordsSum().CompareTo(other.GetSqrCoordsSum()) > 0)
                return 1;
            else
                return 0;
        }
        /// <summary>
        /// Возвращает диагональ параллелепипеда
        /// </summary>
        /// <returns>Диагональ параллелепипеда</returns>
        public double GetDiagonalLength()
        {
            var vec = LeftUpNear.Sub(RightDownFar);
            return Vector.GetVectorLength(vec);
        }

        /// <summary>
        /// Выполняет слияние двух BoundingBox и возвращает новый
        /// </summary>
        /// <param name="other">BoundingBox с которым производится слияние</param>
        /// <returns>Результат слияния двух BoundingBox</returns>
        public BoundingBox Merge(BoundingBox other)
        {
            var merge = new BoundingBox(LeftUpNear, RightDownFar);

            if (other != null)
            {
                merge.LeftUpNear._x = Math.Min(merge.LeftUpNear._x, other.LeftUpNear._x);
                merge.LeftUpNear._y = Math.Max(merge.LeftUpNear._y, other.LeftUpNear._y);
                merge.LeftUpNear._z = Math.Max(merge.LeftUpNear._z, other.LeftUpNear._z);

                merge.RightDownFar._x = Math.Max(merge.RightDownFar._x, other.RightDownFar._x);
                merge.RightDownFar._y = Math.Min(merge.RightDownFar._y, other.RightDownFar._y);
                merge.RightDownFar._z = Math.Min(merge.RightDownFar._z, other.RightDownFar._z);
            }

            return merge;
        }
    }
}
