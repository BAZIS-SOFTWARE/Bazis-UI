using global::Avalonia.Controls;
using global::Avalonia.Threading;
using BazisGUI.Avalonia.Localization;
using BazisGUI.Avalonia.Measurement;
using BazisGUI.Avalonia.Utilities;
using Geometry;
using Model.Interfaces;
using Model.Interfaces.MeshObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace BazisGUI.Avalonia.Shell
{
    // Из GUI/Methods/UtilityToolStrip.cs перенесены измерения и SelectObjectAsync. Сечения, скрытие плоскостью
    // и захват данных (transform feedback) работают с буферами и отсекателем сцены и в Avalonia ещё не перенесены.
    internal partial class MainWindow
    {
        private void измеритьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var btn = sender as MenuItem;
                // CheckOnClick: MenuItem с ToggleType = CheckBox переключает IsChecked до события Click.
                if (btn.IsChecked)
                {

                    var form = new Window()
                    {
                        Name = "measureForm",
                        Title = "Панель измерений",
                        Icon = Icon,
                        SizeToContent = SizeToContent.WidthAndHeight,
                        CanResize = false,
                        Topmost = true,
                        FontFamily = FontFamily,
                        FontSize = FontSize
                    };

                    form.Closed += (s1, s2) =>
                    {
                        btn.IsChecked = false;
                        HideAllGeometryObjects();
                        HideAllText3D();
                        RequestRedraw();
                    };

                    var measuringControl = new MeasuringSet();
                    measuringControl.PreparingMeasureEvent += (ar) =>
                    {
                        SelectedObjects = Converters.ConvertObjTypeToSelectionType(ar);
                        HideAllGeometryObjects();
                        HideAllText3D();
                        RequestRedraw();
                    };
                    measuringControl.MakeMeasureEvent += MeasuringControl_MakeMeasureEvent;
                    form.Content = measuringControl;

                    form.Position = Position;
                    form.Show(this);
                }
                else
                {
                    var form = OwnedWindows.FirstOrDefault(x => x.Name == "measureForm");
                    if (form != null)
                    {
                        form.Close();
                        btn.IsChecked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
        private async void MeasuringControl_MakeMeasureEvent(object arg1, MeasureEventArgs arg2)
        {
            if (!Converters.TryConvertSelectionTypeToObjType(SelectedObjects, out ObjType res))
            {
                console.PrintInfo($"{Resources.UtilityToolStrip_Measuring_InvalidSeletedTypeError} \"{SelectedObjects}\"", Color.Red);
                return;
            }
            try
            {
                switch (arg2.Kind)
                {
                    case MeasureKind.DistancePointToPoint:
                        {
                            DistancePointToPoint(SelectedObjects);
                            break;
                        }
                    case MeasureKind.DistancePointToPlane:
                        {
                            await DistancePointToPlane(SelectedObjects);
                            break;
                        }
                    case MeasureKind.Path:
                        await CreatePathAsync();
                        break;
                    case MeasureKind.Square:
                        {
                            CalcSquare(SelectedObjects);
                            break;
                        }

                    case MeasureKind.Volume:
                        {

                            CalcVolume(SelectedObjects);
                            break;
                        }

                    default:
                        break;
                }
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

        public async Task<List<IPoint>> CreatePathAsync()
        {
            var nodes = new List<IPoint>();

            var message = @"Идет построение пути...";
            console.PrintInfo(message, Color.Black);

            var path = 0.0;
            while (true)
            {
                message = $@"Выберите {ObjType.Узел} и нажмите на клавишу ""E"" для подтверждения или клавишу ""ESC"" для отмены";
                // Esc здесь — штатное завершение построения пути, а не отмена: сообщение не печатаем.
                if (await SelectObjectAsync(ObjType.Узел, message, reportCancel: false) is IPoint node)
                {
                    nodes.Add(node);
                    project.ClearSelection(ObjType.Узел);
                }
                else break;

                if (nodes.Count > 1)
                {
                    var line = new Segment3D(nodes[nodes.Count - 1].Position, nodes[nodes.Count - 2].Position);
                    console.PrintInfo($"{Resources.UtilityToolStrip_Distance_Output} : {path += line.GetLength()}", Color.Black);
                    DisplayDistance(line);

                    var coord = line.P0.Sum(line.P1).Div(2);

                    DisplayText3D(path.ToString(), Color.FromArgb(0, 0, 0), coord);

                    RequestRedraw();
                }
            }
            return nodes;
        }

        public async Task<object> SelectObjectAsync(ObjType objType, string message, bool reportCancel = true)
        {
            var actBreak = new Action(() =>
            {
                if (reportCancel)
                    Dispatcher.UIThread.Invoke(new Action(() => console.PrintInfo(Resources.UtilityToolStrip_SelectObjectAsync_OperationCanceled_Message, Color.Black)));
            });

            var actPointConfirm = new Func<Tuple<bool, object>>(() =>
            {
                var selObjs = project.GetSelected(objType)
                    .Select(number => project.GetModelObject(objType, number));

                if (selObjs.Count() == 0)
                {
                    Dispatcher.UIThread.Invoke(new Action(() => console.PrintInfo($"{Resources.UtilityTiilStrip_SelectObjectAsync_NoObjectSelected_Message} {Localization.Localization.GetSelectionTypeLocalization(Converters.ConvertObjTypeToSelectionType(objType))}!", Color.Orange)));
                    return new Tuple<bool, object>(false, new object());
                }
                else if (selObjs.Count() > 1)
                {
                    Dispatcher.UIThread.Invoke(new Action(() => console.PrintInfo($"{Resources.UtilityTiilStrip_SelectObjectAsync_SelectOne_Message} {Localization.Localization.GetSelectionTypeLocalization(Converters.ConvertObjTypeToSelectionType(objType))}!", Color.Orange)));
                    return new Tuple<bool, object>(false, new object());
                }
                else
                {
                    var node = selObjs.First();
                    Dispatcher.UIThread.Invoke(new Action(() => console.PrintInfo($"{Resources.UtilityTiilStrip_SelectObjectAsync_Selected_Message} {Localization.Localization.GetSelectionTypeLocalization(Converters.ConvertObjTypeToSelectionType(objType))} {Resources.UtilityTiilStrip_SelectObjectAsync_WithNumber_Message} {node.Number}", Color.Green)));
                    return new Tuple<bool, object>(true, node);
                }
            });

            var pointAwait = AsyncMethodContainer(actPointConfirm, actBreak, message);
            await pointAwait;
            return pointAwait.Result;
        }

        private void CalcVolume(SelectionType selection)
        {
            var objType = Converters.ConvertSelectionTypeToObjType(selection);
            var selObjs = project.GetSelected(objType)
                .Select(number => project.GetModelObject(objType, number));

            var vol = 0.0f;
            foreach (var obj in selObjs)
            {
                var e3DObj = (IElement3D)obj;
                vol += (float)e3DObj.CalcVolume();
            }
            console.PrintInfo($"{Resources.UtilityToolStrip_CalcVolume_Output} : {vol}", Color.Black);
        }

        private void CalcSquare(SelectionType select)
        {
            var objType = Converters.ConvertSelectionTypeToObjType(select);
            var selObjs = project.GetSelected(objType)
                .Select(number => project.GetModelObject(objType, number));
            var square = 0.0;
            foreach (var obj in selObjs)
            {
                var sObj = (ISquare)obj;
                square += sObj.CalcSquare();
            }
            console.PrintInfo($"{Resources.UtilityToolStrip_CalcSquare_Output} : {square}", Color.Black);
        }

        private async Task DistancePointToPlane(SelectionType objTypeStr)
        {
            var objType = Converters.ConvertSelectionTypeToObjType(objTypeStr);

            var plane = await CreateSurfaceAsync(objType);
            if (plane is null)
                return;

            project.ClearSelection(objType);
            var message = $@"{Resources.UtilityToolStrip_DistancePointToPlane_InstructionPart1} {Localization.Localization.GetSelectionTypeLocalization(SelectionType.Nodes)} {Resources.UtilityToolStrip_DistancePointToPlane_InstructionPart1}";
            var res = SelectObjectAsync(objType, message);
            await res;

            if (res.Result is IPoint point)
            {
                var proj = point.Position.GetPointProectionOnPlane(plane);
                var line = new Segment3D(point.Position, proj);
                console.PrintInfo($"{Resources.UtilityToolStrip_Distance_Output} : {line.GetLength()}", Color.Black);
                DisplayDistance(line);
                RequestRedraw();
            }
        }

        private void DistancePointToPoint(SelectionType objTypeStr)
        {
            var objType = Converters.ConvertSelectionTypeToObjType(objTypeStr);
            var selObjs = project.GetSelected(objType)
                .Select(number => project.GetModelObject(objType, number))
                .ToList();

            if (selObjs.Count() > 1)
            {
                var nodes = selObjs.Select(x => (IPoint)x);
                var p0 = nodes.First();
                var p1 = nodes.Last();
                var line = new Segment3D(p0.Position, p1.Position);

                console.PrintInfo($"{Resources.UtilityToolStrip_Distance_Output} : {line.GetLength()}", Color.Black);

                DisplayDistance(line);
                RequestRedraw();
            }
            else console.PrintInfo($"{Resources.UtilityToolStrip_DistancePointToPoint_EmptySelectionErrorMessage}: {Localization.Localization.GetSelectionTypeLocalization(objTypeStr)}", Color.Red);
        }
    }
}
