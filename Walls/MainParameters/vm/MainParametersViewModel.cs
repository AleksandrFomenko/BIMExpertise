using System.Windows.Threading;
using KapibaraCore.Parameters;
using Walls.MainParameters.model;
using Walls.Options;

namespace Walls.MainParameters.vm;

internal partial class MainParametersViewModel : ObservableObject
{
    private Document _doc;
    private MainParametersModel _model;
    private readonly Dispatcher _dispatcher;
    
    [ObservableProperty] private List<string> _geomParameters;
    [ObservableProperty] private List<SelectionOption> _selectionOptions;
    [ObservableProperty] private SelectionOption _selectionOption;
    [ObservableProperty] private List<SelectionOptionParameter> _selectionOptionParameters;
    [ObservableProperty] private SelectionOptionParameter _selectionOptionParameter;

    [ObservableProperty] private bool _isLength = true;
    [ObservableProperty] private bool _isHeight = true;
    [ObservableProperty] private bool _isVolume = true;
    [ObservableProperty] private bool _isThickness = true;
    [ObservableProperty] private bool _isAreaPlan = true;
    [ObservableProperty] private bool _isAreaSide = true;
    [ObservableProperty] private bool _isMass = true;

    [ObservableProperty] private string _lengthParameter;
    [ObservableProperty] private string _heightParameter;
    [ObservableProperty] private string _volumeParameter;
    [ObservableProperty] private string _thicknessParameter;
    [ObservableProperty] private string _areaPlanParameter;
    [ObservableProperty] private string _areaSideParameter;
    [ObservableProperty] private string _massParameter;
    
    [ObservableProperty] private double _progressMaximum;
    [ObservableProperty] private double _progressValue;
    [ObservableProperty] private bool _buttonEnable;
    partial void OnLengthParameterChanged(string value) => _model.LengthParameter = value;
    partial void OnHeightParameterChanged(string value) => _model.HeightParameter = value;
    partial void OnVolumeParameterChanged(string value) => _model.VolumeParameter = value;
    partial void OnThicknessParameterChanging(string value) => _model.ThicknessParameter = value;
    partial void OnAreaPlanParameterChanged(string value) => _model.AreaPlanParameter = value;
    partial void OnAreaSideParameterChanged(string value) => _model.AreaSideParameter = value;
    partial void OnMassParameterChanged(string value) => _model.MassParameter = value;
    
    public MainParametersViewModel(MainParametersModel model, Document doc, Dispatcher dispatcher)
    {
        _dispatcher = dispatcher;
        _doc = doc;
        _model = model;
        ButtonEnable = true;
        GeomParameters = doc.GetProjectParameters(BuiltInCategory.OST_Walls);
        SelectionOptions = new List<SelectionOption>()
        {
            new SelectionOption(doc, "Стены на активном виде", true),
            new SelectionOption(doc, "Все стены в проекте", false)
        };
        SelectionOption = SelectionOptions.FirstOrDefault() ?? new SelectionOption(doc, "Все стены в проекте", false);

        SelectionOptionParameters = new List<SelectionOptionParameter>()
        {
            new SelectionOptionParameter("Выбрать по виду", 125),
            new SelectionOptionParameter("Выбрать по параметру IfcExportAs", 210)
        };
        SelectionOptionParameter = SelectionOptionParameters.FirstOrDefault() ??
                                   new SelectionOptionParameter("Выбрать по виду", 300);
    }
    [RelayCommand]
    private void Execute()
    {
        ButtonEnable = false;
        var elements = SelectionOption.Fec.OfCategory(BuiltInCategory.OST_Walls)
            .WhereElementIsNotElementType()
            .ToList();

        _dispatcher.Invoke(() =>
        {
            ProgressMaximum = elements.Count;
        }, DispatcherPriority.Background);

        var count = 0;
        TransactionGroup tg = new TransactionGroup(_doc, "Walls");
        tg.Start();
        
        var mainTransaction = new Transaction(_doc, "Main Transaction");
        mainTransaction.Start();

        DispatcherTimer timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromTicks(1)
        };


        timer.Tick += (s, e) =>
        {
            int batchSize = 50;
            for (int i = 0; i < batchSize && count < elements.Count; i++)
            {
                if (IsLength)
                {
                    _model.SetLength(elements[count]);
                }
                if (IsHeight)
                {
                    _model.SetHeight(elements[count]);
                }
                if (IsVolume)
                {
                    _model.SetVolume(elements[count]);
                }
                if (IsThickness)
                {
                    _model.SetThickness(elements[count]);
                }
                if (IsAreaPlan)
                {
                    _model.SetAreaPlan(elements[count]);
                }
                if (IsAreaSide)
                {
                    _model.SetAreaSide(elements[count]);
                }
                if (IsMass)
                {
                    _model.SetMass(elements[count]);
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