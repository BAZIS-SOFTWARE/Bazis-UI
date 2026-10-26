using System.Drawing;

namespace SceneCore
{
    /// <summary>
    /// Интервал значений и цвет для отображения шкалы на сцене.
    /// </summary>
    public sealed class SceneScaleRange
    {
        public float Min { get; }
        public float Max { get; }
        public Color Color { get; }

        /// <summary>
        /// Создаёт интервал шкалы с заданными границами и цветом.
        /// </summary>
        public SceneScaleRange(float min, float max, Color color)
        {
            Min = min;
            Max = max;
            Color = color;
        }
    }
}
