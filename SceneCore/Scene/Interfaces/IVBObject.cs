namespace WinGUI.Scene.Interfaces
{
    /// <summary>
    /// Transform
    /// </summary>
    public struct Transform
    {
        /// <summary>
        /// x
        /// </summary>
        public float mov_x;
        /// <summary>
        /// y
        /// </summary>
        public float mov_y;
        /// <summary>
        /// z
        /// </summary>
        public float mov_z;
    }
    /// <summary>
    /// IVBObject
    /// </summary>
    public interface IVBObject
    {
        /// <summary>
        /// GL_ObjType
        /// </summary>
        GLObjType GL_ObjType { get; set; }//Публичный, т.к как-то нужно проверить копию и оригинал между собой (идентичность типов) извне
        /// <summary>
        /// Max_x
        /// </summary>
        float Max_x { get; } /*set;*/
        /// <summary>
        /// Max_y
        /// </summary>
        float Max_y { get; }  /*set;*/
        /// <summary>
        /// Max_z
        /// </summary>
        float Max_z { get; }
        /// <summary>
        /// ViewMode
        /// </summary>
        ObjView ViewMode { get; set; }
        /// <summary>
        /// VBObjects id
        /// </summary>
        int ObjID { get; set; }

        /// <summary>
        /// VBObjects name
        /// </summary>
        string ObjName { get; set; }
        /// <summary>
        /// ViewState
        /// </summary>
        bool ViewState { get; set; }
        /// <summary>
        /// Index buffer length
        /// </summary>
        int PtrLength { get; set; }
        /// <summary>
        /// Vertex buffer length
        /// </summary>
        int CoordLength { get; set; }
        /// <summary>
        /// Color buffer length
        /// </summary>
        int ColorLength { get; set; }
        /// <summary>
        /// Load VBObjects to video card memory creating buffers
        /// </summary>
        /// <returns></returns>
        void Load();
        /// <summary>
        /// Draw objects
        /// </summary>
        void Draw();
        /// <summary>
        /// PointsColors
        /// </summary>
        float[] PointsColors { get; set; }
        /// <summary>
        /// PointsCoords
        /// </summary>
        float[] PointsCoords { get; set; }
        /// <summary>
        /// PointsIndexes
        /// </summary>
        int[] PointsIndexes { get; set; }
        /// <summary>
        /// Координаты ограничивающего параллелепипеда
        /// </summary>
        BoundingBox BoundingBox { get; set; }
        /// <summary>
        /// Матрица 4x4 для трансформации объекта (поворот, масштабирование, перенос), вектора определены по столбцам
        /// </summary>
        float[] ModelMatrix { get; set; }
        /// <summary>
        /// Объект рисования, опирающийся на gpu-программу рисования
        /// </summary>
        IActiveDrawingObject ActiveDrawingObject { get; set; }
    }
}
