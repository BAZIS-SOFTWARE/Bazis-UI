using BazisAvaloniaGUI.Localization;
using BazisAvaloniaGUI.Properties;
using Model.Interfaces;
using Project.Interfaces.Tasks;
using Project.Tasks;
using Project.Tasks.Functions.FrameFunctions;
using Project.Tasks.LocalFrames;
using Project.Tasks.Materials;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {

        private void PrepareDataForCreateVolumeMaterial(string materialName, string groupName, string start, string stop, out IGroup group, out float _start, out float _stop)
        {
            var valid = float.TryParse(start, out _start) &
                float.TryParse(stop, out _stop) &
                project.MaterialsDB.Select(k => k.Key).Contains(materialName);

            group = project.GetModelGroup(groupName);
            if (!valid)
                throw new ArgumentException(Resources.InvalidCommandException);
        }

        private void PrepareDataForCreateBeamMaterial(string materialName, string groupName, string diametr, string start, string stop, out IGroup group, out float _diametr, out float _start, out float _stop)
        {
            var valid = float.TryParse(start, out _start) &
                float.TryParse(stop, out _stop) &
                float.TryParse(diametr, out _diametr) &
                project.MaterialsDB.Select(k => k.Key).Contains(materialName);

            group = project.GetModelGroup(groupName);
            if (!valid)
                throw new ArgumentException(Resources.InvalidCommandException);
        }

        private void PrepareBasicDataForCreateHeat(string groupName, string value, string start, string stop, out IGroup group, out float heatValue, out float startTime, out float stopTime)
        {
            group = project.GetModelGroup(groupName);

            var valid = float.TryParse(value.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out heatValue) &
                float.TryParse(start.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out startTime) &
                float.TryParse(stop.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out stopTime);

            if (group == null || !valid ||
                !float.IsFinite(heatValue) || !float.IsFinite(startTime) || !float.IsFinite(stopTime) ||
                startTime >= stopTime)
                throw new ArgumentException(Resources.InvalidCommandException);
        }

        private HeatData PrepareSourceDataForCreateHeat(string type, string parameterValues, HeatData heat)
        {
            var values = parameterValues.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var parameters = new List<double>();
            foreach (var value in values)
            {
                if (!double.TryParse(value.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedValue) ||
                    !double.IsFinite(parsedValue))
                    throw new ArgumentException(Resources.InvalidCommandException);

                parameters.Add(parsedValue);
            }

            if (type == "SPH")
            {
                if (parameters.Count != 2 || parameters[0] <= 0)
                    throw new ArgumentException(Resources.InvalidCommandException);

                var function = new SPH();
                function["Width"].SetValue(parameters[0]);
                function["Pulse"].SetValue(parameters[1]);
                heat.Function = function;
            }
            else if (type == "CIL")
            {
                if (parameters.Count != 3 || parameters.Any(value => value <= 0))
                    throw new ArgumentException(Resources.InvalidCommandException);

                var function = new CIL
                {
                    Length = parameters[0],
                    UpperDiam = parameters[1],
                    BottomDiam = parameters[2]
                };
                heat.Function = function;
            }
            else
                throw new ArgumentException(Resources.InvalidCommandException);

            return heat;
        }

        private HeatData PrepareFrameDataForCreateHeat(string type, string parameterValues, HeatData heat)
        {
            var values = parameterValues.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (type == "MRF")
            {
                if (values.Length != 3)
                    throw new ArgumentException(Resources.InvalidCommandException);

                var baseLine = project.GetModelGroup(values[0]);
                var refLine = project.GetModelGroup(values[1]);

                var isValidNodes = baseLine is not null && refLine is not null && baseLine.ObjType == ObjType.Узел && refLine.ObjType == ObjType.Узел;
                var isValidSpeed = float.TryParse(values[2].Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out var speed) && float.IsFinite(speed);

                if (!isValidNodes || !isValidSpeed)
                    throw new ArgumentException(Resources.InvalidCommandException);

                heat.LocalFrame = new MovedFrame(baseLine, refLine, speed);
            }
            else if (type == "SRF")
            {
                if (values.Length != 1)
                    throw new ArgumentException(Resources.InvalidCommandException);

                if (values[0] == "*")
                    heat.LocalFrame = new StaticFrame();
                else
                {
                    var plane = project.GetModelGroup(values[0]);
                    if (plane == null)
                        throw new ArgumentException(Resources.InvalidCommandException);
                    heat.LocalFrame = new StaticFrame(plane);
                }
            }
            return heat;
        }
        private void CheckMatsAndFuncs()
        {
            var matDB = project.MaterialsDB;
            if (matDB == null)
                throw new Exception(Resources.DataBase_Materials_MissingException);
            if (matDB.Count == 0)
                throw new Exception(Resources.DataBase_Materials_NoDataException);

            var funDB = project.FunctionsDB;
            if (funDB == null)
                throw new Exception(Resources.DataBase_Functions_MissingException);
            if (funDB.Count == 0)
                throw new Exception(Resources.DataBase_Functions_NoDataException);
        }

        private ICondData CreateMaterial(PropertyChangedEventArgs obj, IGroup _objectsGr)
        {
            if (project.TaskType == TaskType.Linear)
                return new BeamMatData(1, project.MaterialsDB.First().Value, _objectsGr, 0, 1);
            else if (project.TaskType == TaskType.Plain)
                return new PlateMatData(project.MaterialsDB.First().Value, _objectsGr, 0, 1);
            else if (project.TaskType == TaskType.AxiPlain)
            {
                if (_objectsGr.ObjType == ObjType.Элемент1D)
                    return new PlateMatData(project.MaterialsDB.First().Value, _objectsGr, 0, 1);
                else
                    return new MatData(project.MaterialsDB.First().Value, _objectsGr, 0, 1);
            }
            else if (project.TaskType == TaskType.Volume)
                return new MatData(project.MaterialsDB.First().Value, _objectsGr, 0, 1);
            else
            {
                if (_objectsGr.ObjType == ObjType.Элемент1D)
                    return new BeamMatData(1, project.MaterialsDB.First().Value, _objectsGr, 0, 1);
                else if (_objectsGr.ObjType == ObjType.Элемент2D)
                    return new PlateMatData(project.MaterialsDB.First().Value, _objectsGr, 0, 1);
                else
                    return new MatData(project.MaterialsDB.First().Value, _objectsGr, 0, 1);
            }
        }
    }
}
