using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using System.Resources;

namespace BazisAvaloniaGUI.Shell
{
    /// <summary>Avalonia-аналог формы с AboutLicenseControl: владелец, ключи лицензии и адрес сервера.</summary>
    internal sealed class AboutLicenseWindow : Window
    {
        private readonly TextBlock lblCompanyName;
        private readonly TextBlock lblKeyInfo;
        private readonly TextBlock lblServerAdress;

        public AboutLicenseWindow()
        {
            var resources = new ResourceManager(typeof(AboutLicenseWindow));

            Name = "aboutLicenseForm";
            Width = 530;
            SizeToContent = SizeToContent.Height;
            MinHeight = 232;
            CanResize = false;
            Topmost = true;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            FontFamily = new FontFamily("Microsoft Sans Serif");
            FontSize = 11;
            Background = Brushes.White;

            lblCompanyName = Value();
            lblKeyInfo = Value();
            lblServerAdress = Value();

            var tableLayoutPanel1 = new Grid { ColumnDefinitions = new ColumnDefinitions("*,*"), RowDefinitions = new RowDefinitions("*,*,*"), Margin = new Thickness(16), RowSpacing = 16, ColumnSpacing = 16 };
            Add(tableLayoutPanel1, Caption(resources.GetString("label1.Text")), 0, 0);
            Add(tableLayoutPanel1, lblCompanyName, 0, 1);
            Add(tableLayoutPanel1, Caption(resources.GetString("label3.Text")), 1, 0);
            Add(tableLayoutPanel1, lblKeyInfo, 1, 1);
            Add(tableLayoutPanel1, Caption(resources.GetString("label2.Text")), 2, 0);
            Add(tableLayoutPanel1, lblServerAdress, 2, 1);
            Content = tableLayoutPanel1;
        }

        public string OwnerInfo { set { lblCompanyName.Text = value; } }

        public string AdressInfo { set { lblServerAdress.Text = value; } }

        public string KeysInfo
        {
            get { return lblKeyInfo.Text; }
            set { lblKeyInfo.Text = value; }
        }

        private static TextBlock Caption(string text) => new() { Text = text, HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Center };

        private static TextBlock Value() => new() { Text = "?", TextWrapping = TextWrapping.Wrap, VerticalAlignment = VerticalAlignment.Center };

        private static void Add(Grid grid, Control control, int row, int column)
        {
            Grid.SetRow(control, row);
            Grid.SetColumn(control, column);
            grid.Children.Add(control);
        }
    }
}
