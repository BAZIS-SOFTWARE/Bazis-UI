using System;
using System.Collections;
using System.Collections.Generic;
using DrawingColor = System.Drawing.Color;

namespace BazisAvaloniaGUI.Navigator
{
    /// <summary>
    /// Avalonia-аналог System.Windows.Forms.TreeNode.
    /// Нужен, чтобы код обработчиков навигатора переносился из WinForms без изменений.
    /// Узел не владеет визуальными элементами: NavigatorControl показывает только раскрытые узлы
    /// плоским виртуализируемым списком, поэтому тысячи объектов набора не создают тысячи контролов.
    /// </summary>
    internal sealed class TreeNode
    {
        private string text;
        private int imageIndex = -1;
        private DrawingColor foreColor = DrawingColor.Empty;
        private bool isSelected;

        internal NavigatorControl Owner { get; private set; }
        internal TreeNodeCollection Collection { get; set; }

        /// <summary>Изменилось отображаемое состояние узла (текст, цвет, иконка, раскрытие, выделение).</summary>
        internal event Action Changed;

        public TreeNode(string text)
        {
            this.text = text;
            Nodes = new TreeNodeCollection(this);
        }

        public string Name { get; set; } = string.Empty;

        public object Tag { get; set; }

        public string Text
        {
            get => text;
            set
            {
                text = value;
                RaiseChanged();
            }
        }

        public DrawingColor ForeColor
        {
            get => foreColor;
            set
            {
                foreColor = value;
                RaiseChanged();
            }
        }

        public int ImageIndex
        {
            get => imageIndex;
            set
            {
                imageIndex = value;
                RaiseChanged();
            }
        }

        public int SelectedImageIndex { get; set; } = -1;

        public TreeNodeCollection Nodes { get; }

        public TreeNode Parent => Collection?.OwnerNode;

        public int Level => Parent == null ? 0 : Parent.Level + 1;

        public int Index => Collection?.IndexOf(this) ?? -1;

        public bool IsExpanded { get; internal set; }

        internal bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected == value)
                    return;
                isSelected = value;
                RaiseChanged();
            }
        }

        public void Expand()
        {
            if (Owner != null)
                Owner.ExpandNode(this);
            else
                IsExpanded = true;
        }

        public void Collapse()
        {
            if (Owner != null)
                Owner.CollapseNode(this);
            else
                IsExpanded = false;
        }

        internal void Toggle()
        {
            if (IsExpanded)
                Collapse();
            else
                Expand();
        }

        public void Remove() => Collection?.Remove(this);

        internal void Attach(NavigatorControl owner)
        {
            Owner = owner;
            foreach (var child in Nodes.Items)
                child.Attach(owner);
        }

        internal void RaiseChanged() => Changed?.Invoke();

        public override string ToString() => $"TreeNode: {Text}";
    }

    /// <summary>
    /// Avalonia-аналог System.Windows.Forms.TreeNodeCollection.
    /// Изменения раскрытых коллекций сразу переносятся в видимые строки навигатора.
    /// </summary>
    internal sealed class TreeNodeCollection : IEnumerable
    {
        private readonly List<TreeNode> nodes = new();

        internal TreeNode OwnerNode { get; }
        internal NavigatorControl OwnerControl { get; set; }

        internal TreeNodeCollection(TreeNode ownerNode)
        {
            OwnerNode = ownerNode;
        }

        private NavigatorControl Control => OwnerNode?.Owner ?? OwnerControl;

        internal IReadOnlyList<TreeNode> Items => nodes;

        public int Count => nodes.Count;

        public TreeNode this[int index] => nodes[index];

        public TreeNode this[string key] => nodes.Find(node => node.Name == key);

        public int Add(TreeNode node)
        {
            AddRange(new[] { node });
            return nodes.Count - 1;
        }

        public void AddRange(TreeNode[] newNodes)
        {
            var control = Control;
            var startIndex = nodes.Count;
            foreach (var node in newNodes)
            {
                node.Collection = this;
                node.Attach(control);
                nodes.Add(node);
            }
            control?.OnNodesInserted(this, startIndex, newNodes);
            OwnerNode?.RaiseChanged();
        }

        public void Clear()
        {
            Control?.OnChildrenRemoving(this);
            foreach (var node in nodes)
                node.Collection = null;
            nodes.Clear();

            // Как в WinForms: узел без дочерних элементов не остаётся раскрытым,
            // поэтому повторно добавленный виртуальный узел загружается при следующем Expand.
            if (OwnerNode != null)
                OwnerNode.IsExpanded = false;
            OwnerNode?.RaiseChanged();
        }

        public bool ContainsKey(string key) => nodes.Exists(node => node.Name == key);

        public TreeNode[] Find(string key, bool searchAllChildren)
        {
            var result = new List<TreeNode>();
            foreach (var node in nodes)
            {
                if (node.Name == key)
                    result.Add(node);
                if (searchAllChildren)
                    result.AddRange(node.Nodes.Find(key, true));
            }
            return result.ToArray();
        }

        internal int IndexOf(TreeNode node) => nodes.IndexOf(node);

        internal void Remove(TreeNode node)
        {
            if (!nodes.Contains(node))
                return;
            Control?.OnNodeRemoving(node);
            nodes.Remove(node);
            node.Collection = null;
            OwnerNode?.RaiseChanged();
        }

        // Копия позволяет обработчикам изменять коллекцию во время перебора, как это делал код WinForms.
        public IEnumerator GetEnumerator() => nodes.ToArray().GetEnumerator();
    }
}
