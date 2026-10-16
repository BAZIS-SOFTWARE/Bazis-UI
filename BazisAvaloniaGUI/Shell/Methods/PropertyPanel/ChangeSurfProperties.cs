using BazisAvaloniaGUI.Extensions;
using BazisAvaloniaGUI.Localization;
using BazisAvaloniaGUI.Properties;
using GmshApi;
using OperationalController;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private void ChangeSurfaceProperty(PropertyChangedEventArgs obj, int number, ref bool flag)
        {
            if (!Enum.TryParse(obj.Key, out SurfacePropertyKeys key))
                return;

            var current = project.GetSurfaceMeshingSettings(number);
            var isTransfinite = current.IsTransfinite;
            var cornerPointNumbers = current.CornerPointNumbers;
            var arrangement = current.Arrangement;
            var isRecombined = current.IsRecombined;
            var embeddedCurveNumbers = current.EmbeddedCurveNumbers;

            if (key == SurfacePropertyKeys.MeshType)
            {
                isTransfinite = obj.NewValue == "регулярная";
                if (isTransfinite && cornerPointNumbers.Count == 0)
                    cornerPointNumbers = current.BoundaryPointNumbers;

                flag = true;
            }
            else if (key == SurfacePropertyKeys.AddedCurves)
                embeddedCurveNumbers = ParseObjectNumbers(obj.NewValue);
            else if (key == SurfacePropertyKeys.Quadratization)
            {
                if (!bool.TryParse(obj.NewValue, out isRecombined))
                    throw new ArgumentException(Resources.InvalidCommandException);
            }
            else if (key == SurfacePropertyKeys.CornerPoints)
                cornerPointNumbers = ParseObjectNumbers(obj.NewValue);
            else if (key == SurfacePropertyKeys.RibersOrientation)
                arrangement = obj.NewValue.ToEnum<Arrangement>();

            var settings = new SurfaceMeshingSettings(isTransfinite, cornerPointNumbers, arrangement, isRecombined, embeddedCurveNumbers, current.BoundaryPointNumbers);
            project.SetSurfaceMeshingSettings(number, settings);
        }

        private void PrepareDataForSetRegularMeshSurface(string number, string cornerPoints, string ribersOrientation, string quadratization, out int _number, out Arrangement _arrangement, out List<int> _cornerPoints, out bool _quadratization)
        {
            var valid = int.TryParse(number, out _number) &
                ribersOrientation.TryToEnum<Arrangement>(out _arrangement);

            _cornerPoints = cornerPoints
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => int.Parse(x.Trim()))
                .ToList();

            _quadratization = quadratization == "quad";

            if (!valid)
                throw new ArgumentException(Resources.InvalidCommandException);
        }

        private void PrepareDataForGetRelatedGeometryObjects(string geoDim, string geoNumber, string level, out int _geoDim, out int _geoNumber, out bool _lvlUp)
        {
            if (!int.TryParse(geoDim, out _geoDim))
                throw new ArgumentException(Resources.InvalidCommandException);

            if (!int.TryParse(geoNumber, out _geoNumber))
                throw new ArgumentException(Resources.InvalidCommandException);

            _lvlUp = level == "up";
        }

        private Tuple<int[], int[]> GetAdjacentGeometryObjects(int geoDim, int geoNumber)
        {
            return project.GetAdjacentGeometryObjects(geoDim, geoNumber);
        }

        private void PrepareDataForSetEmbeddedMeshSurface(string targetType, string targetNumber, string embeddedType, string embeddedNumbers, out int _targetType, out int _targetNumber, out int _embeddedType, out IEnumerable<int> _embeddedNumbers)
        {
            var valid = int.TryParse(targetType, out _targetType) &
                int.TryParse(targetNumber, out _targetNumber) &
                int.TryParse(embeddedType, out _embeddedType);

            _embeddedNumbers = embeddedNumbers
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => int.Parse(x.Trim()))
                .ToList();


            if (!valid || _embeddedNumbers == null)
                throw new ArgumentException(Resources.InvalidCommandException);
        }

        private void SetEmbeddedMesh(int hostDimension, int hostTag, int embeddedDimension, IEnumerable<int> embeddedEntities)
        {
            project.SetEmbeddedGeometryEntities(hostDimension, hostTag, embeddedDimension, embeddedEntities);
        }

        private void SetRegularMeshSurface(int number, List<int> cornerPoints, Arrangement arrangement, bool quadratization)
        {
            var current = project.GetSurfaceMeshingSettings(number);
            var settings = new SurfaceMeshingSettings(true, cornerPoints, arrangement, quadratization, current.EmbeddedCurveNumbers, current.BoundaryPointNumbers);
            project.SetSurfaceMeshingSettings(number, settings);
        }

        private List<int> ParseObjectNumbers(string value)
        {
            var values = value.Split(',', StringSplitOptions.RemoveEmptyEntries);
            var numbers = new List<int>();
            foreach (var item in values)
            {
                if (!int.TryParse(item.Trim(), out var number))
                    throw new ArgumentException(Resources.InvalidCommandException);

                numbers.Add(number);
            }

            return numbers;
        }
    }
}
