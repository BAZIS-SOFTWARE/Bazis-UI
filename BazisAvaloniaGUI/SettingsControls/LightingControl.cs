using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using System;
using System.Drawing;

namespace BazisAvaloniaGUI.SettingsControls
{
    /// <summary>
    /// Avalonia-аналог LightingControl: положение источника света задаётся перетаскиванием точки на панели.
    /// Координаты положения отсчитываются от центра панели, ось Y направлена вверх.
    /// </summary>
    internal sealed class LightingControl : UserControl
    {
        public Action<System.Drawing.Point> SetBallPositionEvent;
        private System.Drawing.Point ballPosition;
        public System.Drawing.Point BallPosition
        {
            get => ballPosition;
            set
            {
                ballPosition = value;
                panel.InvalidateVisual();
            }
        }
        private int BallRadius { get; set; }
        private bool IsPointInsideBall { get; set; }
        private bool IsMouseDownState { get; set; }

        private IBrush BallBrush { get; set; }

        private readonly BallPanel panel;

        public LightingControl()
        {
            panel = new BallPanel(this);
            panel.PointerPressed += OnDown;
            panel.PointerMoved += OnMove;
            panel.PointerReleased += OnUp;
            panel.PointerExited += panel_MouseLeave;
            panel.PointerCaptureLost += (_, _) =>
            {
                IsPointInsideBall = false;
                IsMouseDownState = false;
            };

            // tableLayoutPanel1: панель и пустая строка 20 px снизу.
            var layout = new Grid { RowDefinitions = new RowDefinitions("*,20") };
            layout.Children.Add(panel);
            Content = layout;

            BallRadius = 5;
            BallBrush = Brushes.Black;
        }

        private void OnMove(object sender, PointerEventArgs e)
        {
            var position = e.GetPosition(panel);
            if (IsPointInsideBall && IsMouseDownState)
            {
                var status = false;
                if (position.X - BallRadius >= 0 && position.X + BallRadius < panel.Bounds.Width)
                    status = true;
                if (position.Y - BallRadius >= 0 && position.Y + BallRadius < panel.Bounds.Height && status == true)
                {
                    BallPosition = new System.Drawing.Point((int)position.X - (int)(panel.Bounds.Width / 2), -(int)position.Y + (int)(panel.Bounds.Height / 2));
                }
            }
        }

        private void OnDown(object sender, PointerPressedEventArgs e)
        {
            var position = e.GetPosition(panel);
            if (!IsMouseDownState && e.GetCurrentPoint(panel).Properties.IsLeftButtonPressed)
            {
                // Захват и отрисовка используют центр одной и той же панели без нижнего отступа.
                var xDif = position.X - panel.Bounds.Width / 2 - BallPosition.X;
                var xPow = xDif * xDif;
                var yDif = -position.Y + panel.Bounds.Height / 2 - BallPosition.Y;
                var yPow = yDif * yDif;
                if (xPow + yPow <= BallRadius * BallRadius)
                {
                    IsPointInsideBall = true;
                    IsMouseDownState = true;
                    e.Pointer.Capture(panel);
                    e.Handled = true;
                }
            }
        }

        private void OnUp(object sender, PointerReleasedEventArgs e)
        {
            if (!IsMouseDownState || e.InitialPressMouseButton != MouseButton.Left)
                return;

            IsPointInsideBall = false;
            IsMouseDownState = false;
            e.Pointer.Capture(null);
            SetBallPositionEvent?.Invoke(BallPosition);
            e.Handled = true;
        }

        private void OnPaint(DrawingContext context)
        {
            if (BallBrush == null)
                return;

            var leftX = BallPosition.X + panel.Bounds.Width / 2 - BallRadius;
            var leftY = -BallPosition.Y + panel.Bounds.Height / 2 - BallRadius;
            var rect = new Rect(leftX, leftY, BallRadius * 2, BallRadius * 2);
            context.DrawEllipse(BallBrush, null, rect);
        }

        private void panel_MouseLeave(object sender, PointerEventArgs e)
        {
            if (IsMouseDownState)
                return;

            IsPointInsideBall = false;
            IsMouseDownState = false;
        }

        /// <summary>Панель с отрисовкой точки (в WinForms — Panel с обработчиком Paint).</summary>
        private sealed class BallPanel(LightingControl owner) : Control
        {
            public override void Render(DrawingContext context)
            {
                context.FillRectangle(Brushes.Transparent, new Rect(Bounds.Size));
                owner.OnPaint(context);
            }
        }
    }
}
