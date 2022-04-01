using System;
using System.Diagnostics.CodeAnalysis;
using CoordinateSharp;
using Shared.Models.ObjectAttributes;

namespace Shared.Models
{

    public struct GenericCoordinate 
    {
        public double latitude;
        public double longitude;

        public GenericCoordinate(double lattitude, double longitude)
        {
            this.latitude = lattitude;
            this.longitude = longitude;
        }

        public GenericCoordinate(ObjectAttribute objectAttribute)
        {
            var coordValues = (List<ObjectAttribute>)objectAttribute.Value;
            latitude = Convert.ToDouble(coordValues[0].Value);
            longitude = Convert.ToDouble(coordValues[1].Value);
        }

        public GenericCoordinate(ObjectAttribute objectAttribute, string geographicFormat, string utmZoneLetter, int utmZoneNumber)
        {
            var coordValues = (List<ObjectAttribute>)objectAttribute.Value;

            double coord1 = Convert.ToDouble(coordValues[0].Value);
            double coord2 = Convert.ToDouble(coordValues[1].Value);

            var latlongformat = ConvertFromFormat(new GenericCoordinate(coord1, coord2), geographicFormat, utmZoneLetter, utmZoneNumber);
            longitude = latlongformat.longitude;
            latitude = latlongformat.latitude;
     
        }

        public static GenericCoordinate ConvertFromFormat(GenericCoordinate coord, string geographicFormat, string utmZoneLetter, int utmZoneNumber)
        {

            if (geographicFormat == "utm" && utmZoneLetter != null)
            {
                UniversalTransverseMercator utm = new UniversalTransverseMercator(utmZoneLetter, utmZoneNumber, coord.latitude, coord.longitude);
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
            coords.RemoveAt(coords.Count - 1);
            int lowestCoordinateIndex = GetFirstCoordinate(coords);

            if (AreCoordsSortedRightHandRule(coords, lowestCoordinateIndex) == false)
            {
                coords.Reverse();
            }
            coords.Add(coords[0]);
            return coords;
        }

        private static bool AreCoordsSortedRightHandRule(List<GenericCoordinate> coords, int lowestCoordinateIndex)
        {
            GenericCoordinate firstCoordinate = coords[lowestCoordinateIndex];
            GenericCoordinate a;
            GenericCoordinate b;

            if (lowestCoordinateIndex != coords.Count() - 1)
                b = coords[lowestCoordinateIndex + 1];
            else
                b = coords[0];

            if (lowestCoordinateIndex != 0)
                a = coords[lowestCoordinateIndex - 1];
            else
                a = coords.LastOrDefault();

            int i = 0;
            while(a.Equals(b) && i < coords.Count)
            {
                int index = lowestCoordinateIndex + 1 + i;
                if (index >= coords.Count)
                    index -= coords.Count;
                b = coords[index];
                i++;
            }


            if (a.longitude - firstCoordinate.longitude >= 0 && b.longitude - firstCoordinate.longitude < 0)
                return true;
            if (a.longitude - firstCoordinate.longitude < 0 && b.longitude - firstCoordinate.longitude >= 0)
                return false;
            if (a.longitude - firstCoordinate.longitude == 0 && b.longitude - firstCoordinate.longitude == 0)
            {
                if (a.latitude - firstCoordinate.latitude >= 0 || b.latitude - firstCoordinate.latitude >= 0)
                {
                    if (a.latitude > b.latitude)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                if (b.latitude > a.latitude)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            // compute the cross product of vectors (center -> a) x (center -> b)
            double det = (a.longitude - firstCoordinate.longitude) * (b.latitude - firstCoordinate.latitude) - (b.longitude - firstCoordinate.longitude) * (a.latitude - firstCoordinate.latitude);
            if (det > 0)
                return true;
            if (det < 0)
                return false;

            // points a and b are on the same line from the center
            // check which point is closer to the center
            double d1 = (a.longitude - firstCoordinate.longitude) * (a.longitude - firstCoordinate.longitude) + (a.latitude - firstCoordinate.latitude) * (a.latitude - firstCoordinate.latitude);
            double d2 = (b.longitude - firstCoordinate.longitude) * (b.longitude - firstCoordinate.longitude) + (b.latitude - firstCoordinate.latitude) * (b.latitude - firstCoordinate.latitude);
            if (d1 > d2)
            {
                return true;
            }
            else
            {
                throw new Exception();
            }
        }

        private static int GetFirstCoordinate(List<GenericCoordinate> coords)
        {

            int firstCoordinateIndex = 0;
            GenericCoordinate firstCoordinate = coords[0];
            double lowestLatitude = double.PositiveInfinity;

            int currentIndex = 0;
            foreach (GenericCoordinate item in coords)
            {
                if (item.latitude < lowestLatitude || item.latitude == lowestLatitude && item.longitude < firstCoordinate.longitude)
                {
                    lowestLatitude = item.latitude;
                    firstCoordinate = item;
                    firstCoordinateIndex = currentIndex;
                }
                currentIndex++;
            }

            return firstCoordinateIndex;
        }
    }
}

