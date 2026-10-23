using System;
using System.Drawing;
using SceneCore.Camera;
using WinGUI.Scene.EventsArgs;
using WinGUI.Scene.Interfaces;
using Geometry;

namespace SceneCore.Input
{
    /// <summary>
    /// Перенос BaseForm.GlControl_MouseMove/MouseDown/MouseUp/MouseWheel/KeyDown, без зависимости
    /// от System.Windows.Forms.MouseEventArgs/Keys — см. scene.avalonia.md, раздел 6.
    /// Выбор объектов (ScenePicker.SelectByPoint/SelectByRect) требует данных модели
    /// (IEnumerable&lt;ISetInfo&gt;), которых у Core нет — поэтому контроллер ввода только
    /// вычисляет прямоугольник выделения и сообщает о жесте через SelectionRequested;
    /// сам подбор объектов выполняет вызывающий код через SceneController.SelectByPoint/SelectByRect.
    /// </summary>
    public class SceneInputController
    {
        private readonly SceneCamera camera;
        private readonly RubberBandSelection rubberBand = new RubberBandSelection();

        private Point2D lastPosition;
        private bool moved;

        public Viewport Viewport { get; set; }
        public ViewAxis RotationAxis { get; set; } = ViewAxis.XYZ;
        public float RotationAngle { get; set; } = 2.5f; // как SettingsConfig.RotationAngle

        public event Action Invalidated;
        public event EventHandler<SelectObjectsEventArgs> SelectionRequested;
        public event Action<bool> RotationPointVisibilityChanged;
        public event Action FitToScreenRequested;

        public SceneInputController(SceneCamera camera)
        {
            this.camera = camera;
        }

        private Point2D ToCentered(Point2D p) => new Point2D(p._x - Viewport.Width / 2f, -p._y + Viewport.Height / 2f);

        public void OnPointerPressed(SceneMouseEventArgs args)
        {
            moved = false;
            lastPosition = args.Position;

            if (args.Button == SceneMouseButton.Middle)
                RotationPointVisibilityChanged?.Invoke(true);
            else if (args.Button == SceneMouseButton.Left)
                rubberBand.Begin(args.Position);
        }

        public void OnPointerMoved(SceneMouseEventArgs args)
        {
            var centered = ToCentered(args.Position);
            moved = true;

            if (args.Button == SceneMouseButton.Left && rubberBand.IsActive)
            {
                rubberBand.Update(args.Position);
            }
            else if (args.Button == SceneMouseButton.Right)
            {
                camera.Move(
                    new Point((int)centered._x, (int)centered._y),
                    new Point((int)ToCentered(lastPosition)._x, (int)ToCentered(lastPosition)._y),
                    camera.ScaleFactor);
            }
            else if (args.Button == SceneMouseButton.Middle)
            {
                var previous = ToCentered(lastPosition);
                const int moveCamZ = -5;
                var dx = (float)((centered._x - previous._x) * (2 * -moveCamZ) / Viewport.Width);
                var dy = (float)((centered._y - previous._y) * (2 * -moveCamZ) / Viewport.Height);
                camera.Rotate(dx, dy, RotationAxis, RotationAngle);
            }

            lastPosition = args.Position;
            Invalidated?.Invoke();
        }

        public void OnPointerReleased(SceneMouseEventArgs args)
        {
            // Отпускание без начатой рамки (нажатие было вне сцены или рамка прервана) — не выбор.
            if (args.Button == SceneMouseButton.Left && rubberBand.IsActive)
            {
                var isSelected = (args.Modifiers & SceneModifierKeys.Shift) == 0;
                RectangleBox box;

                if (!moved)
                {
                    var p = ToCentered(rubberBand.Start);
                    box = new RectangleBox((int)p._x - 5, (int)p._x + 5, (int)p._y - 5, (int)p._y + 5);
                }
                else
                {
                    var start = ToCentered(rubberBand.Start);
                    var end = ToCentered(rubberBand.Current);
                    box = new RectangleBox((int)start._x, (int)end._x, (int)end._y, (int)start._y);
                }

                rubberBand.End();
                SelectionRequested?.Invoke(this, new SelectObjectsEventArgs(box, moved, isSelected));
            }
            else if (args.Button == SceneMouseButton.Middle)
            {
                RotationPointVisibilityChanged?.Invoke(false);
            }

            Invalidated?.Invoke();
        }

        public void OnWheelChanged(SceneMouseEventArgs args)
        {
            // Дробные шаги: тачпад и колёса высокого разрешения дают Delta меньше 120 за событие.
            // Для целых шагов результат совпадает с прежним циклом по 1.1 / 0.9.
            if (args.Delta != 0)
            {
                var steps = Math.Abs(args.Delta) / 120f;
                camera.Scale((float)Math.Pow(args.Delta > 0 ? 1.1 : 0.9, steps));
            }

            Invalidated?.Invoke();
        }

        /// <summary>Прерывает выделение рамкой без выбора объектов (потеря захвата указателя).</summary>
        public void CancelSelection()
        {
            rubberBand.End();
            moved = false;
        }

        public void OnKeyDown(SceneKeyEventArgs args)
        {
            if (args.Key == SceneKey.F)
                FitToScreenRequested?.Invoke();
        }
    }
}
