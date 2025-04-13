using Wpf.Ui;
using Wpf.Ui.Abstractions;
using Wpf.Ui.Appearance;
using WpfResourcesBimExpertise.Services.Appearance;

namespace Floor.Views;

public sealed partial class FloorView
{
    public FloorView(
        INavigationViewPageProvider serviceProvider,
        INavigationService navigationService)
    { 
        
        InitializeComponent();
       // navigationService.SetNavigationControl(NavigationControl);
       // NavigationControl.SetPageProviderService(serviceProvider);

    }
}