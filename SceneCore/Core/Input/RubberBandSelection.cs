using Geometry;

namespace SceneCore.Input
{
    /// <summary>Отслеживает состояние выделения рамкой между PointerPressed и PointerReleased.</summary>
    public class RubberBandSelection
    {
        public Point2D Start { get; private set; }
        public Point2D Current { get; private set; }
        public bool IsActive { get; private set; }

        public void Begin(Point2D start)
        {
            Start = start;
            Current = start;
            IsActive = true;
        }

        public void Update(Point2D current) => Current = current;

        public void End() => IsActive = false;

        public bool HasMoved => IsActive && (Start._x != Current._x || Start._y != Current._y);
    }
}
