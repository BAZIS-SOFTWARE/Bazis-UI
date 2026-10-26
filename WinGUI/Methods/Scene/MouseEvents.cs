using Geometry;
using System;
using System.Drawing;
using System.Windows.Forms;
using SceneCore.Coordinates;

using RectangleBox = SceneCore.Picking.RectangleBox;

namespace WinGUI
{
    public partial class BaseForm
    {
        public bool IsSceneExpand        
        {
            get
            {
                return 
                    splitContainer2.Panel2Collapsed & 
                    splitContainer3.Panel1Collapsed ? true : false;
  
            }
            private set
            {
                splitContainer2.Panel2Collapsed = value;
                splitContainer3.Panel1Collapsed = value;
            }
        }

        private void scene_MouseClick(object sender, MouseEventArgs e)
        {
            if (!MouseMoveFlag)
                if (e.Button == MouseButtons.Right)
                    contextMenu.Show(scene, e.Location);
        }
        private void GlControl_MouseMove(object sender, MouseEventArgs e)
        {
            scene.Focus();

            var new_mousePosition = new Point(e.X - (scene.Width / 2), -e.Y + scene.Height / 2);
            MouseMoveFlag = true;

            if (e.Button == MouseButtons.Left)
            {
                sceneController.SelectionRectangle.winScreneCoord.X = e.Location.X;
                sceneController.SelectionRectangle.winScreneCoord.Y = scene.Height - e.Location.Y;

                RequestRedraw();
            }

            else if (e.Button == MouseButtons.Right)
            {
                MoveCamera(new_mousePosition, ScreenMousePosition, ScaleFactor);
                RequestRedraw();
            }


            else if (e.Button == MouseButtons.Middle)
            {
                var moveCam_z = -5;
                var dx = (new_mousePosition.X - ScreenMousePosition.X) * (2 * (-moveCam_z)) / (float)(scene.Width); //(mousePosition.Y - new_mousePosition.Y)
                var dy = (new_mousePosition.Y - ScreenMousePosition.Y) * (2 * (-moveCam_z)) / (float)(scene.Height);
                RotateCamera(dx, dy, settingsConfig.RotationAxis, settingsConfig.RotationAngle);

                RequestRedraw();
            }
            ScreenMousePosition = new_mousePosition;
        }

        private void GlControl_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var points = Math.Abs(e.Delta / 120);
            for (int i = 0; i < points; i++)
            {
                if (Math.Sign(e.Delta) > 0)
                    ScaleObjs(1.1f);
                else ScaleObjs(0.9f);
                RequestRedraw();
            }
        }

        private void GlControl_MouseDown(object sender, MouseEventArgs e)
        {
            MouseMoveFlag = false;
            if (e.Button == MouseButtons.Middle)
                DisplayRotationPointEvent += CreateRotationPoint();
            else if (e.Button == MouseButtons.Left)
            {
                sceneController.SelectionRectangle.winScrenePosit.X = e.X;
                sceneController.SelectionRectangle.winScrenePosit.Y = -e.Y + scene.Height;
                sceneController.SelectionRectangle.winScreneCoord.X = sceneController.SelectionRectangle.winScrenePosit.X + 10;
                sceneController.SelectionRectangle.winScreneCoord.Y = sceneController.SelectionRectangle.winScrenePosit.Y - 10;
            }
        }

        private void GlControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                if (e.Location.X > scene.Width - 16 & e.Location.X < scene.Width - 8 && e.Location.Y <= 10)
                    if (!IsSceneExpand)
                        IsSceneExpand = true;
                    else
                        IsSceneExpand = false;
                else
                {
                    var left = sceneController.SelectionRectangle.winScrenePosit.X - scene.Width / 2;
                    var rigth = sceneController.SelectionRectangle.winScreneCoord.X - scene.Width / 2;
                    var top = sceneController.SelectionRectangle.winScrenePosit.Y - scene.Height / 2;
                    var bottom = sceneController.SelectionRectangle.winScreneCoord.Y - scene.Height / 2;

                    var selectionBox = new RectangleBox(left, rigth, bottom, top);

                    if (!MouseMoveFlag)
                    {
                        var p = selectionBox.GetPoints();

                        var cx = (p[3].X + p[1].X) / 2.0;
                        var cy = (p[3].Y + p[1].Y) / 2.0;

                        if (ModifierKeys != Keys.Shift)
                            SelectObjects(new ViewportPoint(cx, cy), true);
                        else
                            SelectObjects(new ViewportPoint(cx, cy), false);
                    }
                    else
                    {
                        if (ModifierKeys != Keys.Shift)
                            SelectObjects(selectionBox, true);
                        else
                            SelectObjects(selectionBox, false);
                        
                    }
                    sceneController.SelectionRectangle.Remove();
                    RequestRedraw();
                }
            }
            else if (e.Button == MouseButtons.Middle)
            {
                DisplayRotationPointEvent = null;
                RequestRedraw();
            }
        }

        private void scene_SceneExpandEvent()
        {
            splitContainer2.Panel2Collapsed = true;
            splitContainer3.Panel1Collapsed = true;
        }

        private void scene_SceneFoldEvent()
        {
            splitContainer2.Panel2Collapsed = false;
            splitContainer3.Panel1Collapsed = false;
        }
    }
}
