using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using OperationalController;
using System.Reflection;

namespace BazisGUI.Avalonia.Tests;

public partial class ResourceLifetimeTests
{
    // Защищает импортированный файл от перезаписи при отмене первого сохранения:
    // диалог вызывается один раз, исходные данные сохраняются, путь проекта не назначается.
    [TestCase("part.stp")]
    [TestCase("mesh.stl")]
    [TestCase("mesh.inp")]
    public async Task ImportedData_SaveCancellationPreservesSource(string sourceName)
    {
        var pickerCalls = 0;
        var provider = CreateSavePicker(() => { pickerCalls++; return null; });
        var folder = CreateTestFolder();
        var source = Path.Combine(folder, sourceName);
        File.WriteAllText(source, "original input data");
        var window = CreateProjectWindow(source);
        typeof(TopLevel).GetFields(InstanceMembers).Single(field => field.FieldType == typeof(IStorageProvider)).SetValue(window, provider);
        try
        {
            Assert.That(await Save(window), Is.False);
            Assert.That(pickerCalls, Is.EqualTo(1));
            Assert.That(File.ReadAllText(source), Is.EqualTo("original input data"));
            Assert.That(GetField(window, "projectFilePath").GetValue(window), Is.Null);
        }
        finally
        {
            window.Close();
            Directory.Delete(folder, true);
        }
    }

    // Проверяет выбор расширения и запоминание пути при первом сохранении, в том числе для кириллицы и пробелов.
    // Повторное сохранение использует этот путь без диалога, сохраняет исходную сетку и задаёт рабочий каталог проекта.
    [TestCase("result.bpf2")]
    [TestCase("result.bpf")]
    [TestCase("mesh.stl")]
    [TestCase("исходная сетка.stl")]
    public async Task FirstSave_UsesProjectExtensionAndRemembersDestination(string selectedName)
    {
        var folder = CreateTestFolder();
        var source = Path.Combine(folder, "исходная сетка.stl");
        File.WriteAllText(source, "original mesh");
        var selected = Path.Combine(folder, selectedName);
        var expected = selectedName.EndsWith(".stl") ? selected + ".bpf2" : selected;
        var pickerCalls = 0;
        var provider = CreateSavePicker(() => { pickerCalls++; return selected; });
        var window = CreateProjectWindow(source);
        typeof(TopLevel).GetFields(InstanceMembers).Single(field => field.FieldType == typeof(IStorageProvider)).SetValue(window, provider);
        try
        {
            Assert.That(await Save(window), Is.True);
            Assert.That(File.Exists(expected), Is.True);
            Assert.That(GetField(window, "projectFilePath").GetValue(window), Is.EqualTo(expected));
            // Display text must never become the destination of a subsequent Ctrl+S.
            ((TextBlock)GetField(window, "lblStatus").GetValue(window)).Text = source;
            Assert.That(await Save(window), Is.True);
            Assert.That(pickerCalls, Is.EqualTo(1));
            Assert.That(File.ReadAllText(source), Is.EqualTo("original mesh"));
            Assert.That(window.GetType().GetProperty("WorkingDir", InstanceMembers).GetValue(window), Is.EqualTo(folder));
        }
        finally
        {
            window.Close();
            Directory.Delete(folder, true);
        }
    }

    // Отмена «Сохранить как» не должна менять текущий путь проекта и его отображение в строке состояния.
    [Test]
    public async Task SaveAsCancellation_KeepsExistingProjectDestination()
    {
        var folder = CreateTestFolder();
        var oldPath = Path.Combine(folder, "old.bpf2");
        var window = CreateProjectWindow(oldPath);
        GetField(window, "projectFilePath").SetValue(window, oldPath);
        var pickerCalls = 0;
        var provider = CreateSavePicker(() => { pickerCalls++; return null; });
        typeof(TopLevel).GetFields(InstanceMembers).Single(field => field.FieldType == typeof(IStorageProvider)).SetValue(window, provider);
        try
        {
            Assert.That(await Save(window, saveAs: true), Is.False);
            Assert.That(pickerCalls, Is.EqualTo(1));
            Assert.That(GetField(window, "projectFilePath").GetValue(window), Is.EqualTo(oldPath));
            Assert.That(((TextBlock)GetField(window, "lblStatus").GetValue(window)).Text, Is.EqualTo(oldPath));
        }
        finally
        {
            window.Close();
            Directory.Delete(folder, true);
        }
    }

    // Ошибка записи в отсутствующий каталог не должна менять путь, имя проекта и строку состояния.
    [Test]
    public void SaveFailure_PreservesProjectPathAndName()
    {
        var folder = CreateTestFolder();
        var oldPath = Path.Combine(folder, "old.bpf2");
        var window = CreateProjectWindow(oldPath);
        GetField(window, "projectFilePath").SetValue(window, oldPath);
        var project = (ProjectController)GetField(window, "project").GetValue(window);
        var oldName = project.Name;
        try
        {
            Assert.Throws<TargetInvocationException>(() => Call(window, "SaveProjectToFile", Path.Combine(folder, "missing", "new.bpf2")));
            Assert.That(GetField(window, "projectFilePath").GetValue(window), Is.EqualTo(oldPath));
            Assert.That(project.Name, Is.EqualTo(oldName));
            Assert.That(((TextBlock)GetField(window, "lblStatus").GetValue(window)).Text, Is.EqualTo(oldPath));
        }
        finally
        {
            window.Close();
            Directory.Delete(folder, true);
        }
    }

    private Window CreateProjectWindow(string statusPath)
    {
        var window = (Window)Create("Shell.MainWindow");
        var project = new ProjectController();
        project.CreateProject("newProject.bpf2");
        project.CreateTask();
        GetField(window, "project").SetValue(window, project);
        ((TextBlock)GetField(window, "lblStatus").GetValue(window)).Text = statusPath;
        return window;
    }

    private static string CreateTestFolder()
    {
        var folder = Path.Combine(Path.GetTempPath(), "Bazis save tests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(folder);
        return folder;
    }

    private static Task<bool> Save(Window window, bool saveAs = false) => (Task<bool>)window.GetType().GetMethod("SaveProjectAsync", InstanceMembers).Invoke(window, [saveAs]);

    private static IStorageProvider CreateSavePicker(Func<string> selectPath)
    {
        var provider = DispatchProxy.Create<IStorageProvider, StorageProxy>();
        ((StorageProxy)provider).Handler = (method, _) =>
        {
            if (method.Name != "SaveFilePickerAsync")
                throw new NotSupportedException(method.Name);
            var path = selectPath();
            IStorageFile file = null;
            if (path != null)
            {
                file = DispatchProxy.Create<IStorageFile, StorageProxy>();
                ((StorageProxy)file).Handler = (member, _) => member.Name == "get_Path" ? new Uri(path) : throw new NotSupportedException(member.Name);
            }
            return Task.FromResult(file);
        };
        return provider;
    }

    public class StorageProxy : DispatchProxy
    {
        public Func<MethodInfo, object[], object> Handler { get; set; }
        protected override object Invoke(MethodInfo targetMethod, object[] args) => Handler(targetMethod, args);
    }
}
