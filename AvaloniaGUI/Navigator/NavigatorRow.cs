using Avalonia;
using Avalonia.Automation;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using System.Collections.Generic;

namespace AvaloniaGUI.Navigator
{
    /// <summary>
    /// Строка навигатора: отступ уровня, стрелка раскрытия, иконка, текст и иконки действий выбранного узла.
    /// Иконки действий входят в саму строку (аналог DrawImages в treeView_DrawNode), поэтому
    /// они исчезают вместе со строкой при сворачивании и не выходят за её высоту.
    /// </summary>
    internal sealed class NavigatorRow : Grid
    {
        internal const double RowHeight = 18;
        private const double LevelIndent = 16;
        private static readonly Avalonia.Media.Geometry CollapsedGlyph = Avalonia.Media.Geometry.Parse("M 2,0 L 6,4 L 2,8");
        private static readonly Avalonia.Media.Geometry ExpandedGlyph = Avalonia.Media.Geometry.Parse("M 0,2 L 4,6 L 8,2");

        private readonly NavigatorControl navigator;
        private readonly Border expander = new() { Width = 16, Height = RowHeight, Background = Brushes.Transparent, Cursor = new Cursor(StandardCursorType.Hand) };
        private readonly Avalonia.Controls.Shapes.Path glyph = new() { Stroke = Brush.Parse("#606060"), StrokeThickness = 1.2, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
        private readonly Image image = new() { Width = 16, Height = 16, Margin = new Thickness(0, 0, 4, 0), VerticalAlignment = VerticalAlignment.Center };
        private readonly TextBlock text = new() { VerticalAlignment = VerticalAlignment.Center, TextTrimming = TextTrimming.CharacterEllipsis, Foreground = Brushes.Black };
        private readonly StackPanel actions = new() { Orientation = Orientation.Horizontal, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(4, 0, 4, 0) };
        private TreeNode node;
        private string actionsNodeName;

        public NavigatorRow(NavigatorControl navigator)
        {
            this.navigator = navigator;
            Height = RowHeight;
            Background = Brushes.Transparent;
            ColumnDefinitions = new ColumnDefinitions("Auto,Auto,*,Auto");

            expander.Child = glyph;
            expander.PointerPressed += (_, e) =>
            {
                // Как в WinForms: щелчок по стрелке только раскрывает/сворачивает узел, не выделяя его.
                e.Handled = true;
                node?.Toggle();
            };
            DoubleTapped += (_, e) =>
            {
                if (e.Source is Image { Parent: StackPanel panel } && panel == actions)
                    return;
                // Двойной щелчок по стрелке уже переключил узел дважды в PointerPressed — третий раз не нужен.
                if (e.Source == expander || e.Source == glyph)
                    return;
                node?.Toggle();
            };

            SetColumn(expander, 0);
            SetColumn(image, 1);
            SetColumn(text, 2);
            SetColumn(actions, 3);
            Children.Add(expander);
            Children.Add(image);
            Children.Add(text);
            Children.Add(actions);
        }

        protected override void OnDataContextChanged(System.EventArgs e)
        {
            base.OnDataContextChanged(e);
            if (node != null)
                node.Changed -= UpdateView;
            node = DataContext as TreeNode;
            if (node != null)
                node.Changed += UpdateView;
            UpdateView();
        }

        private void UpdateView()
        {
            if (node == null)
                return;

            expander.Margin = new Thickness(node.Level * LevelIndent, 0, 0, 0);
            glyph.Data = node.IsExpanded ? ExpandedGlyph : CollapsedGlyph;
            // у узла без потомков стрелка скрыта, но место под неё сохраняется для выравнивания уровня
            var hasChildren = node.Nodes.Count > 0;
            glyph.IsVisible = hasChildren;
            expander.IsHitTestVisible = hasChildren;

            var bitmap = node.ImageIndex < 0 ? null : navigator.GetImage(node.ImageIndex);
            image.Source = bitmap;
            image.IsVisible = bitmap != null;

            text.Text = node.Text;
            text.Foreground = node.ForeColor.IsEmpty ? Brushes.Black : Properties.PropertiesPanelControl.ColorBrush(node.ForeColor);
            AutomationProperties.SetName(this, node.Text);

            UpdateActions();
        }

        private void UpdateActions()
        {
            var indexes = navigator.GetActionImageIndexes(node);
            if (actionsNodeName != node.Name)
            {
                actionsNodeName = node.Name;
                actions.Children.Clear();
                for (var position = 0; position < indexes.Count; position++)
                    actions.Children.Add(CreateActionIcon(indexes[position], position));
            }
            actions.IsVisible = node.IsSelected && indexes.Count > 0;
        }

        private Image CreateActionIcon(int imageIndex, int position)
        {
            var icon = new Image
            {
                Source = navigator.GetHelpImage(imageIndex),
                Width = 16,
                Height = 16,
                Margin = new Thickness(4, 0, 0, 0),
                Cursor = new Cursor(StandardCursorType.Hand)
            };
            AutomationProperties.SetName(icon, $"{node.Name}.{position}");
            icon.PointerPressed += (_, e) =>
            {
                e.Handled = true;
                navigator.treeView_NodeMouseClick(node, position);
            };
            return icon;
        }

        internal IReadOnlyList<Control> ActionIcons => actions.Children;
    }
}
