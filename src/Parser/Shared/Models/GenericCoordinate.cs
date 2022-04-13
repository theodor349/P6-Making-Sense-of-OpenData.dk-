using System;
using System.Diagnostics.CodeAnalysis;
using CoordinateSharp;
using Shared.Models.ObjectAttributes;

namespace Shared.Models
{

    public struct GenericCoordinate 
    {
        public double latitude = 0;
        public double longitude = 0;

        public GenericCoordinate(double latitude, double longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }

        public GenericCoordinate(ObjectAttribute objectAttribute, CoordinateReferenceSystem crs)
        {
            var coordValues = (List<ObjectAttribute>)objectAttribute.Value;
            double coord1 = Convert.ToDouble(coordValues[0].Value);
            double coord2 = Convert.ToDouble(coordValues[1].Value);

            if(crs != null)
            {
                if (crs.IsWgs84)
                {
                    longitude = coord1;
                    latitude = coord2;
                }
                else if (crs.IsUtm)
                {
                    var latlongformat = ConvertFromUtm(new GenericCoordinate(coord1, coord2), crs.GeodeticCrs, crs.UtmZoneLetter, crs.UtmZoneNumber);
                    longitude = latlongformat.longitude;
                    latitude = latlongformat.latitude;
                }
            }
        }

        public static GenericCoordinate ConvertFromUtm(GenericCoordinate coord, string geographicFormat, string utmZoneLetter, int? utmZoneNumber)
        {

            if (utmZoneLetter != null && utmZoneNumber != null)
            {
                UniversalTransverseMercator utm = new UniversalTransverseMercator(utmZoneLetter, (int)utmZoneNumber, coord.latitude, coord.longitude);
                var latlongformat = UniversalTransverseMercator.ConvertUTMtoLatLong(utm);

                coord.longitude = latlongformat.Latitude.ToDouble();
                coord.latitude = latlongformat.Longitude.ToDouble();
                return coord;
            }
            else
            {
                throw new FormatException();
            }
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            try
            {
                var other = (GenericCoordinate)obj;
                if (other.longitude == longitude && other.latitude == latitude)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public static List<GenericCoordinate> SortAccordingToRightHandRule(List<GenericCoordinate> coords)
        {
            double sum = 0;
            for (var i = 0; i < (coords.Count - 1); i++)
            {
                sum += (coords[i + 1].latitude - coords[i].latitude) * (coords[i + 1].longitude + coords[i].longitude);
            }
            if (sum > 0)
            {
                coords.Reverse();
            }
            return coords;
        }

        public static bool IsSymetric(List<ObjectAttribute> children)
        {
            List<ObjectAttribute> childrenReverse = new List<ObjectAttribute>(children);
            childrenReverse.Reverse();
            bool symetric = true;

            for (int i = 0; i < children.Count; i++)
            {
                if (IsSameCoordinate(children[i], childrenReverse[i]) == false)
                {
                    symetric = false;
                }
            }
            return symetric;
        }
        public static void FixSymetetricPolygonStructure(List<ObjectAttribute> children, bool symetric)
        {           
            if (symetric)
            {
                int half = (children.Count - (children.Count / 2));
                children.RemoveRange(half, half - 1);
            }
        }

        public static void FixSymetetricPolygonStructure(List<GenericCoordinate> children, bool symetric)
        {
            if (symetric)
            {
                int half = (children.Count - (children.Count / 2));
                children.RemoveRange(half + 1, half);
            }
        }

        public static bool IsSameCoordinate(ObjectAttribute first, ObjectAttribute last)
        {
            if (!first.HasLabel(ObjectLabel.Point) || !last.HasLabel(ObjectLabel.Point))
                return false;

            var lat1 = GetLatitue((ListAttribute)first);
            var lat2 = GetLatitue((ListAttribute)last);
            if (lat1 != lat2)
                return false;

            var long1 = GetLongitude((ListAttribute)first);
            var long2 = GetLongitude((ListAttribute)last);
            return long1 == long2;
        }

        private static double GetLongitude(ListAttribute obj)
        {
            var attr = (DoubleAttribute)((List<ObjectAttribute>)obj.Value).Last();
            return (double)attr.Value;
        }

        private static double GetLatitue(ListAttribute obj)
        {
            var attr = (DoubleAttribute)((List<ObjectAttribute>)obj.Value).First();
            return (double)attr.Value;
        }


    }
}

