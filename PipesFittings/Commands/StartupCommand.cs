using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using PipesFittings.ViewModels;
using PipesFittings.Views;

namespace PipesFittings.Commands;

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
        var view = new PipesFittingsView();
        var viewModel = new PipesFittingsViewModel(doc, view.Dispatcher);
        view.DataContext = viewModel;
        
        view.ShowDialog();
    }
}