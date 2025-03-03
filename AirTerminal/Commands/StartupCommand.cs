using AirTerminal.ViewModels;
using AirTerminal.Views;
using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;

namespace AirTerminal.Commands;

/// <summary>
///     External command entry point invoked from the Revit interface
/// </summary>
[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class StartupCommand : ExternalCommand
{
    public override void Execute()
    {
        var doc = Context.ActiveDocument;
        var view = new AirTerminalView();
        var viewModel = new AirTerminalViewModel(doc, view.Dispatcher);
        view.DataContext = viewModel;
        view.ShowDialog();
    }
}