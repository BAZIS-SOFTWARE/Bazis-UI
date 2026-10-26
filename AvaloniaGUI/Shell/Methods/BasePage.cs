using Avalonia.Input;
using Avalonia.Threading;
using AvaloniaGUI.Localization;
using Geometry;
using Model.Interfaces;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace AvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private Key pressedKey;
        private TaskCompletionSource<object> interactiveSelection;
        private Func<Tuple<bool, object>> confirmSelection;

        /// <summary>Последняя клавиша; подтверждение и отмена выбора обрабатываются в UI-потоке.</summary>
        public Key PressedKey
        {
            get => pressedKey;
            set
            {
                pressedKey = value;
                if (value == Key.Escape)
                    interactiveSelection?.TrySetResult(null);
                else if (value == Key.E && interactiveSelection != null && !interactiveSelection.Task.IsCompleted)
                {
                    try
                    {
                        var result = confirmSelection();
                        if (result.Item1)
                            interactiveSelection.TrySetResult(result.Item2);
                    }
                    catch (Exception error)
                    {
                        interactiveSelection.TrySetException(error);
                    }
                }
            }
        }

        private void CancelInteractiveSelection() => interactiveSelection?.TrySetCanceled();

        public async Task<Geometry.Plane> CreateSurfaceAsync(ObjType objType)
        {
            var actBreak = new Action(() =>
            {
                Dispatcher.UIThread.Invoke(new Action(() => console.PrintInfo(Resources.BasePage_CreateSurfaceAync_OperationCanceled_Message, Color.Black)));
            });
            var message = @$"{Resources.BasePage_CreateSurfaceAsync_AsyncContainer_Message}";
            var actSurfaceConfirm = new Func<Tuple<bool, object>>(() =>
            {
                var selObjs = project.GetSelection()
                    .Where(x => x.ObjType == objType)
                    .ToArray();

                if (selObjs.Length < 3)
                {
                    Dispatcher.UIThread.Invoke(new Action(() => console.PrintInfo(Resources.BasePage_CreateSurfaceAsync_SelectThreeNodes_Message, Color.Orange)));
                    return new Tuple<bool, object>(false, new object());
                }
                else if (objType != ObjType.Узел & objType != ObjType.Точка)
                {
                    Dispatcher.UIThread.Invoke(new Action(() => console.PrintInfo(Resources.BasePage_CreateSurfaceAsync_SelectNodeType_Message, Color.Orange)));
                    return new Tuple<bool, object>(false, new object());
                }
                else
                {
                    var p0 = selObjs[0];
                    var p1 = selObjs[1];
                    var p2 = selObjs[2];

                    var plane = new Geometry.Plane(p0.CalcCentr(), p1.CalcCentr(), p2.CalcCentr());
                    Dispatcher.UIThread.Invoke(new Action(() => console.PrintInfo(Resources.BasePage_CreateSurfaceAsync_SurfaceSet_Message, Color.Green)));
                    return new Tuple<bool, object>(true, plane);
                }
            });
            // TODO: придумать, как решить проблему upcast-а ниже (мб передан null)
            return (Geometry.Plane)await AsyncMethodContainer(actSurfaceConfirm, actBreak, message);
        }

        public async Task<object> AsyncMethodContainer(Func<Tuple<bool, object>> actConfirm, Action actBreak, string cmdMessage)
        {
            Dispatcher.UIThread.VerifyAccess();
            CancelInteractiveSelection();
            var completion = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
            interactiveSelection = completion;
            confirmSelection = actConfirm;
            PressedKey = Key.None;
            DisplayText2D(cmdMessage, GetTextColor(), new Point2D(10, 10));
            RequestRedraw();
            try
            {
                var result = await completion.Task;
                if (result == null && !isClosing)
                    actBreak();
                return result;
            }
            catch (OperationCanceledException)
            {
                if (!isClosing)
                    actBreak();
                throw;
            }
            finally
            {
                if (ReferenceEquals(interactiveSelection, completion))
                {
                    interactiveSelection = null;
                    confirmSelection = null;
                    HideAllText2D();
                    RequestRedraw();
                    PressedKey = Key.None;
                }
            }
        }

        private Color GetTextColor()
        {
            var backgroundBrightness = 0.299 * settingsConfig.BackGroundColor.R + 0.587 *
                settingsConfig.BackGroundColor.G + 0.114 *
                settingsConfig.BackGroundColor.B;

            if (backgroundBrightness > 125) // Или другое пороговое значение
            {
                return Color.Black; // Светлый фон -> черный шрифт
            }
            else
            {
                return Color.White; // Темный фон -> белый шрифт
            }
        }
    }
}
