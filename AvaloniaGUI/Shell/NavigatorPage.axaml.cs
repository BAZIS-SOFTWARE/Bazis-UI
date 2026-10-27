using Avalonia.Controls;

namespace AvaloniaGUI.Shell;

internal partial class NavigatorPage : Grid
{
    public NavigatorPage()
    {
        InitializeComponent();
        navigatorHeader.Text = Localization.Resources.NavigatorControl_headerName_text;
        propertiesHeader.Text = Localization.Resources.PropertiesPanelControl_headerName_text;
    }
}
