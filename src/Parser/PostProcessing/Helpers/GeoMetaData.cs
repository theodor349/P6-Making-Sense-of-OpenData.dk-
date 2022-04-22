using CoordinateSharp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared.Models;
using Shared.Models.ObjectAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PostProcessing.Helpers
{
    internal interface IGeoMetaData
    {
        Task AssignGeoMetaData(DatasetObject dataset);
    }

    internal class GeoMetaData : IGeoMetaData
    {
        private readonly ILogger<GeoMetaData> _logger;
        private readonly IConfiguration _configuration;

        private readonly GenericCoordinate _targetUpperLeft;
        private readonly GenericCoordinate _targetLowerRight;

        public GeoMetaData(ILogger<GeoMetaData> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            _targetUpperLeft = new GenericCoordinate(
                double.Parse(_configuration["GeoMetaData:TargetArea:LatiMax"]),
                double.Parse(_configuration["GeoMetaData:TargetArea:LongMin"])
                );
            _targetLowerRight = new GenericCoordinate(
                double.Parse(_configuration["GeoMetaData:TargetArea:LatiMin"]),
                double.Parse(_configuration["GeoMetaData:TargetArea:LongMax"])
                );
        }

        public Task AssignGeoMetaData(DatasetObject dataset)
        {
            if(!dataset.HasProperty("CoordinateReferenceSystem"))
                AssignCoordinateReferenceSystem(dataset);
            return Task.CompletedTask;
        }

        private void AssignCoordinateReferenceSystem(DatasetObject dataset)
        {
            var point = GetFirstPoint(dataset.Objects.FirstOrDefault());
            if (point != null)
                InferCoordinateReferenceSystem(dataset, (GenericCoordinate)point);
            else
                _logger.LogWarning("Unable to find a any points");
        }

        private void InferCoordinateReferenceSystem(DatasetObject dataset, GenericCoordinate point)
        {
            CoordinateReferenceSystem? crs = null;
            if (IsInDenmark(point))
            {
                crs = new CoordinateReferenceSystem(isWgs84: true);
            }
            else if (IsInDenmark(Swap(point)))
            {
                crs = new CoordinateReferenceSystem(isWgs84: true);
                crs.CoordsAreSwappedBefore = true;
            }
            else if (IsInDenmark(ConvertFromUtm(point, "N", 33)))
            {
                crs = new CoordinateReferenceSystem("N", 33);
            }
            else if (IsInDenmark(ConvertFromUtm(point, "N", 32)))
            {
                crs = new CoordinateReferenceSystem("N", 32);
            }
            else if (IsInDenmark(ConvertFromUtm(Swap(point), "N", 33)))
            {
                crs = new CoordinateReferenceSystem("N", 33);
                crs.CoordsAreSwappedBefore = true;
            }
            else if (IsInDenmark(ConvertFromUtm(Swap(point), "N", 32)))
            {
                crs = new CoordinateReferenceSystem("N", 32);
                crs.CoordsAreSwappedBefore = true;
            }
            else if (IsInDenmark(Swap(ConvertFromUtm(point, "N", 33))))
            {
                crs = new CoordinateReferenceSystem("N", 33);
                crs.CoordsAreSwappedAfter = true;
            }
            else if (IsInDenmark(Swap(ConvertFromUtm(point, "N", 32))))
            {
                crs = new CoordinateReferenceSystem("N", 32);
                crs.CoordsAreSwappedAfter = true;
            }
            else if (IsInDenmark(ConvertFromUtm(point, "V", 32)))
            {
                crs = new CoordinateReferenceSystem("V", 32);
            }
            else if (IsInDenmark(ConvertFromUtm(Swap(point), "V", 32)))
            {
                crs = new CoordinateReferenceSystem("V", 32);
                crs.CoordsAreSwappedBefore = true;
            }


            if (crs != null)
                dataset.Properties.Add("CoordinateReferenceSystem", JsonSerializer.Serialize(crs));
            else
                _logger.LogWarning("Unable to identify the Coordinate Reference System");
        }

        private static GenericCoordinate Swap(GenericCoordinate point)
        {
            return new GenericCoordinate(point.Longitude, point.Latitude);
        }

        private GenericCoordinate ConvertFromUtm(GenericCoordinate point, string utmLetter, int utmNumber)
        {
            UniversalTransverseMercator utm = new UniversalTransverseMercator(utmLetter, utmNumber, point.Longitude, point.Latitude);
            var latlongformat = UniversalTransverseMercator.ConvertUTMtoLatLong(utm);

            return new GenericCoordinate(latlongformat.Latitude.ToDouble(), latlongformat.Longitude.ToDouble());
        }

        private bool IsInDenmark(GenericCoordinate point)
        {
            if (point.Longitude <= _targetUpperLeft.Longitude || point.Longitude >= _targetLowerRight.Longitude)
                return false;
            if (point.Latitude <= _targetLowerRight.Latitude || point.Latitude >= _targetUpperLeft.Latitude)
                return false;
            return true;
        }

        private GenericCoordinate? GetFirstPoint(IntermediateObject? intermediateObject)
        {
            foreach (var attr in intermediateObject.Attributes)
            {
                var point = GetFirstPoint(attr);
                if (point != null)
                    return GetPointData(point);
            }
            return null;
        }

        private GenericCoordinate GetPointData(ListAttribute point)
        {
            var list = (List<ObjectAttribute>)point.Value;
            var coordLong = (double)list[0].Value;
            var coordLati = (double)list[1].Value;
            Console.WriteLine("Long: " + coordLong + ", Lati: " + coordLati);
            return new GenericCoordinate(coordLati, coordLong);
        }

        private ListAttribute? GetFirstPoint(ObjectAttribute attr)
        {
            ListAttribute point = null;
            if (attr.GetType() == typeof(ListAttribute))
            {
                if (attr.HasLabel(ObjectLabel.Point))
                {
                    return (ListAttribute)attr;
                }
                else
                {
                    foreach (var a in (List<ObjectAttribute>)attr.Value)
                    {
                        point = GetFirstPoint(a);
                        if (point != null)
                            return point;
                    }
                }
            }
            return point;
        }
    }
}
