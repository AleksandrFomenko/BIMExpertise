using System.Windows.Threading;
using Valve.MainParameters.Model;
using Valve.MainParameters.Vm;

namespace Valve.ViewModels;

public sealed partial class ValveViewModel : ObservableObject
{
    [ObservableProperty] private MainParametersVm _mainParametersViewModel;
    [ObservableProperty] private MainParametersVm _additionalParametersViewModel;
    
    public ValveViewModel (Document doc, Dispatcher dispatcher)
    {
        var mainParametersModel = new MainParametersModel();
        MainParametersViewModel = new MainParametersVm(mainParametersModel, doc, dispatcher);
        
        //AdditionalParametersVm = new AdditionalParametersViewModel();
    }
}