using BazisGUI.Win.Localization;
using BazisGUI.Win.Properties;
using Geometry;
using Model.Interfaces;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        [Category("General")]
        [Description("Кнопка на клавиатуре")]
        public Keys PressedKey { get; set; }

        

        public async void WaitProcessAsync(Process process, Action<object, EventArgs> action)
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                process.WaitForExit();
            });
            action.Invoke(process, new EventArgs());
        }

        public async Task<Geometry.Plane?> CreateSurfaceAsync(ObjType objType)
        {
            var actBreak = new Action(() =>
            {
                Invoke(new Action(() => console.PrintInfo(Resources.BasePage_CreateSurfaceAync_OperationCanceled_Message, Color.Black)));
            });
            var message = @$"{Resources.BasePage_CreateSurfaceAsync_AsyncContainer_Message}";
            var actSurfaceConfirm = new Func<Tuple<bool, object>>(() =>
            {
                var selObjs = project.GetSelection()
                    .Where(x => x.ObjType == objType)
                    .ToArray();

                if (selObjs.Length < 3)
                {
                    Invoke(new Action(() => console.PrintInfo(Resources.BasePage_CreateSurfaceAsync_SelectThreeNodes_Message, Color.Orange)));
                    return new Tuple<bool, object>(false, new object());
                }
                else if (objType != ObjType.Узел & objType != ObjType.Точка)
                {
                    Invoke(new Action(() => console.PrintInfo(Resources.BasePage_CreateSurfaceAsync_SelectNodeType_Message, Color.Orange)));
                    return new Tuple<bool, object>(false, new object());
                }
                else
                {
                    var p0 = selObjs[0];
                    var p1 = selObjs[1];
                    var p2 = selObjs[2];

                    var plane = new Geometry.Plane(p0.CalcCentr(), p1.CalcCentr(), p2.CalcCentr());
                    Invoke(new Action(() => console.PrintInfo(Resources.BasePage_CreateSurfaceAsync_SurfaceSet_Message, Color.Green)));
                    return new Tuple<bool, object>(true, plane);
                }
            });
            // TODO: придумать, как решить проблему upcast-а ниже (мб передан null)
            var surfaceAwait = AsyncMethodContainer(actSurfaceConfirm, actBreak, message);
            await surfaceAwait;
            return (Geometry.Plane)surfaceAwait.Result;
        }        

        public async Task<object?> AsyncMethodContainer(Func<Tuple<bool,object>> actConfirm, Action actBreak, string cmdMessage)
        {
            object resObject = null;
            PressedKey = Keys.None;
            Invoke(new Action(() => 
            {

                var color = GetTextColor();

                DisplayText2D(cmdMessage, color, new Point2D(10, 10));
                RequestRedraw();
            }));
            await System.Threading.Tasks.Task.Run(() =>
            {
                while (true)
                {
                    if (PressedKey == Keys.E)
                    {
                        var resAction = actConfirm.Invoke();
                        if (resAction.Item1)
                        {
                            resObject = resAction.Item2;
                            break;
                        }
                        PressedKey = Keys.None;
                    }
                    if (PressedKey == Keys.Escape)
                    {
                        actBreak.Invoke();
                        break;
                    }
                }             
            });

            DisplayText2DEvent = null;
            RequestRedraw();

            PressedKey = Keys.None;
            return resObject;
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
