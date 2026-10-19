using BazisAvaloniaGUI.Localization;
using BazisAvaloniaGUI.Properties;
using BazisAvaloniaGUI.Utilities;
using GmshApi;
using System.Collections.Generic;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        enum CurvePropertyKeys { Number, PointsNumber, Algorithm, Coefficient, }
        private List<RowProperty> GetCurveProperties(int arg3)
        {
            var rows = new List<RowProperty>();
            var algo = Converters.GetEnumNames<MeshType>();

            rows.Add(new RowProperty(CurvePropertyKeys.Number.ToString(), Resources.Header_curve_number, arg3));

            var settings = project.GetCurveMeshingSettings(arg3);
            rows.Add(new RowProperty(CurvePropertyKeys.PointsNumber.ToString(), Resources.Header_curve_PointsNumber, settings.NodesCount));
            rows.Add(new RowProperty(CurvePropertyKeys.Algorithm.ToString(), Resources.Header_curve_algorithm, new DropDownPropertyValue(settings.MeshType, algo)));
            rows.Add(new RowProperty(CurvePropertyKeys.Coefficient.ToString(), Resources.Header_curve_coefficient, settings.Coefficient));

            // - TO DO снять все ограничения (кнопка)

            return rows;
        }
    }
}
