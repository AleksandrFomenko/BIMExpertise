using Walls.ViewModels;

namespace Walls.Views;

public sealed partial class WallsView
{
    public WallsView(WallsViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}