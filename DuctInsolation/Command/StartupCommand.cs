using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using DuctInsolation.ViewModels;
using DuctInsolation.Views;



namespace DuctInsolation.Command;

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
        var view = new DuctInsolationView();
        var viewModel = new DuctInsolationViewModel(doc, view.Dispatcher);
        view.DataContext = viewModel;
        view.ShowDialog();
    }
}