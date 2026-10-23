using WinGUI.Properties;
using SceneCore.VBO;
using Model.Interfaces;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace WinGUI
{
    public partial class BaseForm
    {
        private async Task FindCoincidentNodes(float distance)
        {
            if (!project.HasProject)
                return;
            Invoke(new Action(() => { console.PrintInfo(Resources.FindCoincidentNodes_Action_Message, Color.Black); }));

            var coincidentNodes = project.FindCoincidentObjects(ObjType.Узел, distance);

            Invoke(new Action(() => { console.PrintInfo($"{Resources.FindCoincidentNodes_Action_Found_Message} {coincidentNodes.Count()} {Resources.FindCoincidentNodes_Action_Matches_Message}", Color.Black); }));
            Invoke(new Action(() =>
            {
                var numbers = coincidentNodes.SelectMany(x => x).ToList();
                ApplySelectionColor();
                project.SetSelection(ObjType.Узел, numbers);

                //var ndSet = project.GetModelSetsInfo(ObjType.Узел).First();
                //var pres = project.CreateModelObjectsPresentor(ndSet);
                //SetVBObjectAttribute(pres, "цвет");

            }));
            var actConfirm = new Func<Tuple<bool, object>>(() =>
            {
                project.MergeNodes(coincidentNodes);

                Invoke(new Action(() =>
                {
                    //navigator.TrySearchNodes(NodeName.сетка, out List<TreeNode> objects);
                    //objects[0].Nodes[0].Nodes[0].Text = $"{set.Name} : {set.NumberOfObjects}";
                    console.PrintInfo(Resources.FindCoincidentNodes_ActionConfirm_MergeNodes_Message, Color.Green);
                }));
                return new Tuple<bool, object>(true, new object());
            });

            var actBreak = new Action(() =>
            {
                Invoke(new Action(() => console.PrintInfo(Resources.FindCoincidentNodes_Action_OperationCanceled_Message, Color.Black)));
            });
            await AsyncMethodContainer(actConfirm, actBreak, $@"{Resources.FindCoincidentNodes_AsyncContainer_Message}");
        }
    }
}
