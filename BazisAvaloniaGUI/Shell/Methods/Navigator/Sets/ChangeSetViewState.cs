using BazisAvaloniaGUI.Extensions;
using Model.Interfaces;
using Model.Interfaces.ObjectsCollections;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private void ChangeSetViewState(string objInfo, string setName, bool viewState)
        {
            ISetInfo set;
            ObjType objType;
            // пока заглушим обработку объема
            if (!objInfo.TryToEnum(out objType))
            {
                set = project.GetModelSetInfo(ObjType.Поверхность, ObjType.Поверхность.ToString());

                var numbers = set.GetNumbers();
                project.SetVisible(set.ObjType, numbers, viewState);
            }
            else
            {
                set = project.GetModelSetInfo(objType, setName);
                var numbers = set.GetNumbers();
                project.SetVisible(objType, numbers, viewState);
            }
            // Сделать выключение vbo не получиться. Потеряется синхронизация.
        }
    }
}
