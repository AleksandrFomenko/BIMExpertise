using Wpf.Ui.Abstractions;
using Wpf.Ui.Appearance;
using WpfResourcesBimExpertise.Services.Appearance;

namespace Floor.Views;

public sealed partial class FloorView
{
    public FloorView (INavigationViewPageProvider serviceProvider, IThemeWatcherService themeWatcherService)
    { 
        themeWatcherService.Watch(this); 
        InitializeComponent();
        
        RootNavigationView.SetPageProviderService(serviceProvider);
    }
}
