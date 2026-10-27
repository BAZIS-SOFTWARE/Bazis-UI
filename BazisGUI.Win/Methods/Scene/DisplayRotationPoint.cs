using BazisGUI.SceneCore.Interfaces;
using System;
using Geometry;
using System.Drawing;
using BazisGUI.SceneCore;
using OpenTK.Graphics.OpenGL;
using static BazisGUI.Win.Methods.PlatformSpecific.PlatformSpecific;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        public event Action DisplayRotationPointEvent;
        private Action CreateRotationPoint()
        {
            return new Action(() =>
            {
                GL.PushMatrix();
                GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Fill);
                GL.Color3(1, 0.75f, 0);

                var quadObj = gluNewQuadric();

                GL.Scale(1 / ScaleFactor, 1 / ScaleFactor, 1 / ScaleFactor);
                gluSphere(quadObj, 0.003, 10, 10); // рисуем сферу
                GL.PopMatrix();
                gluDeleteQuadric(quadObj);
            });
        }
    }
}
