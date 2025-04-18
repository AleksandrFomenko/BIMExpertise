using Insulation.ViewModels;
using Insulation.Views.Pages;
using Wpf.Ui.Abstractions;
using Wpf.Ui.Appearance;
using WpfResourcesBimExpertise.Services.Appearance;

namespace Insulation.Views.Window;

public sealed partial class InsulationView
{
    public InsulationView(
        INavigationViewPageProvider serviceProvider,
        IThemeWatcherService themeWatcherService,
        InsulationViewModel viewModel)
    {
        
        InitializeComponent();
        themeWatcherService.Watch(this); 
        ThemeWatcherService.ApplyTheme(ApplicationTheme.Dark);
        DataContext = viewModel;
        Loaded += (_, __) =>
        {
            RootNavigationView.SetPageProviderService(serviceProvider);
            RootNavigationView.Navigate(typeof(GeometryParameters));
        };
    }
    
}