using BazisAvaloniaGUI.Localization;
using BazisAvaloniaGUI.Properties;
using Model.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        enum SurfacePropertyKeys { Number, MeshType, MeshKind, CornerPoints, RibersOrientation, Quadratization, AddedCurves, PointsNumbers }
        private List<RowProperty> GetSurfaceProperties(int number)
        {
            var rows = new List<RowProperty>();

            rows.Add(new RowProperty(SurfacePropertyKeys.Number.ToString(),
                Resources.Header_surface_number,
                number,
                true));

            var settings = project.GetSurfaceMeshingSettings(number);
            var meshTypes = new List<string>() { "*", "регулярная" };

            if (!settings.IsTransfinite)
                rows.Add(new RowProperty(SurfacePropertyKeys.MeshType.ToString(),
                    Resources.Header_surface_meshType,
                    new DropDownPropertyValue("*", meshTypes)));
            else
            {
                rows.Add(new RowProperty(SurfacePropertyKeys.MeshKind.ToString(),
                    Resources.Header_surface_meshKind,
                    new DropDownPropertyValue("регулярная", meshTypes)));

                rows.Add(new RowProperty(SurfacePropertyKeys.CornerPoints.ToString(),
                    Resources.Header_surface_cornerPoints,
                    string.Join(",", settings.CornerPointNumbers)));

                rows.Add(new RowProperty(SurfacePropertyKeys.RibersOrientation.ToString(),
                    Resources.Header_surface_ribersOrientation,
                    settings.Arrangement));

                rows.Add(new RowProperty(SurfacePropertyKeys.Quadratization.ToString(),
                    Resources.Header_surface_quadratization,
                    settings.IsRecombined));
            }

            var embeddedCurveNumbers = string.Join(",", settings.EmbeddedCurveNumbers);

            rows.Add(new RowProperty(SurfacePropertyKeys.AddedCurves.ToString(),
                Resources.Header_surface_addedCurves,
                embeddedCurveNumbers));

            rows.Add(new RowProperty(SurfacePropertyKeys.PointsNumbers.ToString(),
                Resources.Header_surface_pointsNumbers,
                new ButtonPropertyValue(Resources.Показать, () =>
                {
                    var boundaryPointNumbers = settings.BoundaryPointNumbers.ToArray();
                    ShowObjectsNumbers(ObjType.Точка, boundaryPointNumbers);
                    RequestRedraw();
                })));

            return rows;
        }
    }
}
