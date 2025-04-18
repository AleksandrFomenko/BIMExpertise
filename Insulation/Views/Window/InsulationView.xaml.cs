using Insulation.ViewModels;
using Insulation.Views.Pages;
using Wpf.Ui.Abstractions;
using WpfResourcesBimExpertise.Services.Appearance;

namespace Insulation.Views.Window;

public sealed partial class InsulationView
{
    public InsulationView(
        INavigationViewPageProvider serviceProvider,
        IThemeWatcherService themeWatcherService,
        InsulationViewModel viewModel)
    {
        
        themeWatcherService.Watch(this); 
        DataContext = viewModel;
        InitializeComponent();
        Loaded += (_, __) =>
        {
            RootNavigationView.SetPageProviderService(serviceProvider);
            RootNavigationView.Navigate(typeof(GeometryParameters));
        };
    }
}