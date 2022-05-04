using FluentAssertions;
using LabelRecognizer.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.Models;
using Shared.Models.ObjectAttributes;
using System;
using LabelRecognizer.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.Models.ObjectAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Shared.Models;

namespace LabelRecognizer.Tests.Labeling.GeoTests
{
    [TestClass]
    public class Coordinates
    {
        [TestMethod]
        public void LabelGenerator_Coordinate_Correct()
        {
            var ios = new List<IntermediateObject>();
            var coordinate = ModelFactory.GetCoordinateAttr(349);
            ios.Add(ModelFactory.GetIntermediateObject(coordinate));
            var inputDataset = ModelFactory.GetDatasetObject(ios);

            var setup = new TestSetup();
            var labelGenerator = setup.LabelGenerator();
            labelGenerator.AddLabels(inputDataset).Wait();

            // Assert 
            var res = inputDataset.Objects[0].Attributes[0].Labels.FirstOrDefault(x => x.Label == PredefinedLabels.Point);
            res.Should().NotBeNull();
        }
        [TestMethod]
        public void LabelGenerator_PolygonCoordinates_Correct()
        {
            var ios = new List<IntermediateObject>();
            var latitude1 = ModelFactory.GetObjectAttr("FloatValue", 8.241631367419494d);
            var longitude1 = ModelFactory.GetObjectAttr("FloatValue", 56.090578540697706d);
            var coord1 = ModelFactory.GetListAttribute("StartArray", latitude1, longitude1);
            var latitude2 = ModelFactory.GetObjectAttr("FloatValue", 10.241631367419494d);
            var longitude2 = ModelFactory.GetObjectAttr("FloatValue", 69.090578540697706d);
            var coord2 = ModelFactory.GetListAttribute("StartArray", latitude2, longitude2);
            var coords = ModelFactory.GetListAttribute("coordinates", coord1, coord2);

            ios.Add(ModelFactory.GetIntermediateObject(coords));
            var inputDataset = ModelFactory.GetDatasetObject(ios);

            var setup = new TestSetup();
            var labelGenerator = setup.LabelGenerator();
            labelGenerator.AddLabels(inputDataset).Wait();

            // Assert 
            var coordinates = inputDataset.Objects[0].Attributes[0];
            var coordinateList = (List<ObjectAttribute>)coordinates.Value;
            // Coordinates not flagged as Coordinate
            var res1 = coordinates.Labels.FirstOrDefault(x => x.Label == PredefinedLabels.Point);
            res1.Should().BeNull();
            // Coord 1 flagged as Coordinate
            var res2 = coordinateList[0].Labels.FirstOrDefault(x => x.Label == PredefinedLabels.Point);
            res2.Should().NotBeNull();
            // Coord 2 flagged as Coordinate
            var res3 = coordinateList[1].Labels.FirstOrDefault(x => x.Label == PredefinedLabels.Point);
            res3.Should().NotBeNull();
        }

        /// <summary>
        /*
            "geometry": {
                "type": "Point",
                "coordinates": [
                    8.241631367419494,
                    56.090578540697706
                ]
            }
         */
        /// </summary>
        [TestMethod]
        public void LabelGenerator_CoordinateWithText_Correct()
        {
            var ios = new List<IntermediateObject>();
            var typeAttr = new TextAttribute("type", "Point");
            var latitude = ModelFactory.GetObjectAttr("FloatValue", 8.241631367419494d);
            var longitude = ModelFactory.GetObjectAttr("FloatValue", 56.090578540697706d);
            var list = ModelFactory.GetListAttribute("coordinates", latitude, longitude);
            var listAttr = ModelFactory.GetListAttribute("geometry", typeAttr, list);
            ios.Add(ModelFactory.GetIntermediateObject(listAttr));
            var inputDataset = ModelFactory.GetDatasetObject(ios);

            var setup = new TestSetup();
            var labelGenerator = setup.LabelGenerator();
            labelGenerator.AddLabels(inputDataset).Wait();

            var res = ((List<ObjectAttribute>)inputDataset.Objects[0].Attributes[0].Value)[1].Labels.FirstOrDefault(x => x.Label == PredefinedLabels.Point);
            res.Should().NotBeNull();
        }

        /// <summary>
        /*
            "geometry": {
                "type": "Polygon",
                "coordinates": [
                [
                    [
                        8.240479957252727,
                        56.09079007170635
                    ],
                    [
                        8.240479957252727,
                        56.09079007170635
                    ]
                ]
            }
         */
        /// </summary>
        [TestMethod]
        public void LabelGenerator_PolygonCoordinatesWithText_Correct()
        {
            var ios = new List<IntermediateObject>();
            var latitude1 = ModelFactory.GetObjectAttr("FloatValue", 8.241631367419494d);
            var longitude1 = ModelFactory.GetObjectAttr("FloatValue", 56.090578540697706d);
            var coord1 = ModelFactory.GetListAttribute("StartArray", latitude1, longitude1);
            var latitude2 = ModelFactory.GetObjectAttr("FloatValue", 8.240479957252727d);
            var longitude2 = ModelFactory.GetObjectAttr("FloatValue", 56.09079007170635d);
            var coord2 = ModelFactory.GetListAttribute("StartArray", latitude2, longitude2);
            var coords = ModelFactory.GetListAttribute("coordinates", coord1, coord2);
            var typeAttr = new TextAttribute("type", "Polygon");
            var attrList = ModelFactory.GetListAttribute("geometry", typeAttr, coords);
            ios.Add(ModelFactory.GetIntermediateObject(attrList));
            var inputDataset = ModelFactory.GetDatasetObject(ios);

            var setup = new TestSetup();
            var labelGenerator = setup.LabelGenerator();
            labelGenerator.AddLabels(inputDataset).Wait();

            // Assert 
            var coordinates = ((List<ObjectAttribute>)inputDataset.Objects[0].Attributes[0].Value)[1];
            var coordinateList = (List<ObjectAttribute>)coordinates.Value;
            // Coordinates not flagged as Coordinate
            var res1 = coordinates.Labels.FirstOrDefault(x => x.Label == PredefinedLabels.Point);
            res1.Should().BeNull();
            // Coord 1 flagged as Coordinate
            var res2 = coordinateList[0].Labels.FirstOrDefault(x => x.Label == PredefinedLabels.Point);
            res2.Should().NotBeNull();
            // Coord 2 flagged as Coordinate
            var res3 = coordinateList[1].Labels.FirstOrDefault(x => x.Label == PredefinedLabels.Point);
            res3.Should().NotBeNull();
        }
    }
}
