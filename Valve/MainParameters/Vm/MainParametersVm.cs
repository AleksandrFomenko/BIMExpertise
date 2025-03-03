using System.Windows;
using System.Windows.Threading;
using Autodesk.Revit.UI;
using KapibaraCore.Parameters;
using Valve.MainParameters.Model;

namespace Valve.MainParameters.Vm;

public partial class MainParametersVm : ObservableObject
{
    private Document _doc;
    private Dispatcher _dispatcher;
    private readonly MainParametersModel _model;

    [ObservableProperty] private List<Options> _options1;
    [ObservableProperty] private Options _option1;
    [ObservableProperty] private List<Options2> _options2;
    [ObservableProperty] private Options2 _option2;
    [ObservableProperty] private List<string> _geomParameters;
    [ObservableProperty] private string _widthParameter;
    [ObservableProperty] private string _heightParameter;
    [ObservableProperty] private string _lengthParameter;
    [ObservableProperty] private string _massParameter;
    
    [ObservableProperty] private bool _isWidth = true;
    [ObservableProperty] private bool _isHeight = true;
    [ObservableProperty] private bool _isLength = true;
    [ObservableProperty] private bool _isMass = true;
    
    [ObservableProperty] private double _progressMaximum;
    [ObservableProperty] private double _progressValue;

    [ObservableProperty] private bool _buttonEnable;

    partial void OnWidthParameterChanged(string value) => _model.WidthParameter = value;
    partial void OnHeightParameterChanged(string value) => _model.HeightParameter= value;
    partial void OnLengthParameterChanged(string value) => _model.LengthParameter = value;
    partial void OnMassParameterChanged(string value) => _model.MassParameter = value;
    
    public MainParametersVm(MainParametersModel model, Document doc, Dispatcher dispatcher)
    {
        _model = model;
        _doc = doc;
        _dispatcher = dispatcher;
        GeomParameters = doc.GetProjectParameters(BuiltInCategory.OST_PipeAccessory);
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
            new Options2("Вся арматура", new GridLength(1, GridUnitType.Star)),
            new Options2("По параметру", new GridLength(200, GridUnitType.Pixel))
        };
        Option2 = Options2.FirstOrDefault() ??
                  new Options2("Вся арматура", new GridLength(1, GridUnitType.Star));
       
    }
    [RelayCommand]
    private void Execute()
    {
        ButtonEnable = false;
        var elements = Option1.Fec.OfCategory(BuiltInCategory.OST_PipeAccessory)
            .WhereElementIsNotElementType()
            .ToList();

        _dispatcher.Invoke(() =>
        {
            ProgressMaximum = elements.Count;
        }, DispatcherPriority.Background);

        var count = 0;
        
        var mainTransaction = new Transaction(_doc, "Valve Transaction");
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
                try
                {
                    _model.SetGeomParameters(elements[count], IsLength, IsWidth, IsHeight);
                    if(IsMass) _model.SetMass(elements[count]);
                }
                catch (Exception exception)
                {
                    TaskDialog.Show("", exception.ToString());
                }
               
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
                _dispatcher.Invoke(() => { ButtonEnable = true; }, DispatcherPriority.Background);
            }
        };

        timer.Start();
    }
}