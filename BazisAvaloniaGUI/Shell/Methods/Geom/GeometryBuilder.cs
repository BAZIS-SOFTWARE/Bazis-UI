using BazisAvaloniaGUI.Console.Enums;
using BazisAvaloniaGUI.Localization;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private string ExtrudeCurveBySurface(string surfaceNumber, string curveNumbers, string startPoint, string step, string transfinite)
        {
            if (!int.TryParse(surfaceNumber, out var surfaceTag))
                throw new ArgumentException(Resources.InvalidCommandException);

            ParseCurvePathParameters(curveNumbers, startPoint, step, out var curveTags, out var startPointTag, out var extrusionStep);
            var useTransfiniteMesh = ParseTransfiniteOption(transfinite);

            var setName = ExtrudeCurve(surfaceTag, curveTags, startPointTag, extrusionStep, useTransfiniteMesh);
            PresentExtrude();
            return setName;
        }

        private string ExtrudeCurveBySetName(string setName, string curveNumbers, string startPoint, string step, string transfinite)
        {
            if (string.IsNullOrWhiteSpace(setName) || project.GetModelSetInfo(ObjType.Элемент2D, setName) == null)
                throw new ArgumentException(Resources.InvalidCommandException);

            ParseCurvePathParameters(curveNumbers, startPoint, step, out var curveTags, out var startPointTag, out var extrusionStep);
            var useTransfiniteMesh = ParseTransfiniteOption(transfinite);

            var resultSetName = ExtrudeCurve(setName, curveTags, startPointTag, extrusionStep, useTransfiniteMesh);
            PresentExtrude();
            return resultSetName;
        }

        private string Extrude1DFromPoint(string nodeNumber, string curveNumbers, string startPoint, string step)
        {
            if (!int.TryParse(nodeNumber, out var nodeTag))
                throw new ArgumentException(Resources.InvalidCommandException);

            ParseCurvePathParameters(curveNumbers, startPoint, step, out var curveTags, out var startPointTag, out var extrusionStep);

            var setName = project.ExtrudeElement1DAlongCurve(curveTags, startPointTag, nodeTag, extrusionStep);
            PresentExtrude();
            return setName;
        }

        private void ParseCurvePathParameters(string curveNumbers, string startPoint, string step, out int[] curveTags, out int startPointTag, out double extrusionStep)
        {
            curveTags = ParseCurveTags(curveNumbers);

            if (!int.TryParse(startPoint, out startPointTag) ||
                !double.TryParse(step.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out extrusionStep))
                throw new ArgumentException(Resources.InvalidCommandException);
        }

        private bool ParseTransfiniteOption(string transfinite)
        {
            if (transfinite != "0" && transfinite != "1")
                throw new ArgumentException(Resources.InvalidCommandException);

            return transfinite == "1";
        }

        private int[] ParseCurveTags(string curveNumbers)
        {
            try
            {
                return curveNumbers
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(int.Parse)
                    .ToArray();
            }
            catch (Exception exception) when (exception is FormatException or OverflowException)
            {
                throw new ArgumentException(Resources.InvalidCommandException, exception);
            }
        }

        private int GeometryParser(CreateCommandType type, List<string> parameters)
        {
            // -1 означает ошибка
            var tag = -1;
            if (project == null || !project.IsGeometryInitialized)
                return tag;

            switch (type)
            {
                // "TODO: подобрать другой разделитель, чтобы не было конфликта культур"
                case CreateCommandType.AddPoint:
                    var prm = parameters;
                    var coord = prm[0].Split(',');

                    if (double.TryParse(coord[0], out double x) &&
                       double.TryParse(coord[1], out double y) &&
                       double.TryParse(coord[2], out double z))
                        tag = AddPoint(x, y, z);
                    break;
                case CreateCommandType.AddCurve:
                    var points = parameters;
                    if (int.TryParse(points[0], out int startTag) &&
                       int.TryParse(points[1], out int endTag))
                        tag = AddLine(startTag, endTag);
                    break;
                case CreateCommandType.AddSurface:
                    var lineNumbers = parameters[0]
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => int.Parse(x.Trim()))
                        .ToList();
                    tag = AddPlane(lineNumbers);
                    break;
                case CreateCommandType.AddPointByVector:
                    var prmByVector = parameters;
                    if (int.TryParse(prmByVector[0], out int basePointTag) &&
                        int.TryParse(prmByVector[1], out int directionPointTag) &&
                        double.TryParse(prmByVector[2], out double offset))
                        tag = AddPointByVector(basePointTag, directionPointTag, offset);
                    break;
                case CreateCommandType.AddPointProjectToSurface:
                    var prmProjectToSurface = parameters;
                    if (int.TryParse(prmProjectToSurface[0], out int pointTag) &&
                        int.TryParse(prmProjectToSurface[1], out int surfaceTag))
                        tag = AddPointProjectionOntoPlane(pointTag, 2, surfaceTag);
                    break;
                case CreateCommandType.AddPointProjectToCurve:
                    var prmProjectToCurve = parameters;
                    if (int.TryParse(prmProjectToCurve[0], out int pointTag1) &&
                        int.TryParse(prmProjectToCurve[1], out int curveTag))
                        tag = AddPointProjectionOntoCurve(pointTag1, 1, curveTag);
                    break;
                default:
                    throw new NotSupportedException();
            }
            PresentGeoData();
            RequestRedraw();
            return tag;
        }

        private void PrepareDataForCreateGroupByGeo(string meshDim, string geoDim, string tag, out int _meshDim, out int _geoDim, out int _tag)
        {
            var valid = int.TryParse(meshDim, out _meshDim) &
                int.TryParse(geoDim, out _geoDim) &
                int.TryParse(tag, out _tag);

            if (!valid)
                throw new ArgumentException(Resources.InvalidCommandException);
        }

        private bool TryParseInvariantDouble(string value, out double result)
            => double.TryParse(value.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out result);
        private string ExtrudeCurve(int numberSurface, int[] numbersCurve, int numberStartPoint, double step, bool transfinite)
            => project.ExtrudeElement3DAlongCurve(numberSurface, numbersCurve, numberStartPoint, step, transfinite);
        private string ExtrudeCurve(string setName, int[] numbersCurve, int numberStartPoint, double step, bool transfinite)
            => project.ExtrudeElement3DAlongCurve(setName, numbersCurve, numberStartPoint, step, transfinite);
        private string ExtrudeRotate(int numberSurface, float angle, int originPoint, Vector3 rotAxis, bool transfinite)
        {
            var point = project.GetModelPoint(originPoint);
            Vector3 origin = new Vector3(point._x, point._y, point._z);
            return project.ExtrudeElement3DRotate(numberSurface, angle, origin, rotAxis, transfinite);
        }

        private int AddPoint(double x, double y, double z, double meshSize = 0)
        {
            var pointTag = project.CreatePoint(x, y, z);
            RefreshGeometry(ObjType.Точка);
            return pointTag;
        }

        private int AddLine(int startTag, int endTag, int tag = -1)
        {
            var lineTag = project.CreateLine(startTag, endTag);
            RefreshGeometry(ObjType.Кривая);
            return lineTag;
        }

        private int AddPlane(List<int> linesNumber)
        {
            var planeTag = project.CreateSurface(linesNumber.ToArray());
            RefreshGeometry(ObjType.Поверхность);
            return planeTag;
        }

        private int AddPointByVector(int startTag, int endTag, double step)
        {
            var pointTag = project.CreatePointByVector(startTag, endTag, step);
            RefreshGeometry(ObjType.Точка);
            return pointTag;
        }

        private int AddPointProjectionOntoPlane(int pointNumber, int dim, int surfaceTag)
        {
            var pointTag = project.CreatePointProjectionOntoGeometry(pointNumber, dim, surfaceTag);
            RefreshGeometry(ObjType.Точка);
            return pointTag;
        }

        private int AddPointProjectionOntoCurve(int pointNumber, int dim, int curveTag)
        {
            var pointTag = project.CreatePointProjectionOntoGeometry(pointNumber, dim, curveTag);
            RefreshGeometry(ObjType.Точка);
            return pointTag;
        }

        private void RefreshGeometry(ObjType objType)
        {
            foreach (var setInfo in project.GetModelSetsInfo(objType))
                VBOController.DeleteVBObjects(setInfo.Name);

            CreateVBObjsByObjsType(objType);
        }

        private void PresentExtrude()
        {
            VBOController.DeleteAllVBObjects();
            CreateVBObjects("Объекты");

            PresentMeshData();
            RequestRedraw();
        }
    }
}
