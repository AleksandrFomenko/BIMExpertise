using Autodesk.Revit.Attributes;
using Floor.ViewModels;
using Floor.Views;
using Microsoft.Extensions.DependencyInjection;
using Nice3point.Revit.Toolkit.External;
using Wpf.Ui;
using Wpf.Ui.Abstractions;
using Wpf.Ui.Appearance;
using WpfResourcesBimExpertise.Services.Appearance;

namespace Floor.Commands;

[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class StartupCommand : ExternalCommand
{
    public override void Execute()
    {

        
        ThemeWatcherService.Initialize();
        ThemeWatcherService.ApplyTheme(ApplicationTheme.Dark);
        
        var services = new ServiceCollection();
        
        services.AddSingleton<FloorView>();
        services.AddSingleton<FloorViewModel>();
        services.AddSingleton<INavigationViewPageProvider, PageService>();
        
        services.AddTransient<MainParametersPage>();
        
        services.AddSingleton<MainParametersViewModel>();
        services.AddSingleton<INavigationService, NavigationService>();
        
        var serviceProvider = services.BuildServiceProvider();
        var view = serviceProvider.GetService<FloorView>();
        ThemeWatcherService.Watch(view);
        view.ShowDialog();
    }
}