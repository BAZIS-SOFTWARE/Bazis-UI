using BazisGUI.SceneCore.VBO;
using Model.Interfaces;
using Model.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        private void показатьСопряженныеItem_Click(object sender, EventArgs e)
        {
            try
            {
                // TODO подумать над улучшением производительности
                var selObjs = project.GetSelection().ToList();
                foreach (var item in selObjs)
                {
                    var adjacentObjects = project.GetAdjacentGeometryObjects(item.Dim, item.Number);
                    var upperNumbers = adjacentObjects.Item1;
                    var lowerNumbers = adjacentObjects.Item2;

                    if (item.Dim > 0)
                    {
                        var lowerType = (ObjType)(item.Dim - 1);
                        foreach (var number in lowerNumbers)
                        {
                            var obj = project.GetModelObject(lowerType, number);
                            project.SetVisible(obj.ObjType, [obj.Number], true);
                        }
                    }

                    if (item.Dim < 2)
                    {
                        var upperType = (ObjType)(item.Dim + 1);
                        foreach (var number in upperNumbers)
                        {
                            var obj = project.GetModelObject(upperType, number);
                            project.SetVisible(obj.ObjType, [obj.Number], true);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, System.Drawing.Color.Red);
            }

        }
    }
}
