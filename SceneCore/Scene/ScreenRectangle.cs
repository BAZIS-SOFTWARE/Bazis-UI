using Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace SceneCore
{
    public class ScreenRectangle
    {
        public Point winScrenePosit;
        public Point winScreneCoord;

        public float Red { get; set; }
        public float Green { get; set; } = 0.75f;
        public float Blue { get; set; }

        public ScreenRectangle()
        {
            winScrenePosit = new Point(0, 0);
        }

        public void Remove()
        {
            winScreneCoord.X = 0; winScreneCoord.Y = 0;
            winScrenePosit.X = 0; winScrenePosit.Y = 0;
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
        public void Display(int width, int height)
        {
            Initialize_GUI_Plane(width, height);
            {
                GL.Translate(0, 0, -0.1f);
                GL.PushMatrix();
                GL.Color3(Red, Green, Blue);
                GL.LineWidth(1.5f);
                GL.Begin(PrimitiveType.LineLoop);

                GL.Vertex2(winScrenePosit.X, winScrenePosit.Y);
                GL.Vertex2(winScreneCoord.X, winScrenePosit.Y);
                GL.Vertex2(winScreneCoord.X, winScreneCoord.Y);
                GL.Vertex2(winScrenePosit.X, winScreneCoord.Y);

                GL.End();
                GL.PopMatrix();
            }

            Finish_GUI_Plane();
        }
    }
}
