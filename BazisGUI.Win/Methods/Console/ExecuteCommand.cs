using BazisGUI.Win.Console;
using BazisGUI.Win.Console.Enums;
using BazisGUI.Win.Extensions;
using BazisGUI.Win.Properties;
using BazisGUI.Win.Utilities;
using GmshApi;
using MaterialDB.FunctionData;
using MaterialDB.MaterialData;
using Model.Interfaces;
using OperationalController;
using Project.Interfaces.Tasks;
using Project.Tasks;
using Project.Tasks.Materials;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        Dictionary<string, GenCmd> genCmds = new Dictionary<string, GenCmd>()
        {
            { "Load project",GenCmd.LoadProject},
            { "Load material db", GenCmd.LoadMaterialDB },
            { "Load function db", GenCmd.LoadFunctionDB },
            { "Save project",GenCmd.SaveProject},
            { "Solve project",GenCmd.SolveProject},
            { "Renumber mesh",GenCmd.RenumberMesh},
            { "Move node",GenCmd.MoveNodes},
            { "Move mesh",GenCmd.MoveMesh},
            { "Rotate mesh",GenCmd.RotateMesh},
            { "Generate mesh",GenCmd.GenerateMesh},
            { "Find free nodes",GenCmd.FindFreeNodes},
            { "Find Coincident",GenCmd.FindCoincident},
            { "Find 3D elements",GenCmd.FindVolElems},
            { "Find object",GenCmd.FindObject},
            { "Get related geometry objects", GenCmd.GetRelatedGeometryObjects },
            { "Get coordinate point", GenCmd.GetCoordinatePoint },
            { "Get curve length", GenCmd.GetCurveLength },
            { "Connect with beams",GenCmd.BeamConnection},
            { "Set precision level",GenCmd.SetLevel },
            { "Merge elements sets",GenCmd.MergeElementSets },
            { "Build 2D mesh",GenCmd.CreateMesh2DPoligon },
            { "Create point",GenCmd.CreatePoint },
            { "Create point by vector", GenCmd.CreatePointByVector },
            { "Create point by projection onto curve", GenCmd.CreatePointProjectionOntoCurve },
            { "Create point by projection onto plane", GenCmd.CreatePointProjectionOntoPlane },
            { "Create curve",GenCmd.CreateCurve },
            { "Create surface",GenCmd.CreateSurface },
            { "Create task", GenCmd.CreateTask },
            { "Change task type", GenCmd.ChangeTaskType },
            { "Change task kind", GenCmd.ChangeTaskKind },
            { "Set mesh point", GenCmd.SetMeshPoint },
            { "Set mesh curve", GenCmd.SetMeshCurve },
            { "Set regular mesh surface", GenCmd.SetRegularSurface },
            { "Embedded mesh", GenCmd.SetEmbedded },
            { "Set min size", GenCmd.SetMinSize },
            { "Set max size", GenCmd.SetMaxSize },
            { "Algo2D", GenCmd.Algo2D },
            { "Algo3D", GenCmd.Algo3D },
            { "Create surface nodes group", GenCmd.CreateSurfaceNodesGroup },
            { "Create group by geometry objs", GenCmd.CreateGroupByGeoObjs},
            { "Create group", GenCmd.CreateGroup },
            { "Scale factor", GenCmd.ScaleFactor },
            { "Extrude along curve",GenCmd.ExtrudeCurve },
            { "Extrude along curve by set name", GenCmd.ExtrudeCurveBySetName },
            { "Extrude 1D from point", GenCmd.Extrude1DFromPoint },
            { "Extrusion by rotation",GenCmd.ExtrudeRotate },
            { "Save STEP", GenCmd.SaveSTEP },
            { "Create volume material", GenCmd.CreateVolumeMaterial },
            { "Create beam material", GenCmd.CreateBeamMaterial },
            { "Create heat", GenCmd.CreateHeat },
            { "Select objects", GenCmd.SelectObjects },
            { "Quit",GenCmd.Exit }
        };

        Dictionary<GenCmd, string[]> subCmds = new Dictionary<GenCmd, string[]>()
        {
            { GenCmd.LoadProject, new[] { "path" } },
            { GenCmd.SaveProject, new[] { "path" } },
            { GenCmd.SolveProject,new string[] { } },
            { GenCmd.LoadMaterialDB, new[] { "path" }  },
            { GenCmd.LoadFunctionDB, new[] { "path" }  },
            { GenCmd.RenumberMesh, new[] { "type:initial number" } },
            { GenCmd.MoveMesh, new[] { "move", "x,y,z" } },
            { GenCmd.MoveNodes, new[] { "move" } },
            { GenCmd.RotateMesh, new[] { "rotate", "x,y,z:angle" } },
            { GenCmd.FindFreeNodes,new string[] { } },
            { GenCmd.FindCoincident, new[] { "nodes", "distance" } },
            { GenCmd.FindVolElems, new[] { "measure" } },
            { GenCmd.FindObject, new[] { "type,number" } },
            { GenCmd.BeamConnection, new [] { "search radius", "max quantity", "group#1", "group#2" } },
            { GenCmd.SetLevel, new[] { "type", "precision level" } },
            { GenCmd.MergeElementSets, new[] { "type", "set#1", "set#2" } },
            { GenCmd.CreateMesh2DPoligon, new[] { "x1,y1", "x2,y2", "x3,y3", "x4,y4", "number of elements" } },
            { GenCmd.GetRelatedGeometryObjects, new[] { "geoDim", "geoNumbers", "up/low" } },
            { GenCmd.GetCoordinatePoint, new[] { "point" } },
            { GenCmd.GetCurveLength, new[] { "curve" } },
            { GenCmd.CreatePoint, new [] { "x,y,z" } },
            { GenCmd.CreatePointByVector, new[]{ "copy_point#1", "direction_point#2", "offset" } },
            { GenCmd.CreatePointProjectionOntoCurve, new string[]{ "point", "curve" } },
            { GenCmd.CreatePointProjectionOntoPlane, new string[]{ "point", "surface" } },
            { GenCmd.CreateCurve, new [] { "point#1","point#2" } },
            { GenCmd.CreateSurface, new[] { "curves forming the contour", "curve#1,curve#2,curve#3..." } },
            { GenCmd.SetMeshPoint, new [] { "number", "size" }},
            { GenCmd.SetMeshCurve, new [] { "number", "points count", "Progression/Bump/Beta", "factor"}},
            { GenCmd.SetRegularSurface, new [] { "number", "corner points", "Left/Right,", "quad/tria" }},
            { GenCmd.SetEmbedded, new[] { "target type", "target number", "embedded type", "embedded entities" } },
            { GenCmd.SetMinSize, new [] { "size" } },
            { GenCmd.SetMaxSize, new [] { "size" } },
            { GenCmd.Algo2D, new[] { "MeshAdapt/Automatic/InitialMeshOnly/Delaunay/FrontalDelaunay/BAMG/FrontalDelaunayQuads/PackingOfParallelograms/QuasiStructuredQuad" } },
            { GenCmd.Algo3D, new[] { "Delaunay/InitialMeshOnly/Frontal/MMG3D/RTree/HXT" } },
            { GenCmd.CreateSurfaceNodesGroup, new []{ "set name" } },
            { GenCmd.CreateGroupByGeoObjs, new []{ "meshDim", "geoDim", "номер гео.объекта" }},
            { GenCmd.CreateGroup, new[] { "set name" } },
            { GenCmd.ScaleFactor, new[] { "scale"} },
            { GenCmd.ExtrudeCurve, new[] { "Surface number", "curve#1,curve#2,curve#3...", "point", "step", "transfinite mesh 1-yes, 0-no" } },
            { GenCmd.ExtrudeCurveBySetName, new[] { "Set name", "curve#1,curve#2,curve#3...", "point", "step", "transfinite mesh 1-yes, 0-no" } },
            { GenCmd.Extrude1DFromPoint, new[] { "Node number", "curve#1,curve#2,curve#3...", "point", "step" } },
            { GenCmd.ExtrudeRotate, new[] { "Element 2D", "angle in degrees", "point", "XYZ rotation axi", "transfinite mesh 1-yes, 0-no" } },
            { GenCmd.SaveSTEP, new [] { "path" } },
            { GenCmd.CreateVolumeMaterial, new[] { "Material name", "groupName", "start", "stop"} },
            { GenCmd.CreateBeamMaterial, new[] { "Material name", "groupName", "diametr", "start", "stop"} },
            { GenCmd.CreateHeat, new[] { "value", "SPH/CIL", "source parameter values", "MRF/SRF", "frame parameter values", "group", "start", "stop"} },
            { GenCmd.Exit, Array.Empty<string>() },
            { GenCmd.GenerateMesh, Array.Empty<string>()},
            { GenCmd.CreateTask, Array.Empty<string>() },
            { GenCmd.ChangeTaskType, new[] { "Linear/Plain/AxiPlain/Volume/Volume_mixed" } },
            { GenCmd.ChangeTaskKind, new[] { "Chemical/Termal/Mechanical" }},
            { GenCmd.SelectObjects, new[] { "point/curve/surface/node/line/element2d/element3d" } }
        };

        private void PrintAllCommands()
        {
            console.PrintInfo($"{Resources.AvailableCommands}:", Color.Black);

            foreach (var item in genCmds)
            {
                var args = string.Join(" ", subCmds[item.Value].Select(s => $"\"{s}\""));
                console.PrintInfo($"- \"{item.Key}\" {args}", Color.Black);
            }
        }

        /// <summary>
        /// Выполняет одну команду консоли, заданную в виде строки.
        /// </summary>
        /// <param name="line">Строка с командой и аргументами. Разбор полей выполняется через <see cref="FieldsParser.ParseLine(string)"/>.</param>
        /// <returns>
        /// Асинхронно возвращает строковое значение результата выполнения команды (если команда возвращает значение),
        /// либо пустую строку, если результат отсутствует.
        /// </returns>
        /// <remarks>
        /// Поведение:
        /// - Разбирает входную строку на поля.
        /// - Проверяет, что первая часть соответствует зарегистрированной команде в словаре <c>genCmds</c>.
        /// - Проверяет количество аргументов по описанию в <c>subCmds</c>.
        /// - Добавляет команду в историю через <c>ConsoleHistory.AddComand</c>.
        /// - Выполняет действие, соответствующее перечислению <c>GenCmd</c>, используя дополнительные парсеры и вспомогательные методы класса.
        private async Task<string> ExecuteCommand(string line)
        {
            var cmds = FieldsParser.ParseLine(line);
            var returnValue = string.Empty;
            if (cmds.Count != 0)
            {
                if (!this.genCmds.ContainsKey(cmds[0]))
                    throw new Exception(Resources.NotACommandException);
                if (subCmds[genCmds[cmds[0]]].Length != cmds.Count - 1)
                    throw new Exception(Resources.InvalidArgumentsNumberException);

                ConsoleHistory.AddComand(line);
                switch (genCmds[cmds[0]])
                {
                    case GenCmd.CreateMesh2DPoligon:
                        ParsePolygonPoints(cmds[1], cmds[2], cmds[3], cmds[4], cmds[5], out var p1, out var p2, out var p3, out var p4, out var numberOfElemsInt);
                        CreateMesh2DPoligon(p1, p2, p3, p4, numberOfElemsInt);
                        break;
                    case GenCmd.MergeElementSets:
                        MergeEventSets(cmds[1], cmds[2], cmds[3]);
                        break;
                    case GenCmd.FindObject:
                        FindObjectParserStr(cmds[1], out var type, out var number);
                        FindObject(type, number);
                        break;
                    case GenCmd.LoadProject:
                        await OpenProject(cmds[1]);
                        break;
                    case GenCmd.SaveProject:
                        project.Save(cmds[1]);
                        break;
                    case GenCmd.CreateGraph:
                        break;
                    case GenCmd.RenumberMesh:
                        console_RenumberMeshEvent(cmds[1]);
                        break;
                    case GenCmd.MoveMesh:
                        ParseVector(cmds[2], out var x, out var y, out var z);
                        console_ModelShiftCoordinateEvent(x, y, z);
                        break;
                    case GenCmd.RotateMesh:
                        console_ModelRotateEvent(cmds[2]);
                        break;
                    case GenCmd.MoveNodes:
                        if (cmds[1] == Resources.MoveRotNodesOption)
                            console_NodesShiftCoordinate();
                        break;
                    case GenCmd.FindFreeNodes:
                        console_FindFreeNodesEvent();
                        break;
                    case GenCmd.FindVolElems:
                        FindVolElems(cmds[1]);
                        break;
                    case GenCmd.FindCoincident:
                        if (cmds[1] == "nodes")
                            await FindCoincidentNodes(float.Parse(cmds[2]));
                        break;
                    case GenCmd.BeamConnection:
                        PrepareDataForConnectionBeam(cmds[1], cmds[2], out double radius, out int maxBeams);
                        returnValue = BeamConnection(radius, maxBeams, cmds[3], cmds[4]);
                        break;
                    case GenCmd.SolveProject:
                        //TODO: Реализовать метод
                        break;
                    case GenCmd.SetLevel:
                        SetLevel(cmds[1], cmds[2]);
                        break;
                    case GenCmd.Exit:
                        Application.Exit();
                        break;
                    case GenCmd.CreatePoint:
                        returnValue = GeometryParser(CreateCommandType.AddPoint, [cmds[1]]).ToString();
                        break;
                    case GenCmd.CreateCurve:
                        returnValue = GeometryParser(CreateCommandType.AddCurve, [cmds[1], cmds[2]]).ToString();
                        break;
                    case GenCmd.CreateSurface:
                        returnValue = GeometryParser(CreateCommandType.AddSurface, [cmds[2]]).ToString();
                        break;
                    case GenCmd.ExtrudeCurve:
                        returnValue = ExtrudeCurveBySurface(cmds[1], cmds[2], cmds[3], cmds[4], cmds[5]);
                        break;
                    case GenCmd.ExtrudeCurveBySetName:
                        returnValue = ExtrudeCurveBySetName(cmds[1], cmds[2], cmds[3], cmds[4], cmds[5]);
                        break;
                    case GenCmd.Extrude1DFromPoint:
                        returnValue = Extrude1DFromPoint(cmds[1], cmds[2], cmds[3], cmds[4]);
                        break;
                    //case GenCmd.ExtrudeRotate:
                    //    ExtrudeEvent(new CreateExtruderEventArgs(ExtruderType.Rotate, new List<string> { cmds[1], cmds[2], cmds[3], cmds[4], cmds[5] }));
                    //    break;
                    case GenCmd.CreatePointByVector:
                        returnValue = GeometryParser(CreateCommandType.AddPointByVector, [cmds[1], cmds[2], cmds[3]]).ToString();
                        break;
                    case GenCmd.CreatePointProjectionOntoPlane:
                        returnValue = GeometryParser(CreateCommandType.AddPointProjectToSurface, [cmds[1], cmds[2]]).ToString();
                        break;
                    case GenCmd.CreatePointProjectionOntoCurve:
                        returnValue = GeometryParser(CreateCommandType.AddPointProjectToCurve, [cmds[1], cmds[2]]).ToString();
                        break;
                    case GenCmd.GenerateMesh:
                        создать3DСеткуToolStripMenuItem_Click(null, EventArgs.Empty);
                        btnSelect.Text = Resources.btnSelect_Text_Objects;
                        break;
                    case GenCmd.SetMeshPoint:
                        PrepareDataForSetMeshPoint(cmds[1], cmds[2], out int _numberPoint, out double _meshSize);
                        SetMeshPoint(_numberPoint, _meshSize);
                        break;
                    case GenCmd.SetMeshCurve:
                        PrepareDataForSetMeshCurve(cmds[1], cmds[2], cmds[3], cmds[4], out int _number, out CurveMeshingSettings settings);
                        SetMeshCurve(_number, settings);
                        break;
                    case GenCmd.SetRegularSurface:
                        PrepareDataForSetRegularMeshSurface(cmds[1], cmds[2], cmds[3], cmds[4], out int _numberSurface, out Arrangement _arrangement, out List<int> _cornerPoints, out bool _quadratization);
                        SetRegularMeshSurface(_numberSurface, _cornerPoints, _arrangement, _quadratization);
                        break;
                    case GenCmd.SetEmbedded:
                        PrepareDataForSetEmbeddedMeshSurface(cmds[1], cmds[2], cmds[3], cmds[4], out int _targetType, out int _targetNumber, out int _embeddedType, out IEnumerable<int> _embeddedNumbers);
                        SetEmbeddedMesh(_targetType, _targetNumber, _embeddedType, _embeddedNumbers);
                        break;
                    case GenCmd.SetMinSize:
                        project.SetMeshMinimumSize(double.Parse(cmds[1]));
                        break;
                    case GenCmd.SetMaxSize:
                        project.SetMeshMaximumSize(double.Parse(cmds[1]));
                        break;
                    case GenCmd.Algo2D:
                        project.SetMeshAlgorithm2D(cmds[1].ToEnum<MeshAlgorithm2D>());
                        break;
                    case GenCmd.Algo3D:
                        project.SetMeshAlgorithm3D(cmds[1].ToEnum<MeshAlgorithm3D>());
                        break;
                    case GenCmd.ScaleFactor:
                        project.SetMeshSizeFactor(double.Parse(cmds[1]));
                        break;
                    case GenCmd.SaveSTEP:
                        project.ExportGeometry(cmds[1]);
                        break;
                    case GenCmd.CreateSurfaceNodesGroup:
                        returnValue = project.CreateOpenSurfaceNodesGroup(cmds[1]);
                        break;
                    case GenCmd.CreateGroupByGeoObjs:
                        PrepareDataForCreateGroupByGeo(cmds[1], cmds[2], cmds[3], out int _meshDim, out int _geoDim, out int _tag);
                        returnValue = project.CreateGroupByGeoObjs(_meshDim, _geoDim, _tag);
                        break;
                    case GenCmd.LoadMaterialDB:
                        project.MaterialsDB = LoadDB<MaterialDBData>(cmds[1]);
                        break;
                    case GenCmd.LoadFunctionDB:
                        project.FunctionsDB = LoadDB<FunctionDBData>(cmds[1]);
                        break;
                    case GenCmd.CreateTask:
                        project.CreateTask();
                        break;
                    case GenCmd.ChangeTaskType:
                        ChangeTaskType(cmds[1]);
                        break;
                    case GenCmd.ChangeTaskKind: 
                        ChangeTaskKind(cmds[1]);
                        break;
                    case GenCmd.CreateVolumeMaterial:
                        PrepareDataForCreateVolumeMaterial(cmds[1], cmds[2], cmds[3], cmds[4], out IGroup groupVolumeMaterial, out float _startV, out float _stopV);
                        var matV = new MatData(project.MaterialsDB[cmds[1]], groupVolumeMaterial, _startV, _stopV);
                        project.AddTaskData(matV);
                        returnValue = matV.Value.ToString();
                        break;
                    case GenCmd.CreateBeamMaterial:
                        PrepareDataForCreateBeamMaterial(cmds[1], cmds[2], cmds[3], cmds[4], cmds[5], out IGroup groupBeamMaterial, out float _diametr, out float _startB, out float _stopB);
                        var matB = new BeamMatData(_diametr, project.MaterialsDB[cmds[1]], groupBeamMaterial, _startB, _stopB);
                        project.AddTaskData(matB);
                        returnValue = matB.Value.ToString();
                        break;
                    case GenCmd.CreateHeat:
                        CheckMatsAndFuncs();
                        PrepareBasicDataForCreateHeat(cmds[6], cmds[1], cmds[7], cmds[8], out IGroup heatGroup, out float heatValue, out float heatStart, out float heatStop);
                        var heat = new HeatData(heatGroup, heatStart, heatStop) { Value = heatValue };
                        heat = PrepareSourceDataForCreateHeat(cmds[2], cmds[3], heat);
                        heat = PrepareFrameDataForCreateHeat(cmds[4], cmds[5], heat);
                        project.AddTaskData(heat);
                        returnValue = heat.Value.ToString(CultureInfo.InvariantCulture);
                        break;
                    case GenCmd.CreateGroup:
                        var set = project.GetAllModelSetsInfo().Where(x => x.Name == cmds[1]).First();
                        var objects = set.GetNumbers().Select(num => project.GetModelObject(set.ObjType, num)).ToList();
                        project.CreateGroup(objects);
                        var group = project.GetAllModelGroups().Last();
                        console.PrintInfo($"{Resources.SelectSetEvent_CreateGroupBySet_Message}: {group.Name}", Color.Black);
                        returnValue = group.Name;
                        break;
                    case GenCmd.GetRelatedGeometryObjects:
                        PrepareDataForGetRelatedGeometryObjects(cmds[1], cmds[2], cmds[3], out int _geometryDim, out int _geoNumber, out bool _lvl);
                        var (upper, lower) = GetAdjacentGeometryObjects(_geometryDim, _geoNumber);
                        var relatedObjects = _lvl ? upper : lower;
                        returnValue = string.Join(",", relatedObjects);
                        break;
                    case GenCmd.GetCoordinatePoint:
                        var coord = project.GetGeoObjPoints(0, int.Parse(cmds[1]), []);
                        returnValue = string.Join(";", coord);
                        break;
                    case GenCmd.SelectObjects:
                        if (!TryParseObjType(cmds[1], out ObjType objType))
                            throw new ArgumentException(Resources.InvalidCommandException);
                        var curveNumbers = await GetSelectedObjectNumbersAsync(objType);
                        returnValue = string.Join(",", curveNumbers);
                        break;
                    case GenCmd.GetCurveLength:
                        if (!int.TryParse(cmds[1], out int curveId))
                            throw new ArgumentException(Resources.InvalidCommandException);
                        returnValue = project.GetCurveLength(curveId).ToString(CultureInfo.InvariantCulture);
                        break;
                }
            }
            return returnValue;
        }

        //TODO: вынести в отдельный файл, чтобы не засорять ExecuteCommand
        private void ChangeTaskType(string taskType)
        {
            if (!CheckTask())
            {
                console.PrintInfo(Resources.ChangeTaskTypeWithoutProjectExc, Color.Red);
                return;
            }
            if (!Enum.TryParse(taskType, out TaskType _taskType))
            {
                console.PrintInfo(Resources.ChangeTaskInvalidExc, Color.Red);
                return;
            }
            project.ChangeTaskType(_taskType);
        }

        private void ChangeTaskKind(string taskKind)
        {
            if (!CheckTask())
            {
                console.PrintInfo(Resources.ChangeTaskTypeWithoutProjectExc, Color.Red);
                return;
            }

            var parsedTaskKind = (TaskKind)0;
            foreach (var taskKindPart in taskKind.Split('/'))
            {
                TaskKind? parsedTaskKindPart = taskKindPart switch
                {
                    "Chemical" => TaskKind.химическая,
                    "Termal" => TaskKind.термическая,
                    "Mechanical" => TaskKind.механическая,
                    _ => null
                };

                if (parsedTaskKindPart is null)
                {
                    console.PrintInfo(Resources.ChangeTaskInvalidExc, Color.Red);
                    return;
                }

                parsedTaskKind |= parsedTaskKindPart.Value;
            }

            project.ChangeTaskKind(parsedTaskKind);
        }

        private bool CheckTask() =>
            project?.TaskKind is not null &&
            project?.TaskType is not null;
    }
}
