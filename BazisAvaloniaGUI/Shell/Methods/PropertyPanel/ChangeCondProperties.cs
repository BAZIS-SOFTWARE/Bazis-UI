using BazisAvaloniaGUI.Extensions;
using BazisAvaloniaGUI.Localization;
using BazisAvaloniaGUI.Properties;
using BazisAvaloniaGUI.Utilities;
using Project.Interfaces.Tasks;
using Project.Tasks;
using Project.Tasks.Functions;
using Project.Tasks.Functions.FrameFunctions;
using Project.Tasks.LocalFrames;
using Project.Tasks.Materials;
using System;
using System.Drawing;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private void ChangeGeneralProperties(PropertyChangedEventArgs obj, ICondData cond, ref bool refresh)
        {
            if (Enum.TryParse<CondPropertyKeys>(obj.Key, out var condKey))
            {
                switch (condKey)
                {
                    case CondPropertyKeys.Value:
                        cond.Value = float.Parse(obj.NewValue);
                        break;
                    case CondPropertyKeys.ObjectsGroup:
                        HandleObjectGroupParameter(obj.NewValue, cond);
                        break;

                    case CondPropertyKeys.StartTime:
                        HandleStartTimeParameter(obj.NewValue, obj.OldValue, cond, ref refresh);
                        break;
                    case CondPropertyKeys.StopTime:
                        cond.StopTime = float.Parse(obj.NewValue);
                        break;

                    case CondPropertyKeys.Function:
                        HandleFunctionParameter(obj.NewValue, cond, ref refresh);
                        break;

                    case CondPropertyKeys.Parameter:
                        HandleParemeter(obj.LocalizedHeader.Split(" ", StringSplitOptions.RemoveEmptyEntries), obj.NewValue, cond, ref refresh);
                        break;

                    case CondPropertyKeys.Table:
                        HandleTableParameter(obj.LocalizedHeader.Split(" ", StringSplitOptions.RemoveEmptyEntries), obj.NewValue, cond, ref refresh);
                        break;

                    case CondPropertyKeys.ParameterValue:
                        HandleParameterValueParameter(obj.LocalizedHeader.Split(' ', StringSplitOptions.RemoveEmptyEntries), obj.NewValue, cond, ref refresh);
                        break;

                    case CondPropertyKeys.CoordinateSystem:
                        HandleCoordinateSystemParameter(obj.NewValue, cond, ref refresh);
                        break;

                    case CondPropertyKeys.File:
                        var cf = cond.Function as CustomFrameFunction;
                        cf.CreateEngine(obj.NewValue);
                        break;
                }
            }

            else if (Enum.TryParse<LocalFramePropertyKeys>(obj.Key, out var localFrameKey))
            {
                switch (localFrameKey)
                {
                    case LocalFramePropertyKeys.Plane:
                        HandlePlaneParameter(obj.NewValue, cond);
                        break;

                    case LocalFramePropertyKeys.Trajectory:
                        HandleTrajectoryParameter(obj.NewValue, cond);
                        break;

                    case LocalFramePropertyKeys.ReferenceLine:
                        HandleReferenceLineParameter(obj.NewValue, cond);
                        break;

                    case LocalFramePropertyKeys.Speed:
                        HandleSpeedParameter(obj.NewValue, cond, ref refresh);
                        break;

                    case LocalFramePropertyKeys.ShiftingX:
                        cond.LocalFrame.Shifting._x = float.Parse(obj.NewValue);
                        break;

                    case LocalFramePropertyKeys.ShiftingY:
                        cond.LocalFrame.Shifting._y = float.Parse(obj.NewValue);
                        break;

                    case LocalFramePropertyKeys.ShiftingZ:
                        cond.LocalFrame.Shifting._z = float.Parse(obj.NewValue);
                        break;

                    case LocalFramePropertyKeys.RotX:
                        cond.LocalFrame.Rotation_X = float.Parse(obj.NewValue);
                        break;

                    case LocalFramePropertyKeys.RotY:
                        cond.LocalFrame.Rotation_Y = float.Parse(obj.NewValue);
                        break;

                    case LocalFramePropertyKeys.RotZ:
                        cond.LocalFrame.Rotation_Z = float.Parse(obj.NewValue);
                        break;
                }
            }


        }

        private void HandleObjectGroupParameter(string newValue, ICondData cond)
        {
            var group = project.GetAllModelGroups().First(x => x.Name == newValue);
            cond.Group = group;
        }

        private void HandleStartTimeParameter(string newValue, string oldValue, ICondData cond, ref bool refresh)
        {
            var newTime = float.Parse(newValue);
            newTime = (float)Math.Round(newTime, 2);

            var oldTime = float.Parse(oldValue);
            oldTime = (float)Math.Round(oldTime, 2);

            var delta = newTime - oldTime;
            cond.StartTime = newTime;
            cond.StopTime += delta;
            refresh = true;
        }

        private void HandleFunctionParameter(string newValue, ICondData cond, ref bool refresh)
        {
            switch (newValue)
            {
                case "*":
                    cond.Function = null;
                    break;
                case "SPH":
                    cond.Function = new SPH();
                    break;
                case "CIL":
                    cond.Function = new CIL();
                    break;
                case "FHEX":
                    cond.Function = new FHEX();
                    break;
                case "DHEX":
                    cond.Function = new DHEX();
                    break;
                case "TT":
                    cond.Function = new TT();
                    break;
                case "PT":
                    cond.Function = new PT();
                    break;
                default:
                    // Avalonia: диалог выбора файла асинхронный, поэтому функция назначается
                    // и свойства перерисовываются после его закрытия.
                    LoadPythonFile(file =>
                    {
                        if (!string.IsNullOrEmpty(file))
                        {
                            var pythonFunction = new CustomFrameFunction();
                            pythonFunction.CreateEngine(file);
                            cond.Function = pythonFunction;
                            // Пока был открыт диалог, выделение в навигаторе могло смениться/сброситься.
                            if (navigator.SelectedNode?.Index is int index)
                                Navigator_SelectCondEvent(index);
                        }
                        else
                            console.PrintInfo(Resources.ChangeCondProperties_ChangeGeneralProperties_FileNotSelected_Message, Color.Red);
                    });

                    break;
            }
            refresh = true;
        }

        private void HandleParemeter(string[] headerParts, string newValue, ICondData cond, ref bool refresh)
        {
            if (newValue == ParameterKind.Digit.ToString())
                cond.Function[headerParts[1]] = new Parameter(headerParts[1], ParameterType.Constant, 0);
            else if (newValue == "Table")
                cond.Function[headerParts[1]] = new TableParameter(project.FunctionsDB.First().Value,
                    headerParts[1],
                    cond.Function.GetParameters().
                    First(x => x.ParameterType == ParameterType.Variable));
            refresh = true;
        }

        private void HandleTableParameter(string[] headerParts, string newValue, ICondData cond, ref bool refresh)
        {
            // TODO подумать об регулярных выражениях для поиска
            var tableParam = cond.Function.GetTableParameters().First(x => x.Table.Name == headerParts[1]);
            var setParam = cond.Function.GetParameters().First(x => x.Name == newValue);

            if (setParam.Name != tableParam.Name)
                tableParam.Parameter = setParam;

            refresh = true;
        }

        private void HandleParameterValueParameter(string[] headerParts, string newValue, ICondData cond, ref bool refresh)
        {
            if (headerParts.Length == 1 && double.TryParse(newValue, out var parsed1))
                cond.Value = parsed1;
            else
            {
                var paramName = headerParts[1];
                var parameter = cond.Function[paramName];

                if (parameter.ParameterKind == ParameterKind.Table)
                {
                    var variable = cond.Function
                        .GetParameters()
                        .FirstOrDefault(x => x.ParameterType == ParameterType.Variable);

                    if (variable != null)
                        cond.Function[paramName] = new TableParameter(project.FunctionsDB[newValue], paramName, variable);
                }
                else if (double.TryParse(newValue, out var parsed2))
                    parameter.SetValue(parsed2);
            }
            refresh = true;
        }

        private void HandleCoordinateSystemParameter(string newValue, ICondData cond, ref bool refresh)
        {
            if (newValue == "SRF")
                cond.LocalFrame = new StaticFrame();
            else if (newValue == "MRF")
                cond.LocalFrame = new MovedFrame();
            else
                cond.LocalFrame = null;

            refresh = true;
        }

        private void HandlePlaneParameter(string newValue, ICondData cond)
        {
            if (cond.LocalFrame is StaticFrame srf)
            {
                var group = project.GetAllModelGroups().FirstOrDefault(x => x.Name == newValue);

                if (group == null)
                {
                    var temp = new StaticFrame();
                    temp.Rotation_X = srf.Rotation_X;
                    temp.Rotation_Y = srf.Rotation_Y;
                    temp.Rotation_Z = srf.Rotation_Z;
                    temp.Shifting = srf.Shifting;

                    cond.LocalFrame = temp;
                }
                else
                    srf.BaseGroup = group;
            }
        }

        private void HandleTrajectoryParameter(string newValue, ICondData cond)
        {
            var mrf = cond.LocalFrame as MovedFrame;
            var group = project.GetAllModelGroups().First(x => x.Name == newValue);
            mrf.BaseLine = group;
        }

        private void HandleReferenceLineParameter(string newValue, ICondData cond)
        {
            var mrf = cond.LocalFrame as MovedFrame;
            var group = project.GetAllModelGroups().First(x => x.Name == newValue);
            mrf.RefLine = group;
        }

        private void HandleSpeedParameter(string newValue, ICondData cond, ref bool refresh)
        {
            var mrf = cond.LocalFrame as MovedFrame;
            mrf.Velocity = float.Parse(newValue);

            // добавим корректировку времени остановки, если изменена скорость
            var time = mrf.CalcMotionTime();

            cond.StopTime = cond.StartTime + (float)Math.Round(time, 2);
            refresh = true;
        }

        private void ChangeClampProperties(PropertyChangedEventArgs obj, ClampData clampCond, ref bool flag)
        {
            ChangeGeneralProperties(obj, clampCond, ref flag);

            if (Enum.TryParse(obj.Key, out ClampPopertyKeys clampRes) && clampRes == ClampPopertyKeys.Type)
                clampCond.TrySetKind(Converters.ConvertClampKindKeysToClampKind(Enum.Parse<ClampKindKeys>(obj.NewValue.ToString())).ToString());
            else if (Enum.TryParse(obj.Key, out CondPropertyKeys condRes) && condRes == CondPropertyKeys.Direction)
                clampCond.Direction = obj.NewValue.ToEnum<Direction>();
        }

        private void ChangeMatProperties(PropertyChangedEventArgs obj, MatData matCond, ref bool flag)
        {
            ChangeGeneralProperties(obj, matCond, ref flag);

            if (Enum.TryParse<MaterialPropertyKeys>(obj.Key, out var res))
            {
                switch (res)
                {
                    case MaterialPropertyKeys.Material:
                        matCond.Material = project.MaterialsDB[obj.NewValue.ToString()];
                        break;
                    case MaterialPropertyKeys.Diametr:
                        (matCond as BeamMatData).Diameter = double.Parse(obj.NewValue);
                        break;
                    case MaterialPropertyKeys.Thickness:
                        (matCond as PlateMatData).Thickness = double.Parse(obj.NewValue);
                        break;
                }
            }
            // TO DO дописать метод, так чтобы изменялись все свойства
        }

        private void ChangeHeatProperties(PropertyChangedEventArgs obj, HeatData heatCond, ref bool flag)
        {
            ChangeGeneralProperties(obj, heatCond, ref flag);
        }

        private void ChangeLoadProperties(PropertyChangedEventArgs obj, LoadData loadData, ref bool flag)
        {
            ChangeGeneralProperties(obj, loadData, ref flag);

            if (Enum.TryParse(obj.Key, out CondPropertyKeys condRes) && condRes == CondPropertyKeys.Direction)
                loadData.Direction = obj.NewValue.ToEnum<Direction>();

            else if (Enum.TryParse(obj.Key, out LoadPropertyKeys loadRes) && loadRes == LoadPropertyKeys.Type)
                loadData.LoadKind = Converters.ConvertLoadKindKeysToLoadKind(Enum.Parse<LoadKindKeys>(obj.NewValue));

        }

        private void ChangeMediaProperties(PropertyChangedEventArgs obj, MediaData mediaData, ref bool flag)
        {
            ChangeGeneralProperties(obj, mediaData, ref flag);
        }
    }
}
