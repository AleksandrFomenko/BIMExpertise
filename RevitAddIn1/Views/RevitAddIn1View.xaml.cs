
using BIMExpertiseUI.Services.Appearance;
using Wpf.Ui.Appearance;


namespace RevitAddIn1.Views;

public sealed partial class RevitAddIn1View
{
    public RevitAddIn1View()
    {
        ThemeWatcherService.Initialize();
        ThemeWatcherService.Watch(this);

        ThemeWatcherService.ApplyTheme(ApplicationTheme.Dark);
        InitializeComponent();
    }
}