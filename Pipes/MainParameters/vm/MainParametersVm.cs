using System.Windows;
using System.Windows.Threading;
using KapibaraCore.Parameters;
using Pipes.MainParameters.model;

namespace Pipes.MainParameters.vm;

internal partial class MainParametersVm : ObservableObject
{
    private Document _doc;
    private MainParametersModel _model;


    [ObservableProperty] private List<Options> _options1;
    [ObservableProperty] private Options _option1;
    [ObservableProperty] private List<Options2> _options2;
    [ObservableProperty] private Options2 _option2;
    [ObservableProperty] private List<string> _geomParameters;
    
    [ObservableProperty] private bool _isDiameter = true;
    [ObservableProperty] private bool _isOuterDiameter = true;
    [ObservableProperty] private bool _isPipeThickness = true;
    [ObservableProperty] private bool _isLength = true;
    [ObservableProperty] private bool _isMass = true;
    
    [ObservableProperty] private string _diamParameter;
    [ObservableProperty] private string _outerDiameterParameter;
    [ObservableProperty] private string _pipeThicknessParameter;
    [ObservableProperty] private string _lengthParameter;
    [ObservableProperty] private string _massParameter;
    
    [ObservableProperty] private double _progressMaximum;
    [ObservableProperty] private double _progressValue;

    [ObservableProperty] private bool _buttonEnable;

    

    private readonly Dispatcher _dispatcher;

    internal MainParametersVm(MainParametersModel model, Document doc, Dispatcher dispatcher)
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
            new Options2("Все трубы", new GridLength(1, GridUnitType.Star)),
            new Options2("По параметру", new GridLength(200, GridUnitType.Pixel))
        };
        Option2 = Options2.FirstOrDefault() ??
                  new Options2("Все трубы", new GridLength(1, GridUnitType.Star));

        GeomParameters = doc.GetProjectParameters(BuiltInCategory.OST_PipeCurves);
    }
    partial void OnDiamParameterChanged(string value) => _model.DiamParameter = value;
    partial void OnOuterDiameterParameterChanged(string value) => _model.OuterDiameter = value;
    partial void OnPipeThicknessParameterChanged(string value) => _model.WidthWall = value;
    partial void OnLengthParameterChanged(string value) => _model.LengthPipe = value;
    partial void OnMassParameterChanged(string value) => _model.MassPipe = value;

    
    partial void OnIsDiameterChanged(bool value) =>  _model.IsDiameter = value;
    partial void OnIsOuterDiameterChanged(bool value) => _model.IsOuterDiameter = value;
    partial void OnIsPipeThicknessChanged(bool value) => _model.IsWidthWall = value;
    partial void OnIsLengthChanged(bool value) => _model.IsLengthPipe = value;
    partial void OnIsMassChanged(bool value) => _model.IsMassPipe = value;
    

    [RelayCommand]
    private void Execute()
    {
        ButtonEnable = false;
        var elements = Option1.Fec.OfCategory(BuiltInCategory.OST_PipeCurves)
            .WhereElementIsNotElementType()
            .ToList();

        _dispatcher.Invoke(() =>
        {
            ProgressMaximum = elements.Count;
        }, DispatcherPriority.Background);

        var count = 0;
        TransactionGroup tg = new TransactionGroup(_doc, "Pipes Transaction Group");
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
                if (_model.IsDiameter)
                {
                    _model.SetDiameter(elements[count]);
                }
                if (_model.IsOuterDiameter)
                {
                    _model.SetOuterDiameter(elements[count]);
                }
                if (_model.IsWidthWall)
                {
                    _model.SetWidthWall(elements[count]);
                }
                if (_model.IsLengthPipe)
                {
                    _model.SetLengthPipe(elements[count]);
                }
                if (_model.IsMassPipe)
                {
                    _model.SetMassPipe(elements[count]);
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
                tg.Assimilate();
                _dispatcher.Invoke(() => { ButtonEnable = true; }, DispatcherPriority.Background);
            }
        };

        timer.Start();
    }
}
/*
[RelayCommand]
private async Task ExecuteAsync()
{
     await _asyncExternalHandler.RaiseAsync(async application =>
    {
        var tasks = new List<Task>();
        var document = application.ActiveUIDocument.Document;
        using (var transaction = new Transaction(document))
        {
            transaction.Start("Pipes");

            var elements = new FilteredElementCollector(document)
                .OfCategory(BuiltInCategory.OST_PipeCurves)
                .WhereElementIsNotElementType()
                .ToList();
            
            await _dispatcher.InvokeAsync(() =>
            {
                ProgressMaximum = elements.Count;
            }, DispatcherPriority.Send);

            int count = 0;
            foreach (var element in elements)
            {
                
                if (_model.IsDiameter)
                {
                    _model.SetDiameter(element);
                }
                
                count++;
                
                if (count % 10 == 0)
                {
                    await _dispatcher.InvokeAsync(() =>
                    {
                        ProgressValue = count;
                    }, DispatcherPriority.Send);
                }
            }
            Task.WaitAll();
            
            transaction.Commit();
            TaskDialog.Show("1",transaction.GetStatus().ToString());
        }
    });
}
}
*/

