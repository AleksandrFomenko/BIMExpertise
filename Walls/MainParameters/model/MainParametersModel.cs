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
        var result = CalculateHeight(element);
        parameter.SetParameterValue(result);
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
        var height = CalculateHeight(element);
        var length = CalculateLength(element);
        var res = height * length/1000000;
        parameter.SetParameterValue(res);
    }

    public void SetMass(Element element)
    {
        
    }

    private static double CalculateLength(Element element)
    {
        double maxLen = 0;
        var flag1 = true;
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

                if (xyz1.Z == xyz2.Z)
                {
                    flag1 = false;
                    var lenCurve = curve.Length;
                    if (lenCurve > maxLen)
                        maxLen = lenCurve;
                }
            }

            if (flag1)
            {
                foreach (Edge edge in solid.Edges)
                {
                    var curve = edge.AsCurve();
                    var lenCurve = curve.Length;
                    if (lenCurve > maxLen)
                        maxLen = lenCurve;
                }
            }
        }
        return maxLen * 304.8;
    }

    private static double CalculateHeight(Element element)
    {
        double minZ = 999999999;
        double maxZ = -999999999;
        
        var solids = element.GetSolids();
        foreach (var solid in solids)
        {
            foreach (Edge edge in solid.Edges)
            {
                var curve = edge.AsCurve();
                var tessellated = curve.Tessellate();
                foreach (var xyz in tessellated)
                {
                    if (xyz.Z > maxZ) maxZ = xyz.Z;
                    if (xyz.Z < minZ) minZ = xyz.Z;
                }
            }
        }

        return (maxZ - minZ) * 304.8;
    }
}