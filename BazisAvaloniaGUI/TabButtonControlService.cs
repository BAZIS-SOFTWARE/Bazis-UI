using Avalonia;
using Avalonia.Automation;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using System.Collections.Generic;
using System.Linq;

namespace BazisAvaloniaGUI
{
    /// <summary>
    /// Avalonia-аналог TabButtonControlService: вертикальные вкладки слева, страницы справа.
    /// Расчёт координат и отрисовка повёрнутого текста заменены StackPanel и LayoutTransformControl.
    /// </summary>
    public class TabButtonControlService
    {
        private Grid container;
        private StackPanel buttonsPanel;
        private Grid pagesPanel;
        private Dictionary<string, Button> buttons;
        private Dictionary<string, Control> controls;

        public TabButtonControlService(Grid container)
        {
            this.container = container;
            buttons = new();
            controls = new();

            container.ColumnDefinitions = new ColumnDefinitions("30,*");
            buttonsPanel = new StackPanel();
            pagesPanel = new Grid();
            Grid.SetColumn(pagesPanel, 1);
            container.Children.Add(buttonsPanel);
            container.Children.Add(pagesPanel);
        }

        public Button GetButton(string name) => buttons[name];
        public Control GetControl(string name) => controls[name];
        public IEnumerable<string> GetNames() => buttons.Keys;

        public void AddControl(string name, Control control)
        {
            name = name.Replace("cntr", "").Replace("btnTab", "");
            if (buttons.ContainsKey(name))
                return;

            var btn = CreateTabButton(name);

            control.Margin = new();
            control.Name = $"cntr{name}";

            buttonsPanel.Children.Add(btn);
            pagesPanel.Children.Add(control);
            buttons[name] = btn;
            controls[name] = control;

            BringToFront(name);
        }

        public void RemoveControl(string name)
        {
            if (!buttons.ContainsKey(name))
                return;

            buttonsPanel.Children.Remove(GetButton(name));
            pagesPanel.Children.Remove(GetControl(name));

            buttons.Remove(name);
            controls.Remove(name);

            var nav = controls.Keys.FirstOrDefault();
            if (nav != null)
                BringToFront(nav);
        }

        public Button CreateTabButton(string name)
        {
            var btn = new Button
            {
                Width = 27,
                MinHeight = 130,
                Padding = new Thickness(0),
                Margin = new Thickness(0, 0, 0, 3),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Content = new LayoutTransformControl
                {
                    LayoutTransform = new RotateTransform(-90),
                    Child = new TextBlock { Text = name, TextAlignment = TextAlignment.Center }
                }
            };
            btn.Tag = true;
            btn.Name = $"btnTab{name}";
            AutomationProperties.SetName(btn, name);

            btn.Click += button_MouseDown;

            return btn;
        }

        private void button_MouseDown(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            BringToFront(btn.Name.Replace("btnTab", ""));
        }

        private void BringToFront(string name)
        {
            foreach (var item in buttons)
            {
                var selected = item.Key == name;
                item.Value.Tag = selected;
                item.Value.BorderThickness = new Thickness(selected ? 2 : 1);
                item.Value.BorderBrush = selected ? Brushes.Black : null;
                controls[item.Key].IsVisible = selected;
            }
        }
    }
}
