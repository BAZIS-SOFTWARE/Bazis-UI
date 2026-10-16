using BazisAvaloniaGUI.Properties;
using Model.Interfaces;
using Model.Interfaces.ObjectsCollections;
using Model.MeshObjects;
using System.Collections.Generic;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    // Подбор объектов на сцене выполняет SceneView; из SelectByPoint.cs перенесено заполнение панели свойств.
    internal partial class MainWindow
    {
        private void CreateObjectProperties(ISetInfo setName, int number)
        {

            var rows = new List<RowProperty>
            {
                new RowProperty(ObjectPropertyKey.Type.ToString(),
                Resources.Header_object_object,
                setName.ObjType,
                true)
            };

            switch (setName.ObjType)
            {
                case ObjType.Точка:
                    rows.AddRange(GetPointProperty(number));
                    break;

                case ObjType.Узел:
                    var node = (Node)project.GetModelObject(ObjType.Узел, number);
                    rows.AddRange(GetNodeProperty(node));
                    break;

                case ObjType.Элемент1D | ObjType.Элемент2D | ObjType.Элемент3D:
                    var element = project.GetAllModelElements().First(x => x.Number == number);
                    rows.AddRange(GetElementProperty(element));
                    break;

                case ObjType.Кривая:
                    rows.AddRange(GetCurveProperties(number));
                    break;

                case ObjType.Поверхность:
                    rows.AddRange(GetSurfaceProperties(number));
                    break;
            }

            var objInfo = $"{number} {setName.ObjType}";
            propertiesPanel.DrawTable(rows, objInfo, 1);
        }
    }
}
