using BazisGUI.PropertiesPanel;
using Project.Interfaces.Tasks;
using Project.Tasks;
using System;

namespace BazisGUI
{
    public partial class BaseForm
    {
        private void ChangeMeshGroupProperties(PropertyChangedEventArgs obj, int index)
        {
            var _objectsGr = project.GetModelGroup(index);
            // Текст узла — "{Name} {Count}": для имён с пробелом разбор текста давал не то имя
            var grName = _objectsGr.Name;

            if (Enum.TryParse(obj.Key, out GroupPropertyKeys key))
            {
                switch (key)
                {
                    case GroupPropertyKeys.Name:
                        project.ChangeGroupName(grName, obj.NewValue.ToString());
                        break;

                    case GroupPropertyKeys.CreateCond:

                        ICondData cond;
                        CheckMatsAndFuncs();

                        if (obj.NewValue == CreateCondByGroup.Material.ToString())
                            cond = CreateMaterial(obj, _objectsGr);

                        else if (obj.NewValue == CreateCondByGroup.Heat.ToString())
                            cond = new HeatData(_objectsGr, 0, 1);

                        else if (obj.NewValue == CreateCondByGroup.Media.ToString())
                            cond = new MediaData(_objectsGr, 0, 1);

                        else if (obj.NewValue == CreateCondByGroup.Clamp.ToString())
                            cond = new ClampData(_objectsGr, 0, 1);

                        else
                            cond = new LoadData(_objectsGr, 0, 1);

                        project.AddTaskData(cond);
                        break;
                }
            }
        }
    }
}
