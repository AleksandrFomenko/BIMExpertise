using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using KapibaraCore.Parameters;

namespace Pipes.MainParameters.model
{
    internal class MainParametersModel
    {
        private Document doc;
        public string DiamParameter { get; set; }
        public string OuterDiameter { get; set; }
        public string WidthWall { get; set; }
        public string LengthPipe { get; set; }
        public string MassPipe { get; set; }
        public bool IsDiameter { get; set; }
        public bool IsOuterDiameter { get; set; }
        public bool IsWidthWall { get; set; }
        public bool IsLengthPipe { get; set; }
        public bool IsMassPipe { get; set; }
        
        private const string AdskName = "ADSK_Наименование";
        
        internal MainParametersModel(Document doc)
        {
            this.doc = doc;
            IsDiameter = true;
            IsOuterDiameter = true;
            IsWidthWall = true;
            IsLengthPipe = true;
            IsMassPipe = true;
        }
        
        public string[] GetInfo(Element pipe)
        {
            var dictPipe = new Dictionary<string, string[]>
            {
                { "полиэтилен", new string[] { "ГОСТ 32415-2013", "Полиэтилен", "СТ 10 17 40 30", "Г4", "1000000" } },
                { "водогазопровод", new string[] { "ГОСТ 3262-75", "Сталь", "СТ 10 16 50", "НГ", "1000000" } },
                { "электросвар", new string[] { "ГОСТ 10704-91", "Сталь", "СТ 10 16 50", "НГ", "1200000" } },
                { "медная", new string[] { "ГОСТ Р 52318-2005", "Медь", "СТ 10 16 10", "НГ", "2000000" } },
                { "полиропилен", new string[] { "ГОСТ 26996-86", "Полипропилен", "СТ 10 17 40 10", "Г4", "1000000" } },
                { "чугун", new string[] { "ГОСТ 6942-98", "Чугун", "СТ 10 16 70", "НГ", "800000" } }
            };
            var typeElement = doc.GetElement(pipe.GetTypeId());
            var parameter = pipe.GetParameterByName(DiamParameter)?.AsValueString()
                            ?? typeElement?.Name
                            ?? (typeElement as PipeType)?.FamilyName;
            
            if (!string.IsNullOrEmpty(parameter))
            {
                var parameterLower = parameter.ToLower();
                foreach (var kvp in dictPipe)
                {
                    if (parameterLower.Contains(kvp.Key))
                    {
                        return kvp.Value;
                    }
                }
            }
            return null;
        }

        private double GetMassType(Element pipe)
        {
            if (pipe is not Pipe)
                
                return 0;
            
            var diameterParameter = pipe.get_Parameter(BuiltInParameter.RBS_PIPE_DIAMETER_PARAM);
            var diameter = diameterParameter?.AsDouble() * 304.8;
            if (diameter == null)
                return 0;

            var dictPipe = new Dictionary<string, Dictionary<double, double>>
            {
                { "водогазопровод", new Dictionary<double, double>
                    { 
                        { 10, 0.98 },
                        { 15, 1.16 },
                        { 20, 1.66 },
                        { 25, 2.39 },
                        { 32, 3.09 },
                        { 40, 3.84 },
                        { 50, 4.88 },
                        { 65, 7.05 },
                        { 80, 8.34 },
                        { 100, 12.15 },
                        { 125, 15.04 },
                        { 150, 17.81 }
                    }
                }, { "электросварн", new Dictionary<double, double>
                    {
                        { 50, 4.61 },
                        { 65, 6.25 },
                        { 80, 7.38 },
                        { 100, 10.26 },
                        { 125, 11.178 },
                        { 150, 17.146 },
                        { 200, 31.517 },
                        { 250, 39.508 },
                        { 300, 54.897 }
                    }
                },
                { "полиропилен", new Dictionary<double, double>
                    {
                        { 16, 0.09 },
                        { 20, 0.150 },
                        { 25, 0.230 },
                        { 32, 0.38 },
                        { 40, 0.59 },
                    }
                }
            };

            var typeElement = doc.GetElement(pipe.GetTypeId());
            var parameter = pipe.GetParameterByName(DiamParameter)?.AsValueString()
                            ?? typeElement?.Name
                            ?? (typeElement as PipeType)?.FamilyName;

            if (!string.IsNullOrEmpty(parameter))
            {
               
                var parameterLower = parameter.ToLower();
                foreach (var kvp in dictPipe)
                {
                    if (parameterLower.Contains(kvp.Key))
                    {
                        if (kvp.Value.TryGetValue(diameter.Value, out double mass))
                        {
                            
                            return mass;
                        } 
                        var closestKey = kvp.Value.Keys.OrderBy(k => Math.Abs(k - diameter.Value)).First();
                        return kvp.Value[closestKey];
                    }
                }
            }
            return 0;
        }

        private double GetMass(Element elem)
        {
            if (elem is not Pipe) return 0;
            var lengthRevitParameter = elem.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH)?.AsDouble() * 304.8;
            var qwe = GetMassType(elem);
            return (double)(qwe * lengthRevitParameter)/1000;

        }

        public void SetDiameter(Element elem)
        {
            var parameter = elem.GetParameterByName(DiamParameter);
            if (parameter == null) 
                return;
            var revitParameter = elem.get_Parameter(BuiltInParameter.RBS_PIPE_DIAMETER_PARAM);
            var result = revitParameter?.AsDouble() * 304.8;
            parameter.SetParameterValue(result);
        }
        public void SetOuterDiameter(Element elem)
        {
            var parameter = elem.GetParameterByName(OuterDiameter);
            if (parameter == null) 
                return;
            var revitParameter = elem.get_Parameter(BuiltInParameter.RBS_PIPE_OUTER_DIAMETER);
            var result = revitParameter?.AsDouble() * 304.8;
            parameter.SetParameterValue(result);
        }

        public void SetWidthWall(Element elem)
        {
            var parameter = elem.GetParameterByName(WidthWall);
            if (parameter == null) 
                return;
            var outerDiameterParameter = elem.get_Parameter(BuiltInParameter.RBS_PIPE_OUTER_DIAMETER);
            var innerDiameterParameter = elem.get_Parameter( BuiltInParameter.RBS_PIPE_INNER_DIAM_PARAM);
            var result = (outerDiameterParameter?.AsDouble() - innerDiameterParameter?.AsDouble()) / 2; 
            parameter.SetParameterValue(result * 304.8);
        }

        public void SetLengthPipe(Element elem)
        {
            var parameter = elem.GetParameterByName(LengthPipe);
            if (parameter == null) return;
            var lengthRevitParameter = elem.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
            parameter.SetParameterValue(lengthRevitParameter?.AsDouble() * 304.8);
        }
        
        public void SetMassPipe(Element elem)
        {
            var parameter = elem.GetParameterByName(MassPipe);
            if (parameter == null) return;
            var result = GetMass(elem);
            parameter.SetParameterValue(result);
        }
    }
}