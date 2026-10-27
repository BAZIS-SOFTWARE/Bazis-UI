using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Themes.Fluent;
using Avalonia.Threading;
using Avalonia.VisualTree;
using MaterialDB.MaterialData;

namespace BazisGUI.Avalonia.Tests;

public partial class ResourceLifetimeTests
{
    private (Control Page, TreeView Tree, MaterialDBData Materials) CreateMaterialsPage()
    {
        if (!Application.Current.Styles.OfType<FluentTheme>().Any()) Application.Current.Styles.Add(new FluentTheme());
        var page = (Control)Create("Databases.MaterialsDataBasePage");
        Call(page, "Load", Path.Combine(AppContext.BaseDirectory, "DataBases", "Materials", "Materials_v6.jsf"), false);
        var tree = (TreeView)page.GetType().BaseType.GetProperty("TreeView", InstanceMembers).GetValue(page);
        var materials = (MaterialDBData)page.GetType().GetProperty("Materials").GetValue(page);
        Assert.That(tree.Items.Count, Is.GreaterThan(0), "The real materials database must load.");
        return (page, tree, materials);
    }

    private sealed class MaterialPageHost : IDisposable
    {
        public Window Window { get; }
        public MaterialPageHost(Control page)
        {
            Window = new Window { Content = page, Width = 1000, Height = 700 };
            Window.Show();
        }
        public void Dispose() => Window.Close();
    }

    private static TreeViewItem MaterialCategory(TreeViewItem material, string name) => material.Items.Cast<TreeViewItem>().Single(n => (string)n.Header == name);
    private static void ClickDatabaseMenu(TreeViewItem node, int index) => ((MenuItem)node.ContextMenu.Items[index]).RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));

    [Test]
    public void MaterialMenu_DoesNotOpenForChildWithoutOwnMenu()
    {
        var (page, tree, _) = CreateMaterialsPage();
        var material = (TreeViewItem)tree.Items[0];
        var category = MaterialCategory(material, "Общие сведения");
        var window = new Window { Content = page, Width = 1000, Height = 700 };
        window.Show();
        try
        {
            material.IsExpanded = true;
            window.UpdateLayout();
            category.RaiseEvent(new ContextRequestedEventArgs());
            Assert.That(material.ContextMenu.IsOpen, Is.False, "A category must not offer deletion/renaming of a material.");
        }
        finally { material.ContextMenu.Close(); window.Close(); }
    }

    [Test]
    public void MaterialMenu_RightClickSelectsTargetBeforeDeleting()
    {
        var (page, tree, materials) = CreateMaterialsPage();
        var first = (TreeViewItem)tree.Items[0];
        var target = (TreeViewItem)tree.Items[1];
        var targetName = (string)target.Header;
        var firstName = (string)first.Header;
        var window = new Window { Content = page, Width = 1000, Height = 700 };
        window.Show();
        try
        {
            tree.SelectedItem = first;
            window.UpdateLayout();
            var text = target.GetVisualDescendants().OfType<TextBlock>().First();
            var point = text.TranslatePoint(new Point(3, 3), window).Value;
            window.MouseDown(point, MouseButton.Right);
            window.MouseUp(point, MouseButton.Right);
            Assert.That(tree.SelectedItem, Is.SameAs(target));
            Assert.That(target.ContextMenu.IsOpen, Is.True);
            ClickDatabaseMenu(target, 1);
            Assert.That(materials.ContainsKey(targetName), Is.False);
            Assert.That(materials.ContainsKey(firstName), Is.True);
            Assert.That(tree.Items.Contains(target), Is.False);
        }
        finally { target.ContextMenu.Close(); window.Close(); }
    }

    [Test]
    public void MaterialMenu_AddEditDeleteReactionKeepsDatabaseAndTreeInSync()
    {
        var (page, tree, materials) = CreateMaterialsPage();
        var material = (TreeViewItem)tree.Items[0];
        var category = MaterialCategory(material, "Металлургия");
        var properties = materials[(string)material.Header]["Металлургия"].PropertyData;
        using var host = new MaterialPageHost(page);
        material.IsExpanded = true;
        category.IsExpanded = true;
        host.Window.UpdateLayout();
        var originalCount = properties.Count;
        tree.SelectedItem = category;
        ClickDatabaseMenu(category, 0);
        Assert.That(properties.Count, Is.EqualTo(originalCount + 1));
        var reactionNode = (TreeViewItem)category.Items[^1];
        tree.SelectedItem = reactionNode;
        ClickDatabaseMenu(reactionNode, 1);
        var windows = (System.Collections.IList)page.GetType().BaseType.GetProperty("AuxiliaryWindows", InstanceMembers).GetValue(page);
        var editor = (Window)windows[0];
        try
        {
            var control = editor.Content;
            var selector = (ComboBox)control.GetType().GetProperty("PhaseName", InstanceMembers).GetValue(control);
            selector.SelectedItem = "Нагрев";
            Assert.That(properties.ContainsKey("Реакция R1-R2"), Is.False);
            Assert.That(properties.ContainsKey("Нагрев R1-R2"), Is.True);
            Assert.That((string)reactionNode.Header, Does.StartWith("Нагрев R1-R2,"));
        }
        finally { editor.Close(); }
        Dispatcher.UIThread.RunJobs();
        ClickDatabaseMenu(reactionNode, 0);
        Dispatcher.UIThread.RunJobs();
        Assert.That(properties.Count, Is.EqualTo(originalCount));
        Assert.That(category.Items.Contains(reactionNode), Is.False);
        var grid = page.GetType().BaseType.GetProperty("DataGridView").GetValue(page);
        Assert.That(grid.GetType().GetProperty("DataSource").GetValue(grid), Is.Null);
    }

    [TestCase("renamed material", false)]
    [TestCase("", false)]
    [TestCase("renamed material", true)]
    public void MaterialMenu_RenameHonorsValidationAndCancellation(string newName, bool cancel)
    {
        var (page, tree, materials) = CreateMaterialsPage();
        using var host = new MaterialPageHost(page);
        var material = (TreeViewItem)tree.Items[0];
        var oldName = (string)material.Header;
        var original = materials[oldName];
        tree.SelectedItem = material;
        ClickDatabaseMenu(material, 0);
        var input = (TextBox)material.Header;
        input.Text = newName;
        input.RaiseEvent(new KeyEventArgs { RoutedEvent = InputElement.KeyDownEvent, Key = cancel ? Key.Escape : Key.Enter });
        var expectedName = cancel || string.IsNullOrWhiteSpace(newName) ? oldName : newName;
        Assert.That(material.Header, Is.EqualTo(expectedName));
        Assert.That(materials[expectedName], Is.SameAs(original));
        Assert.That(original.Name, Is.EqualTo(expectedName));
        Assert.That(materials.ContainsKey(oldName), Is.EqualTo(expectedName == oldName));
    }

    [Test]
    public void MaterialMenu_ChangesSurviveSaveAndReload()
    {
        var (page, tree, materials) = CreateMaterialsPage();
        var material = (TreeViewItem)tree.Items[0];
        var name = (string)material.Header;
        var category = MaterialCategory(material, "Металлургия");
        tree.SelectedItem = category;
        ClickDatabaseMenu(category, 0);
        var reaction = materials[name]["Металлургия"]["Реакция R1-R2"];
        reaction.DataTable.Rows.Add(100f, 0.5f);
        var removed = (TreeViewItem)tree.Items[1];
        var removedName = (string)removed.Header;
        tree.SelectedItem = removed;
        ClickDatabaseMenu(removed, 1);
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".jsf");
        try
        {
            Call(page, "SafeDBEventHandler", path);
            Call(page, "Load", path, false);
            var loaded = (MaterialDBData)page.GetType().GetProperty("Materials").GetValue(page);
            Assert.That(loaded.ContainsKey(removedName), Is.False);
            var table = loaded[name]["Металлургия"]["Реакция R1-R2"].DataTable;
            Assert.That(Convert.ToSingle(table.Rows[0][0]), Is.EqualTo(100f));
            Assert.That(Convert.ToSingle(table.Rows[0][1]), Is.EqualTo(0.5f));
        }
        finally { File.Delete(path); }
    }

    [TestCase("Механические свойства", 0, "hardCalc")]
    [TestCase("Металлургия", 1, "diagCalc")]
    public void MaterialMenu_OpensCalculator(string categoryName, int menuIndex, string windowName)
    {
        var (page, tree, _) = CreateMaterialsPage();
        var category = MaterialCategory((TreeViewItem)tree.Items[0], categoryName);
        tree.SelectedItem = category;
        ClickDatabaseMenu(category, menuIndex);
        var windows = (System.Collections.IList)page.GetType().BaseType.GetProperty("AuxiliaryWindows", InstanceMembers).GetValue(page);
        try
        {
            Assert.That(windows.Count, Is.EqualTo(1));
            Assert.That(((Window)windows[0]).Name, Is.EqualTo(windowName));
        }
        finally { foreach (var window in windows.Cast<Window>().ToArray()) window.Close(); }
    }
}
