using WinGUI.Scene.Interfaces;
using WinGUI.Scene.VBO;
using Model.Interfaces.ObjectsCollections;
using System;
using OperationalController;

namespace WinGUI
{
    public partial class BaseForm
    {
        public VBObject CreateVBObject(IObjsPresenter presenter)
        {
            var inds = presenter.CreateIndexes();
            var ptrs = presenter.CreatePointers(inds.Item1);

            var coords = presenter.CreateVertexes(inds.Item2, "координаты");
            var colors = presenter.CreateVertexes(inds.Item3, "цвет");
            var normals = presenter.CreateVertexes(inds.Item2, "нормаль");
            var edges = presenter.CreateEdgeFlags(inds.Item4);
            var objsName = presenter.Name;

            VBObject vb;
            if (presenter.PresenterType == PresenterType.Surface)
            {
                var pres = (ISurfaceObjsPresenter)presenter;
                var separs = pres.CreateSeparators();

                if (presenter.ViewMode == ViewMode.Line)
                    vb = VBOController.CreateSurfaceVBObjects(ptrs, coords, colors, normals, edges, objsName, separs, ObjView.Lines);
                else if (presenter.ViewMode == ViewMode.LineSurface)
                    vb = VBOController.CreateSurfaceVBObjects(ptrs, coords, colors, normals, edges, objsName, separs, ObjView.LinesSurface);
                else
                    vb = VBOController.CreateSurfaceVBObjects(ptrs, coords, colors, normals, edges, objsName, separs, ObjView.Surface);
            }

            else if (presenter.PresenterType == PresenterType.Line)
            {
                vb = VBOController.CreateLineVBObjects(ptrs, coords, colors, normals, edges, objsName);
            }

            else
                vb = VBOController.CreatePointVBObjects(ptrs, coords, colors, normals, objsName);

            vb.ActiveDrawingObject = averageColorRenderer.IsEnable ? averageColorRenderer : null;
            return vb;

        }
        /// <summary>
        /// Предпочтительнее использовать этот метод
        /// </summary>
        /// <param name="presenter"></param>
        /// <param name="vb"></param>
        /// <returns></returns>
        public bool TryCreateVBObject(IObjsPresenter presenter, out VBObject vb)
        {
            var inds = presenter.CreateIndexes();
            var ptrs = presenter.CreatePointers(inds.Item1);

            var coords = presenter.CreateVertexes(inds.Item2, "координаты");
            var colors = presenter.CreateVertexes(inds.Item3, "цвет");
            var normals = presenter.CreateVertexes(inds.Item2, "нормаль");
            var edges = presenter.CreateEdgeFlags(inds.Item4);
            var objsName = presenter.Name;

            // необходимая проверка условия
            if (ptrs.Length == 0)
            {
                vb = null;
                return false;
            }
            else
            {
                if (presenter.PresenterType == PresenterType.Surface)
                {
                    var pres = (ISurfaceObjsPresenter)presenter;
                    var separs = pres.CreateSeparators();

                    if (presenter.ViewMode == ViewMode.Line)
                        vb = VBOController.CreateSurfaceVBObjects(ptrs, coords, colors, normals, edges, objsName, separs, ObjView.Lines);
                    else if (presenter.ViewMode == ViewMode.LineSurface)
                        vb = VBOController.CreateSurfaceVBObjects(ptrs, coords, colors, normals, edges, objsName, separs, ObjView.LinesSurface);
                    else
                        vb = VBOController.CreateSurfaceVBObjects(ptrs, coords, colors, normals, edges, objsName, separs, ObjView.Surface);
                }

                else if (presenter.PresenterType == PresenterType.Line)
                {
                    vb = VBOController.CreateLineVBObjects(ptrs, coords, colors, normals, edges, objsName);
                }

                else
                    vb = VBOController.CreatePointVBObjects(ptrs, coords, colors, normals, objsName);

                vb.ActiveDrawingObject = averageColorRenderer.IsEnable ? averageColorRenderer : null;
                return true;
            }

            

        }
    }
}
