using BazisAvaloniaGUI.Localization;
using BazisAvaloniaGUI.Properties;
using System;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private void ChangePointProperty(PropertyChangedEventArgs obj, int number)
        {
            if (Enum.TryParse(obj.Key, out PointPropertyKeys key))
                if (key == PointPropertyKeys.ElementSize)
                    SetMeshPoint(number, double.Parse(obj.NewValue));
        }

        private void PrepareDataForSetMeshPoint(string number, string meshSize, out int _numberPoint, out double _meshSize)
        {
            var valid =
                int.TryParse(number, out _numberPoint) &
                double.TryParse(meshSize, out _meshSize);
            if (!valid)
                throw new ArgumentException(Resources.InvalidCommandException);
        }

        private void SetMeshPoint(int number, double meshSize) => project.SetMeshPointSize(number, meshSize);
    }
}
