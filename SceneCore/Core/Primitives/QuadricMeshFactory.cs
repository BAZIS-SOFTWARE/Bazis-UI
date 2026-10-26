using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SceneCore.Interfaces;
using SceneCore.VBO;
using Geometry;

namespace SceneCore.Primitives
{
    /// <summary>
    /// Строит меши примитивов вручную (без gluNewQuadric/gluCylinder/gluSphere), см. IQuadricMeshFactory.
    /// Каждый треугольник получает собственные вершины (без разделения по индексам) —
    /// эти фигуры маленькие и статичные (виджеты базиса/компаса), экономия памяти не критична.
    /// </summary>
    public class QuadricMeshFactory : IQuadricMeshFactory
    {
        private const int Slices = 16;
        private const int Stacks = 12;

        public VBObject CreateCylinder(double baseR, double topR, double height, Color color)
        {
            return BuildCylinder(baseR, topR, height, color, "Cylinder");
        }

        public VBObject CreateCone(double baseR, double height, Color color)
        {
            return BuildCylinder(baseR, 0, height, color, "Cone");
        }

        public VBObject CreateSphere(double radius, Color color)
        {
            var triangles = new List<Point3D>();

            for (var i = 0; i < Stacks; i++)
            {
                var phi0 = Math.PI * i / Stacks - Math.PI / 2;
                var phi1 = Math.PI * (i + 1) / Stacks - Math.PI / 2;

                for (var j = 0; j < Slices; j++)
                {
                    var theta0 = 2 * Math.PI * j / Slices;
                    var theta1 = 2 * Math.PI * (j + 1) / Slices;

                    var p00 = SpherePoint(radius, phi0, theta0);
                    var p01 = SpherePoint(radius, phi0, theta1);
                    var p11 = SpherePoint(radius, phi1, theta1);
                    var p10 = SpherePoint(radius, phi1, theta0);

                    triangles.Add(p00); triangles.Add(p01); triangles.Add(p11);
                    triangles.Add(p00); triangles.Add(p11); triangles.Add(p10);
                }
            }

            return BuildTriangleMesh(triangles, color, "Sphere");
        }

        private static Point3D SpherePoint(double radius, double phi, double theta)
        {
            var x = radius * Math.Cos(phi) * Math.Cos(theta);
            var y = radius * Math.Cos(phi) * Math.Sin(theta);
            var z = radius * Math.Sin(phi);
            return new Point3D(x, y, z);
        }

        private static VBObject BuildCylinder(double baseR, double topR, double height, Color color, string name)
        {
            var triangles = new List<Point3D>();

            for (var i = 0; i < Slices; i++)
            {
                var a0 = 2 * Math.PI * i / Slices;
                var a1 = 2 * Math.PI * (i + 1) / Slices;

                var b0 = new Point3D(baseR * Math.Cos(a0), baseR * Math.Sin(a0), 0);
                var b1 = new Point3D(baseR * Math.Cos(a1), baseR * Math.Sin(a1), 0);
                var t0 = new Point3D(topR * Math.Cos(a0), topR * Math.Sin(a0), height);
                var t1 = new Point3D(topR * Math.Cos(a1), topR * Math.Sin(a1), height);

                triangles.Add(b0); triangles.Add(b1); triangles.Add(t1);
                triangles.Add(b0); triangles.Add(t1); triangles.Add(t0);
            }

            return BuildTriangleMesh(triangles, color, name);
        }

        private static VBObject BuildTriangleMesh(List<Point3D> triangles, Color color, string name)
        {
            var vertexCount = triangles.Count;
            var coords = new float[vertexCount * 3];
            var colors = new float[vertexCount * 4];
            var normals = new float[vertexCount * 3];
            var edges = new bool[vertexCount];
            var ptrs = new int[vertexCount];

            var r = color.R / 255f;
            var g = color.G / 255f;
            var b = color.B / 255f;
            var a = color.A / 255f;

            for (var i = 0; i < vertexCount; i += 3)
            {
                var p0 = triangles[i];
                var p1 = triangles[i + 1];
                var p2 = triangles[i + 2];

                var normal = Vector.CrossProd(p1.Sub(p0), p2.Sub(p0));
                var normNormal = Vector.GetVectorNorm(normal);

                for (var k = 0; k < 3; k++)
                {
                    var v = triangles[i + k];
                    var vi = i + k;

                    coords[vi * 3 + 0] = (float)v._x;
                    coords[vi * 3 + 1] = (float)v._y;
                    coords[vi * 3 + 2] = (float)v._z;

                    normals[vi * 3 + 0] = (float)normNormal._x;
                    normals[vi * 3 + 1] = (float)normNormal._y;
                    normals[vi * 3 + 2] = (float)normNormal._z;

                    colors[vi * 4 + 0] = r;
                    colors[vi * 4 + 1] = g;
                    colors[vi * 4 + 2] = b;
                    colors[vi * 4 + 3] = a;

                    edges[vi] = true;
                    ptrs[vi] = vi;
                }
            }

            var vbo = new SurfaceObjects(name, edges, ptrs, coords, colors, normals)
            {
                ViewMode = ObjView.Surface // сплошная заливка без наложения каркаса, как в исходном PolygonMode.Fill
            };
            return vbo;
        }
    }
}
