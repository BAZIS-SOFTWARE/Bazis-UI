using BazisGUI.SceneCore.Interfaces;
using BazisGUI.SceneCore.VBO;
using System;
using Geometry;
using System.Linq;
using Model.Interfaces;
using System.Windows.Forms;
using UserControlsEx;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        private void SetPadding()
        {
            var top = 15;
            if (this.DeviceDpi == 120)
                top = (int)(top * 1.25);
            else if (this.DeviceDpi == 144)
                top = (int)(top * 1.44);

            // TO DO Сделать метод установки сдвига относительно верха

            navigator.Padding = new Padding(0, top, 0, 0);
            propertiesPanel.Padding = new Padding(0, top, 0, 0);
            console.Padding = new Padding(0, top, 0, 0);
        }
    }
}
