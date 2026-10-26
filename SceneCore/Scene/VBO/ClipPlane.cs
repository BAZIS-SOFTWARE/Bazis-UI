using SceneCore.Interfaces;
using Geometry;
using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using OpenTK.Graphics.OpenGL;

namespace SceneCore.VBO
{
    public class ClipPlane : SurfaceObjects
    {
        private float normalSize;

        public ClipPlaneRenderer Renderer { get; set; }

        public float[] ViewMatrix { get; set; } = new float[16];
        /// <param name="pointers">Индексы</param>
        /// <param name="glCoords">Координаты</param>
        /// <param name="glColors">Цвета</param>
        /// <param name="name">Имя</param>
        public ClipPlane(string name,int[] pointers, float[] glCoords, float[] glColors) : base(name,pointers, glCoords, glColors)
        {
            Gl_DisplayMode = PolygonMode.Fill;
            Gl_LineWidth = 2.5f;
            GL_ObjType = GLObjType.triangle;

            normalSize = (float)(Vector.GetVectorLength(BoundingBox.LeftUpNear.Sub(BoundingBox.RightDownFar)) * 0.125);
        }
        /// <summary>
        /// Создает VBO-массивы для 6 плоскостей из паралелепипеда
        /// </summary>
        /// <param name="box">Ограничивающий паралелепипед</param>
        /// <returns>(индексы, координаты, цвета)</returns>
        public static Tuple<int[], float[], float[]> CreateBoundingBoxPlanes(BoundingBox box)
        {
            var leftUp = box.LeftUpNear;
            var rightDown = box.RightDownFar;

            var points = new Point3D[18];
            var indices = Enumerable.Range(0, 18).ToArray();
            var glCoords = new float[54];
            var glColors = new float[72];

            //Left Plane
            points[0] = leftUp;//Left up near
            points[1] = new Point3D(leftUp._x, leftUp._y, rightDown._z);//Left up far
            points[2] = new Point3D(leftUp._x, rightDown._y, rightDown._z);//Left down far
            //Right Plane
            points[3] = new Point3D(rightDown._x, leftUp._y, rightDown._z);//Right up far
            points[4] = new Point3D(rightDown._x, leftUp._y, leftUp._z);//Right up near
            points[5] = new Point3D(rightDown._x, rightDown._y, leftUp._z);//Right down near
            //Back Plane
            points[6] = new Point3D(leftUp._x, leftUp._y, rightDown._z);//Back left up
            points[7] = new Point3D(rightDown._x, leftUp._y, rightDown._z);//Back right up
            points[8] = rightDown;//Back right down
            //Front Plane
            points[9] = new Point3D(rightDown._x, leftUp._y, leftUp._z);//Front right up
            points[10] = leftUp;//Front left up
            points[11] = new Point3D(leftUp._x, rightDown._y, leftUp._z);//Front left down
            //Down Plane
            points[12] = new Point3D(rightDown._x, rightDown._y, leftUp._z);//Down right near
            points[13] = new Point3D(leftUp._x, rightDown._y, leftUp._z);//Down left near
            points[14] = new Point3D(leftUp._x, rightDown._y, rightDown._z);//Down left far
            //Up Plane
            points[15] = leftUp;//Up left near
            points[16] = new Point3D(rightDown._x, leftUp._y, leftUp._z);//Up right near
            points[17] = new Point3D(rightDown._x, leftUp._y, rightDown._z);//Up right far
            for (var i = 0; i < points.Length; ++i)
            {
                glCoords[i * 3] = (float)points[i]._x;
                glCoords[i * 3 + 1] = (float)points[i]._y;
                glCoords[i * 3 + 2] = (float)points[i]._z;

                glColors[i * 4] = 0;
                glColors[i * 4 + 1] = 1;
                glColors[i * 4 + 2] = 0;
                glColors[i * 4 + 3] = 1;
            }
            return Tuple.Create(indices, glCoords, glColors);
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Draw()
        {
            Renderer.Program.Bind();
            Renderer.Program.SetUniform("modelMatrix", ViewMatrix);//Матрица модели IVBObject
            Renderer.Program.SetUniform("normalSize", new float[] { normalSize });

            VBO.Draw(this, PtrLength);
            Renderer.Program.Unbind();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Load()
        {
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.ColorArray);
            VBO.LoadVertexBuffers(this);
            Draw();
            VBO.UnLoadAllBuffers();
            GL.DisableClientState(ArrayCap.ColorArray);
            GL.DisableClientState(ArrayCap.VertexArray);
        }
    }
}
