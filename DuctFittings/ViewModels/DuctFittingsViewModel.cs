using System.Collections.ObjectModel;
using System.Windows.Threading;
using Wpf.Ui.Controls;

namespace DuctFittings.ViewModels;

public  sealed partial class DuctFittingsViewModel : ObservableObject
{
    private Document _doc;
    private Dispatcher _dispatcher;
    
    [ObservableProperty]
    private ObservableCollection<object> _menuItems = new()
    {
        new NavigationViewItem()
        {
            Content = "Home",
            Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
            TargetPageType = typeof(Views.MainParameters)
        }            
    };
    [ObservableProperty]
    private ObservableCollection<object> _footerMenuItems = new()
    {
        new NavigationViewItem()
        {
            Content = "Settings",
            Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
            TargetPageType = typeof(Views.MainParameters)
        }
    };

    [ObservableProperty]
    private ObservableCollection<MenuItem> _trayMenuItems = new()
    {
        new MenuItem { Header = "Home", Tag = "tray_home" }
    };
    

    public DuctFittingsViewModel(Document doc, Dispatcher viewDispatcher)
    {
        _doc = doc;
        _dispatcher = viewDispatcher;
    }
}