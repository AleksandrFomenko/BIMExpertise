using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using FamilyInstance.ViewModels;
using FamilyInstance.Views;

namespace FamilyInstance.Commands;

/// <summary>
///     External command entry point invoked from the Revit interface
/// </summary>
[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class StartupCommand : ExternalCommand
{
    public override void Execute()
    {
        var viewModel = new FamilyInstanceViewModel();
        var view = new FamilyInstanceView(viewModel);
        view.ShowDialog();
    }
}