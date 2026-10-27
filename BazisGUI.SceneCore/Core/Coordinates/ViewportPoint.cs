namespace BazisGUI.SceneCore.Coordinates
{
    /// <summary>
    /// Точка окна просмотра в физических пикселях с началом в центре и осью Y вверх.
    /// </summary>
    public sealed record class ViewportPoint(double X, double Y);
}
