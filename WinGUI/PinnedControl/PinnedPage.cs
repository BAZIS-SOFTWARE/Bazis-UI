using System;
using System.Drawing;
using System.Windows.Forms;
using UserControlsEx;

namespace WinGUI.PinnedControl
{
    public partial class PinnedPage : UserControl, IPinnedControl
    {
        public Color UpColor { get; set; } = Color.Gainsboro;

        public Color DownColor { get; set; } = Color.Gainsboro;

        public Color HeaderColor { get; set; } = Color.Black;

        public bool IsPinndable { get; set; } = false;

        public string HeaderName { get; set; } = "";

        public event Action ControlCollapseEvent;
        public event Action ControlUnpinnedEvent;

        public PinnedPage()
        {
            InitializeComponent();
        }

        private void PinnedPageControl_Paint(object sender, PaintEventArgs e)
        {
            var loc_y = Padding.Top;

            ComponentsPainter.PaintGradientRectangle(e.Graphics, new Point(0, 0), Width, loc_y, UpColor, DownColor);

            var locRect = new Point(Width - 15, loc_y / 2 - 4);
            ComponentsPainter.PaintCloseRectangle(e.Graphics, locRect);

            if (IsPinndable)
            {
                var locPinRect = new Point(Width - 26, loc_y / 2 - 4);
                ComponentsPainter.PaintUnpinnedRectangle(e.Graphics, locPinRect);
            }


            e.Graphics.DrawString(HeaderName, Font, new SolidBrush(HeaderColor), 15, 0);
        }

        private void PinnedPageControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Location.X > Width - 16 & e.Location.X < Width - 8 && e.Location.Y <= 10)
                ControlCollapseEvent?.Invoke();

            if(IsPinndable)
                if (e.Location.X > Width - 26 & e.Location.X < Width - 16 && e.Location.Y <= 10)
                    ControlUnpinnedEvent?.Invoke();
        }

        private void PinnedPageControl_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
