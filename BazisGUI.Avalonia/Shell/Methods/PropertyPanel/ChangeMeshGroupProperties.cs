using BazisGUI.Avalonia.Properties;
using Project.Interfaces.Tasks;
using Project.Tasks;
using System;

namespace BazisGUI.Avalonia.Shell
{
    internal partial class MainWindow
    {
        private void ChangeMeshGroupProperties(PropertyChangedEventArgs obj, int index)
        {
            var _objectsGr = project.GetModelGroup(index);

            if (Enum.TryParse(obj.Key, out GroupPropertyKeys key))
            {
                switch (key)
                {
                    case GroupPropertyKeys.Name:
                        // Старое имя берём у самой группы: текст узла "{Name} {Count}" ломается на именах с пробелом.
                        var newName = obj.NewValue.ToString();
                        project.ChangeGroupName(_objectsGr.Name, newName);
                        OnGroupRenamed?.Invoke(_objectsGr.ObjType, _objectsGr.Number, newName);
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
                        PresentCondDataOnTree();
                        break;
                }
            }
        }
    }
}
