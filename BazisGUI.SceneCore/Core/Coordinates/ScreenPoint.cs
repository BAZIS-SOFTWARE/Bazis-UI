namespace BazisGUI.SceneCore.Coordinates
{
    /// <summary>Точка в физических пикселях экрана с началом в левом верхнем углу.</summary>
    public readonly struct ScreenPoint
    {
        public double X { get; }
        public double Y { get; }

        /// <summary>Создаёт точку в экранных координатах.</summary>
        public ScreenPoint(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
