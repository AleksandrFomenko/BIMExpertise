using System.Windows;
using Autodesk.Revit.Attributes;
using Floor.ViewModels;
using Floor.Views;
using Microsoft.Extensions.DependencyInjection;
using Nice3point.Revit.Toolkit.External;
using Wpf.Ui;
using Wpf.Ui.Abstractions;

namespace Floor.Commands;



[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class StartupCommand : ExternalCommand
{
    public override void Execute()
    {
        
        
        var services = new ServiceCollection();

        services.AddSingleton<FloorView>();
        services.AddSingleton<FloorViewModel>();
        services.AddSingleton<INavigationViewPageProvider, PageService>();
        services.AddSingleton<MainParametersPage>();
        
        var builder = Host.CreateDefaultBuilder();
        builder.Services.AddNavigationViewPageProvider();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        var doc = Context.ActiveDocument;
        var view = new FloorView();
        var viewModel = new FloorViewModel(doc, view.Dispatcher);
        view.DataContext = viewModel;
        view.ShowDialog();
    }
}
