namespace BazisGUI.SceneCore.Coordinates
{
    /// <summary>Точка в системе координат камеры после преобразования матрицей вида.</summary>
    public readonly struct CameraPoint3D
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        /// <summary>Создаёт точку в системе координат камеры.</summary>
        public CameraPoint3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
