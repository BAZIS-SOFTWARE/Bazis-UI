using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using System;

namespace BazisAvaloniaGUI.Player
{
    public enum CheckState : int { start, pause, continuation };

    /// <summary>
    /// Avalonia-аналог PlayerControl: пошаговый просмотр условий задачи или кадров результатов во времени.
    /// Кнопка запуска/паузы, кнопка остановки и ползунок текущего шага.
    /// </summary>
    internal sealed class PlayerControl : UserControl, IDisposable
    {
        private readonly DispatcherTimer timer = new();
        private readonly Button btnCheckDinamic;
        private readonly Button btnStopCheck;
        private readonly Slider colorSlider;
        private readonly TextBlock valueText;
        private readonly Image checkImage;
        private readonly Bitmap startIcon = LoadIcon("StartCheck");
        private readonly Bitmap pauseIcon = LoadIcon("Pause");
        private readonly Bitmap stopIcon = LoadIcon("Stop");
        private bool disposed;
        private int stopValue = 100;

        public bool Cancelation { get; set; } = false;

        public CheckState CheckState { get; set; }

        public event Action<object, int> CheckingEvent;
        public event Action<object> StopCheckingEvent;
        public event Action<object> StartCheckingEvent;
        public event Action<object> PauseCheckingEvent;

        public int CurrentValue
        {
            get => Math.Min((int)Math.Round(colorSlider.Value), StopValue);
            set => colorSlider.Value = Math.Min(value, StopValue);
        }

        public int StartValue
        {
            get => (int)colorSlider.Minimum;
            set => colorSlider.Minimum = value;
        }

        public int StopValue
        {
            get => stopValue;
            set
            {
                if (value < StartValue)
                    throw new ArgumentOutOfRangeException(nameof(value), "The stop value must not precede the start value.");

                // Ползунку нужен ненулевой диапазон, даже если плеер показывает один кадр.
                colorSlider.Maximum = value == StartValue ? checked(value + 1) : value;
                colorSlider.IsEnabled = value > StartValue;
                stopValue = value;
            }
        }

        public int SpeedValue { get; set; } = 500;

        public PlayerControl()
        {
            checkImage = new Image { Width = 20, Height = 20, Source = startIcon };
            btnCheckDinamic = new Button { Name = "btnCheckDinamic", Content = checkImage, Width = 31, Height = 31, Padding = new Thickness(2) };
            btnCheckDinamic.Click += (_, _) => StartChecking_Click();

            btnStopCheck = new Button
            {
                Name = "btnStopCheck",
                Content = new Image { Width = 20, Height = 20, Source = stopIcon },
                Width = 31,
                Height = 31,
                Padding = new Thickness(2)
            };
            btnStopCheck.Click += (_, _) => StopChecking_Click();

            colorSlider = new Slider
            {
                Name = "colorSlider",
                Minimum = 0,
                Maximum = 100,
                SmallChange = 1,
                LargeChange = 1,
                IsSnapToTickEnabled = true,
                TickFrequency = 1,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(4, 0)
            };
            valueText = new TextBlock { VerticalAlignment = VerticalAlignment.Center, MinWidth = 24, Foreground = Brushes.Black };
            colorSlider.PropertyChanged += (_, e) =>
            {
                if (e.Property == RangeBase.ValueProperty)
                    valueText.Text = CurrentValue.ToString();
            };

            var layout = new Grid { ColumnDefinitions = new ColumnDefinitions("Auto,Auto,*,Auto"), Margin = new Thickness(2) };
            layout.Children.Add(btnCheckDinamic);
            Grid.SetColumn(btnStopCheck, 1);
            layout.Children.Add(btnStopCheck);
            Grid.SetColumn(colorSlider, 2);
            layout.Children.Add(colorSlider);
            Grid.SetColumn(valueText, 3);
            layout.Children.Add(valueText);

            MinHeight = 35;
            Content = new Border { BorderBrush = Brushes.Gray, BorderThickness = new Thickness(1), Child = layout };

            StartValue = 0;
            StopValue = 100;
            CurrentValue = 0;
            valueText.Text = CurrentValue.ToString();
            timer.Tick += Timer_Tick;
        }

        private static Bitmap LoadIcon(string name)
        {
            using var stream = AssetLoader.Open(new Uri($"avares://BazisAvaloniaGUI/Player/Assets/{name}.png"));
            return new Bitmap(stream);
        }

        private void StopChecking_Click()
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

            CheckState = CheckState.start;
            Cancelation = false;
            CurrentValue = StartValue;
            SetCheckButtonState();
        }

        private void StartChecking_Click()
        {
            ObjectDisposedException.ThrowIf(disposed, this);
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

                timer.Interval = TimeSpan.FromMilliseconds(Math.Max(1, SpeedValue));

                StartCheckingEvent?.Invoke(this);

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
                if (CurrentValue >= StopValue)
                {
                    timer.Stop();
                    CheckState = CheckState.start;
                    SetCheckButtonState();
                }
                else
                    CurrentValue++;
            }
        }

        private void SetCheckButtonState() =>
            checkImage.Source = CheckState == CheckState.pause ? pauseIcon : startIcon;

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            timer.Stop();
            if (!disposed && CheckState == CheckState.pause)
            {
                CheckState = CheckState.continuation;
                SetCheckButtonState();
            }
            base.OnDetachedFromVisualTree(e);
        }

        public void Dispose()
        {
            if (disposed)
                return;

            disposed = true;
            timer.Stop();
            timer.Tick -= Timer_Tick;
            checkImage.Source = null;
            ((Image)btnStopCheck.Content).Source = null;
            startIcon.Dispose();
            pauseIcon.Dispose();
            stopIcon.Dispose();
        }
    }
}
