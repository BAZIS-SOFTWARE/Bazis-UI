using BazisAvaloniaGUI.Extensions;
using BazisAvaloniaGUI.Localization;
using BazisAvaloniaGUI.Properties;
using GmshApi;
using OperationalController;
using System;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private void ChangeCurveProperty(PropertyChangedEventArgs obj, int number)
        {
            if (!Enum.TryParse(obj.Key, out CurvePropertyKeys key))
                return;

            var current = project.GetCurveMeshingSettings(number);
            var nodesCount = current.NodesCount;
            var meshType = current.MeshType;
            var coefficient = current.Coefficient;

            if (key == CurvePropertyKeys.Algorithm)
                meshType = obj.NewValue.ToEnum<MeshType>();
            else if (key == CurvePropertyKeys.PointsNumber && !int.TryParse(obj.NewValue, out nodesCount))
                throw new ArgumentException(Resources.InvalidCommandException);
            else if (key == CurvePropertyKeys.Coefficient && !double.TryParse(obj.NewValue, out coefficient))
                throw new ArgumentException(Resources.InvalidCommandException);

            var settings = new CurveMeshingSettings(true, nodesCount, meshType, coefficient);
            SetMeshCurve(number, settings);
        }

        private void PrepareDataForSetMeshCurve(string number, string pointsCount, string algorithm, string factor, out int _number, out CurveMeshingSettings settings)
        {
            var valid = int.TryParse(number, out _number) &
                        int.TryParse(pointsCount, out var points) &
                        double.TryParse(factor, out var coefficient) &
                        algorithm.TryToEnum<MeshType>(out var meshType);

            if (!valid)
                throw new ArgumentException(Resources.InvalidCommandException);

            settings = new CurveMeshingSettings(true, points, meshType, coefficient);
        }

        private void SetMeshCurve(int number, CurveMeshingSettings settings)
        {
            project.SetCurveMeshingSettings(number, settings);

            if (settingsConfig.ShowNodesOnCurves)
                ShowNodesOnCurves(true);
        }
    }
}
