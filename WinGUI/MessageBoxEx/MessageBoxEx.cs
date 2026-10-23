using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using UserControlsEx;

namespace WinGUI.MessageBoxEx
{
    public partial class MessageBoxEx : UserControl
    {
        [Category("General")]
        [Description("Set down color gradient")]
        public string Message 
        { 
            get { return message.Text; }
            set { message.Text = value; }
        }
        public MessageBoxEx()
        {
            InitializeComponent();
        }

        private void MessageBoxEx_Paint(object sender, PaintEventArgs e)
        {
            var loc_y = messagePanel.Location.Y;

            ComponentsPainter.PaintGradientRectangle(e.Graphics, new Point(0, 0), Width, loc_y, Color.Gainsboro, Color.Gainsboro);
        }
    }
}
