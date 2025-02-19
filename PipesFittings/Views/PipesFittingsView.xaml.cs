using PipesFittings.ViewModels;

namespace PipesFittings.Views;

public sealed partial class PipesFittingsView
{
    public PipesFittingsView(PipesFittingsViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}