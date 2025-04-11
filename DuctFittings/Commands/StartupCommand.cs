using Autodesk.Revit.Attributes;
using DuctFittings.ViewModels;
using DuctFittings.Views;
using Microsoft.Extensions.DependencyInjection;
using Nice3point.Revit.Toolkit.External;
using Wpf.Ui;
using Wpf.Ui.Controls;


namespace DuctFittings.Commands;

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

    
        var view = new DuctFittingsView();
        var viewModel = new DuctFittingsViewModel(doc, view.Dispatcher);
    
        view.DataContext = viewModel;
        view.ShowDialog();
    }
}