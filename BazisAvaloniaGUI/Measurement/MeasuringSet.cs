using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.Styling;
using Model.Interfaces;
using System;
using System.Resources;

namespace BazisAvaloniaGUI.Measurement
{
    /// <summary>Avalonia-аналог MeasuringSet: выбор вида измерения и запуск измерения.</summary>
    internal sealed class MeasuringSet : UserControl
    {

        public event Action<object, MeasureEventArgs> MakeMeasureEvent;
        public event Action<ObjType> PreparingMeasureEvent;

        MeasureKind measureKind;

        private RadioButton rbtnDistance;
        private RadioButton rbtnPath;
        private RadioButton rbtSquare;
        private RadioButton rbtVolume;
        private ComboBox cmbMeasureObjects;
        private Button btnMeasure;

        public MeasuringSet()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            var resources = new ResourceManager(typeof(MeasuringSet));
            Styles.Add(new StyleInclude(new Uri("avares://BazisAvaloniaGUI/")) { Source = new Uri("avares://BazisAvaloniaGUI/Databases/DatabaseStyles.axaml") });

            rbtnDistance = Radio(resources, "rbtnDistance");
            rbtnPath = Radio(resources, "rbtnPath");
            rbtSquare = Radio(resources, "rbtSquare");
            rbtVolume = Radio(resources, "rbtVolume");

            cmbMeasureObjects = new ComboBox
            {
                Name = "cmbMeasureObjects",
                Height = 20,
                MinHeight = 20,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                IsEnabled = false,
                ItemsSource = new[] { resources.GetString("cmbMeasureObjects.Items"), resources.GetString("cmbMeasureObjects.Items1") }
            };
            cmbMeasureObjects.SelectionChanged += cmbMeasureObjects_SelectedIndexChanged;

            btnMeasure = new Button { Name = "btnMeasure", Content = resources.GetString("btnMeasure.Text"), HorizontalAlignment = HorizontalAlignment.Stretch, HorizontalContentAlignment = HorizontalAlignment.Center };
            btnMeasure.Click += btnMeasure_Click;

            // tableLayoutPanel1: слева панель переключателей, справа список и кнопка измерения.
            var panel1 = new StackPanel { Name = "panel1", Spacing = 10, Margin = new Thickness(12, 15, 4, 4) };
            panel1.Children.Add(rbtnDistance);
            panel1.Children.Add(rbtVolume);
            panel1.Children.Add(rbtSquare);
            panel1.Children.Add(rbtnPath);

            var tableLayoutPanel1 = new Grid { Name = "tableLayoutPanel1", ColumnDefinitions = new ColumnDefinitions("*,*"), RowDefinitions = new RowDefinitions("*,Auto"), Margin = new Thickness(0, 0, 4, 4) };
            Grid.SetRowSpan(panel1, 2);
            tableLayoutPanel1.Children.Add(panel1);
            cmbMeasureObjects.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetColumn(cmbMeasureObjects, 1);
            tableLayoutPanel1.Children.Add(cmbMeasureObjects);
            Grid.SetColumn(btnMeasure, 1);
            Grid.SetRow(btnMeasure, 1);
            tableLayoutPanel1.Children.Add(btnMeasure);

            MinWidth = 340;
            MinHeight = 198;
            Content = tableLayoutPanel1;
        }

        private RadioButton Radio(ResourceManager resources, string name)
        {
            var radio = new RadioButton { Name = name, GroupName = "MeasureKind", Content = resources.GetString(name + ".Text") };
            radio.Classes.Add("database-toggle");
            radio.Click += Rbtn_Click;
            return radio;
        }

        private void Rbtn_Click(object sender, RoutedEventArgs e)
        {
            if (rbtVolume.IsChecked == true)
            {
                measureKind = MeasureKind.Volume;
                cmbMeasureObjects.IsEnabled = false;
                PreparingMeasureEvent?.Invoke(ObjType.Элемент3D);
            }

            else if (rbtSquare.IsChecked == true)
            {
                measureKind = MeasureKind.Square;
                cmbMeasureObjects.IsEnabled = false;
                PreparingMeasureEvent?.Invoke(ObjType.Элемент2D);
            }

            else if (rbtnPath.IsChecked == true)
            {
                measureKind = MeasureKind.Path;
                cmbMeasureObjects.IsEnabled = false;
                PreparingMeasureEvent?.Invoke(ObjType.Узел);
            }

            else
            {
                cmbMeasureObjects.IsEnabled = true;
                measureKind = MeasureKind.DistancePointToPoint;
                cmbMeasureObjects.SelectedIndex = 0;
                PreparingMeasureEvent?.Invoke(ObjType.Узел);
            }

        }

        private void btnMeasure_Click(object sender, RoutedEventArgs e)
        {
            MakeMeasureEvent(this, new MeasureEventArgs(measureKind));
        }

        private void cmbMeasureObjects_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbMeasureObjects.SelectedIndex == 0)
                measureKind = MeasureKind.DistancePointToPoint;
            else if (cmbMeasureObjects.SelectedIndex == 1)
                measureKind = MeasureKind.DistancePointToPlane;
        }
    }
}
