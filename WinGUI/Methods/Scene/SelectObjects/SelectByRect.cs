using WinGUI.Properties;
using WinGUI.SettingsControls;
using Geometry;
using Geometry.Exteisions;
using Model.Interfaces.ObjectsCollections;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace WinGUI
{
    // См. комментарий в SelectByPoint.cs: геометрию подбора считает ScenePicker внутри
    // sceneController, здесь остаётся применение выбора к модели и вывод в консоль.
    public partial class BaseForm
    {
        public void SelectByRect(IEnumerable<ISetInfo> sets, RectangleBox selectionBox, bool isSelected)
        {
            var setList = sets.ToList();
            pickHits.Clear();

            sceneController.SelectByRect(setList, selectionBox, isSelected, project.GetVisible);

            var counter = 0;
            ApplySelectionColor();
            using (project.BeginViewUpdate())
            {
                foreach (var hit in pickHits)
                {
                    var setInfo = setList.First(s => s.Name == hit.Name);
                    var numbers = hit.Numbers.ToList();
                    counter += numbers.Count;

                    if (isSelected)
                        project.Select(setInfo.ObjType, numbers);
                    else
                        project.Deselect(setInfo.ObjType, numbers);
                }
            }

            var objStr = Declination(counter);
            if (isSelected)
                console.PrintInfo($"{Resources.SelectByRect_Selected_Message} {counter} {objStr}", Color.Black);
            else
                console.PrintInfo($"{Resources.SelectByRect_Hidden_Message} {counter} {objStr}", Color.Black);

        }

        private string Declination(int input)
        {
            string s = Resources.SelectByRect_Declination_Type1;
            if (input % 10 == 1) s = Resources.SelectByRect_Declination_Type2;
            if (input % 10 >= 2 && input % 10 <= 4) s = Resources.SelectByRect_Declination_Type3;
            //if (n % 100 >= 11 & n % 100 <= 20) s = "объектов";

            return s;
        }
    }
}
