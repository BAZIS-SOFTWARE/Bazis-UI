using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace BazisAvaloniaGUI.Tests;

public partial class ResourceLifetimeTests
{
    // Для сохранённого проекта запрос снимка должен назначать screenShot.png в каталоге проекта без диалога,
    // независимо от пути в строке состояния; проверяется назначенный путь, а не создание изображения.
    [Test]
    public async Task ScreenShot_UsesProjectDirectoryRatherThanStatusOrApplicationDirectory()
    {
        var folder = CreateTestFolder();
        var window = CreateProjectWindow(Path.Combine(Path.GetTempPath(), "another directory", "import.stl"));
        GetField(window, "projectFilePath").SetValue(window, Path.Combine(folder, "проект с пробелами.bpf2"));
        var pickerCalls = 0;
        var provider = CreateSavePicker(() => { pickerCalls++; return null; });
        typeof(TopLevel).GetFields(InstanceMembers).Single(field => field.FieldType == typeof(IStorageProvider)).SetValue(window, provider);
        try
        {
            await RequestScreenShot(window);

            Assert.That(PendingScreenShotPath(window), Is.EqualTo(Path.Combine(folder, "screenShot.png")));
            Assert.That(pickerCalls, Is.Zero);
        }
        finally
        {
            window.Close();
            Directory.Delete(folder, true);
        }
    }

    // Для несохранённого проекта путь снимка выбирается через диалог: при необходимости добавляется .png.
    // Проверяются кириллица и пробелы в имени, а также отсутствие назначенного пути при отмене диалога.
    [TestCase("снимок сцены.png")]
    [TestCase("снимок сцены")]
    [TestCase(null)]
    public async Task ScreenShot_WithoutProjectPathUsesPickerAndHonorsCancellation(string selectedName)
    {
        var folder = CreateTestFolder();
        var window = CreateProjectWindow(Path.Combine(folder, "source.stl"));
        var selectedPath = selectedName == null ? null : Path.Combine(folder, selectedName);
        var pickerCalls = 0;
        var provider = CreateSavePicker(() => { pickerCalls++; return selectedPath; });
        typeof(TopLevel).GetFields(InstanceMembers).Single(field => field.FieldType == typeof(IStorageProvider)).SetValue(window, provider);
        try
        {
            await RequestScreenShot(window);

            var expected = selectedPath == null || selectedName.EndsWith(".png") ? selectedPath : selectedPath + ".png";
            Assert.That(PendingScreenShotPath(window), Is.EqualTo(expected));
            Assert.That(pickerCalls, Is.EqualTo(1));
        }
        finally
        {
            window.Close();
            Directory.Delete(folder, true);
        }
    }

    private static Task RequestScreenShot(Window window) => (Task)window.GetType().GetMethod("MakeScreenShotAsync", InstanceMembers).Invoke(window, null);

    private static string PendingScreenShotPath(Window window)
    {
        var scene = GetField(window, "scene").GetValue(window);
        var surface = scene.GetType().GetProperty("Surface").GetValue(scene);
        return (string)GetField(surface, "screenShotPath").GetValue(surface);
    }
}
