using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace KapibaraCore.Solids
{
    public static class Solids
    {
        public static IEnumerable<Solid> GetSolids(this Element element)
        {
            if (element == null) yield break;

            var options = new Options
            {
                DetailLevel = ViewDetailLevel.Fine
            };
            var geometryElement = element.get_Geometry(options);
            if (geometryElement == null)
                yield break;

            foreach (var solid in ExtractSolidsFromGeometry(geometryElement))
            {
                yield return solid;
            }
        }

        private static IEnumerable<Solid> ExtractSolidsFromGeometry(GeometryElement geometryElement)
        {
            foreach (var geomObj in geometryElement)
            {
                foreach (var solid in ProcessGeometryObject(geomObj))
                {
                    yield return solid;
                }
            }
        }

        private static IEnumerable<Solid> ProcessGeometryObject(GeometryObject geomObj)
        {
            if (geomObj is Solid solid && solid.Volume > 0)
            {
                yield return solid;
            }

            if (geomObj is GeometryInstance geomInstance)
            {
                var instanceGeometry = geomInstance.GetInstanceGeometry();
                if (instanceGeometry != null)
                {
                    foreach (var instSolid in ExtractSolidsFromGeometry(instanceGeometry))
                    {
                        yield return instSolid;
                    }
                }
            }
        }
    }
}