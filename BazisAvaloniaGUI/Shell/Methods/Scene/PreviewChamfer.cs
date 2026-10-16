using BazisGUI.Scene.Core.Rendering;
using Geometry;
using OpenTK.Graphics.OpenGL;
using Model.Interfaces;
using Avalonia.Threading;
using BazisAvaloniaGUI.Localization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        // These are CPU-side preview data only. They are rendered from DisplayObjects
        // while the OpenGL context is current.
        private Segment3D[] chamferPreviewSegments = Array.Empty<Segment3D>();

        private void PreviewChamfer(double length, double secondValue, bool isByAngle, bool reflected)
        {
            try
            {
                var curveTags = project.GetSelected(ObjType.Кривая).ToArray();

                var segments = new List<Segment3D>(curveTags.Length * 4);

                foreach (var curveTag in curveTags)
                {
                    var adjacencies = project.GetAdjacentGeometryObjects(1, curveTag);
                    var surfaceTags = adjacencies.Item1;

                    if (surfaceTags.Length != 2)
                        throw new InvalidOperationException(
                            string.Format(Resources.ChamferPreview_CurveMustBelongToTwoSurfaces, surfaceTags.Length));

                    var firstAdjacencies = project.GetAdjacentGeometryObjects(2, surfaceTags[0]);
                    var firstVolumes = firstAdjacencies.Item1;
                    var secondAdjacencies = project.GetAdjacentGeometryObjects(2, surfaceTags[1]);
                    var secondVolumes = secondAdjacencies.Item1;
                    var commonVolumes = firstVolumes.Intersect(secondVolumes).Distinct().ToArray();
                    if (commonVolumes.Length != 1)
                        throw new InvalidOperationException(
                            string.Format(Resources.ChamferPreview_SurfacesMustBelongToOneVolume, commonVolumes.Length));

                    var volumeTag = commonVolumes[0];
                    var curveType = project.GetType(1, curveTag);
                    if (!curveType.Contains("Line", StringComparison.OrdinalIgnoreCase))
                        throw new NotSupportedException(
                            Resources.ChamferPreview_OnlyStraightCurvesSupported);

                    var (minimum, maximum) = project.GetParametrizationBounds(1, curveTag);
                    if (minimum.Length == 0 || maximum.Length == 0)
                        throw new InvalidOperationException(Resources.ChamferPreview_CurveParametrizationUnavailable);

                    var middleParameter = (minimum[0] + maximum[0]) / 2.0;
                    var edgeStart = ToPoint3D(project.GetGeoObjPoints(1, curveTag, new[] { minimum[0] }));
                    var edgeEnd = ToPoint3D(project.GetGeoObjPoints(1, curveTag, new[] { maximum[0] }));
                    var edgeMiddle = ToPoint3D(project.GetGeoObjPoints(1, curveTag, new[] { middleParameter }));
                    var tangent = Normalize(edgeEnd.Sub(edgeStart), string.Format(Resources.ChamferPreview_CurveHasZeroLength, curveTag));
                    var secondLength = isByAngle ? CalculateSecondLength(length, secondValue, edgeMiddle, surfaceTags, volumeTag) : secondValue;
                    var firstDirection = GetOffsetDirection(surfaceTags[0], volumeTag, edgeMiddle, tangent);
                    var secondDirection = GetOffsetDirection(surfaceTags[1], volumeTag, edgeMiddle, tangent);
                    var firstOffset = firstDirection.Mult((float)(reflected ? secondLength : length));
                    var secondOffset = secondDirection.Mult((float)(reflected ? length : secondLength));
                    var firstStart = edgeStart.Sum(firstOffset);
                    var firstEnd = edgeEnd.Sum(firstOffset);
                    var secondStart = edgeStart.Sum(secondOffset);
                    var secondEnd = edgeEnd.Sum(secondOffset);

                    segments.Add(new Segment3D(firstStart, firstEnd));
                    segments.Add(new Segment3D(secondStart, secondEnd));
                    segments.Add(new Segment3D(firstStart, secondStart));
                    segments.Add(new Segment3D(firstEnd, secondEnd));
                }

                UpdateChamferPreview(segments.ToArray());
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
                ClearChamferPreview(true);
            }
        }

        private double[] GetOutwardNormal(int surfaceTag, int volumeTag, double[] point)
        {
            var parameters = project.GetParametrization(2, surfaceTag, point);
            var normal = project.GetSurfaceNormal(surfaceTag, parameters);
            if (normal.Length != 3)
                throw new InvalidOperationException(string.Format(Resources.ChamferPreview_SurfaceNormalUnavailable, surfaceTag));

            var length = Math.Sqrt(normal.Sum(component => component * component));
            if (!double.IsFinite(length) || length == 0)
                throw new InvalidOperationException(string.Format(Resources.ChamferPreview_SurfaceHasInvalidNormal, surfaceTag));

            var orientation = GetSurfaceOrientation(volumeTag, surfaceTag);
            return normal.Select(component => orientation * component / length).ToArray();
        }

        private int GetSurfaceOrientation(int volumeTag, int surfaceTag)
        {
            var boundary = project.GetBoundary(new[] { 3, volumeTag }, oriented: true);
            for (var index = 1; index < boundary.Length; index += 2)
            {
                if (Math.Abs(boundary[index]) == surfaceTag)
                    return Math.Sign(boundary[index]);
            }

            throw new InvalidOperationException(string.Format(Resources.ChamferPreview_SurfaceNotOnVolumeBoundary, surfaceTag, volumeTag));
        }

        private Point3D GetOffsetDirection(int surfaceTag, int volumeTag, Point3D edgeMiddle, Point3D tangent)
        {
            var point = new[] { (double)edgeMiddle._x, edgeMiddle._y, edgeMiddle._z };
            var outwardNormal = ToPoint3D(GetOutwardNormal(surfaceTag, volumeTag, point));
            var direction = Normalize(
                Vector.CrossProd(outwardNormal, tangent),
                string.Format(Resources.ChamferPreview_OffsetDirectionUnavailable, surfaceTag));
            var (x, y, z) = project.GetCenterOfMass(2, surfaceTag);
            var toSurfaceCenter = new Point3D(
                (float)x - edgeMiddle._x,
                (float)y - edgeMiddle._y,
                (float)z - edgeMiddle._z);

            return Vector.DotProd(direction, toSurfaceCenter) < 0 ? direction.Mult(-1) : direction;
        }

        private double CalculateSecondLength(double firstLength, double angleInDegrees, Point3D edgeMiddle, int[] surfaceTags, int volumeTag)
        {
            if (!double.IsFinite(angleInDegrees) || angleInDegrees <= 0 || angleInDegrees >= 180)
                throw new ArgumentOutOfRangeException(
                    nameof(angleInDegrees),
                    Resources.ChamferPreview_AngleOutOfRange);

            var point = new[] { (double)edgeMiddle._x, edgeMiddle._y, edgeMiddle._z };
            var firstNormal = GetOutwardNormal(surfaceTags[0], volumeTag, point);
            var secondNormal = GetOutwardNormal(surfaceTags[1], volumeTag, point);
            var dotProduct = firstNormal[0] * secondNormal[0]
                + firstNormal[1] * secondNormal[1]
                + firstNormal[2] * secondNormal[2];
            var normalsAngle = Math.Acos(Math.Clamp(dotProduct, -1.0, 1.0));
            var surfaceAngle = Math.PI - normalsAngle;

            if (surfaceAngle <= 0 || surfaceAngle >= Math.PI)
                throw new InvalidOperationException(
                    Resources.ChamferPreview_InvalidSurfaceAngle);

            var chamferAngle = angleInDegrees * Math.PI / 180.0;
            if (surfaceAngle + chamferAngle >= Math.PI)
                throw new ArgumentOutOfRangeException(
                    nameof(angleInDegrees),
                    string.Format(Resources.ChamferPreview_AngleMustBeLessThan,
                        surfaceAngle * 180.0 / Math.PI,
                        180.0 - surfaceAngle * 180.0 / Math.PI));

            var calculatedLength = firstLength
                * Math.Sin(chamferAngle)
                / Math.Sin(surfaceAngle + chamferAngle);

            if (!double.IsFinite(calculatedLength) || calculatedLength <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(angleInDegrees),
                    Resources.ChamferPreview_CalculatedLengthMustBePositive);

            return calculatedLength;
        }

        private Point3D ToPoint3D(double[] coordinates)
        {
            if (coordinates == null || coordinates.Length != 3)
                throw new InvalidOperationException(Resources.ChamferPreview_InvalidGeometryCoordinates);

            return new Point3D((float)coordinates[0], (float)coordinates[1], (float)coordinates[2]);
        }

        private Point3D Normalize(Point3D vector, string errorMessage)
        {
            var vectorLength = Vector.GetVectorLength(vector);
            if (!float.IsFinite(vectorLength) || vectorLength == 0)
                throw new InvalidOperationException(errorMessage);

            return vector.Mult(1.0f / vectorLength);
        }

        private void UpdateChamferPreview(Segment3D[] segments)
        {
            if (!Dispatcher.UIThread.CheckAccess())
            {
                Dispatcher.UIThread.Post(() => UpdateChamferPreview(segments));
                return;
            }

            chamferPreviewSegments = segments ?? Array.Empty<Segment3D>();
            var previewSegments = chamferPreviewSegments;
            HideGeometryObj(nameof(DisplayChamferPreview));
            DisplayGeometryObject(nameof(DisplayChamferPreview), context => DisplayChamferPreview(context, previewSegments));
        }

        /// <summary>
        /// Draws the current chamfer preview as immediate-mode OpenGL lines.
        /// Called from the regular OpenGL render pass of the scene.
        /// </summary>
        private static void DisplayChamferPreview(IRenderContext context, Segment3D[] segments)
        {
            if (segments.Length == 0)
                return;

            var position = context.Camera.Position;
            GL.PushMatrix();
            GL.Translate(-position._x, -position._y, -position._z);
            GL.Color4(0.0f, 0.0f, 0.0f, 1.0f);
            GL.LineWidth(3.0f);
            GL.Begin(PrimitiveType.Lines);

            foreach (var segment in segments)
            {
                GL.Vertex3(segment.P0._x, segment.P0._y, segment.P0._z);
                GL.Vertex3(segment.P1._x, segment.P1._y, segment.P1._z);
            }

            GL.End();
            GL.PopMatrix();
        }
    }
}
