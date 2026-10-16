using BazisAvaloniaGUI.Properties;
using OperationalController;
using System;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private void ChangeVolProperty(PropertyChangedEventArgs obj, int number, ref bool flag)
        {
            if (!Enum.TryParse(obj.Key, out VolumePropertyKeys key))
                return;

            var current = project.GetVolumeMeshingSettings(number);
            var type = current.Type;
            var gradientSettings = current.GradientSettings;

            if (key == VolumePropertyKeys.MeshType)
            {
                if (!Enum.TryParse(obj.NewValue, out VolumeMeshingType parsedType))
                    throw new ArgumentException("Volume meshing type is invalid.");

                type = parsedType;
                if (type == VolumeMeshingType.Gradient && gradientSettings == null)
                    gradientSettings = new GradientVolumeMeshingSettings(1, 1, 1, 10);

                flag = true;
            }
            else
            {
                var power = gradientSettings?.Power ?? 1;
                var distanceMaximum = gradientSettings?.DistanceMaximum ?? 1;
                var surfaceElementSize = gradientSettings?.SurfaceElementSize ?? 1;
                var coreElementSize = gradientSettings?.CoreElementSize ?? 10;

                if (!double.TryParse(obj.NewValue, out var value))
                    throw new ArgumentException("Volume meshing value is invalid.");

                if (key == VolumePropertyKeys.TransitionGradientDegree)
                    power = value;
                else if (key == VolumePropertyKeys.LayerThickness)
                    distanceMaximum = value;
                else if (key == VolumePropertyKeys.SurfaceElementsSize)
                    surfaceElementSize = value;
                else if (key == VolumePropertyKeys.CenterElementsSize)
                    coreElementSize = value;

                gradientSettings = new GradientVolumeMeshingSettings(power, distanceMaximum, surfaceElementSize, coreElementSize);
                type = VolumeMeshingType.Gradient;
            }

            var settings = new VolumeMeshingSettings(type, gradientSettings);
            project.SetVolumeMeshingSettings(number, settings);
        }
    }
}
