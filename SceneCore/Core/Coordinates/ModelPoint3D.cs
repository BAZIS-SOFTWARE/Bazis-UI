namespace SceneCore.Coordinates
{
    /// <summary>Точка в системе координат модели.</summary>
    public readonly struct ModelPoint3D
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        /// <summary>Создаёт точку в системе координат модели.</summary>
        public ModelPoint3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
