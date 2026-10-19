using BazisAvaloniaGUI.Localization;
using BazisAvaloniaGUI.Properties;
using OperationalController;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        enum VolumePropertyKeys { Number, MeshType, TransitionGradientDegree, LayerThickness, SurfaceElementsSize, CenterElementsSize }
        private List<RowProperty> GetVolProperties(int number)
        {
            var rows = new List<RowProperty>();
            rows.Add(new RowProperty(VolumePropertyKeys.Number.ToString(), Resources.Header_volume_number, number));

            var settings = project.GetVolumeMeshingSettings(number);
            var meshTypes = Enum.GetValues<VolumeMeshingType>().Select(x => x.ToString()).ToList();


            if (settings.Type == VolumeMeshingType.Undefined)
                rows.Add(new RowProperty(VolumePropertyKeys.MeshType.ToString(),
                    Resources.Header_volume_meshType,
                    new DropDownPropertyValue("Undefined", meshTypes)));
            else
            {
                rows.Add(new RowProperty(VolumePropertyKeys.MeshType.ToString(),
                    Resources.Header_volume_meshType,
                    new DropDownPropertyValue(settings.Type, meshTypes)));
                if (settings.Type == VolumeMeshingType.Gradient)
                {
                    rows.Add(new RowProperty(VolumePropertyKeys.TransitionGradientDegree.ToString(),
                        Resources.Header_volume_TransitionGradientDegree,
                        settings.GradientSettings.Power));

                    rows.Add(new RowProperty(VolumePropertyKeys.LayerThickness.ToString(),
                        Resources.Header_volume_layerThickness,
                        settings.GradientSettings.DistanceMaximum));

                    rows.Add(new RowProperty(VolumePropertyKeys.SurfaceElementsSize.ToString(),
                        Resources.Header_volume_surfaceElementsSize,
                        settings.GradientSettings.SurfaceElementSize));

                    rows.Add(new RowProperty(VolumePropertyKeys.CenterElementsSize.ToString(),
                        Resources.Header_volume_centerElementsSize,
                        settings.GradientSettings.CoreElementSize));
                }
            }

            return rows;
        }
    }
}
