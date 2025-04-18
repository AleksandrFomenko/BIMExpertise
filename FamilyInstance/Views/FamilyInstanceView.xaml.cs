using FamilyInstance.ViewModels;

namespace FamilyInstance.Views;

public sealed partial class FamilyInstanceView
{
    public FamilyInstanceView(FamilyInstanceViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}