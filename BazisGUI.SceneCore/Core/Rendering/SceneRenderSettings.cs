using System.Drawing;

namespace BazisGUI.SceneCore.Rendering
{
    /// <summary>
    /// Настройки рендера, выделенные из SettingsConfig — там сейчас смешаны настройки
    /// приложения (язык, путь к солверу...) и настройки собственно рендера
    /// (см. scene.avalonia.md, раздел 6).
    /// </summary>
    public class SceneRenderSettings
    {
        public Color BackGroundColor { get; set; } = Color.White;
        public Color SelectionColor { get; set; } = Color.Red;
        public bool IsLighting { get; set; } = true;
        public bool IsBlending { get; set; }
        public bool IsCutting { get; set; }
        public bool IsClipPlane { get; set; }
        public bool DisplayBasis { get; set; } = true;
        public bool DisplayCompass { get; set; } = true;
        public bool Transparency { get; set; }
        public Coordinates.ScreenPoint LighterPosition { get; set; } = new Coordinates.ScreenPoint(0, 0);
    }
}
