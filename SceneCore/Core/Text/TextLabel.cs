using System.Drawing;
using Geometry;

namespace BazisGUI.Scene.Core.Text
{
    public class TextLabel
    {
        public string Text { get; set; }
        public Color Color { get; set; }
        public Point3D Position3D { get; set; }
        public Point2D Position2D { get; set; }
        public bool IsScreenSpace { get; set; }

        /// <summary>
        /// Размер знакоместа в единицах той системы координат, в которой рисуется подпись
        /// (1 — один пиксель атласа шрифта). null — масштаб рендера по умолчанию
        /// (SkiaGlyphAtlasTextRenderer.GlyphWorldScale), пригодный для подписей в мировых координатах.
        /// </summary>
        public float? WorldScale { get; set; }
    }
}
