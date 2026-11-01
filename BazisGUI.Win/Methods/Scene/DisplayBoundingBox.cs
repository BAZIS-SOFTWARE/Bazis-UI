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
                    GL.Vertex3(item[0].X, item[0].Y, item[0].Z);
                    GL.Vertex3(item[1].X, item[1].Y, item[1].Z);
                    GL.Vertex3(item[2].X, item[2].Y, item[2].Z);
                    GL.Vertex3(item[3].X, item[3].Y, item[3].Z);
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
