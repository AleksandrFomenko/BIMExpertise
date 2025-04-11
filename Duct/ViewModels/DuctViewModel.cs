using System.Windows.Threading;
using Duct.AdditionalParameters.vm;
using Duct.MainParameters.model;
using Duct.MainParameters.vm;

namespace Duct.ViewModels;

public sealed partial class DuctViewModel : ObservableObject
{
    private Document _doc;
    private Dispatcher _dispatcher;

    [ObservableProperty] private MainParametersViewModel _mainParametersViewModel;
    [ObservableProperty] private AdditionalParametersVm _additionalParametersViewModel;
    public DuctViewModel(Document doc, Dispatcher dispatcher)
    {
        _doc = doc;
        _dispatcher = dispatcher;
        var mainParametersModel = new MainParametersModel(doc);
        MainParametersViewModel = new  MainParametersViewModel(mainParametersModel, doc, _dispatcher);
    }
}