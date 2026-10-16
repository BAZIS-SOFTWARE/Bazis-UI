using BazisAvaloniaGUI.Localization;
using BazisAvaloniaGUI.Properties;
using Geometry;
using GmshApi;
using Model.GeometryObjects;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        enum GeoPropertyKeys { MinSize, MaxSize, Algorithm2D, Algorithm3D, ScaleCoef, ShowPointsOnCurves, ShowPointsNumbersOnCurves, ShowSurfaceNumbers, ShowPointsNumbers, ShowVolumeNumbers, ShowMeshOnGeneration }
        private void navigator_SelectGeoEvent()
        {
            try
            {
                if (project == null || !project.IsGeometryInitialized)
                    return;

                var rows = new List<RowProperty>();
                var meshSettings = project.GetMeshSettings();

                var actMinSize = meshSettings.MinimumSize;
                var actMaxSize = meshSettings.MaximumSize;
                var alg2D = meshSettings.Algorithm2D;
                var alg3D = meshSettings.Algorithm3D;
                var actSizeFactor = meshSettings.SizeFactor;

                var algs2D = Enum.GetValues(typeof(MeshAlgorithm2D)).
                    Cast<MeshAlgorithm2D>().Select(x => x.ToString());
                var algs3D = Enum.GetValues(typeof(MeshAlgorithm3D)).
                    Cast<MeshAlgorithm3D>().Select(x => x.ToString());

                rows.Add(new RowProperty(GeoPropertyKeys.MinSize.ToString(), Resources.Header_geo_minSize, actMinSize));
                rows.Add(new RowProperty(GeoPropertyKeys.MaxSize.ToString(), Resources.Header_geo_maxSize, actMaxSize));
                rows.Add(new RowProperty(GeoPropertyKeys.Algorithm2D.ToString(), Resources.Header_geo_algorithm2D, new DropDownPropertyValue(alg2D, algs2D.ToList())));
                rows.Add(new RowProperty(GeoPropertyKeys.Algorithm3D.ToString(), Resources.Header_geo_algorithm3D, new DropDownPropertyValue(alg3D, algs3D.ToList())));
                rows.Add(new RowProperty(GeoPropertyKeys.ScaleCoef.ToString(), Resources.Header_geo_scaleCoef, actSizeFactor));
                rows.Add(new RowProperty(GeoPropertyKeys.ShowPointsOnCurves.ToString(), Resources.Header_geo_showPointsOnCurves, settingsConfig.ShowNodesOnCurves));

                rows.Add(new RowProperty(GeoPropertyKeys.ShowPointsNumbersOnCurves.ToString(),
                    Resources.Header_geo_showPointsNumbersOnCurves,
                    new ButtonPropertyValue(Resources.Показать, () =>
                    {
                        ShowNumberOfCurveNodes();
                        RequestRedraw();
                    })));
                rows.Add(new RowProperty(GeoPropertyKeys.ShowSurfaceNumbers.ToString(),
                    Resources.Header_geo_showSurfacesNumbers,
                    new ButtonPropertyValue(Resources.Показать, () =>
                    {
                        ShowObjectsNumbers(ObjType.Поверхность);
                        RequestRedraw();
                    })));
                rows.Add(new RowProperty(GeoPropertyKeys.ShowPointsNumbers.ToString(),
                    Resources.Header_geo_showPointsNumbers,
                    new ButtonPropertyValue(Resources.Показать, () =>
                    {
                        ShowObjectsNumbers(ObjType.Точка);
                        RequestRedraw();
                    })));
                rows.Add(new RowProperty(GeoPropertyKeys.ShowVolumeNumbers.ToString(), Resources.Header_geo_showVolumesNumbers,
                    new ButtonPropertyValue(Resources.Показать, () =>
                    {
                        ShowVolNumbers();
                        RequestRedraw();
                    })));
                rows.Add(new RowProperty(GeoPropertyKeys.ShowMeshOnGeneration.ToString(),
                    Resources.Header_geo_showMeshOnGeneration,
                    settingsConfig.ShowAllMeshWhenGeneration));
                propertiesPanel.DrawTable(rows);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }

        }

        private void ShowNodesOnCurves(bool flag)
        {
            try
            {
                VBOController.DeleteVBObjects("transPoints");

                if (flag && (project == null || !project.IsGeometryInitialized))
                {
                    RequestRedraw();
                    return;
                }

                if (flag)
                {
                    var curveNumbers = new List<int>();
                    foreach (var curve in project.GetModelObjects(ObjType.Кривая))
                        if (project.GetVisible(ObjType.Кривая, curve.Number))
                            curveNumbers.Add(curve.Number);

                    var points = new List<GeometryPoint>();
                    foreach (var node in project.GetGeometryMeshNodes(1, curveNumbers))
                    {
                        var point = new GeometryPoint(node.Number, node.Position);
                        points.Add(point);
                    }

                    var presentor = presentersCreator.CreatePointObjectsPresenter(points.ToList(), Color.DarkGray);
                    presentor.Name = "transPoints";
                    var vbo = CreateVBObject(presentor);
                    VBOController.AddVbo(vbo);
                }

                RequestRedraw();
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        private void ShowObjectsNumbers(ObjType objType)
        {
            var dim = 0;

            if (objType == ObjType.Поверхность)
                dim = 2;

            foreach (var item in project.GetModelObjects(objType))
            {
                if (project.GetVisible(item.ObjType, item.Number))
                {
                    var point = objType == ObjType.Точка
                        ? item.CalcCentr()
                        : GetCenterOfGeometryEntity(dim, item.Number);
                    var text = $"{objType} {item.Number}";

                    DisplayObjectNumber(text, Color.Black, point);
                }
            }
        }

        private void ShowObjectsNumbers(ObjType objType, int[] numbers)
        {
            var dim = 0;

            if (objType == ObjType.Поверхность)
                dim = 2;

            foreach (var item in numbers)
            {
                var obj = project.GetModelObject(objType, item);

                if (project.GetVisible(obj.ObjType, obj.Number))
                {
                    var point = objType == ObjType.Точка
                        ? obj.CalcCentr()
                        : GetCenterOfGeometryEntity(dim, obj.Number);
                    var text = $"{objType} {obj.Number}";

                    DisplayObjectNumber(text, Color.Black, point);
                }
            }

        }

        private void ShowVolNumbers()
        {
            foreach (var item in project.GetModelVolumes())
            {
                if (item.GetSurfaceFigures().Any(x => project.GetVisible(ObjType.Поверхность, x.Number)))
                {
                    var point = GetCenterOfGeometryEntity(3, item.Number);
                    var text = $"Объем {item.Number}";

                    DisplayObjectNumber(text, Color.Black, point);
                }
            }
        }

        public void DisplayObjectNumber(string str, Color color, Point3D coord)
        {
            // В BaseForm — DisplayText3DTemplate через DisplayText3DEvent; вывод текста на сцену реализует сцена.
            DisplayText3D(str, color, coord);
        }

        private void ShowNumberOfCurveNodes()
        {
            foreach (var curve in project.GetModelObjects(ObjType.Кривая))
            {
                if (project.GetVisible(ObjType.Кривая, curve.Number))
                {
                    var settings = project.GetCurveMeshingSettings(curve.Number);

                    if (settings.IsTransfinite)
                    {
                        var text = $"{settings.NodesCount}";
                        var point = GetCenterOfGeometryEntity(1, curve.Number);

                        DisplayText3D(text, Color.Black, point);
                    }
                }

            }
        }

        /// <summary>
        /// Вернуть центр масс текущей геометрической сущности
        /// </summary>
        private Point3D GetCenterOfGeometryEntity(int dim, int tag)
        {
            var center = project.GetCenterOfMass(dim, tag);
            var point = new Point3D((float)center.X, (float)center.Y, (float)center.Z);
            return point;
        }
    }
}
