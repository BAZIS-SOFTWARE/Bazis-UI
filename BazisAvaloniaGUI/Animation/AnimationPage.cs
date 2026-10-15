using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using System;

namespace BazisAvaloniaGUI.Animation
{
    public class CreateAnimationEventArgs : EventArgs
    {
        public bool DeleteTempImages { get; }

        public int DelayTime { get; }

        public CreateAnimationEventArgs(bool deleteTempImages, int delayTime)
        {
            DeleteTempImages = deleteTempImages;
            DelayTime = delayTime;
        }
    }

    /// <summary>Avalonia-аналог AnimationPage: параметры создания GIF-анимации результатов.</summary>
    internal sealed class AnimationPage : UserControl
    {
        public event Action<object, CreateAnimationEventArgs> CreateGIFAnimationEvent;

        /// <summary>Ошибка ввода (в BaseForm — MessageBox), сообщение выводит оболочка.</summary>
        public event Action<string> ErrorReported;

        private const int MaxGifDelay = ushort.MaxValue * 10;

        private readonly TextBox txbDelayTime;
        private readonly CheckBox chbDelTempScrs;

        public AnimationPage()
        {
            // В WinForms подписи заданы в конструкторе формы без локализации.
            txbDelayTime = new TextBox { Name = "txbDelayTime", Text = "100", Width = 60 };
            chbDelTempScrs = new CheckBox { Name = "chbDelTempScrs" };
            var btnCreateAnimation = new Button { Name = "btnCreateAnimation", Content = "Создать", MinWidth = 146, HorizontalContentAlignment = HorizontalAlignment.Center };
            btnCreateAnimation.Click += btnCreateAnimation_Click;

            var layout = new Grid { ColumnDefinitions = new ColumnDefinitions("Auto,Auto,*"), RowDefinitions = new RowDefinitions("Auto,Auto"), RowSpacing = 16, ColumnSpacing = 12, Margin = new Thickness(16) };
            var label2 = new TextBlock { Text = "Задержка между слайдами", VerticalAlignment = VerticalAlignment.Center };
            var label3 = new TextBlock { Text = "Удалить промежуточные результаты", VerticalAlignment = VerticalAlignment.Center, TextWrapping = Avalonia.Media.TextWrapping.Wrap, MaxWidth = 150 };
            layout.Children.Add(label2);
            Grid.SetColumn(txbDelayTime, 1);
            layout.Children.Add(txbDelayTime);
            Grid.SetRow(label3, 1);
            layout.Children.Add(label3);
            Grid.SetRow(chbDelTempScrs, 1);
            Grid.SetColumn(chbDelTempScrs, 1);
            layout.Children.Add(chbDelTempScrs);
            Grid.SetRow(btnCreateAnimation, 1);
            Grid.SetColumn(btnCreateAnimation, 2);
            layout.Children.Add(btnCreateAnimation);
            Content = layout;
        }

        private void btnCreateAnimation_Click(object sender, RoutedEventArgs e)
        {
            // Задержка кадра GIF — 16-битное число сотых долей секунды: больше 655350 мс поле переполняется.
            if (!int.TryParse(txbDelayTime.Text, out var delay) || delay <= 0 || delay > MaxGifDelay)
            {
                txbDelayTime.Text = "100";
                ErrorReported?.Invoke("Некорректный ввод!");
                return;
            }

            CreateGIFAnimationEvent?.Invoke(this, new CreateAnimationEventArgs(chbDelTempScrs.IsChecked == true, delay));
        }
    }
}
