using Floor.ViewModels;
using Wpf.Ui.Abstractions.Controls;
using WpfResourcesBimExpertise.Services.Appearance;

namespace Floor.Views;

internal partial class MainParametersPage : INavigableView<MainParametersViewModel>
{
    public MainParametersPage(MainParametersViewModel viewModel, IThemeWatcherService themeWatcherService)
    {
        themeWatcherService.Watch(this);
        ViewModel = viewModel; 
        DataContext = this;
        InitializeComponent(); 
    }
    public MainParametersViewModel ViewModel { get; }
}