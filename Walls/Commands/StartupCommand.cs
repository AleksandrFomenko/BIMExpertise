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
        var viewModel = new WallsViewModel();
        var view = new WallsView(viewModel);
        view.ShowDialog();
    }
}