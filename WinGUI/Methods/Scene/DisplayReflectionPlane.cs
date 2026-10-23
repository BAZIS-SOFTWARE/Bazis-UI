using SceneCore.Interfaces;
using SceneCore.VBO;
using System;
using Geometry;
using OpenTK.Graphics.OpenGL;
using WinGUI.SettingsControls;
using System.Drawing;

namespace WinGUI
{
    public partial class BaseForm
    {
        //public event Action DisplayReflectionPlaneEvent;

        public void DisplayReflectionPlane(float[] coeff)
        {
            try
            {
                var plane = new Plane(new Point3D(coeff[0], coeff[1], coeff[2]), coeff[3]);

                var met = new Action(() =>
                {
                    GL.PushMatrix();

                    GL.Translate(-Position._x, -Position._y, -Position._z);

                    GL.Scale(1 / ScaleFactor, 1 / ScaleFactor, 1 / ScaleFactor);
                    var normal = Vector.GetVectorNorm(plane.Normal);

                    var centre = normal.Mult(plane.Shifting);
                    GL.Translate(centre._x, centre._y, centre._z);

                    var z = new Point3D(0, 0, -1);
                    var x = Vector.CrossProd(z, normal);
                    var y = Vector.CrossProd(x, normal);

                    var xn = Vector.GetVectorNorm(x);
                    var yn = Vector.GetVectorNorm(y);

                    var xp = xn.Mult(0.1f).Sum(centre);
                    var yp = yn.Mult(0.1f).Sum(centre);

                    var xyp = xp.Sum(yp);

                    //Рисование рамки
                     GL.Color3(1f, 0, 0);
                    GL.Begin(PrimitiveType.LineStrip);

                    GL.Vertex3(centre._x, centre._y, centre._z);
                    GL.Vertex3(xp._x, xp._y, xp._z);
                    GL.Vertex3(xyp._x, xyp._y, xyp._z);
                    GL.Vertex3(yp._x, yp._y, yp._z);
                    GL.Vertex3(centre._x, centre._y, centre._z);

                    GL.End();
 
                    GL.PopMatrix();
                });

                DisplayGeometryObjectEvent += met;
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
            
        }
    }
}
