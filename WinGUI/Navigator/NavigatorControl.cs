using WinGUI.Extensions;
using WinGUI.Localization;
using WinGUI.PinnedControl;
using WinGUI.Properties;
using Project.TaskParameters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;
using UserControlsEx;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinGUI.Navigator
{
    //public enum ViewRegime : int { ribbers, surfaces, ribbersSurfaces };

    public enum NodeKind : int { real,virt }
    public enum NodeName : int
    {
        Project,
        Geometry,
        Mesh,

        Sets,
        Objects,

        Groups,
        NodesGroup,
        ElementsGroup,

        Task,
        Material,
        Media,
        Heat,
        Clamp,
        Load,

        Calculations,
        Calculation,

        Results,
        Result,
        Time
    }

    public partial class NavigatorControl : PinnedPage//, ILocalizableHeaderControl
    {
        public TreeNode SelectedNode
        {
            get 
            {
                return treeView.SelectedNode; 
            }
        }

        public void SelectNode(TreeNode treeNode)
        {
            treeView.SelectedNode = treeNode;
        }

        private const string VIRTUALNODE = "VIRT";

        Dictionary<NodeName, int> genImgDict;
        Dictionary<NodeName, int[]> helpImgDict;

        [Category("treeView")]
        [Description("Set imageIndex for expand node")]
        public int ExpandIndex { get; set; } = 8;

        [Category("treeView")]
        [Description("Set imageIndex for collapse node")]
        public int CollapseIndex { get; set; } = 7;

        [Category("treeView")]
        [Description("Set imageIndex for project info nodes")]
        public int ProjectInfoIndex { get; set; } = 0;
        public bool DrawNodeFrozen { get; set; }

        public event Action HideResultsEvent;
        public event Action RemoveResultsEvent;
        
        public event Action RemoveAllConditionsEvent;

        public event Action DelAllGroupsEvent;
        public event Action ShowAllGroupsEvent;
        public event Action HideAllGroupsEvent;

        public event Action<bool> ChangeAllGeoViewStateEvent;
        public event Action DelAllGeoEvent;

        public event Action DelAllMeshEvent;
        public event Action<bool> ChangeAllMeshViewStateEvent;

        public event Action ShowSetEvent;
        public event Action HideSetEvent;
        public event Action DelSetEvent;
        public event Action<string> SelectSetEvent;
        public event Action<TreeNode> GetSetsInfoEvent;

        public event Action<int> SelectGroupEvent;
        public event Action DelGroupEvent;
        public event Action HideGroupEvent;
        public event Action ShowGroupEvent;
        public event Action EditGroupEvent;
        public event Action InfoGroupEvent;

        public event Action<TreeNode> GetObjectsInfoEvent;

        public event Action<string, int> SelectObjectEvent;
        //public event Action ShowAdjacenciesEvent;
        //public event Action ShowAdjacenciesSetEvent;
        public event Action DelObjectEvent;
        public event Action<NodeName, string, int> GetObjectInfoEvent;
        public event Action ShowObjectEvent;
        public event Action HideObjectEvent;

        public event Action<int> SelectCondEvent;
        public event Action SelectTaskEvent;
        public event Action SelectGeoEvent;
        public event Action SelectMeshEvent;
        public event Action SelectResultsEvent;
 
        public event Action<string> SelectCompEvent;
        public event Action SelectCompsEvent;
        public event Action SelectGeneralInfoEvent;
        public event Action<string, double> SelectTimeEvent;
        public event Action<string> SelectResultEvent;

        public event Action<TreeNode> GetResultInfoEvent;
        public event Action DelCondEvent;

        public NavigatorControl()
        {
            InitializeComponent();

            typeof(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty).
                SetValue(treeView, true, null);

            //SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            
            //8 - 

            genImgDict = new Dictionary<NodeName, int>()
            {
                { NodeName.Project, 7 },
                { NodeName.Mesh,7},
                { NodeName.Geometry,7},
                { NodeName.Material,2},
                { NodeName.Media,3},
                { NodeName.Heat,4},
                { NodeName.Clamp,5},
                { NodeName.Load,6},
                { NodeName.Results,7},
                { NodeName.Calculations,7},
                { NodeName.Calculation,9},
                { NodeName.Result,7},
                { NodeName.Time,9},
                { NodeName.Task,7},
                { NodeName.Groups,7},
                { NodeName.NodesGroup,0},
                { NodeName.ElementsGroup,1},
                { NodeName.Sets,7},
                { NodeName.Objects,9}
            };

            helpImgDict = new Dictionary<NodeName, int[]>()
            {
                { NodeName.Mesh,new []{ 2,3,4} },
                { NodeName.Geometry,new []{ 2, 3, 4 } },
                { NodeName.Material,new []{ 4} },
                { NodeName.Media,new []{ 4}},
                { NodeName.Heat,new []{ 4}},
                { NodeName.Clamp,new []{ 4}},
                { NodeName.Load,new []{ 4}},
                { NodeName.Results,new []{ 3,4}},
                { NodeName.Result,new int[0]},
                { NodeName.Time,new int[0]},
                { NodeName.Calculations,new int [0]},
                { NodeName.Calculation,new int [0]},
                { NodeName.Task,new []{4} },
                { NodeName.Groups,new []{ 2,3,4}},
                { NodeName.NodesGroup,new []{ 0,1,2,3,4}},
                { NodeName.ElementsGroup,new []{ 0,1,2,3,4}},
                { NodeName.Objects,new []{ 2,3,4}},
                { NodeName.Sets,new []{ 2,3,4}},
            };

            var node = CreateRealNode(NodeName.Project, Resources.Navigator_TreeView_Node_Text_Project);
            treeView.Nodes.Clear();
            treeView.Nodes.Add(node);
            treeView.Nodes[0].Expand();
        }

        public int GetObjectImageIndex(NodeName nodeType)
        {
            return genImgDict[nodeType];
        }

        public TreeNode CreateRealNode(NodeName nodeName)
        {
            return new TreeNode(Localization.Localization.GetNavigatorNodeNameLocalization(nodeName)) { Name = nodeName.ToString(), ImageIndex = GetObjectImageIndex(nodeName) };
        }

        public TreeNode CreateRealNode(NodeName nodeName, string text)
        {
            var imgIndex = GetObjectImageIndex(nodeName);
            return new TreeNode(text)
            {
                Name = nodeName.ToString(),
                ImageIndex = imgIndex,
                SelectedImageIndex = imgIndex
            };
        }

        public TreeNode CreateVirtualNode(NodeName name)
        {
            var tVirt = new TreeNode("Loading...") { Name = name.ToString() };
            tVirt.Name = VIRTUALNODE;
            tVirt.ForeColor = Color.Blue;
            //tVirt.NodeFont = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Underline);
            return tVirt;
        }

        //public TreeNode CreateVirtualNode(string name)
        //{
        //    var tVirt = new TreeNode("Loading...") { Name = name };
        //    tVirt.Name = VIRTUALNODE;
        //    tVirt.ForeColor = Color.Blue;
        //    //tVirt.NodeFont = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Underline);
        //    return tVirt;
        //}

        public TreeNode CreateVirtualNode()
        {
            var tVirt = new TreeNode("Loading...");
            tVirt.Name = VIRTUALNODE;
            tVirt.ForeColor = Color.Blue;
            //tVirt.NodeFont = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Underline);
            return tVirt;
        }

        public TreeNode[] CreateRealNodes(NodeName nodeName, IEnumerable<string> text)
        {

            var childs = new TreeNode[text.Count()];
            var counter = 0;
            foreach (var item in text)
            {
                var imgIndex = GetObjectImageIndex(nodeName);
                childs[counter++] = new TreeNode(item) 
                { 
                    Name = nodeName.ToString(),
                    ImageIndex = imgIndex,
                    SelectedImageIndex = imgIndex
                };
            }

            return childs;
        }

        //public void TryCreateNode(string root, string name,string text, NodeKind kind)
        //{
    
        //    if (TrySearchNodes(root,out List<TreeNode>nodes))
        //    {
        //        if (kind == NodeKind.virt)
        //        {
        //            var v = CreateVirtualNode(name);
        //            nodes.First().Nodes.Add(v);
        //        }

        //        else
        //        {
        //            var r = CreateRealNode(name, text);
        //            nodes.First().Nodes.Add(r);
        //        }
        //    }

        //}

        public void ActionMenu(TreeNode node, int actIndex)
        {
            var nodeName = node.Name.ToEnum<NodeName>();
            if (nodeName == NodeName.Results)
            {
                if (actIndex == 0)
                    HideResultsEvent?.Invoke();
                else if (actIndex == 1)
                    RemoveResultsEvent?.Invoke();
                
            }

            //node.ContextMenuStrip = resultsMenuStrip;
            else if (nodeName == NodeName.Task)
            {
                if (actIndex == 0)
                    RemoveAllConditionsEvent?.Invoke();
            }
            else if (nodeName == NodeName.Material |
                    nodeName == NodeName.Media |
                    nodeName == NodeName.Heat |
                    nodeName == NodeName.Clamp |
                    nodeName == NodeName.Load)
                {
                if (actIndex == 0)
                    DelCondEvent?.Invoke();
            }
            //node.ContextMenuStrip = taskMenuStrip;
            else if (nodeName == NodeName.Geometry)
            {
                if (actIndex == 0)
                    ChangeAllGeoViewStateEvent?.Invoke(true);
                else if (actIndex == 1)
                    ChangeAllGeoViewStateEvent?.Invoke(false);
                else if (actIndex == 2)
                    DelAllGeoEvent?.Invoke();
                
            }
                //node.ContextMenuStrip = geoMenuStrip;
            else if (nodeName == NodeName.Mesh)
            {
                // TODO подключить
                if (actIndex == 0)
                    ChangeAllMeshViewStateEvent?.Invoke(true);
                else if (actIndex == 1)
                    ChangeAllMeshViewStateEvent?.Invoke(false);
                else if (actIndex == 2)
                    DelAllMeshEvent?.Invoke();
                
            }

            else if (nodeName == NodeName.Sets)
            {
                if (actIndex == 0)
                    ShowSetEvent?.Invoke();
                else if (actIndex == 1)
                    HideSetEvent?.Invoke();
                else if (actIndex == 2)
                    DelSetEvent?.Invoke();
            }
            else if (nodeName == NodeName.Objects)
            {
                if (actIndex == 0)
                    ShowObjectEvent?.Invoke();
                else if (actIndex == 1)
                    HideObjectEvent?.Invoke();
                else if (actIndex == 2)
                    DelObjectEvent?.Invoke();
            }
            //node.ContextMenuStrip = meshMenuStrip;
            else if (nodeName == NodeName.Groups)
            {
                if (actIndex == 0)
                    ShowAllGroupsEvent?.Invoke();
                else if (actIndex == 1)
                    HideAllGroupsEvent?.Invoke();
                else if (actIndex == 2)
                    DelAllGroupsEvent?.Invoke();

            }

            else if (nodeName == NodeName.NodesGroup |
                    nodeName == NodeName.ElementsGroup)
            {
                if (actIndex == 0)
                    InfoGroupEvent?.Invoke();
                else if (actIndex == 1)
                    EditGroupEvent?.Invoke();
                else if (actIndex == 2)
                    ShowGroupEvent?.Invoke();
                else if (actIndex == 3)
                    HideGroupEvent?.Invoke();
                else if (actIndex == 4)
                    DelGroupEvent?.Invoke();
                
            }
        } 

        public void SearchNodeRec(TreeNode startNode, string nodeName, List<TreeNode> nodes)
        {
            foreach (TreeNode item in startNode.Nodes)
            {
                // Check the node.  
                if (item.Name == nodeName)
                    nodes.Add(item);
                else
                    SearchNodeRec(item, nodeName, nodes);
            }

        }

        /// <summary>
        /// TrySearchNode.First - res,Second - node
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public bool TrySearchNodes(string nodeName, out List<TreeNode> nodes)
        {       
            nodes = new List<TreeNode>();

            foreach (TreeNode n in treeView.Nodes)
            {
                if (n.Name == nodeName)
                {
                    nodes.Add(n);
                    break;
                }
  
                SearchNodeRec(n, nodeName, nodes);
                if (nodes.Count > 0)
                    break;
            }

            return nodes.Count != 0;
        }

        public bool TrySearchNodes(NodeName nodeType, out List<TreeNode> nodes)
        {
            nodes = new List<TreeNode>();

            foreach (TreeNode n in treeView.Nodes)
            {
                if (n.Name == nodeType.ToString())
                {
                    nodes.Add(n);
                    break;
                }

                SearchNodeRec(n, nodeType.ToString(), nodes);
                if (nodes.Count > 0)
                    break;
            }

            return nodes.Count != 0;
        }


        private void treeView_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = CollapseIndex;
            e.Node.SelectedImageIndex = CollapseIndex;
        }

        private void treeView_AfterExpand(object sender, TreeViewEventArgs e)
        {        
            e.Node.ImageIndex = ExpandIndex;
            e.Node.SelectedImageIndex = ExpandIndex;
        }

        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView.SelectedNode = e.Node;
            if (e.Node.Name == "VIRT") 
                return;
            if (e.Node.Level > 0) // Assuming you store the image in the node's Tag property
            {
                var indexes = helpImgDict[e.Node.Name.ToEnum<NodeName>()];   //e.Node.Tag.ToString().Split(',').Select(x => int.Parse(x));

                // Draw the images
                if (indexes.Length > 0)
                {
                    var image = helpImageList_loc.Images[indexes.First()];
                    var shift = image.Width + 20;


                    if (e.X > treeView.Width - shift & e.X < treeView.Width - shift + image.Width)
                        ActionMenu(e.Node, indexes.Length - 1);

                    for (int i = indexes.Length - 2; i >= 0; i--)
                    {
                        image = helpImageList_loc.Images[i];
                        shift += image.Width + 4;

                        if (e.X > treeView.Width - shift & e.X < treeView.Width - shift + image.Width)
                            ActionMenu(e.Node, i);
                    }
                }
            }
        }

        
        private void treeView_Enter(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null)
            {
                treeView.SelectedNode.BackColor = Color.Empty;
                treeView.SelectedNode.ForeColor = Color.Empty;
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                var node = e.Node; 
                if (e.Node.Name == "VIRT")
                    return;
                //node..BackColor = System.Drawing.Color.LightBlue;

                if (e.Node.Level == 0)
                {
                    if (node.Name == NodeName.Project.ToString())
                        SelectGeneralInfoEvent?.Invoke();
                    else if (node.Name == NodeName.Results.ToString())
                        SelectResultsEvent?.Invoke();
                }

                else if (e.Node.Level == 1)
                {
                    if (node.Name == NodeName.Geometry.ToString())
                        SelectGeoEvent?.Invoke();
                    else if (node.Name == NodeName.Mesh.ToString())
                        SelectMeshEvent?.Invoke();
                    else if (node.Name == NodeName.Task.ToString())
                        SelectTaskEvent?.Invoke();
                    else if (node.Name == NodeName.Results.ToString())
                        SelectResultsEvent?.Invoke();
                    else if (node.Name == NodeName.Calculations.ToString())
                        SelectCompsEvent?.Invoke();
                }

                else if (e.Node.Level == 2)
                {
                    if (e.Node.Name == NodeName.Material.ToString() |
                        e.Node.Name == NodeName.Media.ToString() |
                        e.Node.Name == NodeName.Heat.ToString() |
                        e.Node.Name == NodeName.Clamp.ToString() |
                        e.Node.Name == NodeName.Load.ToString())
                        SelectCondEvent?.Invoke(e.Node.Index);
                    else if (e.Node.Name == NodeName.Calculation.ToString())
                    {
                        SelectCompEvent?.Invoke(e.Node.Text);
                    }

                    else if (e.Node.Name == NodeName.NodesGroup.ToString() |
                        e.Node.Name == NodeName.ElementsGroup.ToString())
                        SelectGroupEvent?.Invoke(e.Node.Index);

                    else if (e.Node.Name == NodeName.Result.ToString())
                        SelectResultEvent?.Invoke(e.Node.Text);
                    else
                        SelectSetEvent?.Invoke( e.Node.Text);
                }

                else if (e.Node.Level == 3)
                {
                    if (e.Node.Name == NodeName.Objects.ToString())
                    {
                        var number = int.Parse(node.Text.Split(' ')[0]);
                        var objType = node.Text.Split(' ')[1];
                        SelectObjectEvent?.Invoke(objType, number);
                    }
                    else if (e.Node.Name == NodeName.Time.ToString())
                        SelectTimeEvent?.Invoke(e.Node.Parent.Text, double.Parse(e.Node.Text));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            // If the node being expanded contains a virtual node then
            // we need to load this node's children on demand. If it doesn't
            // contain a virtual node then we already did it, so do nothing.

            if (e.Node.Nodes.ContainsKey(VIRTUALNODE))
            {
                try
                {
                    // Clear out all of the children
                    e.Node.Nodes.Clear();

                    if(e.Node.Level == 1)
                    {
                        if(e.Node.Name == NodeName.Mesh.ToString() |
                            e.Node.Name == NodeName.Geometry.ToString())
                            GetSetsInfoEvent?.Invoke(e.Node);
                    } 

                    else if(e.Node.Level == 2)
                        if (e.Node.Name == NodeName.Result.ToString())
                            GetResultInfoEvent?.Invoke(e.Node);
                        else
                            GetObjectsInfoEvent?.Invoke(e.Node);

                }
                catch
                {
                    // Error occured, reset to a known state
                    e.Node.Nodes.Clear();
                    //AddVirtualNode(e.Node);
                }
            }
        }

        public void BeginUpdate()
        {
            treeView.BeginUpdate();
        }

        public void EndUpdate()
        {
            treeView.EndUpdate();
        }

        // Draws a node.
        private void treeView_DrawNode(
            object sender, DrawTreeNodeEventArgs e)
        {
            // этот флаг необходим из за того, что в процессе добавления узлов
            // e.Node.Bounds.Y принимает значение "0"
            if (!DrawNodeFrozen) 
            {
                var nodeFont = e.Node.NodeFont;
                if (nodeFont == null)
                    nodeFont = treeView.Font;

                var textS = 3;

                // Draw the background and node text for a selected node.
                if ((e.State & TreeNodeStates.Focused | e.State & TreeNodeStates.Selected) != 0)
                {
                    // Draw the node text.

                    e.Graphics.DrawString(e.Node.Text, nodeFont, Brushes.Black,
                        e.Node.Bounds.X, e.Node.Bounds.Y + textS);
                    // рисуем дополнительные иконки
                    DrawImages(e);
                }

                // Use the default background and node text.
                else
                {

                    // выделяем узел который хранит выделение прозрачным прямоугольником
                    if (treeView.SelectedNode != null & e.Node == treeView.SelectedNode)
                    {
                        var brush = new SolidBrush(Color.FromArgb(100, Color.Gray));
                        e.Graphics.FillRectangle(brush, 0, e.Node.Bounds.Y,
                        treeView.Width, e.Node.Bounds.Height);
                    }

                        e.Graphics.DrawString(e.Node.Text, nodeFont, Brushes.Black,
                    e.Node.Bounds.X, e.Node.Bounds.Y + textS);

                }
            }
        }

        private void DrawImages(DrawTreeNodeEventArgs e)
        {
            if (e.Node.Level > 0 & e.Node.Name != VIRTUALNODE) // Assuming you store the image in the node's Tag property
            {
                var indexes = helpImgDict[e.Node.Name.ToEnum<NodeName>()];   //e.Node.Tag.ToString().Split(',').Select(x => int.Parse(x));

                // Draw the images
                if(indexes.Length > 0)
                {
                    var image = helpImageList_loc.Images[indexes.Last()];
                    var shift = image.Width + 20;
                    float imageY = e.Bounds.Y + (e.Bounds.Height - image.Height) / 2; // Vertically center the image
                    e.Graphics.DrawImage(image, treeView.Width - shift, imageY, image.Width, image.Height);

                    foreach (var index in indexes.Reverse().Skip(1))
                    {
                        image = helpImageList_loc.Images[index];
                        shift += image.Width + 4;

                        e.Graphics.DrawImage(image, treeView.Width - shift, imageY, image.Width, image.Height);
                    }
                }

            }
        }

        // Returns the bounds of the specified node, including the region 
        // occupied by the node label and any node tag displayed.
        //private Rectangle NodeBounds(TreeNode node)
        //{
        //    // Set the return value to the normal node bounds.
        //    Rectangle bounds = node.Bounds;
        //    if (node.Tag != null)
        //    {
        //        // Retrieve a Graphics object from the TreeView handle
        //        // and use it to calculate the display width of the tag.
        //        Graphics g = CreateGraphics();
        //        int tagWidth = (int)g.MeasureString
        //            (node.Tag.ToString(), Font).Width + 6;

        //        // Adjust the node bounds using the calculated value.
        //        bounds.Offset(tagWidth / 2, 0);
        //        bounds = Rectangle.Inflate(bounds, tagWidth / 2, 0);
        //        g.Dispose();
        //    }

        //    return bounds;
        //}   

        //private void SplitButton_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        //{
        //    if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
        //    {
        //        e.Cancel = true;
        //    }
        //}
    }
}
