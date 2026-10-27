using BazisGUI.Win.Extensions;
using BazisGUI.Win.Navigator;
using Model.Interfaces;
using Project.Interfaces.Tasks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        
        private void navigator_RemoveAllConditionsEvent()
        {
            try
            {
                project.ClearTaskData();
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

        private void navigator_RemoveResultsEvent()
        {
            try
            {
                navigator.TrySearchNodes(NodeName.Results, out List<TreeNode> nodes);
                //nodes[0].Nodes["ПоУзлам"].Nodes.Clear();
                //nodes[0].Nodes["Набор результатов"].Nodes["ПоЭлементам"].Nodes.Clear();
                nodes[0].Remove();

                ClearAllDataOnScene();

                foreach (ObjType item in Enum.GetValues(typeof(ObjType)))
                    foreach (var setInfo in project.GetModelSetsInfo(item))
                        CreateVBObject(project.CreateModelObjectsPresentor(setInfo));

                RequestRedraw();
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }

        }
        private void navigator_HideResultsEvent()
        {
            try
            {
                ClearAllDataOnScene();

                CreateVBObjects("Объекты");

                FitObjectsToScreen();
                RequestRedraw();
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }

        }    
     

        private void navigator_ControlCollapseEvent()
        {

        }

        private void navigator_ControlUnpinnedEvent()
        {

        }

        private void navigator_NavigatorPanelCollapseEvent()
        {
            cntrНавигатор.Panel1Collapsed = true;
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
                    //var text = $"{item.Number} {item.Name} {item.NumberOfSides}";
                    var r_node = navigator.CreateRealNode(NodeName.Objects, item.ToString());

                    //navigator.SetContextMenu(r_node);

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

                // Кажется что это костыль, но без него происходит
                // вывод текста узлов в верхний левый угол вякий раз, когда добавляются
                // ноыве узлы. Поэтому DrawNodeFrozen пока использовать обязательно
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
            //var objType = Converters.ConvertNavigatorNodeNameToObjType(nodeType);

            //var sets = project.GetModelSetsInfo(objType);

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
                    //var nodeName = Converters.ConvertToNavigatorNodeType(set.ObjType);
                    var text = $"{set.ObjType} {set.Name} {set.NumberOfObjects}";
                    var r_node = navigator.CreateRealNode(NodeName.Sets, text);
                    var v_node = navigator.CreateVirtualNode();
                    r_node.Nodes.Add(v_node);
                    node.Nodes.Add(r_node);
                    //navigator.SetContextMenu(r_node);
                }
            // загрузка объемов
            if (nodeType == NodeName.Geometry)
            {
                var text = $"Объемы Объем {project.GetModelVolumes().Count()}";
                var r_node = navigator.CreateRealNode(NodeName.Sets, text);

                var v_node = navigator.CreateVirtualNode();
                r_node.Nodes.Add(v_node);
                node.Nodes.Add(r_node);
                //navigator.SetContextMenu(r_node);
            }
                //foreach (var item in project.GetModelVolumes())
                //{
                //    var r_node = navigator.CreateRealNode(NodeName.Объем, $"{NodeName.Объем} {item.Name} {item.NumberOfSides}");
                //    r_node.ImageIndex = 14;
                //    r_node.SelectedImageIndex = 14;
                //    node.Nodes.Add(r_node);
                //}
        }

        private void navigator_GetResultInfoEvent(TreeNode node)
        {
            var times = resultTimes.Select(x => x.ToString());

            var childs = navigator.CreateRealNodes(NodeName.Time, times);
            node.Nodes.AddRange(childs);
        }
    }
}
