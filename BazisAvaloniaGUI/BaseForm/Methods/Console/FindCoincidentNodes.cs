using Avalonia.Threading;
using BazisAvaloniaGUI.Localization;
using Model.Interfaces;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace BazisAvaloniaGUI
{
    internal partial class BaseForm
    {
        private async Task FindCoincidentNodes(float distance)
        {
            if (project == null)
                return;
            Dispatcher.UIThread.Invoke(new Action(() => { console.PrintInfo(Resources.FindCoincidentNodes_Action_Message, Color.Black); }));

            var coincidentNodes = project.FindCoincidentObjects(ObjType.Узел, distance);

            Dispatcher.UIThread.Invoke(new Action(() => { console.PrintInfo($"{Resources.FindCoincidentNodes_Action_Found_Message} {coincidentNodes.Count()} {Resources.FindCoincidentNodes_Action_Matches_Message}", Color.Black); }));
            Dispatcher.UIThread.Invoke(new Action(() =>
            {
                ClearAllDataOnScene();
                // В BaseForm буферы после очистки не восстанавливались, и найденные узлы не были видны
                // до подтверждения; модель возвращается на сцену, чтобы выделение было видно.
                CreateVBObjects("Объекты");

                var numbers = coincidentNodes.SelectMany(x => x).ToList();
                ApplySelectionColor();
                project.SetSelection(ObjType.Узел, numbers);

            }));
            var actConfirm = new Func<Tuple<bool, object>>(() =>
            {
                project.MergeNodes(coincidentNodes);

                Dispatcher.UIThread.Invoke(new Action(() =>
                {
                    var set = project.GetModelSetsInfo(ObjType.Узел).First();

                    console.PrintInfo(Resources.FindCoincidentNodes_ActionConfirm_MergeNodes_Message, Color.Green);
                    PresentMeshData();
                    PresentCondDataOnTree();

                    VBOController.DeleteAllVBObjects();
                    CreateVBObjects("Объекты");
                    RequestRedraw();

                }));
                return new Tuple<bool, object>(true, new object());
            });

            var actBreak = new Action(() =>
            {
                Dispatcher.UIThread.Invoke(new Action(() =>
                {
                    console.PrintInfo(Resources.FindCoincidentNodes_Action_OperationCanceled_Message, Color.Black);
                    // Esc: снимаем подсветку найденных совпадающих узлов, иначе она остаётся
                    // как обычное выделение и попадает в следующую операцию.
                    project?.ClearSelection(ObjType.Узел);
                    RequestRedraw();
                }));
            });
            await AsyncMethodContainer(actConfirm, actBreak, $@"{Resources.FindCoincidentNodes_AsyncContainer_Message}");
        }
    }
}
