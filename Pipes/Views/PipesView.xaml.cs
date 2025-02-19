using Pipes.ViewModels;

namespace Pipes.Views;

internal sealed partial class PipesView
{
    public PipesView()
    {
        DataContext = new PipesViewModel(Context.ActiveDocument,this.Dispatcher);
        InitializeComponent();
    }
}