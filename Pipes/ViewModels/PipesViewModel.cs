using System.Windows.Threading;
using Pipes.AdditionalParameters.vm;
using Pipes.MainParameters.model;
using Pipes.MainParameters.vm;

namespace Pipes.ViewModels
{
    internal sealed partial class PipesViewModel : ObservableObject
    {
        private Document _doc;
        private Dispatcher _dispatcher;

        [ObservableProperty] private MainParametersVm _mainParametersViewModel;
        [ObservableProperty] private AdditionalParametersVm _additionalParametersViewModel;

        internal PipesViewModel(Document doc, Dispatcher dispatcher)
        {
            _doc = doc;
            _dispatcher = dispatcher;
            var mainParametersModel = new MainParametersModel(doc);
            MainParametersViewModel = new MainParametersVm(mainParametersModel, doc, _dispatcher);
        }
    }
}