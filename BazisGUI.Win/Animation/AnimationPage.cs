using BazisGUI.Win.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace BazisGUI.Win.Animation
{
    public partial class AnimationPage : UserControl
    {
        public event Action<object, CreateAnimationEventArgs> CreateGIFAnimationEvent;


        public AnimationPage()
        {
            InitializeComponent();
        }

        private void txbDelayTime_Leave(object sender, EventArgs e)
        {
            int res;

            txbDelayTime.Text = "100";
            MessageBox.Show("Некорректный ввод!");

        }

        private void btnCreateAnimation_Click(object sender, EventArgs e)
        {
            try
            {
                var delay = int.Parse(txbDelayTime.Text);
      
                CreateGIFAnimationEvent(this, new CreateAnimationEventArgs(chbDelTempScrs.Checked, delay));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
   
        }
    }
}
