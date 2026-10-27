using BazisGUI.SceneCore.VBO;

namespace BazisGUI.SceneCore.Interfaces
{
    /// <summary>
    /// Элемент который сейчас активен в интерфейсе IActiveDrawingObject
    /// </summary>
    public enum DrawElements
    {
        /// <summary>
        /// Точки
        /// </summary>
        Points,
        /// <summary>
        /// Линии
        /// </summary>
        Lines,
        /// <summary>
        /// Каркас
        /// </summary>
        Wireframe,
        /// <summary>
        /// Геометрические объекты, не являющиеся vbo-объектами
        /// </summary>
        GeometryObjects,
        /// <summary>
        /// Поверхности
        /// </summary>
        Surfaces
    }
    /// <summary>
    /// Интерфейсный класс, методы которого вызываются перед glDrawElements и после него
    /// </summary>
    public interface IActiveDrawingObject
    {
        /// <summary>
        /// Выполнить действия перед вызовом glDrawElements
        /// </summary>
        /// <param name="vbo">[In]Вбо-объект, который вызывает отрисовку</param>
        /// <param name="element">[In]Элемент отрисовки</param>
        void DoActionsBeforeDrawing(VBObject vbo, DrawElements element);
        /// <summary>
        /// Выполнить действия после вызова glDrawElements
        /// </summary>
        /// <param name="vbo">[In]Вбо-объект, который заканчивает отрисовку</param>
        /// <param name="element">[In]Элемент отрисовки</param>
        void DoActionsAfterDrawing(VBObject vbo, DrawElements element);
    }
}
