using Avalonia.Controls;
using BazisAvaloniaGUI.Animation;
using BazisAvaloniaGUI.Localization;
using BazisAvaloniaGUI.Navigator;
using ResultDB.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    // Из GUI/Methods/Results/CreateAnimation.cs.
    internal partial class MainWindow
    {
        private void создатьАнимациюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var btn = sender as MenuItem;
                // CheckOnClick: MenuItem с ToggleType = CheckBox переключает IsChecked до события Click.
                if (btn.IsChecked)
                {
                    var animationControl = new AnimationPage();
                    var form = new Window()
                    {
                        Name = "animationForm",
                        Title = Resources.AnimationForm_Text,
                        Icon = Icon,
                        Topmost = true,
                        SizeToContent = SizeToContent.WidthAndHeight,
                        CanResize = false,
                        ShowInTaskbar = false,
                        FontFamily = FontFamily,
                        FontSize = FontSize,
                        Content = animationControl
                    };

                    form.Closed += (s1, s2) =>
                    {
                        btn.IsChecked = false;
                        HideAllGeometryObjects();
                        HideAllText3D();
                        RequestRedraw();
                    };

                    animationControl.CreateGIFAnimationEvent += (arg1, arg2) => CreateGIFAnimation(arg2);
                    animationControl.ErrorReported += message => console.PrintInfo(message, Color.Red);

                    form.Show(this);
                    form.Position = Avalonia.VisualExtensions.PointToScreen(scene, new Avalonia.Point(0, 0));
                }
                else
                {
                    OwnedWindows.FirstOrDefault(x => x.Name == "animationForm")?.Close();
                }
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        public async void CreateGIFAnimation(CreateAnimationEventArgs args)
        {
            try
            {
                // WorkingDir может быть null (проект не сохранён) — Path.Combine внутри try.
                var outputFilePath = Path.Combine(WorkingDir, "results.gif");
                // Кадры снимаются асинхронно: за это время проект/база результатов могут смениться.
                var currentProject = project;
                var resultDbPath = ResultDbPath;
                void ThrowIfProjectChanged()
                {
                    if (!ReferenceEquals(currentProject, project) || resultDbPath != ResultDbPath)
                        throw new OperationCanceledException();
                }

                //выбрать узел в дереве асинхронно
                await SelectContainerAsync(Resources.Result_CreateGIFAnimation_SelectContainerAsync_SelectResult_Message);

                ThrowIfProjectChanged();
                if (navigator.SelectedNode?.Name != NodeName.Result.ToString())
                    throw new Exception(Resources.Result_CreateGIFAnimation_Exception);

                var selNode = navigator.SelectedNode;
                var resName = selNode.Text;

                var loader = new LoadResultsFileDB();

                var tables = new List<string>()
                { ResultType.nodes.ToString() };

                var list = new List<float>();
                foreach (TreeNode item in selNode.Nodes)
                    if (float.TryParse(item.Text, out var time))
                        list.Add(time);

                // Узлы времени добавляются в дерево при раскрытии результата; если он не раскрыт — берём из БД.
                if (list.Count == 0 && resultTimes != null)
                    list.AddRange(resultTimes);

                using var stream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.Read);
                using (var gif = new GifWriter(stream))
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        ThrowIfProjectChanged();
                        var result = loader.GetResult(resultDbPath, tables, list[i]);
                        ShowResults(result, resName);
                        // В BaseForm — RenderNow и снимок экрана: кадр снимается после отрисовки поля результата.
                        var image = await scene.Surface.CaptureFrameAsync();
                        ThrowIfProjectChanged();
                        gif.WriteFrame(image, args.DelayTime);

                        var total = ((i + 1) / (float)list.Count * 100).ToString("#.##");
                        console.PrintInfo($@"{Resources.Result_CreateGIFAnimation_CreateGIFAnimationInfo} {total}%", Color.Black);
                    }
                }
                console.PrintInfo($"{Resources.Result_CreateGIFAnimation_AnimationCreated}: {outputFilePath}", Color.Green);
            }
            catch (OperationCanceledException)
            {
                // Отмена интерактивного выбора завершает текущую операцию.
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
    }
}
