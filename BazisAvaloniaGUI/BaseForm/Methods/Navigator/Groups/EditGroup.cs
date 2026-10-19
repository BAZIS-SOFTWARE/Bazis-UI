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
        private async void EditGroup()
        {
            try
            {
                // Узел и проект запоминаются до await: за время интерактивного выбора
                // пользователь может выделить другой узел дерева или сменить проект.
                var node = navigator.SelectedNode;
                var currentProject = project;
                if (node == null || currentProject == null)
                    return;

                var group = currentProject.GetModelGroup(node.Index);
                if (group == null)
                    return;

                ApplySelectionColor();
                using (currentProject.BeginViewUpdate())
                {
                    currentProject.ClearSelection();
                    foreach (var objType in group.Select(x => x.ObjType).Distinct())
                    {
                        var numbers = group.Where(x => x.ObjType == objType).Select(x => x.Number);
                        currentProject.Select(objType, numbers);
                    }
                }

                // Esc: группа не изменена, текст узла не трогаем.
                if (!await EditGroupAsync(group) || !ReferenceEquals(currentProject, project))
                    return;

                node.Text = $"{group.Name} {group.Count}";
            }
            catch (OperationCanceledException)
            {
                // Окно закрыто или начат другой выбор.
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        /// <returns>true — состав группы изменён; false — отмена (Esc).</returns>
        public async Task<bool> EditGroupAsync(IGroup group)
        {
            var actConfirm = new Func<Tuple<bool, object>>(() =>
            {
                var selObj = project.GetSelection().ToList();

                if (selObj.Count() == 0)
                {
                    Dispatcher.UIThread.Invoke(new Action(() => console.PrintInfo(Resources.EditGroup_EditGroupAsync_NoObjectsSelected_Message, Color.Black)));
                    return new Tuple<bool, object>(false, new object());
                }
                else
                {
                    group.Clear();

                    group.AddRange(selObj);

                    Dispatcher.UIThread.Invoke(new Action(() => console.PrintInfo(Resources.EditGroup_EditGroupAsync_GroupChanged_Message, Color.Green)));
                    return new Tuple<bool, object>(true, new object());
                }
            });

            var actBreak = new Action(() =>
            {
                Dispatcher.UIThread.Invoke(new Action(() => console.PrintInfo(Resources.EditGroup_EditGroupAsync_OperationCanceled_Message, Color.Black)));
            });

            var message = $@"{Resources.EditGroup_EditGroupAsync_Preamble_Message}";

            return await AsyncMethodContainer(actConfirm, actBreak, message) != null;
        }
    }
}
