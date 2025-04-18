using Autodesk.Revit.Attributes;
using Insulation.Models;
using Insulation.ViewModels;
using Insulation.Views.Pages;
using Insulation.Views.Window;
using Microsoft.Extensions.DependencyInjection;
using Nice3point.Revit.Toolkit.External;
using Wpf.Ui;
using Wpf.Ui.Abstractions;
using WpfResourcesBimExpertise.Services.Appearance;
using WpfResourcesBimExpertise.Services.Navigation;

namespace Insulation.Commands;

[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class StartupCommand : ExternalCommand
{
    public override void Execute()
    {
        var document = Context.ActiveDocument;
        var services = new ServiceCollection();

        services.AddSingleton<INavigationViewPageProvider, PageService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IThemeWatcherService, ThemeWatcherService>();

        services.AddSingleton<InsulationView>();
        services.AddSingleton<InsulationViewModel>();

        services.AddSingleton<GeometryParameters>();
        services.AddSingleton(provider =>
        {
            var model = provider.GetRequiredService<GeometryParametersModel>();
            return new GeometryParametersViewModel(model, document);
        });
        services.AddSingleton<GeometryParametersModel>();

        services.AddSingleton<TextParameters>();
        services.AddSingleton<TextParametersViewModel>();


        var serviceProvider = services.BuildServiceProvider();
        var view = serviceProvider.GetService<InsulationView>();
        view.Show();
    }
}