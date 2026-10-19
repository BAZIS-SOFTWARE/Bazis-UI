using Model.Interfaces;
using Model.Interfaces.MeshObjects;
using System.Linq;

namespace BazisAvaloniaGUI
{
    internal partial class BaseForm
    {
        private void ShowGroupWithNodes(IGroup group)
        {
            using (project.BeginViewUpdate())
            {
                foreach (var iobj in group)
                {
                    var elem = (IElement)iobj;
                    project.SetVisible(elem.ObjType, [elem.Number], true);
                    var numbers = elem.GetVertexes().Select(x => x.Number);
                    project.SetVisible(ObjType.Узел, numbers, true);
                }
            }
        }
    }
}
