using System.Collections.ObjectModel;
using System.Windows.Threading;
using Floor.Views;
using Wpf.Ui.Controls;

namespace Floor.ViewModels;

public sealed partial class FloorViewModel : ObservableObject
{
    private Document _doc;
    private Dispatcher _dispatcher;
    
    [ObservableProperty]
    private ObservableCollection<object> _navigationItems = [];

    //[ObservableProperty] private MainParametersViewModel _mainParametersViewModel;
   // [ObservableProperty] private AdditionalParametersViewModel _additionalParametersViewModel;
    internal FloorViewModel(Document doc, Dispatcher dispatcher)
    {
        _doc = doc;
        _dispatcher = dispatcher;
        //var mainParametersModel = new MainParametersModel(doc);
        //MainParametersViewModel = new MainParametersViewModel(mainParametersModel, doc, _dispatcher);
        
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