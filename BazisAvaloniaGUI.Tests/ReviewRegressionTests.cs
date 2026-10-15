using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using BazisAvaloniaGUI.Chamfer.Services;
using BazisGUI.Scene.Core;
using OperationalController;
using System.Collections.Concurrent;

namespace BazisAvaloniaGUI.Tests;

public partial class ResourceLifetimeTests
{
    [Test]
    public async Task InteractiveSelection_ConfirmsOnUiThreadAndRetriesInvalidSelection()
    {
        var window = (Window)Create("Shell.MainWindow");
        var attempts = 0;
        var uiThread = Environment.CurrentManagedThreadId;
        Func<Tuple<bool, object>> confirm = () =>
        {
            Assert.That(Environment.CurrentManagedThreadId, Is.EqualTo(uiThread));
            return Tuple.Create(++attempts == 2, (object)"selected");
        };
        try
        {
            var task = StartSelection(window, confirm);
            Assert.That(task.IsCompleted, Is.False);
            Assert.That(attempts, Is.Zero);
            SetKey(window, Key.A);
            Assert.That(attempts, Is.Zero);
            SetKey(window, Key.E);
            Assert.That(attempts, Is.EqualTo(1));
            Assert.That(task.IsCompleted, Is.False);
            SetKey(window, Key.E);
            Dispatcher.UIThread.RunJobs();
            Assert.That(await task, Is.EqualTo("selected"));
            Assert.That(GetField(window, "interactiveSelection").GetValue(window), Is.Null);
        }
        finally { window.Close(); }
    }

    [Test]
    public async Task InteractiveSelection_EscapePreservesNullResultAndCloseCancelsWait()
    {
        var window = (Window)Create("Shell.MainWindow");
        window.Content = null;
        window.Show();
        try
        {
            var task = StartSelection(window, () => Tuple.Create(true, (object)"selected"));
            SetKey(window, Key.Escape);
            Dispatcher.UIThread.RunJobs();
            Assert.That(await task, Is.Null);
            task = StartSelection(window, () => Tuple.Create(true, (object)"selected"));
            window.Close();
            Dispatcher.UIThread.RunJobs();
            Assert.ThrowsAsync<TaskCanceledException>(async () => await task);
            Assert.That(GetField(window, "interactiveSelection").GetValue(window), Is.Null);
        }
        finally { window.Close(); }
    }

    [Test]
    public void ChamferWindow_ClosesWithOwnerAndClearsServiceReference()
    {
        var owner = new Window();
        var service = Create("Chamfer.Services.ChamferWindowService");
        var operation = new TestChamferOperation();
        var closed = 0;
        try
        {
            owner.Show();
            Call(service, "Show", owner, operation, (Action)(() => closed++));
            var child = (Window)GetField(service, "currentWindow").GetValue(service);
            Assert.That(child.Owner, Is.SameAs(owner));
            Assert.That(child.IsVisible, Is.True);
            owner.Close();
            Assert.That(child.IsVisible, Is.False);
            Assert.That(GetField(service, "currentWindow").GetValue(service), Is.Null);
            Assert.That(closed, Is.EqualTo(1));
            Assert.That(operation.ClearCount, Is.GreaterThanOrEqualTo(1));
        }
        finally { owner.Close(); }
    }

    [Test]
    public void ClearResultsState_RemovesOldResultsAndStopsPlayer()
    {
        var window = CreateProjectWindow(Path.Combine(Path.GetTempPath(), "project.bpf2"));
        try
        {
            var navigator = GetField(window, "navigator").GetValue(window);
            var nodeNameType = gui.GetType("BazisAvaloniaGUI.Navigator.NodeName");
            var resultNode = navigator.GetType().GetMethod("CreateRealNode", [nodeNameType])
                .Invoke(navigator, [Enum.Parse(nodeNameType, "Results")]);
            var roots = navigator.GetType().GetProperty("Nodes").GetValue(navigator);
            var root = roots.GetType().GetProperty("Item", [typeof(int)]).GetValue(roots, [0]);
            var children = root.GetType().GetProperty("Nodes").GetValue(root);
            Call(children, "Add", resultNode);
            window.GetType().GetProperty("ResultDbPath", InstanceMembers).SetValue(window, "old-results.db");
            GetField(window, "resultTimes").SetValue(window, new[] { 1f, 2f });
            var player = GetField(window, "checkPlayerControl").GetValue(window);
            ((DispatcherTimer)GetField(player, "timer").GetValue(player)).Start();
            Call(window, "ClearResultsState");

            Assert.That(window.GetType().GetProperty("ResultDbPath", InstanceMembers).GetValue(window), Is.EqualTo(string.Empty));
            Assert.That((IEnumerable<float>)GetField(window, "resultTimes").GetValue(window), Is.Empty);
            Assert.That(resultNode.GetType().GetProperty("Collection", InstanceMembers).GetValue(resultNode), Is.Null);
            Assert.That(((DispatcherTimer)GetField(player, "timer").GetValue(player)).IsEnabled, Is.False);
            Assert.That(player.GetType().GetProperty("StopValue").GetValue(player), Is.EqualTo(0));
        }
        finally { window.Close(); }
    }

    [Test]
    public void Contours_DiscardQueuedUpdatesAfterNewRequestAndProjectReplacement()
    {
        var surface = Create("Scene.SceneSurface");
        var first = new ProjectController();
        GetField(surface, "project").SetValue(surface, first);
        var actions = (ConcurrentQueue<Action<SceneController>>)GetField(surface, "sceneActions").GetValue(surface);
        Call(surface, "SetContoursVisible", false);
        Assert.That(actions.TryDequeue(out var staleToggle), Is.True);
        Call(surface, "SetContoursVisible", false);
        Assert.DoesNotThrow(() => staleToggle(null), "A superseded action must not touch the renderer.");
        Assert.That(actions.TryDequeue(out var staleProject), Is.True);
        Call(surface, "ShowProject", new ProjectController());
        Assert.DoesNotThrow(() => staleProject(null), "An old project's action must not touch the renderer.");
    }

    [Test]
    public void AppendMesh_FailureRestoresOwnerAndClosesProgress()
    {
        var window = CreateProjectWindow(Path.Combine(Path.GetTempPath(), "project.bpf2"));
        window.Content = null;
        window.Show();
        try
        {
            var missing = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "missing.inp");
            var task = (Task)window.GetType().GetMethod("AppendMeshAsync", InstanceMembers).Invoke(window, [missing]);
            Assert.That(window.IsEnabled, Is.False);
            Assert.That(window.OwnedWindows.Any(child => child.Name == "Загрузка"), Is.True);
            Assert.That(SpinWait.SpinUntil(() =>
            {
                Dispatcher.UIThread.RunJobs();
                return task.IsCompleted;
            }, TimeSpan.FromSeconds(10)), Is.True);
            Assert.That(task.IsFaulted, Is.True);
            Assert.That(task.Exception, Is.Not.Null);
            Assert.That(window.IsEnabled, Is.True);
            Assert.That(window.OwnedWindows.Any(child => child.Name == "Загрузка"), Is.False);
        }
        finally { window.Close(); }
    }

    [Test]
    public void Distribution_ContainsUsableDatabasesAndManual()
    {
        var materials = new MaterialDB.MaterialData.MaterialDBData("Materials_v6.jsf", Path.Combine(AppContext.BaseDirectory, "DataBases", "Materials"));
        var functions = new MaterialDB.FunctionData.FunctionDBData("functions.jsf", Path.Combine(AppContext.BaseDirectory, "DataBases", "Functions"));
        Assert.That(materials.Keys, Is.Not.Empty);
        Assert.That(functions.Keys, Is.Not.Empty);
        Assert.That(File.Exists(Path.Combine(AppContext.BaseDirectory, "Help", "ПО Bazis 5.2. Руководство пользователя.pdf")), Is.True);
    }

    private static Task<object> StartSelection(Window window, Func<Tuple<bool, object>> confirm) =>
        (Task<object>)window.GetType().GetMethod("AsyncMethodContainer").Invoke(window, [confirm, (Action)(() => { }), "Select objects"]);

    private static void SetKey(Window window, Key key) => window.GetType().GetProperty("PressedKey").SetValue(window, key);

    private sealed class TestChamferOperation : IChamferOperationService
    {
        public int ClearCount { get; private set; }
        public void AddByAngle(double length, double angle, bool isReflected) { }
        public void AddByLengths(double length1, double length2, bool isReflected) { }
        public void Prewiew(double length, double valueSecond, bool isAngle, bool isReflected) { }
        public void ClearPreview() => ClearCount++;
    }
}
