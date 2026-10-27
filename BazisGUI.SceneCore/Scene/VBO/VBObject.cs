using BazisGUI.SceneCore.Interfaces;
using OpenTK.Graphics.OpenGL;
using System;


namespace BazisGUI.SceneCore.VBO
{
    /// <summary>
    /// VBOjects class
    /// </summary>
    public abstract class VBObject : IVBObject
    {
        /// <summary>
        /// Max_x
        /// </summary>
        public float Max_x { get; set; }
        /// <summary>
        /// Max_y
        /// </summary>
        public float Max_y { get; set; }
        /// <summary>
        /// Max_z
        /// </summary>
        public float Max_z { get; set; }
        /// <summary>
        /// Координаты ограничивающего параллелепипеда
        /// </summary>
        public BoundingBox BoundingBox { get; set; }
        /// <summary>
        /// Матрица 4x4 для трансформации объекта (поворот, масштабирование, перенос), вектора определены по столбцам
        /// </summary>
        public float[] ModelMatrix { get; set; }
        /// <summary>
        /// Объект рисования, опирающийся на gpu-программу рисования
        /// </summary>
        public IActiveDrawingObject ActiveDrawingObject { get; set; }
        /// <summary>
        /// VBObjects constructor
        /// </summary>
        /// <param name="pointers"></param>
        /// <param name="glCoords"></param>
        /// <param name="glColors"></param>
        /// <param name="glNormals"></param>
        /// <param name="objName"></param>
        public VBObject(string objName,int[] pointers, float[] glCoords, float[] glColors, float[] glNormals) : 
            this(objName, pointers, glCoords, glColors)
        {
            NormalLength = glNormals.Length;
            var normalBuff = 0;

            VBO.VertexDataInit(ref normalBuff, glNormals, sizeof(float));

            NormalsBuffer = normalBuff;
        }

        /// <summary>
        /// Перегрузка конструктора
        /// <param name="pointers">Индексы</param>
        /// <param name="glCoords">Координаты</param>
        /// <param name="glColors">Цвета</param>
        /// <param name="objName">Имя</param>
        public VBObject(string objName, int[] pointers, float[] glCoords, float[] glColors)
            : this(objName, pointers, glCoords)
        {

            ColorLength = glColors.Length;

            var colorBuff = 0;

            VBO.VertexDataInit(ref colorBuff, glColors, sizeof(float));

            ColorsBuffer = colorBuff;
        }

        public VBObject(string objName, int[] pointers, float[] glCoords)
        {
            if (pointers.Length == 0)
                throw new ArgumentException("VBObject Error: Index array length cannot be zero");
            ObjName = objName;

            PtrLength = pointers.Length;
            CoordLength = glCoords.Length;

            Gl_LineWidth = 1.0f;//Ширина линии и размер точки используется в Draw, не должен быть 0 и меньше, иначе возращает ошибку
            Gl_PointSize = 1.0f;

            var ptrBuff = 0;
            var coordBuff = 0;

            VBO.IndexDataInit(ref ptrBuff, pointers);
            VBO.VertexDataInit(ref coordBuff, glCoords, sizeof(float));

            PointersBuffer = ptrBuff;
            CoordsBuffer = coordBuff;
            
            BoundingBox = new BoundingBox(glCoords);
            
            ModelMatrix = new float[16];
            SetIdentityModelMatrix();
        }
        /// <summary>
        /// IndexInitialization
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="ptrs"></param>
        public void IndexInitialization(ref int buffer, int[] ptrs)
        {
            VBO.IndexDataInit(ref buffer, ptrs);
        }
        /// <summary>
        /// VertexInitialization
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="data"></param>
        public void VertexInitialization(ref int buffer, float[] data)
        {
            VBO.VertexDataInit(ref buffer, data, sizeof(float));
        }
        /// <summary>
        /// EdgesInitialization
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="glEdges"></param>
        public void EdgesInitialization(ref int buffer, bool[] glEdges)
        {
            VBO.VertexDataInit(ref buffer, glEdges, sizeof(bool));
        }

/// <inheritdoc/>

        public int PtrLength { get; set; }
/// <inheritdoc/>

        public int CoordLength { get; set; }

        /// <inheritdoc/>
        public int ColorLength { get; set; }
        /// <summary>
        /// EdgesLength
        /// </summary>
        public int EdgesLength { get; set; }
        /// <summary>
        /// SeparatorsLength
        /// </summary>
        public int SeparatorsLength { get; set; }
        /// <summary>
        /// NormalLength
        /// </summary>
        public int NormalLength { get; set; }
        /// <summary>
        /// FrameLength
        /// </summary>
        public int FrameLength { get; set; }

        /* Убранные элементы.
         Попробовали, но идея до конца не сработала.
         Снижается производительность. Нельзя выбрать все объекты когда -
         - включен режим только открытые поверхности

        internal int[] Strides { get; set; }
        */
        internal int CoordsBuffer { get; set; }
        internal int ColorsBuffer { get; set; }
        internal int NormalsBuffer { get; set; }
        internal int PointersBuffer { get; set; }
        //internal int EdgeFlagBuffer { get; set; }// Не используется
        /// <inheritdoc/>
        public GLObjType GL_ObjType { get; set; }//Публичный, т.к как-то нужно проверить копию и оригинал между собой (идентичность типов) извне
        internal float Gl_PointSize { get; set; }
        internal float Gl_LineWidth { get; set; }
        internal PolygonMode Gl_DisplayMode { get; set; }

        internal TriangleFace Gl_Face { get; set; } = TriangleFace.FrontAndBack;
        /// <summary>
        /// VBObjects view mode
        /// </summary>
        public ObjView ViewMode { get; set; }
        /// <summary>
        /// VBObjects id
        /// </summary>
        public int ObjID { get; set; }

        /// <summary>
        /// VBObjects name
        /// </summary>
        public string ObjName { get; set; }

        /// <summary>
        /// ViewState
        /// </summary>
        public bool ViewState { get; set; } = true;
        /// <summary>
        /// Load VBObjects to video card memory creating buffers
        /// </summary>
        /// <returns></returns>
        public abstract void Load();
        /// <summary>
        /// Draw objects
        /// </summary>
        public abstract void Draw();

        public override string ToString()
        {
            return $"{ObjName} {ViewState} {ViewMode}";
        }

        /* Убранные элементы
          
         Попробовали, но идея до конца не сработала.
         Снижается производительность. Нельзя выбрать все объекты когда -
         - включен режим только открытые поверхности

        public IEnumerable<List<Point3D>> GetObjsCoords()
        {
            var objsCoord = new float[CoordLength];
            VBO.GetSubData(CoordsBuffer, 0, objsCoord.Length * sizeof(float), objsCoord);

            for (int i = 0; i < Strides.Length; i++)
            {
                var fStride = 0;
                var sStride = Strides[i];
                if (i != 0)
                    fStride = Strides[i - 1];

                var points = new List<Point3D>();

                for (int j = fStride; j < sStride; j++)
                    points.Add(new Point3D(objsCoord[j], objsCoord[j + 1], objsCoord[j + 2]));

                yield return points;
            }
        }



        public IEnumerable<List<Color>> GetObjsColors()
        {
            var objsColors = new float[ColorLength];
            VBO.GetSubData(ColorsBuffer, 0, objsColors.Length * sizeof(float), objsColors);

            for (int i = 0; i < Strides.Length; i++)
            {
                var fStride = 0;
                var sStride = Strides[i];
                if (i != 0)
                    fStride = Strides[i - 1];

                //var objCoord = new float[sStride - fStride];
                var colors = new List<Color>();

                for (int j = fStride; j < sStride; j ++)
                {
                    var color = Color.FromArgb
    ((int)(255 * objsColors[4 * j + 3]),
    (int)(255 * objsColors[4 * j]),
    (int)(255 * objsColors[4 * j + 1]),
    (int)(255 * objsColors[4 * j + 2]));                    
                    colors.Add(color);
                }
                yield return colors;
            }
        }
/// <inheritdoc/>

        public void SetObjsColors(List<int> indexes, Color color)
        {
            var fInd = indexes.First();
            var lInd = indexes.Last();

            var fStride = 0;
            var sStride = Strides[lInd];
            if (fInd != 0)
                fStride = Strides[fInd - 1];

            var length = sStride - fStride;
            var colors = new float[4 * length];

            for (int j = 0; j < length; j++)
            {
                colors[4 * j + 0] = color.R / 255.0f;
                colors[4 * j + 1] = color.G / 255.0f;
                colors[4 * j + 2] = color.B / 255.0f;
                colors[4 * j + 3] = color.A / 255.0f;
            }

            VBO.SetSubData(ColorsBuffer, 4 * fStride * sizeof(float), colors.Length * sizeof(float),colors);
        }
        */
        /// <inheritdoc/>

        public int[] PointsIndexes
        {
            get
            {
                var ptrs = new int[PtrLength];
                VBO.GetSubData(PointersBuffer, 0, ptrs.Length * sizeof(int), ptrs);

                return ptrs;
            }
            set
            {
                VBO.SetSubData(PointersBuffer, 0, value.Length * sizeof(int), value);
            }
        }
        /// <inheritdoc/>


        public float[] PointsColors
        {
            get
            {
                var colors = new float[ColorLength];
                VBO.GetSubData(ColorsBuffer, 0, colors.Length * sizeof(float), colors);
                return colors;

            }
            set
            {
                VBO.SetSubData(ColorsBuffer, 0, value.Length * sizeof(float), value);
            }
        }
        /// <inheritdoc/>
        public float[] PointsCoords
        {
            get
            {
                var coords = new float[CoordLength];
                
                VBO.GetSubData(CoordsBuffer, 0, coords.Length * sizeof(float), coords);
                return coords;
            }
            set
            {
                VBO.SetSubData(CoordsBuffer, 0, value.Length * sizeof(float), value);
            }
        }
        /// <inheritdoc/>
        public float[] NormalsCoords
        {
            get
            {
                var normals = new float[NormalLength];

                VBO.GetSubData(NormalsBuffer, 0, normals.Length * sizeof(float), normals);
                return normals;
            }
            set
            {
                VBO.SetSubData(NormalsBuffer, 0, value.Length * sizeof(float), value);
            }
        }
        /// <summary>
        /// Установить единичную модельную матрицу 
        /// </summary>
        public void SetIdentityModelMatrix()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.GetFloat(GetPName.ModelviewMatrix, ModelMatrix);
            GL.PopMatrix();
        }
    }
}
