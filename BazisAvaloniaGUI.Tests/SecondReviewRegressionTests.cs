using Avalonia.Controls;
using Avalonia.Threading;
using BazisAvaloniaGUI.SettingsControls;
using BazisGUI.Scene.Core;
using BazisGUI.Scene.Core.Layers;
using Model.Interfaces;
using OperationalController;
using Project.Interfaces.Tasks;
using ResultDB;
using System.Collections.Concurrent;
using System.Data;

namespace BazisAvaloniaGUI.Tests;

public partial class ResourceLifetimeTests
{
    [Test]
    public async Task OpenProject_DuplicateRequestDoesNotReplaceActiveProjectOrPath()
    {
        var window = CreateProjectWindow(Path.Combine(Path.GetTempPath(), "current.bpf2"));
        var original = GetField(window, "project").GetValue(window);
        GetField(window, "projectFilePath").SetValue(window, "current.bpf2");
        GetField(window, "isLoadingProject").SetValue(window, true);
        window.IsEnabled = false;
        try
        {
            var task = (Task)window.GetType().GetMethod("OpenProject", InstanceMembers).Invoke(window, ["missing.bpf2"]);
            await task;
            Assert.That(GetField(window, "project").GetValue(window), Is.SameAs(original));
            Assert.That(GetField(window, "projectFilePath").GetValue(window), Is.EqualTo("current.bpf2"));
            Assert.That(window.IsEnabled, Is.False);
        }
        finally { window.Close(); }
    }

    [Test]
    public void OpenProject_ReplacesProjectAndClosesOldGanttTab()
    {
        var folder = CreateTestFolder();
        var path = Path.Combine(folder, "next.bpf2");
        var next = new ProjectController();
        next.CreateProject("next.bpf2");
        next.CreateTask();
        next.Save(path);
        var window = CreateProjectWindow(Path.Combine(folder, "current.bpf2"));
        var original = GetField(window, "project").GetValue(window);
        var tabs = (TabButtonControlService)GetField(window, "TabButtonsService").GetValue(window);
        tabs.AddControl("Old chart", (Control)Create("GantChart.cntrГант"));
        var menu = (MenuItem)GetField(window, "показатьНаДиаграммеToolStripMenuItem").GetValue(window);
        menu.IsChecked = true;
        window.Content = null;
        window.Show();
        try
        {
            var task = (Task)window.GetType().GetMethod("OpenProject", InstanceMembers).Invoke(window, [path]);
            PumpUntilComplete(task);
            Assert.That(task.IsCompletedSuccessfully, Is.True);
            Assert.That(window.IsEnabled, Is.True);
            Assert.That(GetField(window, "isLoadingProject").GetValue(window), Is.False);
            Assert.That(GetField(window, "project").GetValue(window), Is.Not.SameAs(original));
            Assert.That(GetField(window, "projectFilePath").GetValue(window), Is.EqualTo(path));
            Assert.That(tabs.GetNames(), Does.Not.Contain("Old chart"));
            Assert.That(menu.IsChecked, Is.False);
        }
        finally
        {
            window.Close();
            Directory.Delete(folder, true);
        }
    }

    [TestCase(false)]
    [TestCase(true)]
    public void LoadFailure_ClosesProgressWindow(bool importMesh)
    {
        var owner = new Window();
        owner.Show();
        try
        {
            var controller = new IODataController(owner);
            var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "missing.inp");
            var task = importMesh ? controller.ImportMesh(path) : controller.OpenProject(path);
            PumpUntilComplete(task);
            Assert.That(task.IsFaulted, Is.True);
            Assert.That(task.Exception, Is.Not.Null);
            Assert.That(owner.OwnedWindows, Is.Empty);
        }
        finally { owner.Close(); }
    }

    [Test]
    public void GanttChart_AcceptsEmptyConditions()
    {
        var chart = Create("GantChart.cntrГант");
        Assert.DoesNotThrow(() => Call(chart, "AddConds", (object)Array.Empty<ICondData>()));
    }

    [Test]
    public void FrameCapture_DetachingSceneCancelsPendingAndFutureRequests()
    {
        var surface = (Control)Create("Scene.SceneSurface");
        var owner = new Window { Content = surface };
        try
        {
            owner.Show();
            var pending = (Task<byte[]>)surface.GetType().GetMethod("CaptureFrameAsync").Invoke(surface, null);
            Assert.That(pending.IsCompleted, Is.False);
            owner.Content = null;
            Assert.That(pending.IsCanceled, Is.True);
            var afterDetach = (Task<byte[]>)surface.GetType().GetMethod("CaptureFrameAsync").Invoke(surface, null);
            Assert.That(afterDetach.IsCanceled, Is.True);
        }
        finally { owner.Close(); }
    }

    [Test]
    public void ShowResults_KeepsNodeAndElementLabelsTogetherAndClearsOldLabels()
    {
        var window = CreateProjectWindow(Path.Combine(Path.GetTempPath(), "results.bpf2"));
        try
        {
            var project = (ProjectController)GetField(window, "project").GetValue(window);
            project.CreateNode(1, 0, 0, 0);
            project.CreateNode(2, 1, 0, 0);
            project.CreateElementsSet("lines", 1);
            project.CreateElement("lines", 1, [1, 2]);
            var element = project.GetAllModelElements().Single();
            project.Select(ObjType.Узел, [1]);
            project.Select(element.ObjType, [element.Number]);
            var settings = (SettingsConfig)GetField(window, "settingsConfig").GetValue(window);
            settings.ShowResultsField = false;
            settings.MergeResultsValue = false;
            settings.ShowNodeResultsValue = true;
            settings.ShowElementsResultsValue = true;
            var result = new Result(0f, "test");
            foreach (var name in new[] { "nodes", "elements" })
            {
                var table = new DataTable(name);
                var number = table.Columns.Add("Number", typeof(int));
                table.Columns.Add("T", typeof(float));
                table.PrimaryKey = [number];
                table.Rows.Add(name == "nodes" ? 1 : element.Number, name == "nodes" ? 10f : 20f);
                result.AddDataTable(table);
            }
            var scene = GetField(window, "scene").GetValue(window);
            var surface = scene.GetType().GetProperty("Surface").GetValue(scene);
            var actions = (ConcurrentQueue<Action<SceneController>>)GetField(surface, "sceneActions").GetValue(surface);
            actions.Clear();
            var renderer = new SceneController();
            var text = new TextLayer(null);
            GetField(renderer, "textLayer").SetValue(renderer, text);
            renderer.DisplayText3D("old label", System.Drawing.Color.Black, new Geometry.Point3D());
            Call(window, "ShowResults", result, "T");
            while (actions.TryDequeue(out var action))
                action(renderer);
            Assert.That(text.Labels.Select(label => label.Text), Is.EquivalentTo(new[] { "10", "20" }));
            settings.ShowNodeResultsValue = false;
            settings.ShowElementsResultsValue = false;
            Call(window, "ShowResults", result, "T");
            while (actions.TryDequeue(out var action))
                action(renderer);
            Assert.That(text.Labels, Is.Empty);
        }
        finally { window.Close(); }
    }

    private static void PumpUntilComplete(Task task)
    {
        Assert.That(SpinWait.SpinUntil(() =>
        {
            Dispatcher.UIThread.RunJobs();
            return task.IsCompleted;
        }, TimeSpan.FromSeconds(10)), Is.True, "The operation did not complete.");
    }
}
