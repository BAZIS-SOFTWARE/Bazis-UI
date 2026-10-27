namespace BazisGUI.SceneCore.Coordinates
{
    /// <summary>Вектор смещения или направления в системе координат модели.</summary>
    public readonly struct ModelVector3D
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        /// <summary>Создаёт вектор в системе координат модели.</summary>
        public ModelVector3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
