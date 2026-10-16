using Avalonia.Threading;
using BazisAvaloniaGUI.Localization;
using Model.Interfaces;
using System;
using System.Drawing;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private void console_FindFreeNodesEvent()
        {
            var freeNodes = project.FindFreeNodes();

            Dispatcher.UIThread.Invoke(new Action(() =>
            {
                console.PrintInfo($"{Resources.FindFreeNodesEvent_Found_Message} {freeNodes.Count()} {Resources.FindFreeNodesEvent_FreeNodes_Message}", Color.Black);

                if (freeNodes.Count() != 0)
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
