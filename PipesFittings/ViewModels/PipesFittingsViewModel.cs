using System.Windows.Threading;
using PipesFittings.AdditionalParameters.Vm;
using PipesFittings.MainParameters.Model;
using PipesFittings.MainParameters.Vm;

namespace PipesFittings.ViewModels;

public sealed partial class PipesFittingsViewModel : ObservableObject
{
    [ObservableProperty] private MainParametersViewModel _mainParametersVm;
    [ObservableProperty] private AdditionalParametersViewModel _additionalParametersVm;
    
    public PipesFittingsViewModel (Document doc, Dispatcher dispatcher)
    {
        var mainParametersModel = new MainParametersModel();
        MainParametersVm = new MainParametersViewModel(mainParametersModel, doc, dispatcher);
        
        //AdditionalParametersVm = new AdditionalParametersViewModel();
    }
}