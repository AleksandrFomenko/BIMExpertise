using KapibaraCore.Parameters;
using KapibaraCore.Solids;

namespace BimExpertiseCore;

public static class DataTools
{
    public static void SetMass(this Element elem, Parameter parameter)
    {
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
            if (!string.IsNullOrEmpty(familyMassValue))
            {
                if (familyMassValue.Contains("."))
                {
                    var replace = familyMassValue.Replace(".", ",");
                    parameter.SetParameterValue(replace);
                    return;
                }
                parameter.SetParameterValue(familyMassValue);
                return;
            }
        }
        double totalVolume = elem.GetSolids()
            .Sum(solid => solid.Volume);
        
        parameter.SetParameterValue(totalVolume * 110);
    }
}