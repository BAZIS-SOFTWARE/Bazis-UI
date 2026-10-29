using System.Drawing;

namespace BazisGUI.SceneCore.Text
{
    /// <summary>Подпись в трёхмерных координатах текущей проекции.</summary>
    public sealed class TextLabel3D : TextLabel
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        /// <summary>Масштаб знакоместа в единицах текущей проекции; null использует масштаб рендера по умолчанию.</summary>
        public float? WorldScale { get; }

        /// <summary>Создаёт подпись в трёхмерных координатах текущей проекции.</summary>
        public TextLabel3D(string text, Color color, double x, double y, double z, float? worldScale = null) : base(text, color)
        {
            X = x;
            Y = y;
            Z = z;
            WorldScale = worldScale;
        }
    }
}
