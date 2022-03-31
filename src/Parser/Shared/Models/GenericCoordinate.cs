﻿using System;
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

    }
}
