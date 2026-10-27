using System;
using System.Drawing;
using System.Windows.Forms;

namespace BazisGUI.Win.SettingsControls
{
    public partial class LightingControl : UserControl
    {
        public Action<Point> SetBallPositionEvent;
        public Point BallPosition { get; set; }
        private int BallRadius { get; set; }
        private bool IsPointInsideBall { get; set; }
        private bool IsMouseDownState { get; set; }

        private Brush BallBrush { get; set; }

        public LightingControl()
        {
            DoubleBuffered = true;
            InitializeComponent();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            BallRadius = 5;
            BallPosition = new Point();
            BallBrush = new SolidBrush(Color.Black);
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }

        private void OnMove(object sender, MouseEventArgs e)
        {
            if (IsPointInsideBall && IsMouseDownState)
            {
                var status = false;
                if (e.X - BallRadius >= 0 && e.X + BallRadius < panel.Width)
                    status = true;
                if (e.Y - BallRadius >= 0 && e.Y + BallRadius < panel.Height && status == true)
                {
                    BallPosition = new Point(e.X - (panel.Width / 2), -e.Y + (panel.Height / 2));
                    panel.Invalidate();
                }
            }
        }

        private void OnDown(object sender, MouseEventArgs e)
        {
            if (!IsMouseDownState)
            {
                var xDif = e.X - (Width / 2) - BallPosition.X;
                var xPow = xDif * xDif;
                var yDif = -e.Y + (Height / 2) - BallPosition.Y;
                var yPow = yDif * yDif;
                if (xPow + yPow <= BallRadius * BallRadius)
                    IsPointInsideBall = true;
                IsMouseDownState = true;
            }
        }

        private void OnUp(object sender, MouseEventArgs e)
        {
            IsPointInsideBall = false;
            IsMouseDownState = false;

            SetBallPositionEvent(BallPosition);
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            var leftX = BallPosition.X + panel.Width / 2 - BallRadius;
            var leftY = -BallPosition.Y + panel.Height / 2 - BallRadius;
            var rect = new RectangleF(leftX, leftY, BallRadius * 2, BallRadius * 2);
            e.Graphics.FillEllipse(BallBrush, rect);
        }

        private void panel_MouseLeave(object sender, EventArgs e)
        {
            IsPointInsideBall = false;
            IsMouseDownState = false;
        }
    }
}
