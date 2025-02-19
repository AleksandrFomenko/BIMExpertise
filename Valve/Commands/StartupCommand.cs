using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using Valve.ViewModels;
using Valve.Views;

namespace Valve.Commands;

/// <summary>
///     External command entry point invoked from the Revit interface
/// </summary>
[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class StartupCommand : ExternalCommand
{
    public override void Execute()
    {
        var viewModel = new ValveViewModel();
        var view = new ValveView(viewModel);
        view.ShowDialog();
    }
}