using Geometry;
using Model.MeshObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BaseModule.CrossSection
{
    public class GetCrossPoints
    {
        public Dictionary<int, List<Point3D>> CreateCrossNodes(List<Element3D> elements, Plane plane)
        {
            Point3D[] tempPointArr = new Point3D[4];
            List<Point3D> points = new List<Point3D>();
            List<Point3D> newPoints = new List<Point3D>();
            List<int> signs = new List<int>();
            var nodes = new Dictionary<int, List<Point3D>>();

            foreach (var elems in elements)
            {
                foreach (var surface in elems.GetSurfaces())
                {
                    tempPointArr = surface.GetCoordinates().ToArray();
                    for (int i = 0; i < tempPointArr.Length; i++)
                        signs.Add(Math.Sign(plane.PointPosition(tempPointArr[i])));

                    if (signs.Contains(1) & signs.Contains(-1)) // make intersection
                        points.Add(CreatePoint(signs, tempPointArr, plane));
                    else if (signs.Contains(0) && !signs.Contains(1) & !signs.Contains(-1)) // all points of surf
                        points.AddRange(tempPointArr);
                    else if (signs.Contains(0) && signs.Contains(1) | signs.Contains(-1)) // just one point or rib of surf
                    {
                        for (int i = 0; i < signs.Count; i++)
                            if (signs[i] == 0)
                                points.Add(tempPointArr[i]);
                        points.AddRange(tempPointArr);
                    }

                    signs.Clear();
                }
                var res = points.Distinct(new Point3DEqualityComparer()).ToArray();
                points.Clear();
                newPoints.AddRange(res);
                if (newPoints.Count > 0)
                {
                    nodes.Add(elems.Number, newPoints);
                }
                newPoints = new List<Point3D>();
            }

            return nodes;
        }

        public Point3D CreatePoint(List<int> sings, Point3D[] points, Plane plane)
        {

            for (int i = 0; i < points.Length; i++)
            {
                for (int k = i + 1; k < points.Length; i++)
                {
                    var d1 = Math.Abs(plane.PointPosition(points[i]));
                    var d2 = Math.Abs(plane.PointPosition(points[k]));

                    if (!sings[i].Equals(sings[k]))
                    {
                        Point3D point1 = points[i];
                        Point3D point2 = points[k];

                        var res = point2.Sub(point1);

                        var xFO = d1 / (d1 + d2) * res._x; //xFO
                        var yFO = d1 / (d1 + d2) * res._y; //yFO
                        var zFO = d1 / (d1 + d2) * res._z; //zFO

                        Point3D newPoint = new Point3D(xFO + point1._x, yFO + point1._y, zFO + point1._z);
                        return newPoint;
                    }
                }
            }
            return null;
        }
    }
}
