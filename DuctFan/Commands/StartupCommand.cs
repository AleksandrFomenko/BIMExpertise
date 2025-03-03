using Autodesk.Revit.Attributes;
using DuctFan.ViewModels;
using DuctFan.Views;
using Nice3point.Revit.Toolkit.External;


namespace DuctFan.Commands;

/// <summary>
///     External command entry point invoked from the Revit interface
/// </summary>
[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class StartupCommand : ExternalCommand
{
    public override void Execute()
    {
        Document doc = Context.ActiveDocument;
        var view = new DuctFanView();
        var viewModel = new DuctFanViewModel(doc, view.Dispatcher);
        view.DataContext = viewModel;
        view.ShowDialog();
    }
}