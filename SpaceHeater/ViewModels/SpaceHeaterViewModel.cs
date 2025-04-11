using System.Windows.Threading;
using SpaceHeater.AdditionalParameters.Vm;
using SpaceHeater.MainParameters.model;
using SpaceHeater.MainParameters.Vm;

namespace SpaceHeater.ViewModels;

public  sealed partial class SpaceHeaterViewModel : ObservableObject
{
    [ObservableProperty] private MainParametersViewModel _mainParametersVm;
    [ObservableProperty] private AdditionalParametersViewModel _additionalParametersVm;

    public SpaceHeaterViewModel(Document doc, Dispatcher dispatcher)
    {
        var mainParametersModel = new MainParametersModel();
        MainParametersVm = new MainParametersViewModel(mainParametersModel, doc, dispatcher);
        AdditionalParametersVm = new AdditionalParametersViewModel();
    }
}