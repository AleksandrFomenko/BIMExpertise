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
        var doc = Context.ActiveDocument;
        var view = new SpaceHeaterView();
        var viewModel = new SpaceHeaterViewModel(doc,view.Dispatcher);
        view.DataContext = viewModel;
        view.ShowDialog();
    }
}
