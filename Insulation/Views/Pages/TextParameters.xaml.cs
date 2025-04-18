using Insulation.ViewModels;
using Wpf.Ui.Abstractions.Controls;
using WpfResourcesBimExpertise.Services.Appearance;

namespace Insulation.Views.Pages;

public partial class TextParameters : INavigableView<TextParametersViewModel>
{
    public TextParameters(TextParametersViewModel viewModel, IThemeWatcherService themeWatcherService)
    {
        themeWatcherService.Watch(this);
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();
    }

    public TextParametersViewModel ViewModel { get; }
}