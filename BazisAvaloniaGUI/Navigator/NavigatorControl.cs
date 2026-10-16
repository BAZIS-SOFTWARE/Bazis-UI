using Avalonia;
using Avalonia.Controls;
using Avalonia.Collections;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using BazisAvaloniaGUI.Extensions;
using BazisAvaloniaGUI.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using DrawingColor = System.Drawing.Color;

namespace BazisAvaloniaGUI.Navigator
{
    public enum NodeKind : int { real, virt }
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

    internal sealed class NavigatorControl : UserControl
    {
        // Avalonia: перекрывает StyledElement.Resources, чтобы Resources.X ссылался на строковые ресурсы.
        private new class Resources : Localization.Resources { }

        public TreeNode SelectedNode
        {
            get
            {
                return treeView.SelectedItem as TreeNode;
            }
        }

        /// <summary>Аналог treeView.SelectedNode = node: как в WinForms, свёрнутые родители раскрываются.</summary>
        public void SelectNode(TreeNode treeNode)
        {
            if (treeNode == null)
            {
                treeView.SelectedItem = null;
                return;
            }

            for (var parent = treeNode.Parent; parent != null; parent = parent.Parent)
                if (!parent.IsExpanded)
                    ExpandNode(parent);
            treeView.SelectedItem = treeNode;
            treeView.ScrollIntoView(treeNode);
        }

        private const string VIRTUALNODE = "VIRT";

        Dictionary<NodeName, int> genImgDict;
        Dictionary<NodeName, int[]> helpImgDict;

        public int ExpandIndex { get; set; } = 8;

        public int CollapseIndex { get; set; } = 7;

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

        // Avalonia: вместо System.Windows.Forms.TreeView — плоский виртуализируемый список видимых узлов.
        // Avalonia TreeView создаёт контейнер на каждый узел без виртуализации вложенных уровней,
        // из-за чего раскрытие и сворачивание наборов с тысячами объектов заметно тормозило.
        private readonly ListBox treeView = new()
        {
            Background = Brush.Parse("#F0F0F0"),
            FontFamily = new FontFamily("Microsoft Sans Serif"),
            FontSize = 10.6667 // WinForms treeView: 8 pt at 96 DPI
        };
        private readonly AvaloniaList<TreeNode> rows = new();
        private readonly Dictionary<int, Bitmap> imageList = new();
        private readonly Dictionary<int, Bitmap> helpImageList_loc = new();
        private TreeNode selectedNode;

        public TreeNodeCollection Nodes { get; }

        public NavigatorControl()
        {
            InitializeComponent();
            Nodes = new TreeNodeCollection(null) { OwnerControl = this };

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
            Nodes.Clear();
            Nodes.Add(node);
            Nodes[0].Expand();
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
            tVirt.ForeColor = DrawingColor.Blue;
            return tVirt;
        }

        public TreeNode CreateVirtualNode()
        {
            var tVirt = new TreeNode("Loading...");
            tVirt.Name = VIRTUALNODE;
            tVirt.ForeColor = DrawingColor.Blue;
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
            else if (nodeName == NodeName.Geometry)
            {
                if (actIndex == 0)
                    ChangeAllGeoViewStateEvent?.Invoke(true);
                else if (actIndex == 1)
                    ChangeAllGeoViewStateEvent?.Invoke(false);
                else if (actIndex == 2)
                    DelAllGeoEvent?.Invoke();

            }
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
        public bool TrySearchNodes(string nodeName, out List<TreeNode> nodes)
        {
            nodes = new List<TreeNode>();

            foreach (TreeNode n in Nodes)
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

            foreach (TreeNode n in Nodes)
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


        private void treeView_AfterCollapse(TreeNode node)
        {
            node.ImageIndex = CollapseIndex;
            node.SelectedImageIndex = CollapseIndex;
        }

        private void treeView_AfterExpand(TreeNode node)
        {
            node.ImageIndex = ExpandIndex;
            node.SelectedImageIndex = ExpandIndex;
        }

        /// <summary>
        /// Аналог treeView_NodeMouseClick: в Avalonia иконки действий — отдельные элементы,
        /// поэтому вместо проверки координат щелчка сразу известна позиция иконки.
        /// </summary>
        internal void treeView_NodeMouseClick(TreeNode node, int position)
        {
            SelectNode(node);
            if (node.Name == "VIRT")
                return;
            if (node.Level > 0)
            {
                var indexes = helpImgDict[node.Name.ToEnum<NodeName>()];

                if (indexes.Length > 0)
                    ActionMenu(node, position);
            }
        }

        private void treeView_AfterSelect(TreeNode node)
        {
            try
            {
                if (node.Name == "VIRT")
                    return;

                if (node.Level == 0)
                {
                    if (node.Name == NodeName.Project.ToString())
                        SelectGeneralInfoEvent?.Invoke();
                    else if (node.Name == NodeName.Results.ToString())
                        SelectResultsEvent?.Invoke();
                }

                else if (node.Level == 1)
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

                else if (node.Level == 2)
                {
                    if (node.Name == NodeName.Material.ToString() |
                        node.Name == NodeName.Media.ToString() |
                        node.Name == NodeName.Heat.ToString() |
                        node.Name == NodeName.Clamp.ToString() |
                        node.Name == NodeName.Load.ToString())
                        SelectCondEvent?.Invoke(node.Index);
                    else if (node.Name == NodeName.Calculation.ToString())
                    {
                        SelectCompEvent?.Invoke(node.Text);
                    }

                    else if (node.Name == NodeName.NodesGroup.ToString() |
                        node.Name == NodeName.ElementsGroup.ToString())
                        SelectGroupEvent?.Invoke(node.Index);

                    else if (node.Name == NodeName.Result.ToString())
                        SelectResultEvent?.Invoke(node.Text);
                    else
                        SelectSetEvent?.Invoke(node.Text);
                }

                else if (node.Level == 3)
                {
                    if (node.Name == NodeName.Objects.ToString())
                    {
                        var number = int.Parse(node.Text.Split(' ')[0]);
                        var objType = node.Text.Split(' ')[1];
                        SelectObjectEvent?.Invoke(objType, number);
                    }
                    else if (node.Name == NodeName.Time.ToString())
                        SelectTimeEvent?.Invoke(node.Parent.Text, double.Parse(node.Text));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void treeView_BeforeExpand(TreeNode node)
        {
            // If the node being expanded contains a virtual node then
            // we need to load this node's children on demand. If it doesn't
            // contain a virtual node then we already did it, so do nothing.

            if (node.Nodes.ContainsKey(VIRTUALNODE))
            {
                try
                {
                    // Clear out all of the children
                    node.Nodes.Clear();

                    if (node.Level == 1)
                    {
                        if (node.Name == NodeName.Mesh.ToString() |
                            node.Name == NodeName.Geometry.ToString())
                            GetSetsInfoEvent?.Invoke(node);
                    }

                    else if (node.Level == 2)
                        if (node.Name == NodeName.Result.ToString())
                            GetResultInfoEvent?.Invoke(node);
                        else
                            GetObjectsInfoEvent?.Invoke(node);

                }
                catch
                {
                    // Error occured, reset to a known state
                    node.Nodes.Clear();
                }
            }
        }

        // В Avalonia нет TreeView.BeginUpdate/EndUpdate: обновление визуального дерева группирует диспетчер.
        public void BeginUpdate()
        {
        }

        public void EndUpdate()
        {
        }

        // ---- Avalonia: видимые строки, раскрытие узлов и иконки действий (DrawImages) ----

        private void InitializeComponent()
        {
            Background = Brush.Parse("#F0F0F0");
            treeView.Classes.Add("navigator-tree");
            treeView.SelectionMode = SelectionMode.Single;
            treeView.ItemsSource = rows;
            treeView.ItemTemplate = new FuncDataTemplate<TreeNode>((_, _) => new NavigatorRow(this), true);
            // Иконки действий прижаты к правому краю видимой области, как в DrawImages WinForms.
            treeView.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
            Content = treeView;

            treeView.SelectionChanged += (_, _) =>
            {
                var node = SelectedNode;
                if (selectedNode != null)
                    selectedNode.IsSelected = false;
                selectedNode = node;
                if (node == null)
                    return;
                node.IsSelected = true;
                treeView_AfterSelect(node);
            };
            treeView.KeyDown += TreeView_KeyDown;
        }

        // Стрелки влево/вправо сворачивают и раскрывают узел, как в WinForms TreeView.
        private void TreeView_KeyDown(object sender, KeyEventArgs e)
        {
            var node = SelectedNode;
            if (node == null)
                return;

            if (e.Key == Key.Right && node.Nodes.Count > 0)
            {
                if (node.IsExpanded)
                    SelectNode(node.Nodes[0]);
                else
                    ExpandNode(node);
                e.Handled = true;
            }
            else if (e.Key == Key.Left)
            {
                if (node.IsExpanded)
                    CollapseNode(node);
                else if (node.Parent != null)
                    SelectNode(node.Parent);
                e.Handled = true;
            }
        }

        internal void ExpandNode(TreeNode node)
        {
            if (node.IsExpanded)
                return;

            treeView_BeforeExpand(node);
            node.IsExpanded = true;
            if (IsShown(node))
            {
                var descendants = new List<TreeNode>();
                CollectVisibleDescendants(node, descendants);
                rows.InsertRange(rows.IndexOf(node) + 1, descendants);
            }
            treeView_AfterExpand(node);
            node.RaiseChanged();
        }

        internal void CollapseNode(TreeNode node)
        {
            if (!node.IsExpanded)
                return;

            if (IsShown(node))
            {
                // WinForms переносит выделение со скрываемого потомка на сворачиваемый узел.
                if (selectedNode != null && IsDescendant(selectedNode, node))
                    SelectNode(node);
                var count = CountVisibleDescendants(node);
                if (count > 0)
                    rows.RemoveRange(rows.IndexOf(node) + 1, count);
            }
            node.IsExpanded = false;
            treeView_AfterCollapse(node);
            node.RaiseChanged();
        }

        internal void OnNodesInserted(TreeNodeCollection collection, int startIndex, IReadOnlyList<TreeNode> nodes)
        {
            if (nodes.Count == 0 || !IsChildrenShown(collection))
                return;

            int position;
            if (startIndex > 0)
            {
                var previous = collection[startIndex - 1];
                position = rows.IndexOf(previous) + 1 + CountVisibleDescendants(previous);
            }
            else
                position = collection.OwnerNode == null ? 0 : rows.IndexOf(collection.OwnerNode) + 1;

            var inserted = new List<TreeNode>(nodes.Count);
            foreach (var node in nodes)
            {
                inserted.Add(node);
                CollectVisibleDescendants(node, inserted);
            }
            rows.InsertRange(position, inserted);
        }

        internal void OnNodeRemoving(TreeNode node)
        {
            if (!IsShown(node))
                return;
            rows.RemoveRange(rows.IndexOf(node), 1 + CountVisibleDescendants(node));
        }

        internal void OnChildrenRemoving(TreeNodeCollection collection)
        {
            if (collection.Count == 0 || !IsChildrenShown(collection))
                return;

            var count = 0;
            foreach (var node in collection.Items)
                count += 1 + CountVisibleDescendants(node);
            rows.RemoveRange(rows.IndexOf(collection[0]), count);
        }

        private bool IsChildrenShown(TreeNodeCollection collection)
        {
            if (collection.OwnerNode == null)
                return collection == Nodes;
            return collection.OwnerNode.IsExpanded && IsShown(collection.OwnerNode);
        }

        private bool IsShown(TreeNode node)
        {
            var current = node;
            while (current.Parent != null)
            {
                current = current.Parent;
                if (!current.IsExpanded)
                    return false;
            }
            return current.Collection == Nodes;
        }

        private static bool IsDescendant(TreeNode node, TreeNode ancestor)
        {
            for (var parent = node.Parent; parent != null; parent = parent.Parent)
                if (parent == ancestor)
                    return true;
            return false;
        }

        private static void CollectVisibleDescendants(TreeNode node, List<TreeNode> result)
        {
            if (!node.IsExpanded)
                return;
            foreach (var child in node.Nodes.Items)
            {
                result.Add(child);
                CollectVisibleDescendants(child, result);
            }
        }

        private static int CountVisibleDescendants(TreeNode node)
        {
            if (!node.IsExpanded)
                return 0;
            var count = 0;
            foreach (var child in node.Nodes.Items)
                count += 1 + CountVisibleDescendants(child);
            return count;
        }

        internal Bitmap GetImage(int index)
        {
            if (imageList.TryGetValue(index, out var icon)) return icon;
            using var stream = AssetLoader.Open(new Uri($"avares://BazisAvaloniaGUI/Navigator/Assets/{index}.png"));
            icon = new Bitmap(stream);
            imageList[index] = icon;
            return icon;
        }

        internal Bitmap GetHelpImage(int index)
        {
            if (helpImageList_loc.TryGetValue(index, out var icon)) return icon;
            using var stream = AssetLoader.Open(new Uri($"avares://BazisAvaloniaGUI/Navigator/Assets/action-{index}.png"));
            icon = new Bitmap(stream);
            helpImageList_loc[index] = icon;
            return icon;
        }

        /// <summary>Индексы иконок действий узла (условие DrawImages: не корень и не виртуальный узел).</summary>
        internal IReadOnlyList<int> GetActionImageIndexes(TreeNode node)
        {
            if (node.Level == 0 || node.Name == VIRTUALNODE || !helpImgDict.TryGetValue(node.Name.ToEnum<NodeName>(), out var indexes))
                return Array.Empty<int>();
            return indexes;
        }

    }
}
