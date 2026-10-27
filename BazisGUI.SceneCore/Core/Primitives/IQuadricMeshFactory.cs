using System.Drawing;
using BazisGUI.SceneCore.VBO;

namespace BazisGUI.SceneCore.Primitives
{
    /// <summary>
    /// Заменяет gluNewQuadric/gluCylinder/gluSphere (Win32 P/Invoke из glu32.dll) генерацией
    /// треугольной сетки в VBObject (см. scene.avalonia.md, раздел 6). Цвет — обязательный
    /// параметр, так как в отличие от immediate-mode рисования цвет теперь запекается в VBO.
    /// </summary>
    public interface IQuadricMeshFactory
    {
        VBObject CreateCylinder(double baseR, double topR, double height, Color color);
        VBObject CreateSphere(double radius, Color color);
        VBObject CreateCone(double baseR, double height, Color color);
    }
}
