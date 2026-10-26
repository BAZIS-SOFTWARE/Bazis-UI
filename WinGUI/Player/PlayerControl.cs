using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Reflection.Emit;

namespace WinGUI.Player
{
    public enum CheckState : int { start, pause, continuation };
    public partial class PlayerControl: UserControl
    {
        System.Windows.Forms.Timer timer;
        int stopValue = 100;

        public bool Cancelation { get; set; } = false;

        [Category("Colors")]
        public Color TextValueColor
        {
            get { return colorSlider.TextValueColor; }
            set { colorSlider.TextValueColor = value; }
        }
        public Color SliderBarInnerColor
        {
            get { return colorSlider.BarInnerColor; }
            set { colorSlider.BarInnerColor = value; }
        }
        [Category("Colors")]
        public Color SliderBarOuterColor
        {
            get { return colorSlider.BarOuterColor; }
            set { colorSlider.BarOuterColor = value; }
        }

        [Category("Colors")]
        public Color SliderElapsedInnerColor
        {
            get { return colorSlider.ElapsedInnerColor; }
            set { colorSlider.ElapsedInnerColor = value; }
        }

        [Category("Colors")]
        public Color SliderElapsedOuterColor
        {
            get { return colorSlider.ElapsedOuterColor; }
            set { colorSlider.ElapsedOuterColor = value; }
        }

        [Category("General")]
        public bool ShowTextValue 
        {
            get { return colorSlider.ShowTextValue; } 
            set { colorSlider.ShowTextValue = value; }
        }

        [Category("General")]
        public CheckState CheckState { get; set; }

        public event Action<object, int> CheckingEvent;
        public event Action<object> StopCheckingEvent;
        public event Action<object> StartCheckingEvent;
        public event Action<object> PauseCheckingEvent;

        [Category("General")]
        public int CurrentValue 
        {
            get
            {
                var value = colorSlider.Value;
                return Math.Min(value, StopValue);
            }
            set
            {
                var position = Math.Min(value, StopValue);
                colorSlider.Value = position;
            }
        }

        [Category("General")]
        public int StartValue
        {
            get { return colorSlider.Minimum; }
            set { colorSlider.Minimum = value; }
        }
        [Category("General")]
        public int StopValue 
        { 
            get { return stopValue; }
            set
            {
                if (value < StartValue)
                    throw new ArgumentOutOfRangeException(nameof(value), "The stop value must not precede the start value.");

                // Ползунку нужен ненулевой диапазон, даже если плеер показывает один кадр.
                var maximum = value == StartValue ? checked(value + 1) : value;
                colorSlider.Maximum = maximum;
                colorSlider.Enabled = value > StartValue;
                stopValue = value;
            }
        }

        [Category("General")]
        public int SpeedValue { get; set; } = 500;

        public PlayerControl()
        {
            InitializeComponent();
            timer = new System.Windows.Forms.Timer();
            timer.Tick += new System.EventHandler(Timer_Tick);
        }

        public virtual void StopChecking_Click(object sender, EventArgs e)
        {
            StopChecking();

            StopCheckingEvent?.Invoke(this);
        }

        /// <summary>
        /// Останавливает плеер и сбрасывает позицию и отмену проверки.
        /// </summary>
        public void StopChecking()
        {
            timer.Stop();
            timer.Enabled = false;

            CheckState = CheckState.start;
            Cancelation = false;
            CurrentValue = StartValue;
            SetCheckButtonState();
        }

        public virtual void StartChecking_Click(object sender, EventArgs e)
        {

            if (CheckState == CheckState.pause)
            {
                CheckState = CheckState.continuation;
                timer.Stop();
                SetCheckButtonState();

                PauseCheckingEvent?.Invoke(this);
            }
            else if (CheckState == CheckState.start)
            {
                if (CurrentValue >= StopValue)
                    CurrentValue = StartValue;
                CheckState = CheckState.pause;

                timer.Enabled = true;
                timer.Interval = SpeedValue;

                StartCheckingEvent?.Invoke(this);
                Thread.Sleep(100);

                SetCheckButtonState();
                timer.Start();
            }
            else
            {
                CheckState = CheckState.pause;

                timer.Start();
                SetCheckButtonState();
            }

        }

        /// <summary>
        /// Проверяет текущую позицию и завершает воспроизведение, сохраняя конечный кадр.
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (Cancelation)
            {
                StopChecking();

                StopCheckingEvent?.Invoke(this);
            }
            else
            {
                CheckingEvent?.Invoke(this, CurrentValue);
                Thread.Sleep(100);
                if (CurrentValue >= StopValue)
                {
                    timer.Stop();
                    timer.Enabled = false;
                    CheckState = CheckState.start;
                    SetCheckButtonState();
                }
                else
                    CurrentValue++;
            }

        }

        private void SetCheckButtonState()
        {
            if (CheckState == CheckState.start)
            {
                btnCheckDinamic.Image = WinGUI.Properties.Resources.StartCheck.ToBitmap();
            }
            else if (CheckState == CheckState.pause)
            {
                btnCheckDinamic.Image = WinGUI.Properties.Resources.Pause.ToBitmap();
            }
            else
            {
                btnCheckDinamic.Image = WinGUI.Properties.Resources.StartCheck.ToBitmap();
            }
        }
    }
}
