using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using PipeInsulation.ViewModels;
using PipeInsulation.Views;

namespace PipeInsulation.Commands;


[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class StartupCommand : ExternalCommand
{
    public override void Execute()
    {
        Document doc = Context.ActiveDocument;
        var view = new PipeInsulationView();
        var viewModel = new PipeInsulationViewModel(doc, view.Dispatcher);
        view.DataContext = viewModel;
        view.ShowDialog();
    }
}