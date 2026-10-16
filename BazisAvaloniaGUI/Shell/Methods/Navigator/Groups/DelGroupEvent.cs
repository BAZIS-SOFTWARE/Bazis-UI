namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private void navigator_DelGroupEvent()
        {
            var node = navigator.SelectedNode;
            var group = project.GetModelGroup(node.Index);
            var objType = group.ObjType;
            var number = group.Number;

            project.DeleteModelGroup(group.Name);
            OnGroupDeleted?.Invoke(objType, number);

            //удаляем узел
            node.Remove();

            PresentCondDataOnTree();
        }
    }
}
