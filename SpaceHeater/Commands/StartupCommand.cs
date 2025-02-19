using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using SpaceHeater.ViewModels;
using SpaceHeater.Views;

namespace SpaceHeater.Commands;

/// <summary>
///     External command entry point invoked from the Revit interface
/// </summary>
[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class StartupCommand : ExternalCommand
{
    public override void Execute()
    {
        var viewModel = new SpaceHeaterViewModel();
        var view = new SpaceHeaterView(viewModel);
        view.ShowDialog();
    }
}