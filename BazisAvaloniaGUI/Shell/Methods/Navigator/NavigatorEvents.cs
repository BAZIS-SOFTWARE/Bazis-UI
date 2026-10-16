using BazisAvaloniaGUI.Extensions;
using BazisAvaloniaGUI.Navigator;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {

        private void navigator_RemoveAllConditionsEvent()
        {
            try
            {
                project.ClearTaskData();
                navigator.SelectedNode.Nodes.Clear();
                PresentCondDataOnTree();
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }

        }

        public void navigator_RemoveConditionEvent(int index)
        {
            project.DeleteCond(index);
        }


        private void navigator_GetObjectsInfoEvent(TreeNode node)
        {
            //TODO тут преобразование текста узла в тип объекта

            ObjType objType;

            var objInfo = node.Text.Split(' ')[0];
            // пока заглушим обработку объема
            if (!objInfo.TryToEnum(out objType))
            {
                foreach (var item in project.GetModelVolumes())
                {
                    var r_node = navigator.CreateRealNode(NodeName.Objects, item.ToString());

                    node.Nodes.Add(r_node);
                }
            }
            else
            {
                var setName = node.Text.Split(' ')[1];
                if (objType == ObjType.Поверхность)
                {
                    var ar = node.Text.Split(' ');
                    setName = string.Join(" ", ar, 1, ar.Length - 2);
                }

                var setInfo = project.GetModelSetInfo(objType, setName);
                var childs = navigator.CreateRealNodes(NodeName.Objects, setInfo.GetObjectsInfo());

                navigator.DrawNodeFrozen = true;
                navigator.BeginUpdate();

                node.Nodes.AddRange(childs);

                navigator.EndUpdate();
                navigator.DrawNodeFrozen = false;
            }


        }

        private void navigator_GetSetsInfoEvent(TreeNode node)
        {
            var nodeType = node.Name.ToEnum<NodeName>();

            List<ObjType> objTypes;
            if (nodeType == NodeName.Mesh)
                objTypes = new List<ObjType>()
                {
                    ObjType.Узел,
                    ObjType.Элемент1D,
                    ObjType.Элемент2D,
                    ObjType.Элемент3D
                };
            else
            {
                objTypes = new List<ObjType>()
                {
                    ObjType.Точка,
                    ObjType.Кривая,
                    ObjType.Поверхность
                };
            }
            foreach (var item in objTypes)
                foreach (var set in project.GetModelSetsInfo(item))
                {
                    var text = $"{set.ObjType} {set.Name} {set.NumberOfObjects}";
                    var r_node = navigator.CreateRealNode(NodeName.Sets, text);
                    var v_node = navigator.CreateVirtualNode();
                    r_node.Nodes.Add(v_node);
                    node.Nodes.Add(r_node);
                }
            // загрузка объемов
            if (nodeType == NodeName.Geometry)
            {
                var text = $"Объемы Объем {project.GetModelVolumes().Count()}";
                var r_node = navigator.CreateRealNode(NodeName.Sets, text);

                var v_node = navigator.CreateVirtualNode();
                r_node.Nodes.Add(v_node);
                node.Nodes.Add(r_node);
            }
        }
    }
}
