using WinGUI.Scene.Interfaces;
using System;
using OpenTK.Graphics.OpenGL;

namespace WinGUI.Scene.VBO
{
    /// <summary>
    /// SurfaceObjects class
    /// </summary>
    public class SurfaceObjects : VBObject
    {
        /// <inheritdoc/>
        public int FrameBuffer { get; private set; } = 0;//Нужно свойство для изменения прозрачности ребер
        /// <inheritdoc/>
        public int EdgeBuffer { get; private set; } = 0;
        /// <inheritdoc/>
        public int SeparatorBuffer { get; private set; } = 0;

        /// <inheritdoc/>
        public int LeftUpBuffer { get; set; } = 0;

        /// <inheritdoc/>
        public int RightDownBuffer { get; set; } = 0;

        /// <inheritdoc/>
        public float[] FrameColors
        {
            get
            {
                var colors = new float[FrameLength];
                VBO.GetSubData(FrameBuffer, 0, colors.Length * sizeof(float), colors);
                return colors;

            }
            set
            {
                VBO.SetSubData(FrameBuffer, 0, value.Length * sizeof(float), value);
            }
        }

        /// <inheritdoc/>
        public bool[] EdgeFlags
        {
            get
            {
                var edges = new bool[EdgesLength];
                VBO.GetSubData(EdgeBuffer, 0, edges.Length * sizeof(bool), edges);
                return edges;
            }
            set
            {
                VBO.SetSubData(EdgeBuffer, 0, value.Length * sizeof(bool), value);
            }
        }
        /// <inheritdoc/>
        public int[] Separators
        {
            get
            {
                var separators = new int[SeparatorsLength];
                VBO.GetSubData(SeparatorBuffer, 0, separators.Length * sizeof(int), separators);
                return separators;

            }
            set
            {
                VBO.SetSubData(SeparatorBuffer, 0, value.Length * sizeof(int), value);
            }
        }


        /// <summary>
        /// SurfaceObjects
        /// </summary>
        /// <param name="edges"></param>
        /// ribbers for couple of points
        /// strides for points
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

        public SurfaceObjects(string objName,bool[] edges, int[] ptrs, float[] glCoords, float[] glColors, float[] glNormals) : 
            base(objName,ptrs, glCoords, glColors, glNormals)
        {
            var edgeBuff = 0;

            VBO.VertexDataInit(ref edgeBuff, edges,sizeof(bool));
            EdgesLength = edges.Length;
            EdgeBuffer = edgeBuff;

            ViewMode = ObjView.LinesSurface;
            GL_ObjType = GLObjType.triangle;
            Gl_LineWidth = 2.0f;
            //Gl_Face = 1028;
            var frameColors = new float[glColors.Length];

            var length = frameColors.Length / 4;
            for (int i = 3; i < frameColors.Length; i+= 4)
                frameColors[i] = 1;

            var frameBuff = 0;
            VBO.VertexDataInit(ref frameBuff, frameColors, sizeof(float));
            FrameBuffer = frameBuff;
            FrameLength = frameColors.Length;
        }
        /// <summary>
        /// Перегрузка конструктора
        /// </summary>
        /// <param name="objName">Имя</param>
        /// <param name="pointers">Индексы</param>
        /// <param name="glCoords">Координаты</param>
        /// <param name="glColors">Цвета</param>

        public SurfaceObjects(string objName,int[] pointers, float[] glCoords, float[] glColors) 
            : base(objName,pointers, glCoords, glColors)
        {
        }
        /// <inheritdoc/>
        public void CreateSeparators(int[] separators)
        {
            var sepBuffer = 0;
            VBO.VertexDataInit(ref sepBuffer, separators, sizeof(int));
            SeparatorsLength = separators.Length;
            SeparatorBuffer = sepBuffer;
        }
        /// <summary>
        /// Создать ограничивающие параллелепипеды для 3д элементов и 2д элементов
        /// </summary>
        /// <param name="points"></param>
        /// <param name="separators"></param>
        public void Create3DBoundingBoxes(float[] points, int[] separators)
        {
            var leftUpBuffer = 0;
            var rightDownBuffer = 0;

            float[] leftUp, rightDown;
            BuildBoundingBoxes(points, separators, out leftUp, out rightDown);
            VBO.VertexDataInit(ref leftUpBuffer, leftUp, sizeof(float));
            VBO.VertexDataInit(ref rightDownBuffer, rightDown, sizeof(float));

            LeftUpBuffer = leftUpBuffer;
            RightDownBuffer = rightDownBuffer;
        }
        /// <inheritdoc/>

        public override void Load()
        {
            if (ViewState)
            {
                if (ViewMode == ObjView.Lines)
                {
                    Gl_DisplayMode = PolygonMode.Line;
                    //AverageColorRenderer.SetRenderSettings(2);
                    ActiveDrawingObject?.DoActionsBeforeDrawing(this, DrawElements.Wireframe);
                    Draw();
                    ActiveDrawingObject?.DoActionsAfterDrawing(this, DrawElements.Wireframe);
                    //AverageColorRenderer.RestoreRenderSettings(2);
                }
                else if (ViewMode == ObjView.Surface)
                {
                    Gl_DisplayMode = PolygonMode.Fill;
                    //AverageColorRenderer.SetSurfaceRenderSettings(Draw);
                    ActiveDrawingObject?.DoActionsBeforeDrawing(this, DrawElements.Surfaces);
                    Draw();
                    ActiveDrawingObject?.DoActionsAfterDrawing(this, DrawElements.Surfaces);
                }
                else
                {
                    Gl_DisplayMode = PolygonMode.Fill;
                    //AverageColorRenderer.SetSurfaceRenderSettings(Draw);
                    ActiveDrawingObject?.DoActionsBeforeDrawing(this, DrawElements.Surfaces);
                    Draw();
                    ActiveDrawingObject?.DoActionsAfterDrawing(this, DrawElements.Surfaces);
                    //AverageColorRenderer.SetRenderSettings(2);
                    var temp = ColorsBuffer;
                    ColorsBuffer = FrameBuffer;
                    Gl_DisplayMode = PolygonMode.Line;
                    GL.DepthFunc(DepthFunction.Lequal);
                    ActiveDrawingObject?.DoActionsBeforeDrawing(this, DrawElements.Wireframe);
                    Draw();
                    ActiveDrawingObject?.DoActionsAfterDrawing(this, DrawElements.Wireframe);
                    GL.DepthFunc(DepthFunction.Less);
                    ColorsBuffer = temp;
                    //AverageColorRenderer.RestoreRenderSettings(2);
                }
            }      
        }
/// <inheritdoc/>

        public override void Draw()
        {
            GL.EnableClientState(ArrayCap.VertexArray);//По советам ЛГБТшников перенес сюда
            GL.EnableClientState(ArrayCap.ColorArray);
            GL.EnableClientState(ArrayCap.NormalArray);
            GL.EnableClientState(ArrayCap.EdgeFlagArray);
            VBO.LoadVertexBuffers(this);
            VBO.LoadNormalBuffer(this);
            VBO.LoadEdgeBuffer(EdgeBuffer);
            VBO.Draw(this, PtrLength);
            VBO.UnLoadAllBuffers();
            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.ColorArray);
            GL.DisableClientState(ArrayCap.NormalArray);
            GL.DisableClientState(ArrayCap.EdgeFlagArray);
        }

        /// <summary>
        /// Строит ограничивающие боксы для всех 3д элементов
        /// </summary>
        /// <param name="points">[In]Исходные точки модели</param>
        /// <param name="separators">[In]Разметка элементов модели</param>
        /// <param name="leftUp">[In]Левый верхний угол</param>
        /// <param name="rightDown">[In]Правый нижний угол</param>
        private void BuildBoundingBoxes(float[] points, int[] separators, out float[] leftUp, out float[] rightDown)
        {
            leftUp = new float[points.Length];
            rightDown = new float[points.Length];

            for (var i = 1; i < separators.Length; ++i)
            {
                var minX = float.MaxValue;
                var maxX = float.MinValue;
                var minY = float.MaxValue;
                var maxY = float.MinValue;
                var minZ = float.MaxValue;
                var maxZ = float.MinValue;

                var begin = separators[i - 1];
                var triangles = separators[i] - separators[i - 1];
                for (var j = 0; j < triangles; ++j)
                {
                    var stride = begin * 9 + j * 9;//Смещение до первой координаты треугольника
                    for (var k = 0; k < 9; k += 3)
                    {
                        var pointStride = stride + k;
                        minX = Math.Min(minX, points[pointStride + 0]);
                        maxX = Math.Max(maxX, points[pointStride + 0]);

                        minY = Math.Min(minY, points[pointStride + 1]);
                        maxY = Math.Max(maxY, points[pointStride + 1]);

                        minZ = Math.Min(minZ, points[pointStride + 2]);
                        maxZ = Math.Max(maxZ, points[pointStride + 2]);
                    }
                }
                for (var j = 0; j < triangles; ++j)
                {
                    var stride = begin * 9 + j * 9;//Смещение до первой координаты треугольника
                    for (var k = 0; k < 9; k += 3)
                    {
                        var pointStride = stride + k;
                        leftUp[pointStride + 0] = minX;
                        leftUp[pointStride + 1] = maxY;
                        leftUp[pointStride + 2] = maxZ;

                        rightDown[pointStride + 0] = maxX;
                        rightDown[pointStride + 1] = minY;
                        rightDown[pointStride + 2] = minZ;
                    }
                }
            }
        }
    }
}
