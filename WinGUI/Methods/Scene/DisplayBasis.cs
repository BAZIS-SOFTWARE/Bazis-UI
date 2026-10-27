using SceneCore.Interfaces;
using System;
//using Tao.OpenGl;
using Geometry;
using System.Drawing;
using SceneCore;
using MathNet.Numerics.LinearAlgebra;
using OpenTK.Graphics.OpenGL;
using static WinGUI.Methods.PlatformSpecific.PlatformSpecific;

namespace WinGUI
{
    public partial class BaseForm
    {
        event Action DisplayBasisEvent;
        public void DisplayBasis()
        {
            var met = new Action(() =>
            {
                var quadObj = gluNewQuadric();
                var position = Position;

                // draw "Z line"
                GL.PushMatrix();
                GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Fill); // см. выше
                GL.Translate(-position.X, -position.Y, -position.Z);
                GL.Scale(1 / ScaleFactor, 1 / ScaleFactor, 1 / ScaleFactor);
                GL.Color3(0, 0, 1f);
                gluCylinder(quadObj, 0.0015, 0.0015, 0.025, 10, 10); // рисуем цилиндр
                GL.PopMatrix();

                //draw "Y line"
                GL.PushMatrix();
                GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Fill); // см. выше
                GL.Translate(-position.X, -position.Y, -position.Z);
                GL.Scale(1 / ScaleFactor, 1 / ScaleFactor, 1 / ScaleFactor);
                GL.Color3(0, 1f, 0);
                GL.Rotate(-90, 1, 0, 0);
                gluCylinder(quadObj, 0.0015, 0.0015, 0.025, 10, 10); // рисуем цилиндр
                GL.PopMatrix();

                // draw "X line"
                GL.PushMatrix();
                GL.Translate(-position.X, -position.Y, -position.Z);
                GL.Scale(1 / ScaleFactor, 1 / ScaleFactor, 1 / ScaleFactor);
                GL.Rotate(90, 0, 1, 0);
                GL.Color3(1, 0.5f, 0);
                GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Fill); // см. выше
                gluCylinder(quadObj, 0.0015, 0.0015, 0.025, 10, 10); // рисуем цилиндр
                GL.PopMatrix();


                //draw "X tip"
                GL.PushMatrix();
                GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Fill); // см. выше
                GL.Translate(-position.X, -position.Y, -position.Z);
                GL.Scale(1 / ScaleFactor, 1 / ScaleFactor, 1 / ScaleFactor);
                GL.Color3(1, 0.5f, 0);
                GL.Translate(0.025f, 0, 0);
                GL.Rotate(90, 0, 1, 0);
                gluCylinder(quadObj, 0.0025, 0, 0.01, 10, 10); // рисуем цилиндр
                GL.PopMatrix();

                //draw "Y tip"
                GL.PushMatrix();
                GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Fill); // см. выше
                GL.Translate(-position.X, -position.Y, -position.Z);
                GL.Scale(1 / ScaleFactor, 1 / ScaleFactor, 1 / ScaleFactor);
                GL.Color3(0, 1f, 0);
                GL.Translate(0, 0.025f, 0);
                GL.Rotate(-90, 1, 0, 0);
                gluCylinder(quadObj, 0.0025, 0, 0.01, 10, 10); // рисуем цилиндр
                GL.PopMatrix();

                //draw "Z tip"
                GL.PushMatrix();
                GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Fill); // см. выше
                GL.Translate(-position.X, -position.Y, -position.Z);
                GL.Scale(1 / ScaleFactor, 1 / ScaleFactor, 1 / ScaleFactor);
                GL.Color3(0, 0, 1f);
                GL.Translate(0, 0, 0.025f);
                gluCylinder(quadObj, 0.0025, 0, 0.01, 10, 10); // рисуем цилиндр
                GL.PopMatrix();

                GL.PushMatrix();
                GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Fill);
                GL.Translate(-position.X, -position.Y, -position.Z);
                GL.Color3(1f, 1, 0);

                GL.Scale(1 / ScaleFactor, 1 / ScaleFactor, 1 / ScaleFactor);
                gluSphere(quadObj, 0.002, 10, 10); // рисуем сферу
                GL.PopMatrix();

                gluDeleteQuadric(quadObj);
            });
            DisplayBasisEvent += met;
        }
    }
}
