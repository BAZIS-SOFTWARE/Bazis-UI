using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using OperationalController;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BazisAvaloniaGUI.Tests;

[TestFixture]
[SingleThreaded]
[Apartment(ApartmentState.STA)]
public partial class ResourceLifetimeTests
{
    private const BindingFlags InstanceMembers = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
    private readonly Assembly gui = typeof(TabButtonControlService).Assembly;

    [OneTimeSetUp]
    public void InitializeAvalonia()
    {
        AppBuilder.Configure<Application>()
            .UseSkia()
            .UseHeadless(new AvaloniaHeadlessPlatformOptions { UseHeadlessDrawing = false })
            .SetupWithoutStarting();
    }

    [TestCase("OpenMaterialsDB")]
    [TestCase("OpenFunctionsDB")]
    public void RemovedDatabasePages_CanBeCollectedWhileMainWindowLives(string openMethod)
    {
        var window = (Window)Create("Shell.MainWindow");
        GetField(window, "project").SetValue(window, new ProjectController
        {
            MaterialsDB = new MaterialDB.MaterialData.MaterialDBData(),
            FunctionsDB = new MaterialDB.FunctionData.FunctionDBData()
        });
        try
        {
            var pages = Enumerable.Range(0, 3).Select(_ => OpenAndRemovePage(window, openMethod)).ToArray();
            Dispatcher.UIThread.RunJobs();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Assert.That(pages.All(page => !page.IsAlive), Is.True, "Removed pages must not be retained by OnProjectLoaded.");
            GC.KeepAlive(window);
        }
        finally
        {
            window.Close();
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private WeakReference OpenAndRemovePage(Window window, string openMethod)
    {
        Call(window, openMethod, "Database");
        var tabs = (TabButtonControlService)GetField(window, "TabButtonsService").GetValue(window);
        var page = tabs.GetControl("Database");
        var reference = new WeakReference(page);
        tabs.RemoveControl("Database");
        return reference;
    }

    [TestCase("OpenMaterialsDB", "Materials", "MaterialsDB")]
    [TestCase("OpenFunctionsDB", "Functions", "FunctionsDB")]
    public void OpenDatabasePage_UpdatesWhenProjectChanges(string openMethod, string pageProperty, string projectProperty)
    {
        var window = (Window)Create("Shell.MainWindow");
        var project = new ProjectController();
        var databaseProperty = project.GetType().GetProperty(projectProperty);
        var database = Activator.CreateInstance(databaseProperty.PropertyType);
        databaseProperty.SetValue(project, database);
        GetField(window, "project").SetValue(window, new ProjectController
        {
            MaterialsDB = new MaterialDB.MaterialData.MaterialDBData(),
            FunctionsDB = new MaterialDB.FunctionData.FunctionDBData()
        });
        try
        {
            Call(window, openMethod, "Database");
            var tabs = (TabButtonControlService)GetField(window, "TabButtonsService").GetValue(window);
            var page = tabs.GetControl("Database");
            GetField(window, "project").SetValue(window, project);
            ((Action)GetField(window, "OnProjectLoaded").GetValue(window)).Invoke();

            Assert.That(page.GetType().GetProperty(pageProperty).GetValue(page), Is.SameAs(database));
        }
        finally
        {
            window.Close();
        }
    }

    [Test]
    public void Player_DetachStopsTimerAndAllowsContinuation()
    {
        var player = (Control)Create("Player.PlayerControl");
        var window = new Window { Content = player };
        try
        {
            window.Show();
            Call(player, "StartChecking_Click");
            var timer = (DispatcherTimer)GetField(player, "timer").GetValue(player);
            Assert.That(timer.IsEnabled, Is.True);

            window.Content = null;
            Assert.That(timer.IsEnabled, Is.False);
            Assert.That(player.GetType().GetProperty("CheckState").GetValue(player).ToString(), Is.EqualTo("continuation"));

            window.Content = player;
            Call(player, "StartChecking_Click");
            Assert.That(timer.IsEnabled, Is.True);
        }
        finally
        {
            ((IDisposable)player).Dispose();
            window.Close();
        }
    }

    [Test]
    public void Player_StateChangesReuseIconsAndDisposeStopsTimer()
    {
        var player = Create("Player.PlayerControl");
        var image = (Image)GetField(player, "checkImage").GetValue(player);
        var startIcon = image.Source;
        var pauseIcon = (Bitmap)GetField(player, "pauseIcon").GetValue(player);
        try
        {
            for (var i = 0; i < 10; i++)
            {
                Call(player, "StartChecking_Click");
                Assert.That(image.Source, Is.SameAs(pauseIcon));
                Call(player, "StopChecking");
                Assert.That(image.Source, Is.SameAs(startIcon));
            }
            Call(player, "StartChecking_Click");
        }
        finally
        {
            ((IDisposable)player).Dispose();
        }
        Assert.That(((DispatcherTimer)GetField(player, "timer").GetValue(player)).IsEnabled, Is.False);
        Assert.That(image.Source, Is.Null);
        Assert.DoesNotThrow(() => ((IDisposable)player).Dispose());
    }

    [Test]
    public void Scene_ProjectSubscriptionFollowsVisualAttachment()
    {
        var scene = (Control)Create("Scene.SceneSurface");
        var project = new ProjectController();
        var window = new Window();
        int Subscriptions() => ((Delegate)GetField(project, "Message").GetValue(project))?
            .GetInvocationList().Count(handler => ReferenceEquals(handler.Target, scene)) ?? 0;
        try
        {
            Call(scene, "ShowProject", project);
            Assert.That(Subscriptions(), Is.Zero);
            window.Content = scene;
            window.Show();
            Assert.That(Subscriptions(), Is.EqualTo(1));
            window.Content = null;
            Assert.That(Subscriptions(), Is.Zero);
            window.Content = scene;
            Assert.That(Subscriptions(), Is.EqualTo(1));
            Call(scene, "ShowProject", project);
            Assert.That(Subscriptions(), Is.EqualTo(1));
            window.Close();
            Assert.That(Subscriptions(), Is.Zero);
        }
        finally
        {
            window.Close();
        }
    }

    private object Create(string name) => Activator.CreateInstance(gui.GetType("BazisAvaloniaGUI." + name, true), true);
    private static FieldInfo GetField(object target, string name) => target.GetType().GetField(name, InstanceMembers);
    private static void Call(object target, string name, params object[] args) => target.GetType().GetMethod(name, InstanceMembers).Invoke(target, args);
}
