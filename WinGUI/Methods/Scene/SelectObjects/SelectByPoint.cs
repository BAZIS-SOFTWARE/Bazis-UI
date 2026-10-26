using WinGUI.Properties;
using WinGUI.PropertiesPanel;
using WinGUI.Utilities;
using Geometry;
using Model.Interfaces;
using SceneCore.Coordinates;
using Model.Interfaces.ObjectsCollections;
using Model.MeshObjects;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace WinGUI
{
    // Геометрический подбор (проекция координат, попадание в полигон/сегмент) теперь считает
    // ScenePicker внутри sceneController — см. GUI/Documents/scene.avalonia.md, раздел 6.
    // Здесь остаётся то, что Core сознательно не делает: применение выбора к модели
    // (project.Select/Deselect), вывод в консоль/панель свойств. Результат подбора
    // (какой набор/номер задет) сообщается через sceneController.InfoRequested — см. pickHits
    // в SceneInitialization.cs.
    public partial class BaseForm
    {
        public bool SelectByPoint(IEnumerable<ISetInfo> sets, ViewportPoint selectionPoint, bool isSelected)
        {
            var setList = sets.ToList();
            var pickSets = GetPickSets(setList);
            pickHits.Clear();

            var selFlag = sceneController.SelectByPoint(pickSets, selectionPoint, isSelected);

            if (selFlag && pickHits.Count > 0)
            {
                var (setName, numbers) = pickHits[0];
                var tempSetInfo = setList.First(s => s.Name == setName);
                var tempNumb = numbers.Last();

                ApplySelectionColor();
                if (isSelected)
                    project.Select(tempSetInfo.ObjType, [tempNumb]);
                else
                    project.Deselect(tempSetInfo.ObjType, [tempNumb]);

                if (bool.Parse(btnAdvSelection.Tag.ToString()))
                    DispatchSelection(new List<int>() { tempNumb }, isSelected);
                else
                {
                    console.PrintInfo($"{Resources.SelectByPoint_ObjectSelected_Message} : {Localization.Localization.GetSelectionTypeLocalization(Converters.ConvertObjTypeToSelectionType(tempSetInfo.ObjType))} {tempNumb}", Color.Black);
                    CreateObjectProperties(tempSetInfo, tempNumb);
                }
            }
            return selFlag;
        }

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
