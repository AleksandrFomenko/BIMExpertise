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
        var viewModel = new PipesFittingsViewModel();
        var view = new PipesFittingsView(viewModel);
        view.ShowDialog();
    }
}