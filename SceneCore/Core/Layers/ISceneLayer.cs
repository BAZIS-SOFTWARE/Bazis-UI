using SceneCore.Rendering;

namespace SceneCore.Layers
{
    /// <summary>
    /// Заменяет поля-события DisplayBasisEvent/DisplayCompassEvent/... — у слоя есть имя
    /// и порядок, поэтому скрытие перестаёт быть поиском делегата по имени метода
    /// (см. scene.avalonia.md, раздел 6).
    /// </summary>
    public interface ISceneLayer
    {
        string Name { get; }
        int Order { get; }
        bool IsVisible { get; set; }
        void Draw(IRenderContext context);
    }
}
