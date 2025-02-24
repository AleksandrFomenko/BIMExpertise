using KapibaraCore.Elements;
using KapibaraCore.Parameters;
using KapibaraCore.Solids;

namespace Walls.MainParameters.model;

internal class MainParametersModel
{
    private Document _doc;
    
    public string LengthParameter { get; set; }
    public string HeightParameter { get; set; }
    public string VolumeParameter { get; set; }
    public string ThicknessParameter { get; set; }
    public string AreaPlanParameter { get; set; }
    public string AreaSideParameter { get; set; }
    public string MassParameter { get; set; }

    public MainParametersModel(Document doc)
    {
        _doc = doc;
    }
    public void SetLength(Element element)
    {
        var parameter = element.GetParameterByName(LengthParameter);
        if (parameter == null) return;
        var res = CalculateLength(element);
        
    
        parameter.SetParameterValue(res);
    }
    
    public void SetHeight(Element element)
    {
        var parameter = element.GetParameterByName(HeightParameter);
        if (parameter == null) return;
        var parameterLength = element.get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM);
        parameter.SetParameterValue((parameterLength?.AsDouble() ?? 0) * 304.8);
    }

    public void SetVolume(Element element)
    {
        var parameter = element.GetParameterByName(VolumeParameter);
        var solids = element.GetSolids();
        var sum = solids.Sum(solid => solid.Volume);
#if REVIT2021_OR_GREATER
        var res = UnitUtils.ConvertFromInternalUnits(sum, UnitTypeId.CubicMeters);
#else
        var res = UnitUtils.ConvertFromInternalUnits(value, DisplayUnitType.DUT_CUBIC_METERS);
#endif
        parameter.SetParameterValue(res);
    }

    public void SetThickness(Element element)
    {
        var parameter = element.GetParameterByName(ThicknessParameter);
        if (parameter == null) return;
        var parameterThickness = _doc.GetElement(element.GetTypeId())?.get_Parameter(BuiltInParameter.WALL_ATTR_WIDTH_PARAM);
        parameter.SetParameterValue((parameterThickness?.AsDouble() ?? 0) * 304.8);
    }

    public void SetAreaPlan(Element element)
    {
        var parameter = element.GetParameterByName(AreaPlanParameter);
        if (parameter == null) return;
        var parameterThickness = _doc.GetElement(element.GetTypeId())?.get_Parameter(BuiltInParameter.WALL_ATTR_WIDTH_PARAM);
        var thickness = parameterThickness?.AsDouble() * 304.8 ?? 0;
        var length = CalculateLength(element);
        var res = thickness * length/1000000;
        parameter.SetParameterValue(res);
    }

    public void SetAreaSide(Element element)
    {
        var parameter = element.GetParameterByName(AreaSideParameter);
        if (parameter == null) return;
        var height = element.get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM)?.AsDouble() * 304.8 ?? 0;
        var length = CalculateLength(element);
        var res = height * length/1000000;
        parameter.SetParameterValue(res);
    }

    public void SetMass(Element element)
    {
        
    }

    private double CalculateLength(Element element)
    {
        double maxLen = 0;
        var solids = element.GetSolids();
    
        foreach (var solid in solids)
        {
            foreach (Edge edge in solid.Edges)
            {
                var curve = edge.AsCurve();
                var tessellated = curve.Tessellate();
                if (tessellated.Count < 2)
                    continue;
            
                var xyz1 = tessellated[0];
                var xyz2 = tessellated[1];
                if (xyz1 == null || xyz2 == null)
                    continue;
                
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (xyz1.Z == xyz2.Z)
                {
                    double lenCurve = curve.Length;
                    if (lenCurve > maxLen)
                        maxLen = lenCurve;
                }
            }
        }

        return maxLen * 304.8;
    }
}