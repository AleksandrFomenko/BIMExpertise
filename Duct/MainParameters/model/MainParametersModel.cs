using KapibaraCore.Parameters;

namespace Duct.MainParameters.model;

public class MainParametersModel
{
    private Document _doc;
    
    public bool IsHeight = true;
    public bool IsWidthDiameter = true;
    public bool IsLength = true;
    public bool IsMass = true;

    public string HeightParameter;
    public string WidthDiameterParameter;
    public string LengthParameter ;
    public string MassParameter;
    
    public MainParametersModel(Document doc)
    {
        _doc = doc;
    }

    public void Execute(Element element)
    {
        if(element is not Autodesk.Revit.DB.Mechanical.Duct duct) return;
        var shape  = duct.DuctType?.Shape;
        Shape worker = shape switch
        {
            ConnectorProfileType.Rectangular => new DuctRectangular(duct),
            ConnectorProfileType.Round => new DuctRound(duct),
            _ => null
        };
        if(IsLength) worker?.SetLength(LengthParameter);
        if(IsHeight) worker?.SetHeight(HeightParameter);
        if(IsWidthDiameter) worker?.SetWidthOrDiameter(WidthDiameterParameter);
        if (IsMass) worker?.SetMass(MassParameter);
    }
}

internal abstract class Shape
{
    private readonly Autodesk.Revit.DB.Mechanical.Duct _element;

    protected Shape(Autodesk.Revit.DB.Mechanical.Duct element)
    {
        _element = element;
    }
    public void SetLength(string parameterName)
    {
        var result = _element.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH)?.AsDouble() ?? 0;
        var parameter = _element.GetParameterByName(parameterName);
        parameter?.SetParameterValue(result * 304.8);
    }

    public void SetMass(string parameterName)
    {
        const double steelDensity = 7.850;
        var area = _element.get_Parameter(BuiltInParameter.RBS_CURVE_SURFACE_AREA)?.AsDouble() ?? 0;
#if REVIT2021_OR_GREATER
        var areaСonvert =  UnitUtils.ConvertFromInternalUnits(area, UnitTypeId.SquareMeters);
#else
        var areaСonvert =  UnitUtils.ConvertFromInternalUnits(area, DisplayUnitType.DUT_SQUARE_METERS);
#endif
        
        var result = areaСonvert * 0.8 * steelDensity;
        var parameter = _element.GetParameterByName(parameterName);
        parameter?.SetParameterValue(result);

    }
    public abstract void SetWidthOrDiameter(string parameterName);
    public abstract void SetHeight(string parameterName);
    
}

internal class DuctRectangular : Shape
{
    private readonly Autodesk.Revit.DB.Mechanical.Duct _element;
    public DuctRectangular(Autodesk.Revit.DB.Mechanical.Duct element) : base(element)
    {
        _element = element;
    }

    public override void SetWidthOrDiameter(string parameterName)
    {
        if (_element.ConnectorManager == null) return;
        var connector = _element.ConnectorManager.Connectors.Cast<Connector>().FirstOrDefault();
        var result = connector?.Width ?? 0;
        var parameter = _element.GetParameterByName(parameterName);
        parameter?.SetParameterValue(result * 304.8);
        
    }
    

    public override void SetHeight(string parameterName)
    {
        if (_element.ConnectorManager == null) return;
        var connector = _element.ConnectorManager.Connectors.Cast<Connector>().FirstOrDefault();
        var result = connector?.Height ?? 0;
        var parameter = _element.GetParameterByName(parameterName);
        parameter?.SetParameterValue(result * 304.8);
    }
}

internal class DuctRound : Shape
{
    private Autodesk.Revit.DB.Mechanical.Duct _element;
    public DuctRound(Autodesk.Revit.DB.Mechanical.Duct element) : base(element)
    {
        _element = element;
    }

    public override void SetWidthOrDiameter(string parameterName)
    {
        if (_element.ConnectorManager == null) return;
        var connector = _element.ConnectorManager.Connectors.Cast<Connector>().FirstOrDefault();
        var result = connector?.Radius ?? 0;
        var parameter = _element.GetParameterByName(parameterName);
        parameter?.SetParameterValue(result * 304.8 * 2);
    }

    public override void SetHeight(string parameterName)
    {
    }
}