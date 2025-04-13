using System.Windows.Controls;
using Floor.ViewModels;
using Wpf.Ui.Abstractions;
using Wpf.Ui.Abstractions.Controls;
using WpfResourcesBimExpertise.Services.Appearance;

namespace Floor.Views;

internal partial class MainParametersPage : WpfUIPlatformPage, INavigableView<MainParametersViewModel>
{
    public MainParametersPage(MainParametersViewModel viewModel)
    {
        ViewModel = viewModel;
        ThemeWatcherService.Watch(this);
        InitializeComponent();
        
    }

    public MainParametersViewModel ViewModel { get; }
}