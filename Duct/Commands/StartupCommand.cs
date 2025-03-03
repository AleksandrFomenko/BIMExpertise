using System.Runtime.Remoting.Contexts;
using Autodesk.Revit.Attributes;
using Duct.ViewModels;
using Duct.Views;
using Nice3point.Revit.Toolkit.External;
using Context = Nice3point.Revit.Toolkit.Context;

namespace Duct.Commands;

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
        var view = new DuctView();
        var viewModel = new DuctViewModel(doc, view.Dispatcher);
        view.DataContext = viewModel;
        view.ShowDialog();
    }
}