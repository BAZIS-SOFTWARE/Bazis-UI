using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Resources;

namespace AvaloniaGUI.Reflect
{
    /// <summary>
    /// Avalonia-аналог ReflectControl: выбор объекта сцены, задание плоскости отражения
    /// (нормаль ползунками или по координатной плоскости, смещение D протягиванием мыши)
    /// и создание зеркальной копии.
    /// </summary>
    internal sealed class ReflectControl : UserControl
    {
        private float[] Plane { get; } = { 1, 0, 0, 0 };
        private Point MouseLastPos { get; set; }
        private bool IsMouseDown { get; set; }
        private bool PreventRedraw { get; set; }

        /// <summary>Обновляет плоскость отражения на сцене</summary>
        public event Action<string, float[]> UpdateReflectPlane;
        public event Action<string, float[]> CreateReflectObj;
        public event Action<string> ShowObjs;

        private readonly ObservableCollection<string> objects = new();
        private readonly ComboBox comboBox1;
        private readonly Button btnCreateCopy;
        private readonly Slider[] trackBars;
        private readonly TextBlock[] labels;
        private readonly TextBox textBox1;
        private readonly ComboBox domainUpDown1;

        public ReflectControl()
        {
            var resources = new ResourceManager(typeof(ReflectControl));
            string Text(string name) => resources.GetString(name + ".Text");

            comboBox1 = new ComboBox { Name = "comboBox1", ItemsSource = objects, MinWidth = 180, HorizontalAlignment = HorizontalAlignment.Stretch };
            comboBox1.SelectionChanged += comboBox1_SelectedIndexChanged;
            btnCreateCopy = new Button { Name = "btnCreateCopy", Content = Text("btnCreateCopy"), IsEnabled = false };
            btnCreateCopy.Click += OnSetCopyName;

            labels = new[] { Text("label1"), Text("label2"), Text("label3") ?? "C: 0" }
                .Select((text, index) => new TextBlock { Name = $"label{index + 1}", Text = text, MinWidth = 50, VerticalAlignment = VerticalAlignment.Center })
                .ToArray();
            trackBars = new[] { 200, 100, 100 }
                .Select((value, index) => new Slider { Name = $"trackBar{index + 1}", Minimum = 0, Maximum = 200, Value = value, TickFrequency = 1, IsSnapToTickEnabled = true, Tag = index, MinWidth = 160 })
                .ToArray();
            foreach (var trackBar in trackBars)
                trackBar.PropertyChanged += (s, e) =>
                {
                    if (e.Property == RangeBase.ValueProperty)
                        OnChangeNormal((Slider)s);
                };

            var planes = new (string Name, string Vector)[]
            {
                ("radioButton1", "1 0 0 0"), ("radioButton2", "-1 0 0 0"),
                ("radioButton3", "0 1 0 0"), ("radioButton4", "0 -1 0 0"),
                ("radioButton5", "0 0 1 0"), ("radioButton6", "0 0 -1 0")
            };
            var planeButtons = new WrapPanel();
            foreach (var (name, vector) in planes)
            {
                // Как в WinForms (radioButton1.Checked = true): отмечена плоскость, совпадающая с начальной нормалью (1, 0, 0).
                var button = new RadioButton { Name = name, Content = Text(name), GroupName = "reflectPlane", Tag = vector, Margin = new Thickness(0, 0, 6, 0), IsChecked = name == "radioButton1" };
                button.IsCheckedChanged += OnChoicePlane;
                planeButtons.Children.Add(button);
            }

            textBox1 = new TextBox { Name = "textBox1", Text = "0", Width = 70, IsReadOnly = true, Cursor = new Cursor(StandardCursorType.SizeWestEast) };
            textBox1.AddHandler(PointerPressedEvent, (_, e) => { IsMouseDown = true; MouseLastPos = e.GetPosition(this); }, RoutingStrategies.Tunnel | RoutingStrategies.Bubble, handledEventsToo: true);
            textBox1.AddHandler(PointerReleasedEvent, (_, _) => IsMouseDown = false, RoutingStrategies.Tunnel | RoutingStrategies.Bubble, handledEventsToo: true);
            textBox1.AddHandler(PointerMovedEvent, OnMouseMove, RoutingStrategies.Tunnel | RoutingStrategies.Bubble, handledEventsToo: true);

            domainUpDown1 = new ComboBox
            {
                Name = "domainUpDown1",
                ItemsSource = new[] { "domainUpDown1.Items", "domainUpDown1.Items1", "domainUpDown1.Items2", "domainUpDown1.Items3" }.Select(resources.GetString).ToArray(),
                SelectedIndex = 2,
                MinWidth = 70
            };
            var button1 = new Button { Name = "button1", Content = Text("button1") };
            button1.Click += OnResetShifting;

            var normal = new Grid { ColumnDefinitions = new ColumnDefinitions("Auto,*"), RowDefinitions = new RowDefinitions("Auto,Auto,Auto"), RowSpacing = 4, ColumnSpacing = 6 };
            for (var i = 0; i < 3; i++)
            {
                Grid.SetRow(labels[i], i);
                Grid.SetRow(trackBars[i], i);
                Grid.SetColumn(trackBars[i], 1);
                normal.Children.Add(labels[i]);
                normal.Children.Add(trackBars[i]);
            }

            Content = new StackPanel
            {
                Spacing = 8,
                Margin = new Thickness(10),
                Children =
                {
                    new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Spacing = 6,
                        Children = { new TextBlock { Text = Text("label5"), VerticalAlignment = VerticalAlignment.Center }, comboBox1, btnCreateCopy }
                    },
                    normal,
                    planeButtons,
                    new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Spacing = 6,
                        Children = { new TextBlock { Text = Text("label4"), VerticalAlignment = VerticalAlignment.Center }, textBox1, domainUpDown1, button1 }
                    }
                }
            };
        }

        private string SelectedObject => comboBox1.SelectedItem as string;

        private void OnChangeNormal(Slider trackBar)
        {
            var index = (int)trackBar.Tag;
            Plane[index] = (int)trackBar.Value * 0.01f - 1;
            var text = labels[index].Text.Split(' ');
            labels[index].Text = text[0] + " " + Plane[index].ToString("0.##", CultureInfo.InvariantCulture);
            if (!PreventRedraw && SelectedObject != null)
                UpdateReflectPlane?.Invoke(SelectedObject, Plane);
        }

        private void OnMouseMove(object sender, PointerEventArgs e)
        {
            var position = e.GetPosition(this);
            var sign = Math.Sign(position.X - MouseLastPos.X);
            MouseLastPos = position;

            // Как в WinForms (e.Button == Left): отпускание кнопки вне поля не доходит до PointerReleased,
            // и по одному флагу IsMouseDown смещение продолжалось бы при движении без нажатой кнопки.
            if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                IsMouseDown = false;
            if (!IsMouseDown || SelectedObject == null || sign == 0)
                return;

            var delta = float.Parse(domainUpDown1.SelectedItem?.ToString() ?? "0.01", NumberStyles.Any, CultureInfo.InvariantCulture);
            Plane[3] += sign * delta;
            textBox1.Text = Plane[3].ToString("0.##", CultureInfo.InvariantCulture);
            UpdateReflectPlane?.Invoke(SelectedObject, Plane);
        }

        private void OnChoicePlane(object sender, RoutedEventArgs e)
        {
            var rBtn = (RadioButton)sender;
            if (rBtn.IsChecked != true || SelectedObject == null)
                return;

            var vec = rBtn.Tag.ToString().Split(' ').Select(x => float.Parse(x, CultureInfo.InvariantCulture)).ToArray();
            UpdateControlNormal(vec);
            UpdateReflectPlane?.Invoke(SelectedObject, Plane);
        }

        public void SetGlObjs(IEnumerable<string> objsName)
        {
            foreach (var item in objsName)
                if (!objects.Contains(item))
                    objects.Add(item);
        }

        public IEnumerable<string> GetAllSrcObjs() => objects;

        private void OnResetShifting(object sender, RoutedEventArgs e)
        {
            Plane[3] = 0;
            textBox1.Text = "0";
            if (!PreventRedraw && SelectedObject != null)
                UpdateReflectPlane?.Invoke(SelectedObject, Plane);
        }

        private void OnSetCopyName(object sender, RoutedEventArgs e)
        {
            // Нулевая нормаль (A = B = C = 0) плоскости не задаёт: её нормировка даёт NaN в матрице отражения.
            if (SelectedObject != null && (Plane[0] != 0 || Plane[1] != 0 || Plane[2] != 0))
                CreateReflectObj?.Invoke(SelectedObject, Plane);
        }

        private void UpdateControlNormal(float[] vector)
        {
            PreventRedraw = true;
            trackBars[0].Value = (int)(vector[0] * 100 + 100);
            trackBars[1].Value = (int)(vector[1] * 100 + 100);
            trackBars[2].Value = (int)(vector[2] * 100 + 100);
            OnResetShifting(this, null);
            PreventRedraw = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedObject == null)
                return;

            btnCreateCopy.IsEnabled = true;
            ShowObjs?.Invoke(SelectedObject);
        }
    }
}
