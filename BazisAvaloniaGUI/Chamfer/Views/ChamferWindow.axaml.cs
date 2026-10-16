using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace BazisAvaloniaGUI.Chamfer.Views
{
    public partial class ChamferWindow : Window
    {
        public ChamferWindow()
        {
            InitializeComponent();
        }

        private void OnTitleBarPointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                BeginMoveDrag(e);
        }

        private void OnCloseClick(object sender, RoutedEventArgs e) => Close();
    }
}
