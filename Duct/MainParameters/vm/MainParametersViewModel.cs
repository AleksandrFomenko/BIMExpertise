using System.Windows;
using System.Windows.Threading;
using Duct.MainParameters.model;
using KapibaraCore.Parameters;

namespace Duct.MainParameters.vm;

public partial class MainParametersViewModel : ObservableObject
{
    private readonly Dispatcher _dispatcher;
    private Document _doc;
    private MainParametersModel _model;
    
    
    [ObservableProperty] private List<Options> _options1;
    [ObservableProperty] private Options _option1;
    [ObservableProperty] private List<Options2> _options2;
    [ObservableProperty] private Options2 _option2;
    [ObservableProperty] private List<string> _geomParameters;
    
    [ObservableProperty] private string _heightParameter;
    [ObservableProperty] private string _widthDiameterParameter;
    [ObservableProperty] private string _lengthParameter;
    [ObservableProperty] private string _massParameter;

    [ObservableProperty] private bool _isHeight = true;
    [ObservableProperty] private bool _isWidthDiameter = true;
    [ObservableProperty] private bool _isLength = true;
    [ObservableProperty] private bool _isMass = true;
    
    [ObservableProperty] private double _progressMaximum;
    [ObservableProperty] private double _progressValue;

    [ObservableProperty] private bool _buttonEnable;
    
    partial void OnIsHeightChanged(bool value) => _model.IsHeight = value; 
    partial void OnIsWidthDiameterChanged(bool value) => _model.IsWidthDiameter = value;
    partial void OnIsLengthChanged(bool value) => _model.IsLength = value;
    partial void OnIsMassChanged(bool value) => _model.IsMass = value;

    partial void OnHeightParameterChanged(string value) => _model.HeightParameter = value;
    partial void OnWidthDiameterParameterChanged(string value) => _model.WidthDiameterParameter = value;
    partial void OnLengthParameterChanged(string value) => _model.LengthParameter = value;
    partial void OnMassParameterChanged(string value) => _model.MassParameter = value;


    internal MainParametersViewModel(MainParametersModel model, Document doc, Dispatcher dispatcher)
    {
        _dispatcher = dispatcher;
        _model = model;
        _doc = doc;
        
        ButtonEnable = true;
        Options1 = new List<Options>()
        {
            new Options("Выбрать все в проекте", new FilteredElementCollector(doc)),
            new Options("Только на активном виде", new FilteredElementCollector(doc, doc.ActiveView.Id))
        };
        Option1 = Options1.FirstOrDefault() ??
                  new Options("Выбрать все в проекте", new FilteredElementCollector(doc));

        Options2 = new List<Options2>()
        {
            new Options2("Все воздуховоды", new GridLength(1, GridUnitType.Star)),
            new Options2("По параметру", new GridLength(200, GridUnitType.Pixel))
        };
        Option2 = Options2.FirstOrDefault() ??
                  new Options2("Все воздуховоды", new GridLength(1, GridUnitType.Star));

        GeomParameters = doc.GetProjectParameters(BuiltInCategory.OST_DuctCurves);
    }
    
    [RelayCommand]
    private void Execute()
    {
        ButtonEnable = false;
        var elements = Option1.Fec.OfCategory(BuiltInCategory.OST_DuctCurves)
            .WhereElementIsNotElementType()
            .ToList();

        _dispatcher.Invoke(() =>
        {
            ProgressMaximum = elements.Count;
        }, DispatcherPriority.Background);

        var count = 0;
        TransactionGroup tg = new TransactionGroup(_doc, "Ducts Transaction Group");
        tg.Start();
        
        var mainTransaction = new Transaction(_doc, "Main Transaction");
        mainTransaction.Start();

        DispatcherTimer timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(50) 
        };

        timer.Tick += (s, e) =>
        {
            int batchSize = 20;
            for (int i = 0; i < batchSize && count < elements.Count; i++)
            {
                _model.Execute(elements[count]);
                count++;
            }

            _dispatcher.Invoke(() =>
            {
                ProgressValue = count;
            }, DispatcherPriority.Background);

            if (count >= elements.Count)
            {
                timer.Stop();
                mainTransaction.Commit();
                mainTransaction.Dispose();
                tg.Assimilate();
                _dispatcher.Invoke(() => { ButtonEnable = true; }, DispatcherPriority.Background);
            }
        };

        timer.Start();
    }
}
    
