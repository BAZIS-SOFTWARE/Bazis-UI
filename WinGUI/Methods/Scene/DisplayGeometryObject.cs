using WinGUI.Scene.Interfaces;
using System;
using Geometry;
using System.Drawing;
using WinGUI.Scene;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using static WinGUI.Methods.PlatformSpecific.PlatformSpecific;

namespace WinGUI
{
    public partial class BaseForm
    {
        event Action DisplayGeometryObjectEvent;

        public void DisplayLocalFrame(Frame frame)
        {
            DisplayGeometryObjectEvent += new Action(() =>
            {
                GL.PushMatrix();
                GL.Translate(-Position._x, -Position._y, -Position._z);

                GL.LineWidth(1.5f);
                GL.Begin(PrimitiveType.Lines);

                // draw "Z"
                var axis_z = frame.Centre.Sum(frame.Dir_Z);

                GL.Color3(0, 0, 1f);
                GL.Vertex3(frame.Centre._x, frame.Centre._y, frame.Centre._z);
                GL.Color3(0, 0, 1f);
                GL.Vertex3(axis_z._x, axis_z._y, axis_z._z);


                // draw "Y"
                var axis_y = frame.Centre.Sum(frame.Dir_Y);
                GL.Color3(0, 1f, 0);
                GL.Vertex3(frame.Centre._x, frame.Centre._y, frame.Centre._z);
                GL.Color3(0, 1f, 0);
                GL.Vertex3(axis_y._x, axis_y._y, axis_y._z);


                // draw "X"
                var axis_x = frame.Centre.Sum(frame.Dir_X);
                GL.Color3(1, 0.5f, 0);
                GL.Vertex3(frame.Centre._x, frame.Centre._y, frame.Centre._z);
                GL.Color3(1, 0.5f, 0);
                GL.Vertex3(axis_x._x, axis_x._y, axis_x._z);

                GL.End();
                GL.PopMatrix();
            });
        }

        public void DisplayDistance(Segment3D line)
        {
            var met = new Action(() =>
            {
                GL.PushMatrix();
                GL.Translate(-Position._x, -Position._y, -Position._z);
                GL.Color3(1f, 0, 0);
                GL.LineWidth(5.0f);
                GL.Begin(PrimitiveType.Lines);

                GL.Vertex3(line.P0._x, line.P0._y, line.P0._z);
                GL.Vertex3(line.P1._x, line.P1._y, line.P1._z);
                GL.End();
                GL.PopMatrix();

                var p0 = GetSceenCoord(line.P0);
                var p1 = GetSceenCoord(line.P1);

                var p0_2D = GetScreenCoord(p0);
                var p1_2D = GetScreenCoord(p1);
            });

            DisplayGeometryObjectEvent += met;
        }

        public void DisplayPath(Point3D[] points)
        {
            Action met;
            if (points.Length > 1)
            {
                var path = new ScenePath(points);
                var quantity = path.PointsQuantity;
                met = new Action(() =>
                {
                    path.Display(Position);
                    var p0 = path[quantity - 2];
                    var p1 = path[quantity - 1];
                    DisplayText3D(path.Length.ToString(), Color.FromArgb(0, 0, 0),
                    new Point3D((p0._x + p1._x) / 2, (p0._y + p1._y) / 2, (p0._z + p1._z) / 2));
                });

                DisplayGeometryObjectEvent += met;
            }
        }

        public void DisplayVector(Point3D length, Point3D posit, Color objColor)
        {
            DisplayGeometryObjectEvent += new Action(() =>
            {
                GL.PushMatrix();

                GL.Translate(-Position._x, -Position._y, -Position._z);
                GL.Translate(posit._x, posit._y, posit._z);
                GL.Scale(1 / ScaleFactor, 1 / ScaleFactor, 1 / ScaleFactor);

                GL.Color3(objColor.R, objColor.G, objColor.B);
                GL.LineWidth(5.0f);
                GL.Begin(PrimitiveType.Lines);

                GL.Vertex3(0, 0, 0);
                GL.Vertex3(length._x, length._y, length._z);
                GL.End();

                GL.PopMatrix();
            });
        }

        public void DisplaySceneScale(string title, string info)
        {
            var scale = new SceneScale();

            scale.FontBase = FontBase;
            scale.Title = title;
            scale.Info = info;
            var g = CreateGraphics();
            DisplayGeometryObjectEvent += new Action(() =>
            {
                var Coord_X = settingsConfig.Scale_X_Coord;
                var Coord_Y = settingsConfig.Scale_Y_Coord;
                var items = resultsController.GetItems();

                Initialize_GUI_Plane(scene.Width, scene.Height);

                var lenght = scene.Height - Coord_Y - 100;
                var gap_Y = 2;
                var cellSize_Y = (lenght - ((items.Count() - 1) * gap_Y)) / items.Count();

                var step_Y = cellSize_Y + gap_Y;

                scale.DisplayScale(Coord_X, Coord_Y, gap_Y, cellSize_Y, step_Y, items);

                //var dec = (int)resultData.Precision;
                var pos_y = Coord_Y;

                foreach (var item in items)
                {
                    var incrY = pos_y + (step_Y / 2) - (step_Y / 2);

                    DisplayText(item.Min.ToString(), Color.FromArgb(0, 0, 0), new Point3D(Coord_X + 20, incrY, -5));
                    incrY = incrY + step_Y;
                    DisplayText(item.Max.ToString(), Color.FromArgb(0, 0, 0), new Point3D(Coord_X + 20, incrY, -5));

                    pos_y += step_Y;
                }


                SizeF messageSize = g.MeasureString(scale.Title, Font);
                DisplayText(scale.Title, Color.FromArgb(0, 0, 0), new Point3D(Coord_X - messageSize.Width / 2, pos_y + 30, - 5));

                messageSize = g.MeasureString(scale.Info, Font);
                DisplayText(scale.Info, Color.FromArgb(0, 0, 0), new Point3D(Coord_X - messageSize.Width / 2, pos_y + 15, -5));
                Finish_GUI_Plane();
            });
        }
        public void DisplaySpiral(Point3D p0, Point3D p1, Color objColor)
        {
            DisplayGeometryObjectEvent += new Action(() =>
            {
                GL.PushMatrix();
                GL.Translate(-Position._x, -Position._y, -Position._z);
                GL.Color3(objColor.R, objColor.G, objColor.B);
                GL.LineWidth(5.0f);
                GL.Begin(PrimitiveType.Lines);

                GL.Vertex3(p0._x, p0._y, p0._z);
                GL.Vertex3(p1._x, p1._y, p1._z);
                GL.End();
                GL.PopMatrix();
            });
        }
        /// <inheritdoc/>


        public void DisplayConus(float UpperDiam, float BottomDiam, float length, Frame frame)
        {
            var upeer_rad = UpperDiam / 2;
            var lover_rad = BottomDiam / 2;

            DisplayGeometryObjectEvent += new Action(() =>
            {
                var quadObj = gluNewQuadric(); // создаем новый объект
                                                   // для создания сфер и цилиндров
                                                   //Glu.gluQuadricOrientation(quadObj, Glu.GLU_OUTSIDE);
                GL.PushMatrix();
                GL.Color3(1f, 0, 0);
                GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Line);

                GL.Translate(-Position._x, -Position._y, -Position._z);

                //shifting
                GL.Translate(frame.Centre._x, frame.Centre._y, frame.Centre._z);

                //rotation z' and z global
                //var dirZ = frame.Z.Sub(frame.Centre);
                //var dirZnorm = Vector.GetVectorNorm(frame);
                var angleZ = Vector.GetCosAngleVectors(new Point3D(0, 0, 1), frame.Dir_Z);
                angleZ = (float)(Math.Acos(angleZ) * 180 / Math.PI);

                var axisZ = Vector.CrossProd(new Point3D(0, 0, 1), frame.Dir_Z);
                GL.Rotate(angleZ, axisZ._x, axisZ._y, axisZ._z);

                GL.Translate(0, 0, -length);

                gluCylinder(quadObj, lover_rad, upeer_rad, length, 10, 10); // рисуем конус

                GL.PopMatrix();
                gluDeleteQuadric(quadObj);
                //averageColorRenderer.DoActionsAfterDrawing(null, DrawElements.GeometryObjects);
            });
        }
        /// <inheritdoc/>

        public void DisplaySphere(float width, Frame frame)
        {
            DisplayGeometryObjectEvent += new Action(() =>
            {
                //if (settingsConfig.Transparency && !advanced3DClipper.IsEnable)
                //    averageColorRenderer.DoActionsBeforeDrawing(null, DrawElements.GeometryObjects);
                var quadObj = gluNewQuadric(); // создаем новый объект
                                                   // для создания сфер и цилиндров
                                                   //Glu.gluQuadricOrientation(quadObj, Glu.GLU_OUTSIDE);
                GL.PushMatrix();
                GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Line);
                GL.Color3(1f, 0, 0);
                GL.Translate(-Position._x, -Position._y, -Position._z);

                GL.Translate(frame.Centre._x, frame.Centre._y, frame.Centre._z);

                //var dirZ = frame.Z.Sub(frame.Centre);
                var axis = Vector.CrossProd(new Point3D(0, 0, 1), frame.Dir_Z);
                var angle = Vector.GetCosAngleVectors(new Point3D(0, 0, 1), frame.Dir_Z);
                angle = (float)(Math.Acos(angle) * 180 / Math.PI);

                GL.Rotate(angle, axis._x, axis._y, axis._z);
                //Glu.gluQuadricDrawStyle(quadObj, Glu.GLU_FILL); // устанавливаем
                gluSphere(quadObj, width / 2, 10, 10); // рисуем сферу
                                                           // радиусом 0.5
                GL.PopMatrix();
                gluDeleteQuadric(quadObj);
                //averageColorRenderer.DoActionsAfterDrawing(null, DrawElements.GeometryObjects);
            });
        }

        public void HideGeometryObj(string searchMethod)
        {
            //var list = PlugDisplayObjectEvent?.GetInvocationList();
            for (int i = 0; i < DisplayGeometryObjectEvent?.GetInvocationList().Count(); i++)
            {
                var del = DisplayGeometryObjectEvent.GetInvocationList()[i];
                if (del.Method.Name.Contains(searchMethod))
                {
                    DisplayGeometryObjectEvent -= (Action)del;
                    i--;
                }
            }
        }

        public bool FindGeometryObj(string searchMethod)
        {
            //var list = PlugDisplayObjectEvent?.GetInvocationList();
            for (int i = 0; i < DisplayGeometryObjectEvent?.GetInvocationList().Count(); i++)
            {
                var del = DisplayGeometryObjectEvent.GetInvocationList()[i];
                if (del.Method.Name.Contains(searchMethod))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
