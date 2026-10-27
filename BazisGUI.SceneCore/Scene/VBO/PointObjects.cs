using BazisGUI.SceneCore.Interfaces;
using OpenTK.Graphics.OpenGL;

namespace BazisGUI.SceneCore.VBO
{
    /// <summary>
    /// PointObjects
    /// </summary>
    public class PointObjects : VBObject
    {
        /// <summary>
        /// PointObjects
        /// </summary>
        /// <param name="ptrs"></param>
        /// <param name="glCoords"></param>
        /// <param name="glColors"></param>
        /// <param name="glNormals"></param>
        /// <param name="objName"></param>
        public PointObjects(string objName,int[] ptrs, float[] glCoords, float[] glColors, float[] glNormals) : 
            base(objName,ptrs, glCoords, glColors, glNormals)
        {
            ViewMode = ObjView.Points;
            GL_ObjType = GLObjType.point;
            Gl_PointSize = 8.0f;
        }

        /// <inheritdoc/>

        public override void Draw()
        {
            GL.EnableClientState(ArrayCap.VertexArray);//По советам ЛГБТшников перенес сюда
            GL.EnableClientState(ArrayCap.ColorArray);
            VBO.LoadVertexBuffers(this);
            VBO.Draw(this, PtrLength);
            VBO.UnLoadAllBuffers();
            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.ColorArray);
        }

        /// <inheritdoc/>

        public override void Load()
        {
            if (ViewState)
            {
                //AverageColorRenderer.SetRenderSettings(0);
                ActiveDrawingObject?.DoActionsBeforeDrawing(this, DrawElements.Points);
                Draw();
                ActiveDrawingObject?.DoActionsAfterDrawing(this, DrawElements.Points);
            }
        }
    }
}
