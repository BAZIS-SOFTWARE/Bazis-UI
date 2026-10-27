using global::Avalonia.Controls;
using global::Avalonia.Input;
using global::Avalonia.Interactivity;

namespace BazisGUI.Avalonia.Chamfer.Views
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
