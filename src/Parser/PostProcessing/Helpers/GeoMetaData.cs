using CoordinateSharp;
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

        public GeoMetaData(ILogger<GeoMetaData> logger)
        {
            _logger = logger;
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
                InferCoordinateReferenceSystem(dataset, point);
            else
                _logger.LogWarning("Unable to find a any points");
        }

        private void InferCoordinateReferenceSystem(DatasetObject dataset, Tuple<double, double> point)
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


            if (crs != null)
                dataset.Properties.Add("CoordinateReferenceSystem", JsonSerializer.Serialize(crs));
            else
                _logger.LogWarning("Unable to identify the Coordinate Reference System");
        }

        private static Tuple<double, double> Swap(Tuple<double, double> point)
        {
            return new Tuple<double, double>(point.Item2, point.Item1);
        }

        private Tuple<double, double> ConvertFromUtm(Tuple<double, double> point, string utmLetter, int utmNumber)
        {
            UniversalTransverseMercator utm = new UniversalTransverseMercator(utmLetter, utmNumber, point.Item1, point.Item2);
            var latlongformat = UniversalTransverseMercator.ConvertUTMtoLatLong(utm);

            return new Tuple<double, double>(latlongformat.Longitude.ToDouble(), latlongformat.Latitude.ToDouble());
        }

        private bool IsInDenmark(Tuple<double, double> point)
        {
            if (point.Item1 < 54 || 58 < point.Item1)
                return false;
            if (point.Item2 < 6 || 16 < point.Item2)
                return false;
            return true;
        }

        private Tuple<double, double>? GetFirstPoint(IntermediateObject? intermediateObject)
        {
            foreach (var attr in intermediateObject.Attributes)
            {
                var point = GetFirstPoint(attr);
                if (point != null)
                    return GetPointData(point);
            }
            return null;
        }

        private Tuple<double, double> GetPointData(ListAttribute point)
        {
            var list = (List<ObjectAttribute>)point.Value;
            var coord0 = (double)list[0].Value;
            var coord1 = (double)list[1].Value;
            return new Tuple<double, double>(coord0, coord1);
        }

        private ListAttribute? GetFirstPoint(ObjectAttribute attr)
        {
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
                        return GetFirstPoint(a);
                    }
                }
            }
            return null;
        }
    }
}
