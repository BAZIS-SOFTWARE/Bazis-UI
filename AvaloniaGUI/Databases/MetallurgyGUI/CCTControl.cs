using Avalonia;
using Avalonia.Controls;
using Avalonia.Automation;
using Avalonia.Layout;
using System;
using System.Collections.Generic;

namespace AvaloniaGUI.Databases.MetallurgyGUI
{
    internal sealed class CCTControl : UserControl
    {
        private readonly TextBox txbMinVel = new();
        private readonly TextBox txbMaxVel = new();
        private readonly NumericUpDown nudVels = new() { Minimum = 2, Value = 10 };
        private readonly ComboBox cmbPhases = new();
        private readonly TextBox txbIniTemp = new();
        private readonly TextBox txbFinTemp = new();

        public CCTControl()
        {
            txbMinVel.Text = DatabaseControlResources.Get("CCTControl", "txbMinVel.Text");
            txbMaxVel.Text = DatabaseControlResources.Get("CCTControl", "txbMaxVel.Text");
            txbIniTemp.Text = DatabaseControlResources.Get("CCTControl", "txbIniTemp.Text");
            txbFinTemp.Text = DatabaseControlResources.Get("CCTControl", "txbFinTemp.Text");
            AutomationProperties.SetName(cmbPhases, DatabaseControlResources.Get("CCTControl", "cmbPhases.AccessibleName"));
            var panel = new StackPanel { Spacing = 6 };
            panel.Children.Add(Row(DatabaseControlResources.Get("CCTControl", "label1.Text"), txbMinVel));
            panel.Children.Add(Row(DatabaseControlResources.Get("CCTControl", "label2.Text"), txbMaxVel));
            panel.Children.Add(Row(DatabaseControlResources.Get("CCTControl", "label3.Text"), nudVels));
            panel.Children.Add(Row(DatabaseControlResources.Get("CCTControl", "label4.Text"), cmbPhases));
            panel.Children.Add(Row(DatabaseControlResources.Get("CCTControl", "label5.Text"), txbIniTemp));
            panel.Children.Add(Row(DatabaseControlResources.Get("CCTControl", "label6.Text"), txbFinTemp));
            Content = panel;
        }

        internal ComboBox PhaseSelector => cmbPhases;
        internal TextBox MinimumVelocityInput => txbMinVel;
        internal TextBox MaximumVelocityInput => txbMaxVel;
        internal TextBox InitialTemperatureInput => txbIniTemp;
        internal TextBox FinalTemperatureInput => txbFinTemp;
        internal NumericUpDown VelocityQuantity => nudVels;

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
        public float MaxVel => DatabaseNumber.ParseFloat(txbMaxVel.Text);
        public float MinVel => DatabaseNumber.ParseFloat(txbMinVel.Text);
        public decimal VelNumber => nudVels.Value ?? 0;

        private static StackPanel Row(string label, Control control)
        {
            var row = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 6 };
            row.Children.Add(new TextBlock { Text = label, Width = 120, VerticalAlignment = VerticalAlignment.Center });
            row.Children.Add(control);
            return row;
        }
    }
}
