using SceneCore.Interfaces;
using OpenTK.Graphics.OpenGL;

namespace SceneCore.VBO
{
    /// <summary>
    /// LineObjects
    /// </summary>
    public class LineObjects : VBObject
    {
        /// <summary>
        /// SurfaceObjects
        /// </summary>
        /// <param name="edges"></param>
        /// ribbers for couple of points
        /// <param name="ptrs"></param>
        /// pointers for every point of triangle
        /// <param name="glCoords"></param>
        /// coordinates for every point of triangle
        /// <param name="glColors"></param>
        /// colors for every point of triangle
        /// <param name="glNormals"></param>
        /// normals for every points of trialnge
        /// <param name="objName"></param>
        /// type of objects
        public LineObjects(string objName,bool[]edges,int[] ptrs, float[] glCoords, float[] glColors, float[] glNormals) : 
            base(objName,ptrs, glCoords, glColors, glNormals)
        {
            GL_ObjType = GLObjType.line;
            Gl_DisplayMode = PolygonMode.Line;
            Gl_LineWidth = 5.0f;
        }
/// <inheritdoc/>

        public override void Load()
        {
            if (ViewState)
            {
                //AverageColorRenderer.SetRenderSettings(1);
                ActiveDrawingObject?.DoActionsBeforeDrawing(this, DrawElements.Lines);
                Draw();
                ActiveDrawingObject?.DoActionsAfterDrawing(this, DrawElements.Lines);
                //AverageColorRenderer.RestoreRenderSettings(1);
            }
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
    }
}
