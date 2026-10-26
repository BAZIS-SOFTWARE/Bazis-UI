using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using System.Collections;
using System.Data;

namespace BazisAvaloniaGUI.Tests;

public partial class ResourceLifetimeTests
{
    [TestCase(false, false)]
    [TestCase(true, false)]
    [TestCase(false, true)]
    [TestCase(true, true)]
    public void HardeningCalculate_RequiresPhaseAndCalculatesBothTemperatureModes(bool fixedTemperature, bool selectPhase)
    {
        var (page, tree, materials) = CreateMaterialsPage();
        // Для проверки успешного расчёта используем согласованные таблицы свойств.
        var material = tree.Items.Cast<TreeViewItem>().First(node =>
        {
            var properties = materials[(string)node.Header]["Механические свойства"].PropertyData;
            return properties["Предел текучести"].DataTable.Rows.Count > 1
                && properties["Предел текучести"].DataTable.Rows.Count == properties["Предел прочности"].DataTable.Rows.Count
                && properties["Предел текучести"].DataTable.Rows.Count == properties["Коэффициент упрочнения"].DataTable.Rows.Count;
        });
        var category = MaterialCategory(material, "Механические свойства");
        tree.SelectedItem = category;
        ClickDatabaseMenu(category, 0);
        var windows = (IList)page.GetType().BaseType.GetProperty("AuxiliaryWindows", InstanceMembers).GetValue(page);
        var window = (Window)windows[0];
        try
        {
            var control = (Control)window.Content;
            var selector = (ComboBox)control.GetType().GetProperty("PhaseSelector", InstanceMembers).GetValue(control);
            Assert.That(selector.SelectedItem, Is.Null);
            if (selectPhase) selector.SelectedIndex = 0;
            var temperature = (TextBox)control.GetType().GetProperty("TemperatureInput", InstanceMembers).GetValue(control);
            var table = materials[(string)material.Header]["Механические свойства"]["Предел текучести"].DataTable;
            var midpoint = (Convert.ToSingle(table.Rows[0][0]) + Convert.ToSingle(table.Rows[1][0])) / 2;
            temperature.Text = midpoint.ToString(System.Globalization.CultureInfo.InvariantCulture);
            var checkbox = (CheckBox)control.GetType().GetProperty("TemperatureEnabled", InstanceMembers).GetValue(control);
            checkbox.IsChecked = fixedTemperature;
            var calculate = ((Grid)((UserControl)control).Content).Children.OfType<Button>().Single();
            calculate.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            var graph = control.GetType().GetProperty("Graph", InstanceMembers).GetValue(control);
            var curves = ((IEnumerable)graph.GetType().GetProperty("CurrentData").GetValue(graph)).Cast<object>().ToArray();
            if (!selectPhase)
            {
                Assert.That(curves, Is.Empty);
                var dialog = window.OwnedWindows.Single();
                Assert.That(dialog.GetVisualDescendants().OfType<TextBlock>().Select(t => t.Text),
                    Does.Contain("Select a phase before calculating hardening.").Or.Contain("Выберите фазу перед расчётом упрочнения."));
            }
            else
            {
                Assert.That(window.OwnedWindows, Is.Empty, string.Join("; ", window.OwnedWindows.SelectMany(w => w.GetVisualDescendants().OfType<TextBlock>()).Select(t => t.Text)));
                Assert.That(curves.Length, Is.EqualTo(fixedTemperature ? 1 : table.Rows.Count));
                foreach (var curve in curves)
                {
                    var points = ((IEnumerable)curve.GetType().GetProperty("Points").GetValue(curve)).Cast<object>().ToArray();
                    Assert.That(points.Length, Is.GreaterThan(1));
                    Assert.That(points.All(point => float.IsFinite((float)point.GetType().GetProperty("Y").GetValue(point))), Is.True);
                }
            }
        }
        finally
        {
            foreach (var dialog in window.OwnedWindows.ToArray()) dialog.Close();
            window.Close();
        }
    }
}
