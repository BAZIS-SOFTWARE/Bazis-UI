using BazisGUI.SceneCore.Interfaces;
using BazisGUI.SceneCore.VBO;
using System;
using Geometry;
using OpenTK.Graphics.OpenGL;
using BazisGUI.Win.SettingsControls;
using BazisGUI.SceneCore;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        private void DisplayBoundingBox(VBObject vbo)
        {
            var met = new Action(() =>
            {
                //var reflMatrix = GetReflectionMatrix(plane);//from stack
                //DisplayReflectionPlane(src, plane);
                GL.MatrixMode(MatrixMode.Modelview);//видовая и модельная матрица
                GL.PushMatrix();
                //GL.LoadIdentity();
                GL.Translate(-Position.X, -Position.Y, -Position.Z);
                GL.MultMatrix(vbo.ModelMatrix);
                //GL.LoadMatrix(vbo.ModelMatrix);

                //GL.PopMatrix();
     

                GL.Color3(0, 1f, 0);

                foreach (var item in vbo.BoundingBox.GetSidesPoints())
                {
                    //Рисование рамки
                    GL.Begin(PrimitiveType.LineStrip);
                    GL.Vertex3(item[0]._x, item[0]._y, item[0]._z);
                    GL.Vertex3(item[1]._x, item[1]._y, item[1]._z);
                    GL.Vertex3(item[2]._x, item[2]._y, item[2]._z);
                    GL.Vertex3(item[3]._x, item[3]._y, item[3]._z);
                    GL.End();
                }

                GL.PopMatrix();

                // пока обязательный вызов для попадания объекта в буфер цвета.
                if (settingsConfig.Transparency && !advanced3DClipper.IsEnable)
                    averageColorRenderer.DoActionsAfterDrawing(null, DrawElements.GeometryObjects);
            });

            DisplayGeometryObjectEvent += met;
        }
    }
}
