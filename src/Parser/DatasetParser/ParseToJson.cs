using System;
using System.Text.Json;
using Shared.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft;
using System.Linq;
using Newtonsoft.Json.Linq;
using Shared.Models.ObjectAttributes;
using CoordinateSharp;

namespace DatasetParser
{
    public interface IParseToJson
    {
        void ParseIntermediateToJson(DatasetObject datasetObject, int iteration);
    }

    public struct Coordinate
    {
        public double lattitude;
        public double longitude;
    }


    public class ParseToJson : IParseToJson
    {
        private readonly IConfiguration _configuration;

        private Coordinate firstCoordinate;

        private string geographicFormat = null;
        private string utmZone = null;

        public ParseToJson(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ParseIntermediateToJson(DatasetObject dataset, int iteration)
        {
            // parse data
            //var json = JsonSerializer.Serialize(dataset);
            foreach (var prop in dataset.Properties)
            {
                if (prop.name == "geographicFormat")
                {
                    geographicFormat = prop.value;
                    
                }
                else if (prop.name == "utmZone")
                {
                    utmZone = prop.value;
                }
            }
            
            var GeoJson = new JObject(
                new JProperty("type", "FeatureCollection"),
                new JProperty("features", new JArray(
                    FindPolygonsInDataset(dataset)
                    ))
                );
            string output = _configuration["Output:JsonText"] + iteration.ToString() + ".geojson";
            File.WriteAllText(output, GeoJson.ToString());
        }

        private List<JObject> FindPolygonsInDataset(DatasetObject dataset)
        {
            List<JObject> polygons = new List<JObject>();
            foreach(IntermediateObject obj in dataset.Objects)
            {
                foreach (ObjectAttribute objAttr in obj.Attributes)
                {
                    List<JObject> newPolygons = CheckObjAttrForPolygons(objAttr);
                    if(newPolygons != null)
                    {
                        polygons.AddRange(newPolygons);
                    }
                }
            }
                
            return polygons;  
        }
        
        private List<JObject> CheckObjAttrForPolygons(ObjectAttribute objAttr)
        {
            
            List<JObject> polygons = new List<JObject>();

            if (objAttr.Labels.Contains(new LabelModel(ObjectLabel.Polygon)))
            {
                polygons.Add(GetPolygon(objAttr));
            }

            else if (objAttr.Labels.Contains(new LabelModel(ObjectLabel.List)))
            {
                List<JObject> newPolygons = CheckObjAttrForPolygons((List<ObjectAttribute>)objAttr.Value);
                if (newPolygons != null)
                {
                    polygons.AddRange(newPolygons);
                }
            }


            return polygons;
        }

        private List<JObject> CheckObjAttrForPolygons(List<ObjectAttribute> objAttr)
        {
            List<JObject> polygons = new List<JObject>();
            foreach (ObjectAttribute obj in objAttr)
            {

                if (obj.Labels.Contains(new LabelModel(ObjectLabel.Polygon)))
                {
                    polygons.Add(GetPolygon(obj));
                }

                else if (obj.Labels.Contains(new LabelModel(ObjectLabel.List)))
                {
                    List<JObject> newPolygons = CheckObjAttrForPolygons((List<ObjectAttribute>)obj.Value);
                    if (newPolygons != null)
                    {
                        polygons.AddRange(newPolygons);
                    }
                }

            }
            return polygons;
        }


        private JObject GetPolygon(ObjectAttribute objAttr)
        {

            return new JObject(
                       new JProperty("type", "Feature"),
                       new JProperty("geometry", new JObject(
                                new JProperty("type", "Polygon"),
                                new JProperty("coordinates", GetCoordinates(objAttr)))
                           ),
                       new JProperty("properties", new JObject())
                   );
        }

        private JArray GetCoordinates(ObjectAttribute objAttr)
        {
            JArray jArray = new JArray();
            JArray coordinates = new JArray();
            List<Coordinate> coords = new List<Coordinate>();

            foreach (var coord in (List<ObjectAttribute>) objAttr.Value)
            {
                coords.Add(GetCoordinate(coord));
            }
            //  under construction
            coords = SortAccordingToRightHandRule(coords);

            foreach(Coordinate coord in coords)
            {
                coordinates.Add(new JArray(coord.lattitude, coord.longitude));
            }

            jArray.Add(coordinates);
            return jArray;
        }

        private Coordinate GetCoordinate(ObjectAttribute coord)
        {
            var coordValues = (List<ObjectAttribute>)coord.Value;
            double coord1 = Convert.ToDouble(coordValues[0].Value);
            double coord2 = Convert.ToDouble(coordValues[1].Value);

            if (geographicFormat == "utm" && utmZone != null)
            {
                UniversalTransverseMercator utm = new UniversalTransverseMercator("N", 32, coord1, coord2);
                var latlongformat = UniversalTransverseMercator.ConvertUTMtoLatLong(utm);

                coord2 = latlongformat.Latitude.ToDouble();
                coord1 = latlongformat.Longitude.ToDouble();
            }

            return new Coordinate{ lattitude = coord1, longitude = coord2};
        }

        private List<Coordinate> SortAccordingToRightHandRule(List<Coordinate> coords)
        {
            // A linear ring MUST follow the right-hand rule with respect to the area it bounds, i.e.,
            // exterior rings are counterclockwise, and holes are clockwise.

            // all elements must be ubique for Sorting scan, therefore remove the duplicate required by geoJson
            coords.RemoveAt(coords.Count -1);

            // lattitude = y
            int firstCoordinateIndex = 0;
            firstCoordinate = coords[0];
            double lowestLattitude = double.PositiveInfinity;

            int currentIndex = 0;
            foreach (Coordinate item in coords)
            {
                if(item.lattitude < lowestLattitude || item.lattitude == lowestLattitude && item.longitude < firstCoordinate.longitude)
                {
                    lowestLattitude = item.lattitude;
                    firstCoordinate = item;
                    firstCoordinateIndex = currentIndex;
                }
                currentIndex++;
            }

            // remove first coord from list and reinsert at index 0
            coords.RemoveAt(firstCoordinateIndex);
            coords.Sort((coord1, coord2) =>
           {
               return Less(coord1, coord2);
           });

            // innsert first and last duplicate point
            coords.Insert(0, firstCoordinate);
            coords.Add(firstCoordinate);
            return coords;
        }

        private int Less(Coordinate a, Coordinate b)
        {
            if (a.longitude - firstCoordinate.longitude >= 0 && b.longitude - firstCoordinate.longitude < 0)
                return 1;
            if (a.longitude - firstCoordinate.longitude < 0 && b.longitude - firstCoordinate.longitude >= 0)
                return -1;
            if (a.longitude - firstCoordinate.longitude == 0 && b.longitude - firstCoordinate.longitude == 0)
            {
                if (a.lattitude - firstCoordinate.lattitude >= 0 || b.lattitude - firstCoordinate.lattitude >= 0)
                {
                    if (a.lattitude > b.lattitude)
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }
                if (b.lattitude > a.lattitude)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }

            // compute the cross product of vectors (center -> a) x (center -> b)
            double det = (a.longitude - firstCoordinate.longitude) * (b.lattitude - firstCoordinate.lattitude) - (b.longitude - firstCoordinate.longitude) * (a.lattitude - firstCoordinate.lattitude);
            if (det > 0)
                return 1;
            if (det < 0)
                return -1;

            // points a and b are on the same line from the center
            // check which point is closer to the center
            double d1 = (a.longitude - firstCoordinate.longitude) * (a.longitude - firstCoordinate.longitude) + (a.lattitude - firstCoordinate.lattitude) * (a.lattitude - firstCoordinate.lattitude);
            double d2 = (b.longitude - firstCoordinate.longitude) * (b.longitude - firstCoordinate.longitude) + (b.lattitude - firstCoordinate.lattitude) * (b.lattitude - firstCoordinate.lattitude);
            if (d1 > d2)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}

