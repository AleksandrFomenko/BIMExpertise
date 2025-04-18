using KapibaraCore.Parameters;
using Nice3point.Revit.Toolkit.External.Handlers;

namespace Insulation.Models;

public class GeometryParametersModel
{
    
    private readonly AsyncEventHandler _asyncExternalHandler = new();
    
    
    public string Thickness { get; set; }
    public string Area { get; set; }
    public string Volume { get; set; }
    
    public async Task ExecuteAsync(List<Element> elements, Document document)
    {
        await _asyncExternalHandler.RaiseAsync(_ =>
        {
            using (var t = new Transaction(document, "Insulation"))
            {
                t.Start();
                foreach (var element in elements)
                {
                    SetThickness(element);
                    SetArea(element);
                    SetVolume(element);
                }
                t.Commit();
            }
        });
    }

    private void SetThickness(Element element)
    {
        var revitParameter = element.get_Parameter(BuiltInParameter.RBS_INSULATION_THICKNESS_FOR_DUCT) ??
                             element.get_Parameter(BuiltInParameter.RBS_LINING_THICKNESS_FOR_DUCT) ??
                             element.get_Parameter(BuiltInParameter.RBS_INSULATION_THICKNESS_FOR_PIPE);

        var result = revitParameter?.AsDouble() * 304.8 ?? 0;

        var userParameter = element.GetParameterByName(Thickness);
        userParameter.SetParameterValue(Math.Round(result, 1));
    }

    private void SetArea(Element element)
    {
        var revitParameter = element.get_Parameter(BuiltInParameter.RBS_CURVE_SURFACE_AREA);
        var revitValue = revitParameter?.AsDouble() ?? 0;
#if REVIT2021_OR_GREATER
        var result = UnitUtils.ConvertFromInternalUnits(revitValue, UnitTypeId.SquareMeters);
#else
        var result = UnitUtils.ConvertFromInternalUnits(revitValue, DisplayUnitType.DUT_SQUARE_METERS);
#endif
        var userParameter = element.GetParameterByName(Area);
        userParameter.SetParameterValue(Math.Round(result, 3));
    }
    
    private void SetVolume(Element element)
    {
        var revitParameter = element.get_Parameter(BuiltInParameter.RBS_INSULATION_LINING_VOLUME);
        var revitValue = revitParameter?.AsDouble() ?? 0;
#if REVIT2021_OR_GREATER
        var result = UnitUtils.ConvertFromInternalUnits(revitValue, UnitTypeId.CubicMeters);
#else
        var result = UnitUtils.ConvertFromInternalUnits(revitValue, DisplayUnitType.DUT_CUBIC_METERS);
#endif
        var userParameter = element.GetParameterByName(Volume);
        userParameter.SetParameterValue(Math.Round(result, 3));
    }
}