using SpaceHeater.ViewModels;

namespace SpaceHeater.Views;

public sealed partial class SpaceHeaterView
{
    public SpaceHeaterView(SpaceHeaterViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}