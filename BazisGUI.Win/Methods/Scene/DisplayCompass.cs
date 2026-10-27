using BazisGUI.SceneCore.Interfaces;
using System;
//using Tao.OpenGl;
using Geometry;
using System.Drawing;
using BazisGUI.SceneCore;
using MathNet.Numerics.LinearAlgebra;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;
using static BazisGUI.Win.Methods.PlatformSpecific.PlatformSpecific;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        event Action DisplayCompassEvent;

        public void DisplayCompass()
        {
            var viewMatrix = new float[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } };
            var met = new Action(() =>
            {
                //GL.Disable(EnableCap.DepthTest);
                Array.Copy(ViewMatrix.ToArray(), viewMatrix, 16);

                var viewport = new int[4];
                GL.GetInteger(GetPName.Viewport, viewport);

                Initialize_GUI_Plane(viewport[2], viewport[3]);

                float[] matrix = Matrix<float>.Build.DenseOfArray(viewMatrix).AsColumnMajorArray();

                
                matrix[12] = scene.Width - 80; // x coord
                matrix[13] = 70; // y coord
                matrix[14] = -60; // z coord ??????

                GL.LoadMatrix(matrix);

                GL.Scale(1 / ScaleFactor, 1 / ScaleFactor, 1 / ScaleFactor);

                GL.PushMatrix();

                GL.LineWidth(3.0f);
                GL.Begin(PrimitiveType.Lines);

                // draw "Z"
                GL.Color3(0, 0, 1f);
                GL.Vertex3(0.0f, 0.0f, 0.0f);
                GL.Color3(0, 0, 1f);
                GL.Vertex3(0.0f, 0.0f, 50.0f);


                // draw "Y"
                GL.Color3(0, 1f, 0);
                GL.Vertex3(0.0f, 0.0f, 0.0f);
                GL.Color3(0, 1f, 0);
                GL.Vertex3(0.0f, 50.0f, 0.0f);


                // draw "X"
                GL.Color3(1, 0.5f, 0);
                GL.Vertex3(0.0f, 0.0f, 0.0f);
                GL.Color3(1, 0.5f, 0);
                GL.Vertex3(50.0f, 0.0f, 0.0f);

                GL.End();
                GL.PopMatrix();

                var quadObj = gluNewQuadric();
                //draw "X tip"
                GL.PushMatrix();
                GL.Color3(1, 0.5f, 0);
                GL.Translate(40, 0, 0);
                GL.Rotate(90, 0, 1, 0);
                gluCylinder(quadObj, 4, 0, 10, 10, 10); // рисуем цилиндр
                GL.PopMatrix();

                //draw "Y tip"
                GL.PushMatrix();
                GL.Color3(0, 1f, 0);
                GL.Translate(0, 40, 0);
                GL.Rotate(-90, 1, 0, 0);
                gluCylinder(quadObj, 4, 0, 10, 10, 10); // рисуем цилиндр
                GL.PopMatrix();

                //draw "Z tip"
                GL.PushMatrix();
                GL.Color3(0, 0, 1f);
                GL.Translate(0, 0, 40);
                gluCylinder(quadObj, 4, 0, 10, 10, 10); // рисуем цилиндр

                gluDeleteQuadric(quadObj);

                GL.PopMatrix();

                DisplayText("X", Color.FromArgb(0, 0, 0), new Point3D(60, 0, 0));
                DisplayText("Y", Color.FromArgb(0, 0, 0), new Point3D(0, 60, 0));
                DisplayText("Z", Color.FromArgb(0, 0, 0), new Point3D(0, 0, 60));

                Finish_GUI_Plane();
                //GL.Enable(EnableCap.DepthTest);
            });

            DisplayCompassEvent += met;
        }

        public void DisplayText(string str, Color color, Point3D coord)
        {
            GL.PushMatrix();
            GL.Color3(color.R, color.G, color.B);
            GL.RasterPos3(coord._x, coord._y, coord._z);
            GL.PushAttrib(AttribMask.ListBit);//Избегаем пересечений списков, сохраняем старую базу
            GL.ListBase(FontBase);//Устанавливаем базу на FontBase
            var handle = GCHandle.Alloc(str, GCHandleType.Pinned);
            var ptr = handle.AddrOfPinnedObject();
            GL.CallLists(str.Length, ListNameType.UnsignedShort, ptr);
            handle.Free();
            GL.PopAttrib();//Возвращаем старую базу
            GL.PopMatrix();
        }

        private void Initialize_GUI_Plane(int width, int height)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Ortho(0, width, 0, height, 0.1, 200);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.LoadIdentity();
        }

        private void Finish_GUI_Plane()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PopMatrix();
        }
    }
}
