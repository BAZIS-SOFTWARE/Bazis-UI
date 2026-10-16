using BazisAvaloniaGUI.Extensions;
using BazisAvaloniaGUI.Navigator;
using BazisAvaloniaGUI.Properties;
using Model.Interfaces;
using Model.Interfaces.ObjectsCollections;
using OperationalController;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private void ChangeMeshSetProperties(PropertyChangedEventArgs obj, int dimm)
        {
            var setName = navigator.SelectedNode.Text.Split(' ')[1];
            ISetInfo _objectsSet;
            if (dimm == 3)
                _objectsSet = project.GetModelSetInfo(ObjType.Элемент3D, setName);
            else if (dimm == 2)
                _objectsSet = project.GetModelSetInfo(ObjType.Элемент2D, setName);
            else
                _objectsSet = project.GetModelSetInfo(ObjType.Элемент1D, setName);

            if (Enum.TryParse(obj.Key, out SetPropertyKeys key))
            {
                switch (key)
                {
                    case SetPropertyKeys.Name:
                        HandleSetNameParameter(dimm, obj.OldValue, obj.NewValue);
                        break;

                    case SetPropertyKeys.Color:
                        {
                            var color = HandleSetColorParameter(obj.NewValue.ToString());
                            if (_objectsSet != null)
                                project.SetColor(_objectsSet, color);
                            break;
                        }

                    case SetPropertyKeys.View:
                        {
                            var viewMode = obj.NewValue.ToString().ToEnum<ViewMode>();
                            project.SetViewMode(_objectsSet, viewMode);
                            break;
                        }

                    case SetPropertyKeys.PrecisionOrder:
                        SetElementsOrderEvent(int.Parse(obj.NewValue));
                        break;
                }
            }
        }

        private void SetElementsOrderEvent(int obj)
        {

            var setName = navigator.SelectedNode.Text.Split(' ')[1];
            var objInfo = navigator.SelectedNode.Text.Split(' ')[0];

            var isExpand = navigator.SelectedNode.IsExpanded;

            var objType = objInfo.ToEnum<ObjType>();

            if (objType == ObjType.Элемент1D)
                project.ChangeMeshSetOrder(1, setName, obj);
            else if (objType == ObjType.Элемент2D)
                project.ChangeMeshSetOrder(2, setName, obj);
            else if (objType == ObjType.Элемент3D)
                project.ChangeMeshSetOrder(3, setName, obj);

            PresentMeshData();
            navigator.TrySearchNodes(NodeName.Mesh, out List<TreeNode> mesh);

            mesh.First().Collapse();
            mesh.First().Expand();


            if (isExpand)
            {
                var nodeName = navigator.SelectedNode.Name;
                var nodes = mesh.First().Nodes.Find(nodeName, false);
                nodes.FirstOrDefault(x => x.Text.Contains(setName))?.Expand();
            }
        }

        private void HandleSetNameParameter(int dimm, string oldValue, string newValue)
        {
            project.ChangeMeshSetName(dimm,
                    oldValue.ToString(),
                    newValue.ToString());
            PresentMeshData();
        }

        private Color HandleSetColorParameter(string newValue)
        {
            Color color;

            if (newValue.StartsWith("Color [A="))
            {
                string[] parts = newValue.Trim('C', 'o', 'l', 'r', ' ', '[', ']').Split(',');
                int a = int.Parse(parts[0].Split('=')[1]);
                int r = int.Parse(parts[1].Split('=')[1]);
                int g = int.Parse(parts[2].Split('=')[1]);
                int b = int.Parse(parts[3].Split('=')[1]);
                return Color.FromArgb(a, r, g, b);
            }
            else
                return Color.FromName(newValue.Replace("Color [", "").Replace("]", ""));
        }
    }
}
