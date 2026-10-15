using Avalonia.Controls;
using ResultDB;
using ResultDB.IO;
using System.Data;
using System.Reflection;

namespace BazisAvaloniaGUI.Tests;

public partial class ResourceLifetimeTests
{
    [TestCase("corrupt")]
    [TestCase("incompatible")]
    [TestCase("missing")]
    [TestCase("empty")]
    public void InvalidResultsDatabase_PreservesCurrentResults(string failure)
    {
        var folder = CreateTestFolder();
        var window = CreateProjectWindow(Path.Combine(folder, "project.bpf2"));
        try
        {
            var valid = Path.Combine(folder, "valid.db");
            SaveTestResults(valid, "nodes", 2f);
            Call(window, "FillingResultsData", valid);
            var oldTimes = GetField(window, "resultTimes").GetValue(window);
            var oldNodes = FindNavigatorNodes(window, "Results");
            var oldNode = oldNodes.Single();
            var children = oldNode.GetType().GetProperty("Nodes").GetValue(oldNode);
            var oldCount = children.GetType().GetProperty("Count").GetValue(children);
            var invalid = Path.Combine(folder, "invalid.db");
            if (failure == "corrupt")
                File.WriteAllText(invalid, "Not a SQLite database");
            else if (failure == "incompatible")
                SaveTestResults(invalid, "elements", 3f);
            else if (failure == "empty")
                SaveTestResults(invalid, "nodes", 3f, false);

            Assert.Throws<TargetInvocationException>(() => Call(window, "FillingResultsData", invalid));
            Assert.That(window.GetType().GetProperty("ResultDbPath", InstanceMembers).GetValue(window), Is.EqualTo(valid));
            Assert.That(GetField(window, "resultTimes").GetValue(window), Is.SameAs(oldTimes));
            Assert.That(FindNavigatorNodes(window, "Results").Single(), Is.SameAs(oldNode));
            Assert.That(children.GetType().GetProperty("Count").GetValue(children), Is.EqualTo(oldCount));
            if (failure == "missing")
                Assert.That(File.Exists(invalid), Is.False);
        }
        finally
        {
            window.Close();
            // ResultDB releases its SQLite readers through finalizers.
            GC.Collect();
            GC.WaitForPendingFinalizers();
            System.Data.SQLite.SQLiteConnection.ClearAllPools();
            Directory.Delete(folder, true);
        }
    }

    [Test]
    public void InvalidResultsDatabase_ShowsErrorWithoutFaultingUiTask()
    {
        var folder = CreateTestFolder();
        var window = CreateProjectWindow(Path.Combine(folder, "project.bpf2"));
        window.Content = null;
        window.Show();
        try
        {
            var corrupt = Path.Combine(folder, "corrupt.db");
            File.WriteAllText(corrupt, "Not a SQLite database");
            var task = (Task)window.GetType().GetMethod("OpenResultsAsync", InstanceMembers).Invoke(window, [corrupt]);
            Assert.That(window.OwnedWindows.Count, Is.EqualTo(1));
            window.OwnedWindows.Single().Close();
            PumpUntilComplete(task);
            Assert.That(task.IsCompletedSuccessfully, Is.True);
            Assert.That(window.IsEnabled, Is.True);
            Assert.That(window.GetType().GetProperty("ResultDbPath", InstanceMembers).GetValue(window), Is.EqualTo(string.Empty));
        }
        finally
        {
            window.Close();
            // ResultDB releases its SQLite readers through finalizers.
            GC.Collect();
            GC.WaitForPendingFinalizers();
            System.Data.SQLite.SQLiteConnection.ClearAllPools();
            Directory.Delete(folder, true);
        }
    }

    [Test]
    public void ValidResultsDatabase_ReplacesPathTimesAndKeepsOneResultsRoot()
    {
        var folder = CreateTestFolder();
        var window = CreateProjectWindow(Path.Combine(folder, "project.bpf2"));
        try
        {
            var first = Path.Combine(folder, "first.db");
            var second = Path.Combine(folder, "second.db");
            SaveTestResults(first, "nodes", 1f);
            SaveTestResults(second, "nodes", 2f);
            Call(window, "FillingResultsData", first);
            Call(window, "FillingResultsData", second);
            Assert.That(window.GetType().GetProperty("ResultDbPath", InstanceMembers).GetValue(window), Is.EqualTo(second));
            Assert.That((IEnumerable<float>)GetField(window, "resultTimes").GetValue(window), Is.EqualTo(new[] { 2f }));
            Assert.That(FindNavigatorNodes(window, "Results"), Has.Length.EqualTo(1));
        }
        finally
        {
            window.Close();
            // ResultDB releases its SQLite readers through finalizers.
            GC.Collect();
            GC.WaitForPendingFinalizers();
            System.Data.SQLite.SQLiteConnection.ClearAllPools();
            Directory.Delete(folder, true);
        }
    }

    private static void SaveTestResults(string path, string tableName, float time, bool includeRows = true)
    {
        var table = new DataTable(tableName);
        var number = table.Columns.Add("Number", typeof(int));
        table.Columns.Add("T", typeof(float));
        table.PrimaryKey = [number];
        if (includeRows)
            table.Rows.Add(1, 10f);
        var result = new Result(time, "test");
        result.AddDataTable(table);
        new SaveResultsFileDb().Save(new List<Result> { result }, path, false);
    }

    private static bool HasNavigatorNode(Window window, string name) => FindNavigatorNodes(window, name).Length > 0;

    private static object[] FindNavigatorNodes(Window window, string name)
    {
        var navigator = GetField(window, "navigator").GetValue(window);
        var search = navigator.GetType().GetMethods().Single(method => method.Name == "TrySearchNodes"
            && method.GetParameters()[0].ParameterType == typeof(string));
        var arguments = new object[] { name, null };
        search.Invoke(navigator, arguments);
        return ((System.Collections.IEnumerable)arguments[1]).Cast<object>().ToArray();
    }
}
