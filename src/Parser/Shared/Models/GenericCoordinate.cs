using System;
using System.Diagnostics.CodeAnalysis;
using CoordinateSharp;
using Shared.Models.ObjectAttributes;

namespace Shared.Models
{

    public struct GenericCoordinate 
    {
        public double Latitude = 0;
        public double Longitude = 0;

        public GenericCoordinate(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public GenericCoordinate(ObjectAttribute objectAttribute, CoordinateReferenceSystem crs)
        {
            if (crs == null)
                return;
            
            var coordValues = (List<ObjectAttribute>)objectAttribute.Value;
            double coordLong = Convert.ToDouble(coordValues[0].Value);
            double coordLati = Convert.ToDouble(coordValues[1].Value);
            //Console.WriteLine("Long: " + coordLong + ", Lati: " + coordLati);

            if (crs.CoordsAreSwappedBefore)
            {
                var temp = coordLong;
                coordLong = coordLati;
                coordLati = temp;
            }

            if (crs.IsWgs84)
            {
                Longitude = coordLong;
                Latitude = coordLati;
            }
            else if (crs.IsUtm)
            {
                var latlongformat = ConvertFromUtm(new GenericCoordinate(coordLati, coordLong), crs.GeodeticCrs, crs.UtmZoneLetter, crs.UtmZoneNumber);
                Longitude = latlongformat.Longitude;
                Latitude = latlongformat.Latitude;
            }

            if (crs.CoordsAreSwappedAfter)
            {
                var temp = Longitude;
                Longitude = Latitude;
                Latitude = temp;
            }
        }

        public static GenericCoordinate ConvertFromUtm(GenericCoordinate coord, string geographicFormat, string utmZoneLetter, int? utmZoneNumber)
        {

            if (utmZoneLetter != null && utmZoneNumber != null)
            {
                UniversalTransverseMercator utm = new UniversalTransverseMercator(utmZoneLetter, (int)utmZoneNumber, coord.Longitude, coord.Latitude);
                var latlongformat = UniversalTransverseMercator.ConvertUTMtoLatLong(utm);

                coord.Latitude = latlongformat.Latitude.ToDouble();
                coord.Longitude = latlongformat.Longitude.ToDouble();
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
                if (other.Longitude == Longitude && other.Latitude == Latitude)
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
                sum += (coords[i + 1].Latitude - coords[i].Latitude) * (coords[i + 1].Longitude + coords[i].Longitude);
            }
            if (sum < 0)
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

