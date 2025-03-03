using KapibaraCore.Parameters;
using KapibaraCore.Solids;

namespace PipesFittings.MainParameters.Model;

public class MainParametersModel
{
    public string DiamParameter { get; set; }
    public string HeightParameter { get; set; }
    public string MassParameter { get; set; }
    public string LengthParameter { get; set; }

    
    public void SetDiameter(Element elem)
    {
        var parameter = elem.GetParameterByName(DiamParameter);
        if (parameter == null) 
            return;
        var mepModel = (elem as FamilyInstance)?.MEPModel;
        if (mepModel is null)
            return;

        var maxRadius = mepModel.ConnectorManager?.Connectors
            .Cast<Connector>()
            .Where(c => c.Shape == ConnectorProfileType.Round)
            .Select(c => c.Radius)
            .DefaultIfEmpty(0)
            .Max();
        parameter.SetParameterValue(maxRadius * 304.8 * 2);
    }

    public void SetLength(Element elem)
    {
        var parameter = elem.GetParameterByName(LengthParameter);
        if (parameter == null) return;

        double result = elem.GetSolids()
            .SelectMany(solid => solid.Edges.Cast<Edge>())
            .Select(edge => edge.ApproximateLength)
            .DefaultIfEmpty(0)
            .Max();

        parameter.SetParameterValue(result * 304.8);
    }

    public void SetMass(Element elem)
    {
        var parameter = elem.GetParameterByName(MassParameter);
        if (parameter == null) return;

        var parameterFamilyMass = elem.GetParameterByName("ADSK_Масса");
        if (parameterFamilyMass != null)
        {
            double familyMassValue = parameterFamilyMass.AsDouble();
            if (familyMassValue != 0)
            {
                parameter.SetParameterValue(familyMassValue);
                return;
            }
        }
        var parameterFamilyMassText = elem.GetParameterByName("ADSK_Масса_Текст");
        if (parameterFamilyMassText != null)
        {
            var familyMassValue = parameterFamilyMassText.AsString();
            if (familyMassValue.Contains("."))
            {
                var replace = familyMassValue.Replace(".", ",");
                parameter.SetParameterValue(replace);
                return;
            }
            parameter.SetParameterValue(familyMassValue);
            return;
        }

        double totalVolume = elem.GetSolids()
            .Sum(solid => solid.Volume);
        
        parameter.SetParameterValue(totalVolume * 110);
    }
    public void SetHeight(Element elem)
    {
        var parameter = elem.GetParameterByName(DiamParameter);
        if (parameter == null) 
            return;
        var mepModel = (elem as FamilyInstance)?.MEPModel;
        if (mepModel is null)
            return;

        var maxRadius = mepModel.ConnectorManager?.Connectors
            .Cast<Connector>()
            .Where(c => c.Shape == ConnectorProfileType.Round)
            .Select(c => c.Radius)
            .DefaultIfEmpty(0)
            .Max();
        parameter.SetParameterValue(maxRadius * 304.8 * 2 * 1.1);
    }
    
}