using BazisAvaloniaGUI.Extensions;
using BazisAvaloniaGUI.Properties;
using GmshApi;
using System;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private void ChangeGeoProperties(PropertyChangedEventArgs obj)
        {
            if (Enum.TryParse(obj.Key, out GeoPropertyKeys key))
            {
                switch (key)
                {
                    case GeoPropertyKeys.MinSize:
                        project.SetMeshMinimumSize(double.Parse(obj.NewValue));
                        break;
                    case GeoPropertyKeys.MaxSize:
                        project.SetMeshMaximumSize(double.Parse(obj.NewValue));
                        break;
                    case GeoPropertyKeys.Algorithm2D:
                        project.SetMeshAlgorithm2D(obj.NewValue.ToEnum<MeshAlgorithm2D>());
                        break;
                    case GeoPropertyKeys.Algorithm3D:
                        project.SetMeshAlgorithm3D(obj.NewValue.ToEnum<MeshAlgorithm3D>());
                        break;
                    case GeoPropertyKeys.ScaleCoef:
                        project.SetMeshSizeFactor(double.Parse(obj.NewValue));
                        break;
                    case GeoPropertyKeys.ShowPointsOnCurves:
                        settingsConfig.ShowNodesOnCurves = bool.Parse(obj.NewValue);
                        ShowNodesOnCurves(settingsConfig.ShowNodesOnCurves);
                        break;
                    case GeoPropertyKeys.ShowMeshOnGeneration:
                        settingsConfig.ShowAllMeshWhenGeneration = bool.Parse(obj.NewValue);
                        break;
                }
            }
        }
    }
}
