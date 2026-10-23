
using SceneCore.Interfaces;
using System;
using OpenTK.Graphics.OpenGL;

namespace SceneCore.VBO
{
    /// <summary>
    /// VBO methods
    /// </summary>
    public static class VBO
    {
        /// <summary>
        /// DeleteAllBuffers
        /// </summary>
        /// <param name="obj"></param>
        public static void DeleteAllBuffers(VBObject obj)
        {
            var buffers = new int[] { obj.PointersBuffer, obj.CoordsBuffer, obj.ColorsBuffer, obj.NormalsBuffer };
            GL.DeleteBuffers(buffers.Length, buffers);

            if (obj.GL_ObjType == GLObjType.triangle)
            {
                var sObj = (SurfaceObjects)obj;
                buffers = [sObj.FrameBuffer, sObj.EdgeBuffer, sObj.SeparatorBuffer, sObj.LeftUpBuffer, sObj.RightDownBuffer];
                GL.DeleteBuffers(buffers.Length, buffers);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public static void LoadVertexBuffers(VBObject obj)
        {

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, obj.PointersBuffer);

            GL.BindBuffer(BufferTarget.ArrayBuffer, obj.CoordsBuffer);
            //IntObjPtr ObjPtr = new IntObjPtr(obj.GlCoord.Length);
            //Gl.glBufferSubData(Gl.GL_ARRAY_BUFFER, IntObjPtr.Zero, ObjPtr, obj.GlCoord);
            GL.VertexPointer(3, VertexPointerType.Float, 0, IntPtr.Zero);

            GL.BindBuffer(BufferTarget.ArrayBuffer, obj.ColorsBuffer);
            //ObjPtr = new IntObjPtr(obj.GlColor.Length);
            //Gl.glBufferSubData(Gl.GL_ARRAY_BUFFER, IntObjPtr.Zero, ObjPtr, obj.GlColor);
            GL.ColorPointer(4, ColorPointerType.Float, 0, IntPtr.Zero);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public static void LoadNormalBuffer(VBObject obj)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, obj.NormalsBuffer);
            GL.NormalPointer(NormalPointerType.Float, 0, IntPtr.Zero);
        }

        /// <summary>
        /// LoadEdgeBuffer
        /// </summary>
        public static void LoadEdgeBuffer(int buffer)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
            GL.EdgeFlagPointer(0, IntPtr.Zero);
        }
        /// <summary>
        /// Draw objects. indexLength - начало диапазона, IntPtr.Zero - конц диапазона
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="indexLength"></param>
        public static void Draw(VBObject obj, int indexLength)
        {

            GL.PointSize(obj.Gl_PointSize);
            GL.LineWidth(obj.Gl_LineWidth);

            GL.PolygonMode(obj.Gl_Face, obj.Gl_DisplayMode);

            //Gl.glEnable(Gl.GL_BLEND);
            //Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
            //
            GL.DrawElements((PrimitiveType)obj.GL_ObjType, indexLength, DrawElementsType.UnsignedInt, IntPtr.Zero);
            //Gl.glDisable(Gl.GL_BLEND);
        }
        /// <summary>
        /// UnLoadAllBuffers
        /// </summary>
        public static void UnLoadAllBuffers()
        {
            //Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, 0);
            //Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, 0);
            //Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }
        /// <summary>
        /// IndexInit
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="indices"></param>
        public static void IndexDataInit(ref int buffer, int[] indices)
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DeleteBuffers(1, ref buffer);
            GL.GenBuffers(1, out buffer);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, buffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer,
                 (IntPtr)(indices.Length * sizeof(int)),
                              indices, BufferUsageHint.DynamicDraw);
            //Gl.glBindBuffer(Gl.GL_ELEMENT_ARRAY_BUFFER, 0);
        }
        /// <summary>
        /// BufferDataInit
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="data"></param>
        /// <param name="size"></param>
        public static void VertexDataInit<T>(ref int buffer, T[] data, int size) where T : struct
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffers(1, ref buffer);
            GL.GenBuffers(1, out buffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);

            GL.BufferData(BufferTarget.ArrayBuffer,
                 (IntPtr)(data.Length * size),
                              data, BufferUsageHint.DynamicDraw);
        }
        /// <summary>
        /// SetSubData
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <param name="data"></param>
        public static void SetSubData<T>(int buffer, int offset, int size,T[] data) where T : struct
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
            GL.BufferSubData(BufferTarget.ArrayBuffer, offset, (IntPtr)size, data);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
        /// <summary>
        /// GetSubData
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <param name="data"></param>
        public static void GetSubData<T>(int buffer, int offset, int size, T[] data) where T : struct
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
            GL.GetBufferSubData(BufferTarget.ArrayBuffer, offset, (IntPtr)size, data);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

    }
}
