using System;
using CoordinateSharp;
using Shared.Models.ObjectAttributes;

namespace Shared.Models
{

    public struct GenericCoordinate
    {
        public double lattitude;
        public double longitude;

        public GenericCoordinate(double lattitude, double longitude)
        {
            this.lattitude = lattitude;
            this.longitude = longitude;
        }

        public GenericCoordinate(ObjectAttribute objectAttribute)
        {
            var coordValues = (List<ObjectAttribute>)objectAttribute.Value;
            lattitude = Convert.ToDouble(coordValues[0].Value);
            longitude = Convert.ToDouble(coordValues[1].Value);
        }

        public GenericCoordinate(ObjectAttribute objectAttribute, string geographicFormat, string utmZoneLetter, int utmZoneNumber)
        {
            var coordValues = (List<ObjectAttribute>)objectAttribute.Value;

            double coord1 = Convert.ToDouble(coordValues[0].Value);
            double coord2 = Convert.ToDouble(coordValues[1].Value);

            var latlongformat = ConvertFromFormat(new GenericCoordinate(coord1, coord2), geographicFormat, utmZoneLetter, utmZoneNumber);

            if (latlongformat.Item2)
            {
                longitude = latlongformat.Item1.longitude;
                lattitude = latlongformat.Item1.lattitude;
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        public static Tuple<GenericCoordinate, bool> ConvertFromFormat (GenericCoordinate coord, string geographicFormat, string utmZoneLetter, int utmZoneNumber)
        {

            if (geographicFormat == "utm" && utmZoneLetter != null)
            {
                UniversalTransverseMercator utm = new UniversalTransverseMercator(utmZoneLetter, utmZoneNumber, coord.lattitude, coord.longitude);
                var latlongformat = UniversalTransverseMercator.ConvertUTMtoLatLong(utm);

                coord.longitude = latlongformat.Latitude.ToDouble();
                coord.lattitude = latlongformat.Longitude.ToDouble();
                return new Tuple<GenericCoordinate, bool>(coord, true);
            }
            else
            {
                return new Tuple<GenericCoordinate, bool>(coord, false);
            }
        }

    }
}

