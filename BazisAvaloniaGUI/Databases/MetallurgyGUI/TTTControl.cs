using Avalonia;
using Avalonia.Controls;
using Avalonia.Automation;
using Avalonia.Layout;
using System;
using System.Collections.Generic;

namespace BazisAvaloniaGUI.Databases.MetallurgyGUI
{
    internal sealed class TTTControl : UserControl
    {
        private readonly TextBox txbFinTemp = new();
        private readonly TextBox txbIniTemp = new();
        private readonly TextBox txbMaxPhase = new();
        private readonly TextBox txbMinPhase = new();
        private readonly NumericUpDown nudTemps = new() { Minimum = 2, Value = 10 };
        private readonly ComboBox cmbPhases = new();
        private readonly TextBox txbMaxTime = new();

        public TTTControl()
        {
            txbFinTemp.Text = DatabaseControlResources.Get("TTTControl", "txbFinTemp.Text");
            txbIniTemp.Text = DatabaseControlResources.Get("TTTControl", "txbIniTemp.Text");
            txbMaxPhase.Text = DatabaseControlResources.Get("TTTControl", "txbMaxPhase.Text");
            txbMinPhase.Text = DatabaseControlResources.Get("TTTControl", "txbMinPhase.Text");
            txbMaxTime.Text = DatabaseControlResources.Get("TTTControl", "txbMaxTime.Text");
            AutomationProperties.SetName(cmbPhases, DatabaseControlResources.Get("TTTControl", "cmbPhases.AccessibleName"));
            AutomationProperties.SetName(txbMaxTime, DatabaseControlResources.Get("TTTControl", "txbMaxTime.AccessibleName"));
            var panel = new StackPanel { Spacing = 6 };
            panel.Children.Add(Row(DatabaseControlResources.Get("TTTControl", "label1.Text"), txbMinPhase));
            panel.Children.Add(Row(DatabaseControlResources.Get("TTTControl", "label2.Text"), txbMaxPhase));
            panel.Children.Add(Row(DatabaseControlResources.Get("TTTControl", "label4.Text"), txbFinTemp));
            panel.Children.Add(Row(DatabaseControlResources.Get("TTTControl", "label3.Text"), txbIniTemp));
            panel.Children.Add(Row(DatabaseControlResources.Get("TTTControl", "label7.Text"), nudTemps));
            panel.Children.Add(Row(DatabaseControlResources.Get("TTTControl", "label5.Text"), cmbPhases));
            panel.Children.Add(Row(DatabaseControlResources.Get("TTTControl", "label6.Text"), txbMaxTime));
            Content = panel;
        }

        internal ComboBox PhaseSelector => cmbPhases;
        internal TextBox MinimumPhaseInput => txbMinPhase;
        internal TextBox MaximumPhaseInput => txbMaxPhase;
        internal TextBox InitialTemperatureInput => txbIniTemp;
        internal TextBox FinalTemperatureInput => txbFinTemp;
        internal TextBox MaximumTimeInput => txbMaxTime;
        internal NumericUpDown TemperatureQuantity => nudTemps;

        public void AddPhase(string phaseName)
        {
            if (!cmbPhases.Items.Contains(phaseName))
                cmbPhases.Items.Add(phaseName);
        }

        public IEnumerable<string> GetPhases()
        {
            foreach (var item in cmbPhases.Items)
                yield return (string)item;
        }

        public string InitialPhase => cmbPhases.SelectedItem?.ToString() ?? "";
        public float IniTemp => DatabaseNumber.ParseFloat(txbIniTemp.Text);
        public float FinTemp => DatabaseNumber.ParseFloat(txbFinTemp.Text);
        public float MaxPhase => DatabaseNumber.ParseFloat(txbMaxPhase.Text);
        public float MinPhase => DatabaseNumber.ParseFloat(txbMinPhase.Text);
        public float MaxTime => DatabaseNumber.ParseFloat(txbMaxTime.Text);

        private static StackPanel Row(string label, Control control)
        {
            var row = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 6 };
            row.Children.Add(new TextBlock { Text = label, Width = 120, VerticalAlignment = VerticalAlignment.Center });
            row.Children.Add(control);
            return row;
        }
    }
}
