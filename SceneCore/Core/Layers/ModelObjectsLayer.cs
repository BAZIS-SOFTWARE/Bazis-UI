using System.Linq;
using SceneCore.Rendering;
using WinGUI.Scene.Interfaces;
using WinGUI.Scene.VBO;
using OpenTK.Graphics.OpenGL;

namespace SceneCore.Layers
{
    /// <summary>Перенос BaseForm.DisplayModelObjects().</summary>
    public class ModelObjectsLayer : ISceneLayer
    {
        public string Name => "ModelObjects";
        public int Order { get; set; } = 40;
        public bool IsVisible { get; set; } = true;

        private readonly VBOController vboController;

        public ModelObjectsLayer(VBOController vboController)
        {
            this.vboController = vboController;
        }

        public void Draw(IRenderContext context)
        {
            var settings = context.Settings;
            var position = context.Camera.Position;

            if (settings.IsCutting)
            {
                GL.Enable(EnableCap.CullFace);
                GL.CullFace(TriangleFace.Back);
                GL.FrontFace(FrontFaceDirection.Ccw);
            }

            GL.PushMatrix();
            GL.Translate(-position._x, -position._y, -position._z);

            GL.Enable(EnableCap.Normalize);
            GL.Enable(EnableCap.Light0);

            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Translate(settings.LighterPosition._x, settings.LighterPosition._y, 0);
            GL.Light(LightName.Light0, LightParameter.Position, new float[] { 0f, 0f, 1f, 1f });
            GL.PopMatrix();

            GL.BlendFuncSeparate(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha, BlendingFactorSrc.One, BlendingFactorDest.One);

            if (settings.IsLighting)
                GL.Enable(EnableCap.Lighting);

            GL.LightModel(LightModelParameter.LightModelAmbient, new float[] { 0.2f, 0.2f, 0.2f, 1f });
            GL.Light(LightName.Light0, LightParameter.Diffuse, new float[] { 1f, 1f, 1f, 1f });
            GL.LightModel(LightModelParameter.LightModelTwoSide, 1);
            GL.Enable(EnableCap.ColorMaterial);
            GL.LineWidth(1.5f);

            foreach (var obj in vboController.GetVBObjs().Where(x => x.GL_ObjType == GLObjType.triangle))
                DrawObject(obj);

            GL.Disable(EnableCap.Lighting);

            foreach (var obj in vboController.GetVBObjs().Where(x => x.GL_ObjType == GLObjType.line))
                DrawObject(obj);

            foreach (var obj in vboController.GetVBObjs().Where(x => x.GL_ObjType == GLObjType.point))
                DrawObject(obj);

            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.ColorMaterial);
            GL.PopMatrix();
        }

        private static void DrawObject(VBObject obj)
        {
            GL.PushMatrix();
            GL.MultMatrix(obj.ModelMatrix);
            obj.Load();
            GL.PopMatrix();
        }
    }
}
