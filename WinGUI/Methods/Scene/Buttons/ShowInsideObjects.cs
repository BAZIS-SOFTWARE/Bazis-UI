using WinGUI;
using WinGUI.Properties;
using SceneCore.Interfaces;
using SceneCore.Interfaces;
using SceneCore.VBO;
using Geometry;
using MathNet.Numerics.LinearAlgebra;
using Model.Interfaces;
using Model.Interfaces.MeshObjects;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinGUI
{
    public partial class BaseForm
    {
        private void btnShowInsideObjects_Click(object sender, EventArgs e)
        {
            try
            {
                var btn = sender as Button;
                var flag = bool.Parse(btn.Tag.ToString());
                if (!flag)
                {
                    btn.Tag = true;
                    //settingsConfig.IsInsideObjectsShown = true;
                }

                else
                {
                    btn.Tag = false;
                    //settingsConfig.IsInsideObjectsShown = false;
                }
                    
                ChangeInsideObjects(!flag);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        public void ChangeInsideObjects(bool flag)
        {
            project.SetHideInsideSurfaces(!flag);

            if (!flag)
                console.PrintInfo(Resources.ShowInsideObjects_HideInnerObjects_Message, Color.Black);
            else
                console.PrintInfo(Resources.ShowInsideObjects_ShowAllObjects_Message, Color.Black);
        }
    }
}
