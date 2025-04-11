using KapibaraCore.Elements;
using KapibaraCore.Parameters;
using KapibaraCore.Solids;


namespace Valve.MainParameters.Model
{
    public class MainParametersModel
    {
        public string HeightParameter { get; set; }
        public string WidthParameter { get; set; }
        public string MassParameter { get; set; }
        public string LengthParameter { get; set; }

        public void SetMass(Element element)
        {
            var parameterMass = element.GetParameterByName(MassParameter);
            if (element is not FamilyInstance familyInstance) return;
            var allSolids = new List<Solid>();
            var mainSolids = element.GetSolids();
            if (mainSolids != null && mainSolids.Any())
                allSolids.AddRange(mainSolids);
            var subComponents = element.GetAllSubComponents();
            foreach (var subElement in subComponents)
            {
                var subSolids = subElement.GetSolids();
                if (subSolids != null && subSolids.Any())
                {
                    allSolids.AddRange(subSolids);
                }
            }
            
            if (!allSolids.Any()) return;
            var volume = allSolids.Sum(v => v.Volume);
            var density = 7850;
#if REVIT2021_OR_GREATER
            var result = UnitUtils.ConvertFromInternalUnits(volume, UnitTypeId.CubicMeters);
#else
            var result = UnitUtils.ConvertFromInternalUnits(value, DisplayUnitType.DUT_CUBIC_METERS);
#endif
            parameterMass?.SetParameterValue(result * density);
            
        }
    
        public void SetGeomParameters(Element element, bool len, bool width, bool height)
        {
            var parameterHeight = element.GetParameterByName(HeightParameter);
            var widthParameter = element.GetParameterByName(WidthParameter);
            var lengthParameter = element.GetParameterByName(LengthParameter);
    
            var familyInstance = element as FamilyInstance;
            if (familyInstance == null) return;
            var allSolids = new List<Solid>();
            var mainSolids = element.GetSolids();
            if (mainSolids != null && mainSolids.Any())
                allSolids.AddRange(mainSolids);
            var subComponents = element.GetAllSubComponents();
            foreach (var subElement in subComponents)
            {
                var subSolids = subElement.GetSolids();
                if (subSolids != null && subSolids.Any())
                {
                    allSolids.AddRange(subSolids);
                }
            }
            
            if (!allSolids.Any()) return;
            
            var unrotateTransform = GetFullUnrotationTransform(familyInstance);
            /*
            foreach (var solid in solids)
            {
                var unrotatedSolid = SolidUtils.CreateTransformed(solid, unrotateTransform);
                var bbox = unrotatedSolid.GetBoundingBox();
                
                var q = CreateSolidFromBoundingBox(bbox);
                CreateDirectShapeFromSolid(element.Document, q);
            }
            */
    
            
            
            var unionBox = GetUnionBoundingBox(allSolids, unrotateTransform);
            if (unionBox == null) return;

    
            var lenghtRes = unionBox.Max.X - unionBox.Min.X;
            var widthRes = unionBox.Max.Y - unionBox.Min.Y;
            var heightRes = unionBox.Max.Z - unionBox.Min.Z;
    
            if (len) lengthParameter?.SetParameterValue(Math.Round(lenghtRes * 304.8));
            if (width) widthParameter?.SetParameterValue(Math.Round(widthRes * 304.8));
            if (height) parameterHeight?.SetParameterValue(Math.Round(heightRes * 304.8));
            //var boxSolid = CreateSolidFromBoundingBox(unionBox);
           // CreateDirectShapeFromSolid(element.Document, boxSolid);
        }
    
        private void CreateDirectShapeFromSolid(Document doc, Solid solid)
        {
            var ds = DirectShape.CreateElement(doc, new ElementId(BuiltInCategory.OST_GenericModel));
            ds.Name = "Test Solid";
            ds.SetShape(new GeometryObject[] { solid });
        }
    
        private Transform GetFullUnrotationTransform(FamilyInstance fi)
        {
            var locPoint = fi.Location as LocationPoint;
            if (locPoint == null) return null;
            var origin = locPoint.Point;
    
            var xDir = fi.FacingOrientation.Normalize();
            var yDir = fi.HandOrientation.Normalize();
            if (yDir.IsAlmostEqualTo(XYZ.Zero)) return null;
            var zDir = xDir.CrossProduct(yDir).Normalize();
    
            var instanceTransform = Transform.Identity;
            instanceTransform.Origin = origin;
            instanceTransform.BasisX = xDir;
            instanceTransform.BasisY = yDir;
            instanceTransform.BasisZ = zDir;
    
            var rotationPart = instanceTransform;
            rotationPart.Origin = XYZ.Zero;
    
            var unrotateRotation = rotationPart.Inverse;
            var correction = Transform.CreateRotation(XYZ.BasisZ, -Math.PI / 2);
            var translateToOrigin = Transform.CreateTranslation(-origin);
            var translateBack = Transform.CreateTranslation(origin);
    
            var compositeTransform = translateBack
                .Multiply(correction)
                .Multiply(unrotateRotation)
                .Multiply(translateToOrigin);
    
            var mirror = Transform.CreateReflection(Plane.CreateByNormalAndOrigin(XYZ.BasisZ, origin));
            compositeTransform = mirror.Multiply(compositeTransform);
    
            return compositeTransform;
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
            var worldMin = bbox.Transform.OfPoint(bbox.Min);
            var worldMax = bbox.Transform.OfPoint(bbox.Max);

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
}
