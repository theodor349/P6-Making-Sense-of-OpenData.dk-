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

            if (geographicFormat == "utm" && utmZoneLetter != null)
            {
                UniversalTransverseMercator utm = new UniversalTransverseMercator(utmZoneLetter, utmZoneNumber, coord1, coord2);
                var latlongformat = UniversalTransverseMercator.ConvertUTMtoLatLong(utm);

                longitude = latlongformat.Latitude.ToDouble();
                lattitude = latlongformat.Longitude.ToDouble();
            }
            else
            {
                throw new NullReferenceException();
            }
        }

    }
}

