using Model.Interfaces;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private void ChangeGroupViewState(IGroup group, bool viewState)
        {
            // TO DO сделать метод group.HideObjects() в целях инкапсуляции
            using (project.BeginViewUpdate())
            {
                foreach (var objType in group.Select(x => x.ObjType).Distinct())
                {
                    var numbers = group.Where(x => x.ObjType == objType).Select(x => x.Number);
                    project.SetVisible(objType, numbers, viewState);
                }
            }
        }
    }
}
