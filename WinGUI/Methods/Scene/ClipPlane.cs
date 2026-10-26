using System;
using Geometry;
using System.Linq;
using SceneCore;

namespace WinGUI
{
    // Матрица сечения теперь считает SceneController.ChangeClipPlane/ClipPlaneLayer (пересчёт от
    // текущей камеры на каждый кадр, без GL) — см. GUI/Documents/scene.avalonia.md, раздел 6.
    // DisplayClipPlaneEvent оставлено объявленным ради UtilityToolStrip.cs (`= null`), но больше
    // не вызывается — вызывать больше нечего, всю работу теперь делает sceneController каждый кадр.
    public partial class BaseForm
    {
        event Action DisplayClipPlaneEvent;

        public void CreateClipPlane()
        {
            BoundingBox current = null;
            foreach (var set in project.GetModelSetsInfo(Model.Interfaces.ObjType.Элемент3D).Where(v => GetVisibleNumbers(v).Any()))
            {
                var vbo = VBOController.FindVBObj(set.Name);
                if (vbo != null)
                    current = current == null ? vbo.BoundingBox.Merge(null) : current.Merge(vbo.BoundingBox);
            }

            sceneController.CreateClipPlane(current);
        }

        public void DeleteClipPlane() => sceneController.DeleteClipPlane();

        public void DisplayClipPlane(Plane plane) => sceneController.ChangeClipPlane(plane);
    }
}
