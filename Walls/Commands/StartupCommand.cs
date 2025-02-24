using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using Walls.ViewModels;
using Walls.Views;

namespace Walls.Commands;

[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class StartupCommand : ExternalCommand
{
    public override void Execute()
    {
        var doc = Context.ActiveDocument;
        var view = new WallsView();
        var viewModel = new WallsViewModel(doc, view.Dispatcher);
        view.DataContext = viewModel;
        view.ShowDialog();
    }
}