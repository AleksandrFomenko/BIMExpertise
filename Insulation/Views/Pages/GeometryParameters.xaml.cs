using Insulation.ViewModels;
using Wpf.Ui.Abstractions.Controls;
using WpfResourcesBimExpertise.Services.Appearance;

namespace Insulation.Views.Pages;

public partial class GeometryParameters : INavigableView<GeometryParametersViewModel>
{
    public GeometryParameters(GeometryParametersViewModel viewModel, IThemeWatcherService themeWatcherService)
    {
        themeWatcherService.Watch(this);
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();
    }

    public GeometryParametersViewModel ViewModel { get; }
}