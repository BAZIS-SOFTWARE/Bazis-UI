namespace BazisAvaloniaGUI.Chamfer.Services
{
    /// <summary>
    /// Boundary between the Chamfer ViewModel and the application's geometry logic.
    /// </summary>
    public interface IChamferOperationService
    {
        void AddByAngle(double length, double angle, bool isReflected);

        void AddByLengths(double length1, double length2, bool isReflected);

        void Prewiew(double length, double valueSecond, bool isAngle, bool isReflected);

        void ClearPreview();
    }
}
