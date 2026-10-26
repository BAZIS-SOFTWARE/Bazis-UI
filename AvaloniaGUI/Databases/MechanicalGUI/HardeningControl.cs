using Avalonia;
using Avalonia.Controls;
using Avalonia.Automation;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.Styling;
using AvaloniaGUI.Localization;
using MaterialDB.MaterialData;
using MaterialDB.Utilities;
using PropertiesCalculator.PropertiesCalculator.MechanicalModels;
using PropertiesCalculator.PropertiesController.Interfaces;
using PropertiesCalculator.PropertiesController.MechanicalModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AvaloniaGUI.Databases.MechanicalGUI
{
    internal sealed class HardeningControl : UserControl
    {
        private readonly DataTable yieldTable;
        private readonly DataTable ultimateTable;
        private readonly DataTable hardKoeffTable;
        private readonly int hardModel;
        private readonly IHardeningModel<float> hardeningModel;
        private readonly ComboBox cmbPhases = new();
        private readonly TextBox txbTemp = new() { IsEnabled = false };
        private readonly CheckBox chbTemp = new();
        private readonly GraphContainer graphContainer = new();

        private new class Resources : Localization.Resources { }
        internal GraphContainer Graph => graphContainer;
        internal ComboBox PhaseSelector => cmbPhases;
        internal TextBox TemperatureInput => txbTemp;
        internal CheckBox TemperatureEnabled => chbTemp;

        public HardeningControl(PropertyData mechProps, PropertyData genProps)
        {
            Styles.Add(new StyleInclude(new Uri("avares://AvaloniaGUI/")) { Source = new Uri("avares://AvaloniaGUI/Databases/DatabaseStyles.axaml") });
            chbTemp.Classes.Add("database-toggle");
            yieldTable = mechProps["Предел текучести"].DataTable;
            ultimateTable = mechProps["Предел прочности"].DataTable;
            hardKoeffTable = mechProps["Коэффициент упрочнения"].DataTable;
            var modelNumberTable = genProps["Модель упрочнения"].DataTable;
            hardModel = Convert.ToInt32(modelNumberTable.Rows[0]["Модель упрочнения"]);
            var phaseTable = genProps["Структура"].DataTable;

            foreach (var phase in phaseTable.AsEnumerable().Select(r => r.Field<string>(0)))
                cmbPhases.Items.Add(phase);

            hardeningModel = hardModel == 1 ? new LinearHardeningModel() : new ExponentialHardeningModel();
            chbTemp.Content = DatabaseControlResources.Get("HardeningControl", "chbTemp.Text");
            AutomationProperties.SetName(txbTemp, DatabaseControlResources.Get("HardeningControl", "txbTemp.AccessibleName"));
            AutomationProperties.SetName(cmbPhases, DatabaseControlResources.Get("HardeningControl", "cmbPhases.AccessibleName"));
            chbTemp.IsCheckedChanged += chbTemp_CheckedChanged;

            var settings = new StackPanel { Spacing = 6 };
            settings.Children.Add(chbTemp);
            settings.Children.Add(Row(DatabaseControlResources.Get("HardeningControl", "label1.Text"), cmbPhases));
            settings.Children.Add(Row(DatabaseControlResources.Get("HardeningControl", "label2.Text"), txbTemp));
            var calculate = new Button { Content = DatabaseControlResources.Get("HardeningControl", "btnCalc.Text"), HorizontalAlignment = HorizontalAlignment.Stretch };
            calculate.Click += btnCalc_Click;

            var layout = new Grid { ColumnDefinitions = new ColumnDefinitions("*,220") };
            layout.Children.Add(graphContainer);
            Grid.SetColumn(settings, 1);
            layout.Children.Add(settings);
            Grid.SetColumn(calculate, 1);
            Grid.SetRow(calculate, 1);
            layout.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            layout.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            layout.Children.Add(calculate);
            Content = layout;
        }

        internal List<GraphData> CaclHardeningForTemp()
        {
            var temps = yieldTable.AsEnumerable().Select(r => Convert.ToSingle(r[0])).ToList();
            var temp = DatabaseNumber.ParseFloat(txbTemp.Text);
            // FindIndex вместо Find: Find при температуре выше таблицы возвращал default(float) = 0,
            // и интерполяция молча шла по строке с 0 °C (или падала с индексом -2)
            var temp1_index = temps.FindIndex(x => Math.Abs(x - temp) < 1e-4 || x > temp);
            if (temps.Count < 2 || temp1_index < 0 || temp < temps[0] - 1e-4)
                throw new ArgumentOutOfRangeException(nameof(temp), temp, "Температура вне диапазона таблицы свойств");
            // Температура, равная первой в таблице: интерполяция по первому интервалу (раньше индекс -1)
            temp1_index = Math.Max(temp1_index, 1);
            var temp1 = temps[temp1_index];
            var temp0_index = temp1_index - 1;
            var temp0 = temps[temp0_index];

            GetPhysicalData(temp0_index, out var st0, out var su0, out var m0);
            GetPhysicalData(temp1_index, out var st1, out var su1, out var m1);
            var deps = 1.0f / yieldTable.Rows.Count;
            var st = InterpolationSearch.InterpolatedValue(new[] { temp0, temp1 }, new[] { st0, st1 }, temp);
            var su = InterpolationSearch.InterpolatedValue(new[] { temp0, temp1 }, new[] { su0, su1 }, temp);
            var m = InterpolationSearch.InterpolatedValue(new[] { temp0, temp1 }, new[] { m0, m1 }, temp);
            var points = CalcHardnessForStructure(st, su, m, deps, hardModel);

            return [new GraphData("Упрочнение", System.Drawing.Color.Orange, "", "", points.ToArray())];
        }

        internal List<GraphData> CaclHardeningForTemps()
        {
            var grDataRange = new List<GraphData>();
            for (var i = 0; i < yieldTable.Rows.Count; i++)
            {
                var st = Convert.ToSingle(yieldTable.Rows[i][cmbPhases.SelectedItem.ToString()]);
                var sr = Convert.ToSingle(ultimateTable.Rows[i][cmbPhases.SelectedItem.ToString()]);
                var m = Convert.ToSingle(hardKoeffTable.Rows[i][cmbPhases.SelectedItem.ToString()]);
                var points = CalcHardnessForStructure(st, sr, m, 1.0f / yieldTable.Rows.Count, hardModel);
                grDataRange.Add(new GraphData("Упрочнение", System.Drawing.Color.Orange, "", "", points.ToArray()));
            }
            return grDataRange;
        }

        private void GetPhysicalData(int temp_index, out float st, out float su, out float m)
        {
            st = Convert.ToSingle(yieldTable.Rows[temp_index][cmbPhases.SelectedItem.ToString()]);
            su = Convert.ToSingle(ultimateTable.Rows[temp_index][cmbPhases.SelectedItem.ToString()]);
            m = Convert.ToSingle(hardKoeffTable.Rows[temp_index][cmbPhases.SelectedItem.ToString()]);
        }

        internal List<GraphPoint> CalcHardnessForStructure(float st, float sr, float m, float d_eps, int modelNumber)
        {
            var points = new List<GraphPoint>();
            var eps = 0.0f;
            while (eps <= 1)
            {
                points.Add(new GraphPoint(eps, st + hardeningModel.Calc(st, m, sr, eps)));
                eps += d_eps;
            }
            return points;
        }

        private async void btnCalc_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            try
            {
                if (cmbPhases.SelectedItem == null)
                {
                    await MessageBox.Show(this, DatabaseControlResources.Get("HardeningControl", "SelectPhase"));
                    return;
                }
                var data = chbTemp.IsChecked == true ? CaclHardeningForTemp() : CaclHardeningForTemps();
                graphContainer.CreateGraphData("Упрочнение", data, new AxisFormat(), new AxisFormat());
            }
            catch (Exception ex)
            {
                await MessageBox.Show(this, ex.Message);
            }
        }

        private void chbTemp_CheckedChanged(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            txbTemp.IsEnabled = chbTemp.IsChecked == true;
        }

        private static StackPanel Row(string label, Control control)
        {
            var row = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 6 };
            row.Children.Add(new TextBlock { Text = label, Width = 90, VerticalAlignment = VerticalAlignment.Center });
            row.Children.Add(control);
            return row;
        }
    }
}
