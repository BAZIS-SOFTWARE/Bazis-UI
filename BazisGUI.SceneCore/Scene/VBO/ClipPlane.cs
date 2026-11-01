using BazisGUI.SceneCore.Interfaces;
using BazisGUI.SceneCore.Coordinates;
using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using OpenTK.Graphics.OpenGL;

namespace BazisGUI.SceneCore.VBO
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

            normalSize = (float)(BoundingBox.GetDiagonalLength() * 0.125);
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

            var points = new ModelPoint3D[18];
            var indices = Enumerable.Range(0, 18).ToArray();
            var glCoords = new float[54];
            var glColors = new float[72];

            //Left Plane
            points[0] = leftUp;//Left up near
            points[1] = new ModelPoint3D(leftUp.X, leftUp.Y, rightDown.Z);//Left up far
            points[2] = new ModelPoint3D(leftUp.X, rightDown.Y, rightDown.Z);//Left down far
            //Right Plane
            points[3] = new ModelPoint3D(rightDown.X, leftUp.Y, rightDown.Z);//Right up far
            points[4] = new ModelPoint3D(rightDown.X, leftUp.Y, leftUp.Z);//Right up near
            points[5] = new ModelPoint3D(rightDown.X, rightDown.Y, leftUp.Z);//Right down near
            //Back Plane
            points[6] = new ModelPoint3D(leftUp.X, leftUp.Y, rightDown.Z);//Back left up
            points[7] = new ModelPoint3D(rightDown.X, leftUp.Y, rightDown.Z);//Back right up
            points[8] = rightDown;//Back right down
            //Front Plane
            points[9] = new ModelPoint3D(rightDown.X, leftUp.Y, leftUp.Z);//Front right up
            points[10] = leftUp;//Front left up
            points[11] = new ModelPoint3D(leftUp.X, rightDown.Y, leftUp.Z);//Front left down
            //Down Plane
            points[12] = new ModelPoint3D(rightDown.X, rightDown.Y, leftUp.Z);//Down right near
            points[13] = new ModelPoint3D(leftUp.X, rightDown.Y, leftUp.Z);//Down left near
            points[14] = new ModelPoint3D(leftUp.X, rightDown.Y, rightDown.Z);//Down left far
            //Up Plane
            points[15] = leftUp;//Up left near
            points[16] = new ModelPoint3D(rightDown.X, leftUp.Y, leftUp.Z);//Up right near
            points[17] = new ModelPoint3D(rightDown.X, leftUp.Y, rightDown.Z);//Up right far
            for (var i = 0; i < points.Length; ++i)
            {
                glCoords[i * 3] = (float)points[i].X;
                glCoords[i * 3 + 1] = (float)points[i].Y;
                glCoords[i * 3 + 2] = (float)points[i].Z;

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
