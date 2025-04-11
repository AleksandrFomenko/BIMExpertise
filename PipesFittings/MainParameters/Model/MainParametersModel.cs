using System.Diagnostics;
using BimExpertiseCore;
using KapibaraCore.Elements;
using KapibaraCore.Parameters;
using KapibaraCore.Solids;

namespace PipesFittings.MainParameters.Model;

public class MainParametersModel
{
    public string DiamParameter { get; set; }
    public string HeightParameter { get; set; }
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
        var bb = elem.GetRotateBox();
        var result = bb.GetLength();
        parameter.SetParameterValue(result);
    }
    public void SetHeight(Element elem)
    {
        var parameter = elem.GetParameterByName(HeightParameter);
        if (parameter == null) return;
        var bb = elem.GetRotateBox();
        var result = bb.GetHeight();
        parameter.SetParameterValue(result);
    }
}