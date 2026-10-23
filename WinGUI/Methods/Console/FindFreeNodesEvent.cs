using WinGUI.Properties;
using Model.Interfaces;
using System;
using System.Drawing;
using System.Linq;

namespace WinGUI
{
    public partial class BaseForm
    {
        private void console_FindFreeNodesEvent()
        {
            var freeNodes = project.FindFreeNodes();

            Invoke(new Action(() =>
            {
                console.PrintInfo($"{Resources.FindFreeNodesEvent_Found_Message} {freeNodes.Count()} {Resources.FindFreeNodesEvent_FreeNodes_Message}", Color.Black);

                if(freeNodes.Count() != 0)
                {
                    using (project.BeginViewUpdate())
                    {
                        foreach (var set in project.GetModelSetsInfo(ObjType.Узел))
                            project.SetVisible(ObjType.Узел, set.GetNumbers(), false);

                        project.SetVisible(ObjType.Узел, freeNodes, true);
                    }
                }
            }));
        }
    }
}
