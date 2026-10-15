using BazisGUI.Scene.Core.Rendering;
using BazisGUI.Scene.VBO;
using Geometry;
using BazisGUI.Scene.Core;
using BazisGUI.Scene.Core.Camera;

namespace BazisGUI.Scene.Core.Layers
{
    /// <summary>
    /// Не рисует объект "ClipPlane" сам — он triangle-типа и уже попадает в общий перебор
    /// ModelObjectsLayer (иначе получим двойную отрисовку, как в старом
    /// DisplayModelObjects()/GetVBObjs()). Задача этого слоя — то, что раньше делал
    /// BaseForm.DisplayClipPlaneEvent: каждый кадр пересчитывать модельную и клип-матрицы
    /// от ТЕКУЩЕЙ матрицы камеры (плоскость сечения задаётся один раз, а камера крутится).
    /// </summary>
    public class ClipPlaneLayer : ISceneLayer
    {
        public string Name => "ClipPlane";
        public int Order { get; set; } = 30;
        public bool IsVisible { get; set; } = true;

        private readonly VBOController vboController;
        private readonly Advanced3DClipper clipper;
        private Plane plane;

        public ClipPlaneLayer(VBOController vboController, Advanced3DClipper clipper)
        {
            this.vboController = vboController;
            this.clipper = clipper;
        }

        public void SetPlane(Plane plane) => this.plane = plane;

        public void Clear() => plane = null;

        public void Draw(IRenderContext context)
        {
            if (plane == null)
                return;

            if (!(vboController.FindVBObj("ClipPlane") is ClipPlane clipPlaneVbo))
                return;

            clipper.ScaleFactor = context.ScaleFactor;

            var model = SceneController.ComputeClipPlaneModelMatrix(plane, clipPlaneVbo.BoundingBox);
            // Как в старом DisplayModelObjects: сечение строилось уже после GL.Translate(-Position)
            var position = context.Camera.Position;
            var view = GlMatrix.Translate(context.Camera.GetViewMatrix(), -position._x, -position._y, -position._z);

            clipPlaneVbo.ModelMatrix = model.AsColumnMajorArray();
            clipper.ClipMatrix = (view * model).AsColumnMajorArray();
            clipPlaneVbo.ViewMatrix = view.AsColumnMajorArray();
        }
    }
}
