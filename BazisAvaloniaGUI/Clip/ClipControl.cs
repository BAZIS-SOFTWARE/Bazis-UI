using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using BazisGUI.Scene;
using System;
using System.Globalization;
using System.Linq;
using System.Resources;

namespace BazisAvaloniaGUI.Clip
{
    /// <summary>Коэффициенты плоскости Ax + By + Cz = D (BazisGUI.Reflect.Plane).</summary>
    public struct ClipPlaneValue
    {
        public float X;
        public float Y;
        public float Z;
        public float D;
    }

    /// <summary>
    /// Avalonia-аналог ClipControl: включение отсечения плоскостью, выбор нормали ползунками или
    /// по координатной плоскости, смещение D протягиванием мыши по полю, режим отображения
    /// 3D-элементов и толщина слоя, захват видимых элементов.
    /// </summary>
    internal sealed class ClipControl : UserControl
    {
        private bool IsMouseDown { get; set; }
        private Point MouseLastPos { get; set; }
        private bool PreventRedraw { get; set; }

        private ClipPlaneValue plane;

        /// <summary>Включить\выключить плоскость отсечения</summary>
        public event Action<bool> SwitchOnOff;
        /// <summary>Задать плоскость отсечения</summary>
        public event Action<ClipPlaneValue> SetClipPlaneEvent;
        /// <summary>Перерисовывает плоскость отсечения на сцене</summary>
        public event Action RedrawClipPlane;
        /// <summary>Смена режима отображения для 3д элементов</summary>
        public event Action<ClipMode> ChangeClipMode;
        /// <summary>Смена толщины слоя</summary>
        public event Action<float> ChangeLayerThickness;
        /// <summary>Кнопка «Захват»: оставить видимыми только элементы, попавшие в сечение</summary>
        public event Action CaptureData;

        /// <summary>Режим отсечения</summary>
        public ClipMode Regime { get; private set; } = ClipMode.Default;

        private readonly CheckBox checkBox1;
        private readonly Slider[] sliders;
        private readonly TextBlock[] labels;
        private readonly TextBox textBox1;
        private readonly TextBox textBox2;
        private readonly ComboBox domainUpDown1;
        private readonly RadioButton radioButton7;
        private readonly RadioButton radioButton8;
        private readonly RadioButton radioButton9;
        private readonly TextBlock label6;
        private readonly Button button2;
        private readonly Panel tableLayoutPanel1;

        public ClipControl()
        {
            var resources = new ResourceManager(typeof(ClipControl));
            string Text(string name) => resources.GetString(name + ".Text");

            plane.Z = -1;

            checkBox1 = new CheckBox { Name = "checkBox1", Content = Text("checkBox1") };
            checkBox1.Click += OnEnableClipPlane;

            labels = new[] { Text("label1"), Text("label2"), Text("label3") }
                .Select((text, index) => new TextBlock { Name = $"label{index + 1}", Text = text, MinWidth = 50, VerticalAlignment = VerticalAlignment.Center })
                .ToArray();
            sliders = new[] { 100, 100, 0 }
                .Select((value, index) => new Slider { Name = $"colorSlider{index + 1}", Minimum = 0, Maximum = 200, Value = value, TickFrequency = 1, IsSnapToTickEnabled = true, Tag = index, MinWidth = 160 })
                .ToArray();
            foreach (var slider in sliders)
                slider.PropertyChanged += (s, e) =>
                {
                    if (e.Property == RangeBase.ValueProperty)
                        OnChangeValue((Slider)s);
                };

            // Координатные плоскости: значения ползунков A, B, C (100 — ноль).
            var planes = new (string Name, string Values)[]
            {
                ("radioButton1", "200 100 100"), ("radioButton2", "0 100 100"),
                ("radioButton3", "100 200 100"), ("radioButton4", "100 0 100"),
                ("radioButton5", "100 100 200"), ("radioButton6", "100 100 0")
            };
            var planeButtons = new WrapPanel();
            foreach (var (name, values) in planes)
            {
                // Как в WinForms (radioButton6.Checked = true): отмечена плоскость начальной нормали (0, 0, -1).
                var button = new RadioButton { Name = name, Content = Text(name), GroupName = "clipPlane", Tag = values, Margin = new Thickness(0, 0, 6, 0), IsChecked = name == "radioButton6" };
                button.Click += OnChoicePlane;
                planeButtons.Children.Add(button);
            }

            textBox1 = new TextBox { Name = "textBox1", Text = Text("textBox1") ?? "0", Width = 70, IsReadOnly = true, Cursor = new Cursor(StandardCursorType.SizeWestEast) };
            AttachDrag(textBox1);
            domainUpDown1 = new ComboBox
            {
                Name = "domainUpDown1",
                ItemsSource = new[] { "domainUpDown1.Items", "domainUpDown1.Items1", "domainUpDown1.Items2", "domainUpDown1.Items3" }.Select(resources.GetString).ToArray(),
                SelectedIndex = 2,
                MinWidth = 70
            };
            var button1 = new Button { Name = "button1", Content = Text("button1") };
            button1.Click += OnResetShifting;

            radioButton7 = new RadioButton { Name = "radioButton7", Content = Text("radioButton7"), GroupName = "clipMode", IsChecked = true };
            radioButton8 = new RadioButton { Name = "radioButton8", Content = Text("radioButton8"), GroupName = "clipMode" };
            radioButton9 = new RadioButton { Name = "radioButton9", Content = Text("radioButton9"), GroupName = "clipMode" };
            foreach (var radio in new[] { radioButton7, radioButton8, radioButton9 })
                radio.Click += OnChangeDrawMode;

            label6 = new TextBlock { Name = "label6", Text = Text("label6"), VerticalAlignment = VerticalAlignment.Center };
            textBox2 = new TextBox { Name = "textBox2", Text = Text("textBox2") ?? "1.0", Width = 70, IsReadOnly = true, Cursor = new Cursor(StandardCursorType.SizeWestEast) };
            AttachDrag(textBox2);
            button2 = new Button { Name = "button2", Content = Text("button2") };
            button2.Click += (_, _) => CaptureData?.Invoke();

            var grid = new Grid { ColumnDefinitions = new ColumnDefinitions("Auto,*"), RowDefinitions = new RowDefinitions("Auto,Auto,Auto,Auto,Auto"), RowSpacing = 4, ColumnSpacing = 6 };
            for (var i = 0; i < 3; i++)
            {
                Grid.SetRow(labels[i], i);
                Grid.SetRow(sliders[i], i);
                Grid.SetColumn(sliders[i], 1);
                grid.Children.Add(labels[i]);
                grid.Children.Add(sliders[i]);
            }
            Grid.SetRow(planeButtons, 3);
            Grid.SetColumnSpan(planeButtons, 2);
            grid.Children.Add(planeButtons);
            var shifting = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 6,
                Children =
                {
                    new TextBlock { Text = Text("label4"), VerticalAlignment = VerticalAlignment.Center },
                    textBox1,
                    new TextBlock { Text = Text("label5"), VerticalAlignment = VerticalAlignment.Center },
                    domainUpDown1,
                    button1
                }
            };
            Grid.SetRow(shifting, 4);
            Grid.SetColumnSpan(shifting, 2);
            grid.Children.Add(shifting);
            tableLayoutPanel1 = grid;

            var modes = new StackPanel
            {
                Name = "panel2",
                Spacing = 4,
                Children =
                {
                    radioButton7, radioButton8, radioButton9,
                    new StackPanel { Orientation = Orientation.Horizontal, Spacing = 6, Children = { label6, textBox2 } },
                    button2
                }
            };

            Content = new StackPanel
            {
                Spacing = 8,
                Margin = new Thickness(10),
                Children = { checkBox1, tableLayoutPanel1, modes }
            };

            SetControlsEnabled(false);
        }

        private void AttachDrag(TextBox textBox)
        {
            // TextBox сам обрабатывает нажатие, поэтому подписка — с handledEventsToo.
            textBox.AddHandler(PointerPressedEvent, OnMouseDown, RoutingStrategies.Tunnel | RoutingStrategies.Bubble, handledEventsToo: true);
            textBox.AddHandler(PointerMovedEvent, OnMouseMove, RoutingStrategies.Tunnel | RoutingStrategies.Bubble, handledEventsToo: true);
            textBox.AddHandler(PointerReleasedEvent, OnMouseUp, RoutingStrategies.Tunnel | RoutingStrategies.Bubble, handledEventsToo: true);
        }

        private float Delta => float.Parse(domainUpDown1.SelectedItem?.ToString() ?? "0.01", NumberStyles.Any, CultureInfo.InvariantCulture);

        private void OnChangeValue(Slider slider)
        {
            if (sliders.All(s => (int)s.Value == 100))
                return;

            var index = (int)slider.Tag;
            var value = ((int)slider.Value - 100) * 0.01f;
            var text = labels[index].Text.Split(' ');
            labels[index].Text = text[0] + " " + value.ToString("0.##", CultureInfo.InvariantCulture);
            if (index == 0)
                plane.X = value;
            else if (index == 1)
                plane.Y = value;
            else
                plane.Z = value;
            if (!PreventRedraw)
            {
                SetClipPlaneEvent?.Invoke(plane);
                RedrawClipPlane?.Invoke();
            }
        }

        private void SetControlsEnabled(bool enabled)
        {
            tableLayoutPanel1.IsEnabled = enabled;

            radioButton7.IsEnabled = enabled;
            radioButton8.IsEnabled = enabled;
            radioButton9.IsEnabled = enabled;

            button2.IsEnabled = enabled && radioButton7.IsChecked != true;

            label6.IsEnabled = enabled && radioButton9.IsChecked == true;
            textBox2.IsEnabled = enabled && radioButton9.IsChecked == true;
        }

        private void OnEnableClipPlane(object sender, RoutedEventArgs e)
        {
            var enabled = checkBox1.IsChecked == true;
            SetControlsEnabled(enabled);

            SwitchOnOff?.Invoke(enabled);
            SetClipPlaneEvent?.Invoke(plane);
            RedrawClipPlane?.Invoke();
        }

        private void OnMouseDown(object sender, PointerPressedEventArgs e)
        {
            if (checkBox1.IsChecked == true)
            {
                MouseLastPos = e.GetPosition(this);
                IsMouseDown = true;
            }
        }

        private void OnMouseUp(object sender, PointerReleasedEventArgs e)
        {
            IsMouseDown = false;
        }

        private void OnMouseMove(object sender, PointerEventArgs e)
        {
            // Как в WinForms (смещение только при нажатой левой кнопке): отпускание вне поля не доходит
            // до OnMouseUp, и без этой проверки D менялось бы при простом наведении на поле.
            if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                IsMouseDown = false;
            if (!IsMouseDown || checkBox1.IsChecked != true)
                return;

            var txtControl = (TextBox)sender;
            var position = e.GetPosition(this);
            var sign = Math.Sign(position.X - MouseLastPos.X);
            MouseLastPos = position;
            if (sign == 0)
                return;

            if (ReferenceEquals(txtControl, textBox1))
            {
                plane.D += sign * Delta;
                txtControl.Text = plane.D.ToString("0.##", CultureInfo.InvariantCulture);
            }
            else
            {
                var temp = float.Parse(txtControl.Text, NumberStyles.Any, CultureInfo.InvariantCulture) + sign * Delta;
                if (temp < 0.01f)
                    return;
                txtControl.Text = temp.ToString("0.##", CultureInfo.InvariantCulture);
                ChangeLayerThickness?.Invoke(temp);
            }
            SetClipPlaneEvent?.Invoke(plane);
            RedrawClipPlane?.Invoke();
        }

        private void OnChoicePlane(object sender, RoutedEventArgs e)
        {
            var rBtn = (RadioButton)sender;
            PreventRedraw = true;
            var values = rBtn.Tag.ToString().Split(' ').Select(int.Parse).ToArray();
            // Как в WinForms, сначала выставляется ненулевой (отличный от 100) ползунок: иначе при переходе,
            // например, с X на Y все ползунки на миг становятся 100, OnChangeValue выходит без обновления
            // и в плоскости остаётся старая компонента (нормаль (1, 1, 0) вместо (0, 1, 0)), а подпись — старое значение.
            foreach (var i in Enumerable.Range(0, sliders.Length).OrderBy(i => values[i] == 100))
                sliders[i].Value = values[i];
            PreventRedraw = false;

            SetClipPlaneEvent?.Invoke(plane);
            RedrawClipPlane?.Invoke();
        }

        private void OnResetShifting(object sender, RoutedEventArgs e)
        {
            plane.D = 0;
            textBox1.Text = "0";
            SetClipPlaneEvent?.Invoke(plane);
            RedrawClipPlane?.Invoke();
        }

        private void OnChangeDrawMode(object sender, RoutedEventArgs e)
        {
            label6.IsEnabled = radioButton9.IsChecked == true;
            textBox2.IsEnabled = radioButton9.IsChecked == true;
            button2.IsEnabled = radioButton7.IsChecked != true;

            Regime = ClipMode.Default;
            if (ReferenceEquals(sender, radioButton8))
                Regime = ClipMode.KeepElement;
            else if (ReferenceEquals(sender, radioButton9))
                Regime = ClipMode.Layered;

            ChangeClipMode?.Invoke(Regime);
            RedrawClipPlane?.Invoke();
        }
    }
}
