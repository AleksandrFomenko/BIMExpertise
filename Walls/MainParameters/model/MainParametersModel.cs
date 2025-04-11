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
        var parameterThickness =
            _doc.GetElement(element.GetTypeId())?.get_Parameter(BuiltInParameter.WALL_ATTR_WIDTH_PARAM);
        parameter.SetParameterValue((parameterThickness?.AsDouble() ?? 0) * 304.8);
    }

    public void SetAreaPlan(Element element)
    {
        var parameter = element.GetParameterByName(AreaPlanParameter);
        if (parameter == null) return;
        var parameterThickness =
            _doc.GetElement(element.GetTypeId())?.get_Parameter(BuiltInParameter.WALL_ATTR_WIDTH_PARAM);
        var thickness = parameterThickness?.AsDouble() * 304.8 ?? 0;
        var length = CalculateLength(element);
        var res = thickness * length / 1000000;
        parameter.SetParameterValue(res);
    }

    public void SetAreaSide(Element element)
    {
        var parameter = element.GetParameterByName(AreaSideParameter);
        if (parameter == null) return;
        var height = CalculateHeight(element);
        var length = CalculateLength(element);
        var res = height * length / 1000000;
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

    public void SetGeomParameters(Element element, bool len, bool width, bool height, bool areaSide, bool areaPlan)
    {
        var parameterHeight = element.GetParameterByName(HeightParameter);
        var lengthParameter = element.GetParameterByName(LengthParameter);
        var thicknessParameter =  element.GetParameterByName(ThicknessParameter);
        var areaSideParameter =  element.GetParameterByName(AreaSideParameter);
        var areaPlanParameter =  element.GetParameterByName(AreaPlanParameter);
        if (element is not Wall wall) return;
        if (wall.CurtainGrid == null)
        {
            if(len) SetLength(element);
            if(width) SetThickness(element);
            if(height) SetHeight(element);
            if (areaSide) SetAreaSide(element);
            if (areaPlan) SetAreaPlan(element);
            return;
        }
        var curtainGrid = wall.CurtainGrid;
        var panels = curtainGrid.GetPanelIds();
        var mullions = curtainGrid.GetMullionIds();
        var elements = new List<Element>();
        elements.AddRange(panels.Select(id => _doc.GetElement(id)));
        elements.AddRange(mullions.Select(id => _doc.GetElement(id)));
        var allSolids = new List<Solid>();
        Transform unrotateTransform = null;
        foreach (var elem in elements)
        {
            if (elem.Category.Id == new ElementId(BuiltInCategory.OST_Doors)) continue;
            if (elem.Category.Id == new ElementId(BuiltInCategory.OST_Windows)) continue;
            
            if (elem is not FamilyInstance familyInstance) continue;
            unrotateTransform =  GetPlanRotationTransform(familyInstance);
            
            var mainSolids = elem.GetSolids();
            if (mainSolids != null && mainSolids.Any())
                allSolids.AddRange(mainSolids);
            var subComponents = elem.GetAllSubComponents();
            foreach (var subElement in subComponents)
            {
                var subSolids = subElement.GetSolids();
                if (subSolids != null && subSolids.Any())
                    allSolids.AddRange(subSolids);
            }

        }

        Solid unionSolid = allSolids.First();
        foreach (var solid in allSolids.Skip(1))
        {
            try
            {
                unionSolid = BooleanOperationsUtils.ExecuteBooleanOperation(unionSolid, solid, BooleanOperationsType.Union);
            }
            catch (Exception)
            {
                // ignored
            }
        }
        
        var finalSolid = SolidUtils.CreateTransformed(unionSolid, unrotateTransform);

        var unionBox = finalSolid.GetBoundingBox();
        
        var lenghtRes = unionBox.Max.X - unionBox.Min.X;
        var widthRes = unionBox.Max.Y - unionBox.Min.Y;
        var heightRes = unionBox.Max.Z - unionBox.Min.Z;
        var areaSideRes = (lenghtRes * 304.8 * heightRes * 304.8)/ 1000000;
        var areaPlanRes = (lenghtRes * 304.8 * widthRes * 304.8)/ 1000000;
        if (len) lengthParameter?.SetParameterValue(lenghtRes * 304.8);
        if (height) parameterHeight?.SetParameterValue(heightRes * 304.8);
        if (width) thicknessParameter?.SetParameterValue(widthRes * 304.8);
        if (areaSide) areaSideParameter?.SetParameterValue(areaSideRes);
        if (areaSide) areaPlanParameter?.SetParameterValue(areaPlanRes);
        
    }

    public void testFunc(Element element)
    {
     
        if (element is not Wall wall) return;

        var curtainGrid = wall.CurtainGrid;
        var panels = curtainGrid.GetPanelIds();
        var mullions = curtainGrid.GetMullionIds();

        var elements = new List<Element>();
        elements.AddRange(panels.Select(id => _doc.GetElement(id)));
        elements.AddRange(mullions.Select(id => _doc.GetElement(id)));
        var allSolids = new List<Solid>();
        Transform unrotateTransform = null;
        foreach (var elem in elements)
        {
            if (elem.Category.Id == new ElementId(BuiltInCategory.OST_Doors)) continue;
            if (elem.Category.Id == new ElementId(BuiltInCategory.OST_Windows)) continue;
            
            if (elem is not FamilyInstance familyInstance) continue;
            unrotateTransform =  GetPlanRotationTransform(familyInstance);
            
            var mainSolids = elem.GetSolids();
            if (mainSolids != null && mainSolids.Any())
                allSolids.AddRange(mainSolids);
            var subComponents = elem.GetAllSubComponents();
            foreach (var subElement in subComponents)
            {
                var subSolids = subElement.GetSolids();
                if (subSolids != null && subSolids.Any())
                    allSolids.AddRange(subSolids);
            }

        }

        Solid unionSolid = allSolids.First();
        foreach (var solid in allSolids.Skip(1))
        {
            unionSolid = BooleanOperationsUtils.ExecuteBooleanOperation(unionSolid, solid, BooleanOperationsType.Union);
        }
        
        Solid finalSolid = SolidUtils.CreateTransformed(unionSolid, unrotateTransform);
        
        CreateDirectShapeFromSolid(_doc, finalSolid);
    }
    
    private void CreateDirectShapeFromSolid(Document doc, Solid solid)
    {
        var ds = DirectShape.CreateElement(doc, new ElementId(BuiltInCategory.OST_GenericModel));
        ds.Name = "Test Solid";
        ds.SetShape(new GeometryObject[] { solid });
    }
    private Transform GetFullUnrotationTransform(FamilyInstance fi)
    {
        Transform familyTf = fi.GetTransform();
        XYZ origin = familyTf.Origin;


        Transform rotationOnly = Transform.Identity;
        rotationOnly.BasisX = familyTf.BasisX;
        rotationOnly.BasisY = familyTf.BasisY;
        rotationOnly.BasisZ = familyTf.BasisZ;

        Transform rotationInv = rotationOnly.Inverse;
        
        Transform toOrigin = Transform.CreateTranslation(-origin);
        Transform fromOrigin = Transform.CreateTranslation(origin);

        Transform unrotateTransform = fromOrigin.Multiply(rotationInv).Multiply(toOrigin);
        return unrotateTransform;
    }
    
    private Transform GetPlanRotationTransform(FamilyInstance fi)
    {
        XYZ origin = fi.GetTransform().Origin;
        XYZ facing = fi.FacingOrientation;
        XYZ projFacing = new XYZ(facing.X, facing.Y, 0);
        if (projFacing.IsAlmostEqualTo(XYZ.Zero))
            return Transform.Identity;
        projFacing = projFacing.Normalize();
        double currentAngle = Math.Atan2(projFacing.Y, projFacing.X);
        double desiredAngle = -Math.PI / 2;
        double rotationAngle = desiredAngle - currentAngle;
        var rotateZ = Transform.CreateRotation(XYZ.BasisZ, rotationAngle);
        var toOrigin = Transform.CreateTranslation(-origin);
        var fromOrigin = Transform.CreateTranslation(origin);
        return fromOrigin.Multiply(rotateZ).Multiply(toOrigin);
    }

    private BoundingBoxXYZ GetUnionBoundingBox(IEnumerable<Solid> solids, Transform unrotateTransform)
    {
        var isFirstBoundingBox = true;
        XYZ unionMin = null;
        XYZ unionMax = null;

        foreach (var solid in solids)
        {
            var unrotatedSolid = SolidUtils.CreateTransformed(solid, unrotateTransform);
            var bbox = unrotatedSolid.GetBoundingBox();
            if (bbox == null) 
                continue;
                
            var worldMin = bbox.Transform.OfPoint(bbox.Min);
            var worldMax = bbox.Transform.OfPoint(bbox.Max);

            if (isFirstBoundingBox)
            {
                unionMin = worldMin;
                unionMax = worldMax;
                isFirstBoundingBox = false;
            }
            else
            {
                unionMin = new XYZ(
                    Math.Min(unionMin.X, worldMin.X),
                    Math.Min(unionMin.Y, worldMin.Y),
                    Math.Min(unionMin.Z, worldMin.Z)
                );

                unionMax = new XYZ(
                    Math.Max(unionMax.X, worldMax.X),
                    Math.Max(unionMax.Y, worldMax.Y),
                    Math.Max(unionMax.Z, worldMax.Z)
                );
            }
        }
            
        if (isFirstBoundingBox)
        {
            return null;
        }

        var unionBox = new BoundingBoxXYZ
        {
            Min = unionMin,
            Max = unionMax,
            Transform = Transform.Identity
        };

        return unionBox;
    }
    public Solid CreateSolidFromBoundingBox(BoundingBoxXYZ bbox)
    {
        var worldMin = bbox.Min;
        var worldMax = bbox.Max;

        var pts = new List<XYZ>
        {
            worldMin,
            new XYZ(worldMax.X, worldMin.Y, worldMin.Z),
            new XYZ(worldMax.X, worldMax.Y, worldMin.Z),
            new XYZ(worldMin.X, worldMax.Y, worldMin.Z)
        };

        var curves = new List<Curve>
        {
            Line.CreateBound(pts[0], pts[1]),
            Line.CreateBound(pts[1], pts[2]),
            Line.CreateBound(pts[2], pts[3]),
            Line.CreateBound(pts[3], pts[0])
        };

        var loop = CurveLoop.Create(curves);
        var profile = new List<CurveLoop> { loop };
            
        var height = worldMax.Z - worldMin.Z;

        var solid = GeometryCreationUtilities.CreateExtrusionGeometry(profile, XYZ.BasisZ, height);
        return solid;
    }
}