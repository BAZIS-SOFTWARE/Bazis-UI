using System.Drawing;

namespace BazisGUI.SceneCore.Text
{
    /// <summary>Экранная подпись в физических пикселях от левого нижнего угла окна.</summary>
    public sealed class TextLabel2D : TextLabel
    {
        public double X { get; }
        public double Y { get; }

        /// <summary>Создаёт экранную подпись.</summary>
        public TextLabel2D(string text, Color color, double x, double y) : base(text, color)
        {
            X = x;
            Y = y;
        }
    }
}
