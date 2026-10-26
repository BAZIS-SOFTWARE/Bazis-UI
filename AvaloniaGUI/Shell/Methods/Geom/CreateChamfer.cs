using Model.Interfaces;
using Geometry;
using Avalonia.Threading;
using System;
using System.Drawing;
using System.Linq;

namespace AvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private void CreateChamfer(double length, double angle, bool isByAngle, bool reflected)
        {
            try
            {
                var selObjs = project.GetSelected(ObjType.Кривая).ToArray();
                var s = project.CreateChamfer(selObjs, length, angle, isByAngle, reflected);
                VBOController.DeleteAllVBObjects();
                CreateVBObjects("Объекты");
                PresentMeshData();
                RequestRedraw();
                PresentGeoData();
                RequestClearChamferPreview();
                // Окно фаски закрывается только после успешного построения.
                chamferWindowService.Close();
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
                // Окно остаётся открытым — превью по текущим параметрам сохраняем.
            }
        }


        private void ClearChamferPreview(bool redraw = true)
        {
            if (!Dispatcher.UIThread.CheckAccess())
            {
                Dispatcher.UIThread.Post(() => ClearChamferPreview(redraw));
                return;
            }

            chamferPreviewSegments = Array.Empty<Segment3D>();
            HideGeometryObj(nameof(DisplayChamferPreview));

            if (redraw)
                RequestRedraw();
        }
    }
}
