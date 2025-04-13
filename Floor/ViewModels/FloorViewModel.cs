using System.Collections.ObjectModel;
using System.Windows.Threading;
using Floor.Views;
using Wpf.Ui.Controls;

namespace Floor.ViewModels;

public sealed partial class FloorViewModel : ObservableObject
{
    
    [ObservableProperty]
    private ObservableCollection<object> _navigationItems = [];
    
    internal FloorViewModel()
    {
        NavigationItems = new ObservableCollection<object>()
        {
            new NavigationViewItem()
            {
                Content = "Home",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(MainParametersPage),
            }
        };
    }
}