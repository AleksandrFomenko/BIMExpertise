using System.Windows.Threading;
using Insulation.Models;
using Insulation.ViewModels.Option;
using KapibaraCore.Parameters;
using Nice3point.Revit.Toolkit.External.Handlers;

namespace Insulation.ViewModels;

public partial class GeometryParametersViewModel : ObservableObject
{
    private readonly Document _document;
    private readonly GeometryParametersModel _model;
    
    [ObservableProperty] private List<string> _projectParameters;
    [ObservableProperty] private SelectionOption _selectionOption;
    
    [ObservableProperty] private List<SelectionOption> _selectionOptions;
    [ObservableProperty] private string _thicknessParameter;
    [ObservableProperty] private string _areaParameter;
    [ObservableProperty] private string _volumeParameter;

    partial void OnThicknessParameterChanged(string value) => _model.Thickness = value;
    partial void OnAreaParameterChanged(string value) => _model.Area = value;
    partial void OnVolumeParameterChanged(string value) => _model.Volume = value;


    public GeometryParametersViewModel(GeometryParametersModel model, Document document)
    {
        _model = model;
        _document = document;
        SelectionOptions = new List<SelectionOption>
        {
            new(document, "Изоляция на активном виде", true),
            new(document, "Вся изоляция в проекте", false)
        };
        SelectionOption = SelectionOptions.FirstOrDefault()
                          ?? new SelectionOption(document, "Изоляция на активном виде", true);
        ProjectParameters = document.GetProjectParameters();
    }

    [RelayCommand]
    private async Task ExecuteAsync()
    {
        var elementMulticategoryFilter = new ElementMulticategoryFilter(new List<BuiltInCategory>
        {
            BuiltInCategory.OST_DuctLinings,
            BuiltInCategory.OST_DuctInsulations,
            BuiltInCategory.OST_PipeInsulations
        });

        var elements = SelectionOption.Fec
            .WherePasses(elementMulticategoryFilter)
            .WhereElementIsNotElementType()
            .ToList();
        
        await _model.ExecuteAsync(elements, _document);
        
    }
}

