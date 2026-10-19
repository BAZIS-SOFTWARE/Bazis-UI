using BazisAvaloniaGUI.Extensions;
using BazisAvaloniaGUI.Localization;
using BazisAvaloniaGUI.Properties;
using Model.Interfaces;
using Model.MeshObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        enum ObjectPropertyKey { Type }
        private void navigator_SelectObjectEvent(string objInfo, int number)
        {
            try
            {
                ObjType objType;
                // пока заглушим обработку объема
                if (objInfo.TryToEnum(out objType))
                {
                    ApplySelectionColor();
                    project.SetSelection(objType, [number]);

                    CreateObjectProperties(objType, number);
                }
                else
                {
                    var vol = project.GetModelVolumes().First(x => x.Number == number);
                    var numbers = vol.GetSurfaceFigures().Select(x => x.Number);
                    ApplySelectionColor();
                    project.SetSelection(ObjType.Поверхность, numbers);

                    CreateVolProperties(number);
                }
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        private void CreateVolProperties(int number)
        {
            try
            {
                var rows = new List<RowProperty>();

                rows.Add(new RowProperty(ObjectPropertyKey.Type.ToString(), Resources.Header_object_object, Resources.Header_volume_volume, true));

                rows.AddRange(GetVolProperties(number));

                propertiesPanel.DrawTable(rows);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }

        }

        private void CreateObjectProperties(ObjType objType, int number)
        {
            var rows = new List<RowProperty> { new RowProperty(ObjectPropertyKey.Type.ToString(), "Объект", objType, true) };

            switch (objType)
            {
                case ObjType.Точка:
                    rows.AddRange(GetPointProperty(number));
                    break;

                case ObjType.Узел:
                    var node = (Node)project.GetModelObject(ObjType.Узел, number);
                    rows.AddRange(GetNodeProperty(node));
                    break;

                case ObjType.Кривая:
                    rows.AddRange(GetCurveProperties(number));
                    break;

                case ObjType.Элемент1D | ObjType.Элемент2D | ObjType.Элемент3D:
                    var element = project.GetAllModelElements().First(x => x.Number == number);
                    rows.AddRange(GetElementProperty(element));
                    break;

                case ObjType.Поверхность:
                    rows.AddRange(GetSurfaceProperties(number));
                    break;
            }

            propertiesPanel.DrawTable(rows);
        }
    }
}
