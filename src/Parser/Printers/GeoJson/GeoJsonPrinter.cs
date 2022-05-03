using Newtonsoft.Json.Linq;
using Shared.ComponentInterfaces;
using Shared.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Printers.GeoJson
{
    public interface IGeoJsonPrinter : IPrinter { }

    public class GeoJsonPrinter : IGeoJsonPrinter
    {
        private readonly IFilePrinter _filePrinter;

        public GeoJsonPrinter(IFilePrinter filePrinter)
        {
            _filePrinter = filePrinter;
        }

        public async Task Print(OutputDataset dataset, int iteration)
        {
            var expected = new JObject(
                             new JProperty("type", "FeatureCollection"),
                             new JProperty("features",
                                new JArray(
                                    new JObject(
                                        new JProperty("type", "Feature"),
                                        new JProperty("geometry",
                                            new JObject(
                                                new JProperty("type", "Multipolygon"),
                                                new JProperty("coordinates", new JArray(new JArray[]
                                                    {
                                                        new JArray(new JArray[]
                                                        {
                                                            new JArray(new JArray[]
                                                            {
                                                                new JArray(new JValue[]
                                                                {
                                                                    new JValue(552677.64679076),
                                                                    new JValue(6191070.56990207),
                                                                }),
                                                                new JArray(new JValue[]
                                                                {
                                                                    new JValue(552677.64679076),
                                                                    new JValue(6191070.56990207),
                                                                })
                                                            }),
                                                            new JArray(new JArray[]
                                                            {
                                                                new JArray(new JValue[]
                                                                {
                                                                    new JValue(552677.64679076),
                                                                    new JValue(6191070.56990207),
                                                                }),
                                                                new JArray(new JValue[]
                                                                {
                                                                    new JValue(552677.64679076),
                                                                    new JValue(6191070.56990207),
                                                                })
                                                            })
                                                        }),
                                                                                                                new JArray(new JArray[]
                                                        {
                                                            new JArray(new JArray[]
                                                            {
                                                                new JArray(new JValue[]
                                                                {
                                                                    new JValue(552677.64679076),
                                                                    new JValue(6191070.56990207),
                                                                }),
                                                                new JArray(new JValue[]
                                                                {
                                                                    new JValue(552677.64679076),
                                                                    new JValue(6191070.56990207),
                                                                })
                                                            }),
                                                            new JArray(new JArray[]
                                                            {
                                                                new JArray(new JValue[]
                                                                {
                                                                    new JValue(552677.64679076),
                                                                    new JValue(6191070.56990207),
                                                                }),
                                                                new JArray(new JValue[]
                                                                {
                                                                    new JValue(552677.64679076),
                                                                    new JValue(6191070.56990207),
                                                                })
                                                            })
                                                        })

                                                })))),
                                        new JProperty("properties", new JObject())))));

            var json = expected.ToString();
            await _filePrinter.Print(dataset, expected, iteration);
        }

        internal static JProperty ReturnPolygonProperty()
        {
            JArray coords = new JArray();
            coords.Add(new JArray(1.1, 1.2));
            coords.Add(new JArray(1.3, 1.4));
            coords.Add(new JArray(1.5, 1.6));
            coords.Add(new JArray(1.1, 1.2));
            JArray JArr = new JArray();
            JArr.Add(coords);
            JProperty JProp = new JProperty("coordinates", JArr);
            return JProp;
        }
    }
}
