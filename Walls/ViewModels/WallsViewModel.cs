using System.Windows.Threading;
using Walls.AdditionalParameters.vm;
using Walls.MainParameters.model;
using Walls.MainParameters.vm;

namespace Walls.ViewModels;

internal sealed partial class WallsViewModel : ObservableObject
{
    private Document _doc;
    private Dispatcher _dispatcher;

    [ObservableProperty] private MainParametersViewModel _mainParametersViewModel;
    [ObservableProperty] private AdditionalParametersViewModel _additionalParametersViewModel;

    internal WallsViewModel(Document doc, Dispatcher dispatcher)
    {
        _doc = doc;
        _dispatcher = dispatcher;
        var mainParametersModel = new MainParametersModel(doc);
        MainParametersViewModel = new MainParametersViewModel(mainParametersModel, doc, _dispatcher);
            
    }
}