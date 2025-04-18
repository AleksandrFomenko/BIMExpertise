using Autodesk.Revit.Attributes;
using Floor.ViewModels;
using Floor.Views;
using Microsoft.Extensions.DependencyInjection;
using Nice3point.Revit.Toolkit.External;
using Wpf.Ui;
using Wpf.Ui.Abstractions;
using WpfResourcesBimExpertise.Services.Appearance;
using WpfResourcesBimExpertise.Services.Navigation;

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
        
        services.AddSingleton<MainParametersPage>();
        services.AddSingleton<MainParametersViewModel>();
        
        services.AddSingleton<INavigationViewPageProvider, PageService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IThemeWatcherService, ThemeWatcherService>();
    
        
        var serviceProvider = services.BuildServiceProvider();
        var view = serviceProvider.GetService<FloorView>();
        view.ShowDialog();
    }
}
