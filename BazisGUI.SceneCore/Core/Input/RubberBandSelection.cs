
namespace BazisGUI.SceneCore.Input
{
    /// <summary>Отслеживает состояние выделения рамкой между PointerPressed и PointerReleased.</summary>
    public class RubberBandSelection
    {
        public Coordinates.ScreenPoint Start { get; private set; }
        public Coordinates.ScreenPoint Current { get; private set; }
        public bool IsActive { get; private set; }

        public void Begin(Coordinates.ScreenPoint start)
        {
            Start = start;
            Current = start;
            IsActive = true;
        }

        public void Update(Coordinates.ScreenPoint current) => Current = current;

        public void End() => IsActive = false;

        public bool HasMoved => IsActive && (Start.X != Current.X || Start.Y != Current.Y);
    }
}
