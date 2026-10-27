using Avalonia;
using global::Avalonia.Controls;
using global::Avalonia.Interactivity;
using global::Avalonia.Layout;
using BazisGUI.Avalonia.Localization;
using System;
using System.Globalization;
using System.Numerics;
using System.Resources;

namespace BazisGUI.Avalonia.CrossSection
{
    public class CreatePlaneFromTextArgs
    {
        public Vector3 point1 { get; }
        public Vector3 point2 { get; }
        public Vector3 point3 { get; }

        public CreatePlaneFromTextArgs(Vector3 point1, Vector3 point2, Vector3 point3)
        {
            this.point1 = point1;
            this.point2 = point2;
            this.point3 = point3;
        }
    }

    /// <summary>
    /// Avalonia-аналог CrossSectionControl: сечение модели плоскостью по трём точкам
    /// (координатная плоскость, ввод координат или три выбранных узла).
    /// </summary>
    internal sealed class CrossSectionControl : UserControl
    {
        public event Action<object, CreatePlaneFromTextArgs> CreateCrossFromTextArgs;
        public event Action CreateCrossFromNodesEvent;
        public event Action RemoveCrossEvent;
        public event Action SelectNodesEvent;
        /// <summary>Ошибка ввода координат — сообщение выводит оболочка.</summary>
        public event Action<string> ErrorReported;

        private readonly TextBox txbPoint1;
        private readonly TextBox txbPoint2;
        private readonly TextBox txbPoint3;
        private readonly RadioButton rbtXY;
        private readonly RadioButton rbtXZ;
        private readonly RadioButton rbtYZ;
        private readonly CheckBox chbSelectPoints;

        public CrossSectionControl()
        {
            var resources = new ResourceManager(typeof(CrossSectionControl));
            string Text(string name) => resources.GetString(name + ".Text");

            txbPoint1 = new TextBox { Name = "txbPoint1", Text = Text("txbPoint1") };
            txbPoint2 = new TextBox { Name = "txbPoint2", Text = Text("txbPoint2") };
            txbPoint3 = new TextBox { Name = "txbPoint3", Text = Text("txbPoint3") };

            rbtXY = new RadioButton { Name = "rbtXY", Content = Text("rbtXY"), GroupName = "crossPlane" };
            rbtXZ = new RadioButton { Name = "rbtXZ", Content = Text("rbtXZ"), GroupName = "crossPlane" };
            rbtYZ = new RadioButton { Name = "rbtYZ", Content = Text("rbtYZ"), GroupName = "crossPlane" };
            rbtXY.IsCheckedChanged += (_, _) => { if (rbtXY.IsChecked == true) SetPoints("0;0;0", "1;0;0", "0;1;0"); };
            rbtXZ.IsCheckedChanged += (_, _) => { if (rbtXZ.IsChecked == true) SetPoints("0;0;0", "1;0;0", "0;0;1"); };
            rbtYZ.IsCheckedChanged += (_, _) => { if (rbtYZ.IsChecked == true) SetPoints("0;0;0", "0;1;0", "0;0;1"); };

            chbSelectPoints = new CheckBox { Name = "chbSelectPoints", Content = Text("chbSelectPoints") };
            chbSelectPoints.IsCheckedChanged += chbSelectPoints_CheckedChanged;

            var btnCreateCross = new Button { Name = "btnCreateCross", Content = Text("btnCreateCross") };
            btnCreateCross.Click += btnCreatePlane_Click;
            var btnRemoveCross = new Button { Name = "btnRemoveCross", Content = Text("btnRemoveCross") };
            btnRemoveCross.Click += (_, _) => RemoveCrossEvent?.Invoke();

            var points = new Grid { ColumnDefinitions = new ColumnDefinitions("Auto,*"), RowDefinitions = new RowDefinitions("Auto,Auto,Auto"), RowSpacing = 4, ColumnSpacing = 6 };
            var rows = new[] { (Text("label1"), txbPoint1), (Text("label2"), txbPoint2), (Text("label3"), txbPoint3) };
            for (var i = 0; i < rows.Length; i++)
            {
                var label = new TextBlock { Text = rows[i].Item1, VerticalAlignment = VerticalAlignment.Center };
                Grid.SetRow(label, i);
                Grid.SetRow(rows[i].Item2, i);
                Grid.SetColumn(rows[i].Item2, 1);
                points.Children.Add(label);
                points.Children.Add(rows[i].Item2);
            }

            Content = new StackPanel
            {
                Spacing = 6,
                Margin = new Thickness(10),
                MinWidth = 250,
                Children =
                {
                    new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Children = { rbtXY, rbtXZ, rbtYZ } },
                    points,
                    chbSelectPoints,
                    new StackPanel { Orientation = Orientation.Horizontal, Spacing = 6, HorizontalAlignment = HorizontalAlignment.Right, Children = { btnCreateCross, btnRemoveCross } }
                }
            };
        }

        private void SetPoints(string p1, string p2, string p3)
        {
            txbPoint1.Text = p1;
            txbPoint2.Text = p2;
            txbPoint3.Text = p3;
        }

        private void chbSelectPoints_CheckedChanged(object sender, RoutedEventArgs e)
        {
            var manual = chbSelectPoints.IsChecked != true;
            txbPoint1.IsEnabled = manual;
            txbPoint2.IsEnabled = manual;
            txbPoint3.IsEnabled = manual;
            rbtXY.IsEnabled = manual;
            rbtXZ.IsEnabled = manual;
            rbtYZ.IsEnabled = manual;

            if (!manual)
                SelectNodesEvent?.Invoke();
        }

        private void btnCreatePlane_Click(object sender, RoutedEventArgs e)
        {
            if (chbSelectPoints.IsChecked == true)
            {
                CreateCrossFromNodesEvent?.Invoke();
                return;
            }

            try
            {
                var point1 = Parse(txbPoint1.Text);
                var point2 = Parse(txbPoint2.Text);
                var point3 = Parse(txbPoint3.Text);

                // Сравниваются координаты, а не строки: "0;0;0" и "0.0; 0; 0" — одна точка.
                // Совпадающие или лежащие на одной прямой точки плоскость не задают (нулевая нормаль).
                if (Vector3.Cross(point2 - point1, point3 - point1) == Vector3.Zero)
                    throw new Exception(Localization.Resources.CrossSectionControl_InvalidSurfaceCoordsSetException);

                CreateCrossFromTextArgs?.Invoke(this, new CreatePlaneFromTextArgs(point1, point2, point3));
            }
            catch (Exception ex)
            {
                ErrorReported?.Invoke(ex.Message);
            }
        }

        private static Vector3 Parse(string text)
        {
            // Пустое поле или меньше трёх координат — сообщение о неверном задании точек вместо NullReference/IndexOutOfRange.
            var p = (text ?? string.Empty).Split(';');
            if (p.Length < 3)
                throw new FormatException(Localization.Resources.CrossSectionControl_InvalidSurfaceCoordsSetException);
            float Coordinate(int index) => float.Parse(p[index].Trim().Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture);
            return new Vector3(Coordinate(0), Coordinate(1), Coordinate(2));
        }
    }
}
