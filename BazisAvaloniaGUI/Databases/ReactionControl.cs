using Avalonia;
using Avalonia.Automation;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.Styling;
using MaterialDB.MaterialData;
using System;
using System.Data;
using System.Text.RegularExpressions;

namespace BazisAvaloniaGUI.Databases;

internal sealed class ReactionControl : UserControl
{
    private new class Resources : Localization.Resources { }
    private readonly Property reaction;
    internal ComboBox InitialPhase { get; } = new() { Name = "InitialPhase", MinWidth = 100, MinHeight = 20 };
    internal ComboBox FinalPhase { get; } = new() { Name = "FinalPhase", MinWidth = 100, MinHeight = 20 };
    internal ComboBox PhaseName { get; } = new() { Name = "PhaseName", MinWidth = 100, MinHeight = 20 };
    internal CheckBox TimeDependent { get; } = new() { Name = "TimeDependent" };
    internal TextBox PhaseValue { get; } = new() { Name = "PhaseValue", Text = "1", Height = 20, MinHeight = 20, MinWidth = 80, Padding = new Thickness(2, 0) };
    internal DataTableEditor DataGridView { get; } = new();
    private readonly Button add = new() { Content = "+", Name = "AddPhaseValue" };
    private readonly Button change = new() { Content = "↔", Name = "ChangePhaseValue" };
    private readonly Button delete = new() { Content = "−", Name = "DeletePhaseValue" };
    public event Action<string, string> ChangeReactionName;

    public ReactionControl(string[] phaseNames, Property reaction)
    {
        Styles.Add(new StyleInclude(new Uri("avares://BazisAvaloniaGUI/")) { Source = new Uri("avares://BazisAvaloniaGUI/Databases/DatabaseStyles.axaml") });
        TimeDependent.Classes.Add("database-toggle");
        this.reaction = reaction;
        foreach (var phase in phaseNames) { InitialPhase.Items.Add(phase); FinalPhase.Items.Add(phase); }
        foreach (var kind in new[] { "Охлаждение", "Нагрев", "Выдержка" }) PhaseName.Items.Add(kind);
        var reacData = reaction.Name.Split(' ');
        var phases = reacData[1].Split('-');
        void Select(ComboBox selector, string value) { if (!selector.Items.Contains(value)) selector.Items.Add(value); selector.SelectedItem = value; }
        Select(PhaseName, reacData[0]); Select(InitialPhase, phases[0]); Select(FinalPhase, phases[1]);
        foreach (var selector in new[] { PhaseName, InitialPhase, FinalPhase })
        {
            AutomationProperties.SetName(selector, selector.Name);
            selector.SelectionChanged += (_, _) => ChangeReactionName?.Invoke(reaction.Name,
                $"{PhaseName.SelectedItem} {InitialPhase.SelectedItem}-{FinalPhase.SelectedItem}");
        }
        TimeDependent.Content = DatabaseControlResources.Get("ReactionControl", "chbTimeDependent.Text");
        TimeDependent.IsChecked = reaction.DataTable.Columns.Count > 2;
        UpdateButtons();
        TimeDependent.Click += chbTimeDependent_Click;
        DataGridView.DataSource = reaction.DataTable;
        DataGridView.ColumnHeaderClick += dgv_ColumnHeaderMouseClick;
        add.Click += btnAddTime_Click; change.Click += btnChange_Click; delete.Click += btnDelTime_Click;
        var settings = new Grid { ColumnDefinitions = new ColumnDefinitions("*,*,*"), RowDefinitions = new RowDefinitions("Auto,Auto,Auto,Auto") };
        var selectors = new[] { PhaseName, InitialPhase, FinalPhase };
        var labels = new[] { "label3.Text", "label1.Text", "label2.Text" };
        for (var i = 0; i < selectors.Length; i++)
        {
            var label = new TextBlock { Text = DatabaseControlResources.Get("ReactionControl", labels[i]), Margin = new Thickness(3) };
            Grid.SetColumn(label, i); settings.Children.Add(label);
            selectors[i].Margin = new Thickness(3); Grid.SetColumn(selectors[i], i); Grid.SetRow(selectors[i], 1); settings.Children.Add(selectors[i]);
        }
        Grid.SetRow(TimeDependent, 2); Grid.SetColumnSpan(TimeDependent, 3); settings.Children.Add(TimeDependent);
        var columnTools = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 4 };
        columnTools.Children.Add(PhaseValue); columnTools.Children.Add(add); columnTools.Children.Add(change); columnTools.Children.Add(delete);
        var addRow = new Button { Content = "+", Name = "AddReactionRow" };
        ToolTip.SetTip(addRow, DatabaseControlResources.Get("DataBasePage", "btnAddNewRow.Text"));
        addRow.Click += (_, _) => reaction.DataTable.Rows.Add(reaction.DataTable.NewRow());
        columnTools.Children.Add(addRow);
        Grid.SetRow(columnTools, 3); Grid.SetColumnSpan(columnTools, 3); settings.Children.Add(columnTools);
        var layout = new Grid { RowDefinitions = new RowDefinitions("Auto,*"), Margin = new Thickness(4) };
        layout.Children.Add(settings); Grid.SetRow(DataGridView, 1); layout.Children.Add(DataGridView); Content = layout;
    }

    internal void btnAddTime_Click(object sender, EventArgs e)
    {
        var regex = new Regex(@"(^([0]\.)(\d{0,3}[1-9])$)|(^[1]$)|(^[0]$)");
        if (!regex.IsMatch(PhaseValue.Text)) { _ = MessageBox.Show(this, Resources.InvalidRegexPhaseMatchWarning, Localization.Localization.GetAttentionCaption()); return; }
        try { reaction.DataTable.Columns.Add(new DataColumn($"Масс.Доли_{PhaseValue.Text}", typeof(float)) { DefaultValue = 0 }); }
        catch (Exception ex) { _ = MessageBox.Show(this, ex.Message); }
    }
    internal void btnChange_Click(object sender, EventArgs e)
    {
        try
        {
            var i = DataGridView.SelectedColumn;
            if (i > 0 && !reaction.DataTable.Columns[i].ColumnName.Split('_')[1].Equals(PhaseValue.Text))
                reaction.DataTable.Columns[i].ColumnName = $"Масс.Доли_{PhaseValue.Text}";
        }
        catch (Exception ex) { _ = MessageBox.Show(this, ex.Message); }
        DataGridView.Refresh();
    }
    private void dgv_ColumnHeaderMouseClick(int index)
    {
        if (index > 0) PhaseValue.Text = reaction.DataTable.Columns[index].ColumnName.Split('_')[1];
    }
    internal void btnDelTime_Click(object sender, EventArgs e)
    {
        if (DataGridView.SelectedColumn > 0) reaction.DataTable.Columns.RemoveAt(DataGridView.SelectedColumn);
        DataGridView.SelectedColumn = -1; DataGridView.Refresh();
    }
    private async void chbTimeDependent_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var result = await MessageBox.Show(this, Resources.EnteredDataDeletingWarning, Localization.Localization.GetAttentionCaption(), MessageBoxButtons.OKCancel);
        if (result == DialogResult.OK) ApplyTimeDependent(TimeDependent.IsChecked == true);
        // CheckBox переключается до Click: при отмене (Cancel/None) возвращаем прежнее состояние,
        // иначе флажок разойдётся с фактическим составом столбцов таблицы.
        else TimeDependent.IsChecked = TimeDependent.IsChecked != true;
    }
    internal void ApplyTimeDependent(bool enabled)
    {
        for (var i = reaction.DataTable.Columns.Count - 1; i > 0; i--) reaction.DataTable.Columns.RemoveAt(i);
        TimeDependent.IsChecked = enabled;
        UpdateButtons();
        if (enabled)
        {
            reaction.DataTable.Columns.Add(new DataColumn("Масс.Доли_0.1", typeof(float)) { DefaultValue = 0 });
            reaction.DataTable.Columns.Add(new DataColumn("Масс.Доли_1", typeof(float)) { DefaultValue = 0 });
        }
        else reaction.DataTable.Columns.Add(new DataColumn("Масс.Доли", typeof(float)) { DefaultValue = 0 });
        DataGridView.SelectedColumn = -1; DataGridView.Refresh();
    }
    private void UpdateButtons() { add.IsEnabled = change.IsEnabled = delete.IsEnabled = TimeDependent.IsChecked == true; }
}
