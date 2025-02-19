using Valve.ViewModels;

namespace Valve.Views;

public sealed partial class ValveView
{
    public ValveView(ValveViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}