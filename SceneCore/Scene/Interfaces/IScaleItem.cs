using System.Drawing;

namespace WinGUI.Scene.Interfaces
{
    /// <summary>
    /// IScaleItem
    /// </summary>
    public interface IScaleItem
    {
        /// <summary>
        /// Color
        /// </summary>
        Color Color { get; }
        /// <summary>
        /// Min
        /// </summary>
        float Min { get; }
        /// <summary>
        /// Max
        /// </summary>
        float Max { get; }
    }
}