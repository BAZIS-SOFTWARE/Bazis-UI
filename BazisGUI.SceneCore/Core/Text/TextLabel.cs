using System.Drawing;

namespace BazisGUI.SceneCore.Text
{
    /// <summary>Общие данные подписи сцены.</summary>
    public abstract class TextLabel
    {
        public string Text { get; }
        public Color Color { get; }

        /// <summary>Создаёт подпись с текстом и цветом.</summary>
        protected TextLabel(string text, Color color)
        {
            Text = text;
            Color = color;
        }
    }
}
