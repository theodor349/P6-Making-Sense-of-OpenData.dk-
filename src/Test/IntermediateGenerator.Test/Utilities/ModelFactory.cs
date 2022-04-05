using Shared.Models;
using Shared.Models.ObjectAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasetGenerator.Test.Utilities
{
    internal static class ModelFactory
    {
        public static DatasetObject GetDatasetObject(IntermediateObject intermediateObject)
        {
            List<IntermediateObject> list = new List<IntermediateObject>();
            list.Add(intermediateObject);
            return new DatasetObject(".json", "filename", list);
        }

        public static DatasetObject GetDatasetObject(List<IntermediateObject> list)
        {
            return new DatasetObject(".json", "filename", list);
        }

        public static ListAttribute GetRingkøbingSkjernParking()
        {
            return new ListAttribute("StartObject", new List<ObjectAttribute>()
            {
                new TextAttribute("type", "Feature"),
                new ListAttribute("properties", new List<ObjectAttribute>()
                {
                    new LongAttribute("OBJECTID", 267),
                    new TextAttribute("P_PLADS_NR", "RK 1"),
                    new TextAttribute("INDKOERSEL_VEJ", "Torvegade_ Enghavevej"),
                    new TextAttribute("UDKOERSEL_VEJ", "Enghavevej"),
                    new TextAttribute("EJER", "Institution"),
                    new TextAttribute("ANTAL_PLADSER", "92"),
                    new TextAttribute("HANDICAP_PLADSER", "2"),
                    new TextAttribute("TIDSZONE", "3 timer"),
                    new TextAttribute("BELAEGNING", "Asfalt"),
                    new TextAttribute("AFMAERKNING", "Hvid"),
                    new TextAttribute("ANTAL_LYSMASTER", "s20"),
                    new TextAttribute("URL", "https://gisext.rksk.dk/pdf/PPlads/RK_1_Torvegade_Enghavevej.pdf"),
                    new TextAttribute("BEMAERKNINGER", "kommunen holder_aftaler?"),
                    new TextAttribute("GlobalID", "{A0864547-1F2E-43F4-87F5-34B5ECACEC1A}"),
                    new NullAttribute("created_user"),
                    new NullAttribute("created_date"),
                    new TextAttribute("last_edited_user", "RKSK"),
                    new DateAttribute("last_edited_date", new DateTime(2019,12,17,7,23,46)),
                    new NullAttribute("Brugernavn"),
                    new DoubleAttribute("ShapeSTArea", 3950.336602756815),
                    new DoubleAttribute("ShapeSTLength", 437.8971282813771),
                }),
                new ListAttribute("geometry", new List<ObjectAttribute>()
                {
                    new TextAttribute("type", "Polygon"),
                    new ListAttribute("coordinates", new List<ObjectAttribute>()
                    {
                        new ListAttribute("StartArray", new List<ObjectAttribute>()
                        {
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("FloatValue", 8.240479957252727), new DoubleAttribute("FloatValue", 56.09079007170635),  }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("FloatValue", 8.240318595897797), new DoubleAttribute("FloatValue", 56.090837686750646), }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("FloatValue", 8.240047779834569), new DoubleAttribute("FloatValue", 56.090904377291984), }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("FloatValue", 8.239825529070766), new DoubleAttribute("FloatValue", 56.09075226701193),  }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("FloatValue", 8.239608726838116), new DoubleAttribute("FloatValue", 56.090590206517284), }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("FloatValue", 8.240022603150605), new DoubleAttribute("FloatValue", 56.09043407736101),  }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("FloatValue", 8.24019233686947),  new DoubleAttribute("FloatValue", 56.09056237798621),  }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("FloatValue", 8.240395238968205), new DoubleAttribute("FloatValue", 56.09046966900493),  }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("FloatValue", 8.240585490036453), new DoubleAttribute("FloatValue", 56.09038274916644),  }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("FloatValue", 8.240779697910575), new DoubleAttribute("FloatValue", 56.0902939934467),   }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("FloatValue", 8.241035223152073), new DoubleAttribute("FloatValue", 56.090177248322696), }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("FloatValue", 8.241072187406699), new DoubleAttribute("FloatValue", 56.09020206775729),  }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("FloatValue", 8.24123737902105),  new DoubleAttribute("FloatValue", 56.090191366335105), }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("FloatValue", 8.24145804337276),  new DoubleAttribute("FloatValue", 56.090481386660706), }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("FloatValue", 8.241567533899898), new DoubleAttribute("FloatValue", 56.09059793116452),  }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("FloatValue", 8.241617303686505), new DoubleAttribute("FloatValue", 56.090584300935404), }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("FloatValue", 8.241815237368847), new DoubleAttribute("FloatValue", 56.09079504913395),  }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("FloatValue", 8.241851540593345), new DoubleAttribute("FloatValue", 56.090833692352234), }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("FloatValue", 8.241681157785298), new DoubleAttribute("FloatValue", 56.09088098333104),  }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("FloatValue", 8.241343630277166), new DoubleAttribute("FloatValue", 56.09052731829657),  }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("FloatValue", 8.241219705532911), new DoubleAttribute("FloatValue", 56.09052280917696),  }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("FloatValue", 8.241138757384064), new DoubleAttribute("FloatValue", 56.09054854466553),  }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("FloatValue", 8.241082890945895), new DoubleAttribute("FloatValue", 56.090491987788184), }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("FloatValue", 8.24068850056752),  new DoubleAttribute("FloatValue", 56.090605738043045), }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("FloatValue", 8.240689451412898), new DoubleAttribute("FloatValue", 56.090610262501514), }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("FloatValue", 8.240634176396334), new DoubleAttribute("FloatValue", 56.09055912707765),  }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("FloatValue", 8.240351930550798), new DoubleAttribute("FloatValue", 56.090678658608425), }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("FloatValue", 8.240479957252727), new DoubleAttribute("FloatValue", 56.09079007170635),  }),
                        }),
                    }),
                }),
            });
        }

        public static ListAttribute GetObjectAttributes(int coordinateCount)
        {
            var coordinates = new List<ObjectAttribute>();
            for (int i = 0; i < coordinateCount; i++)
            {
                coordinates.Add(new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("FloatValue", 8.240479957252727), new DoubleAttribute("FloatValue", 56.09079007170635), }));
            }
            var res = new ListAttribute("StartObject", new List<ObjectAttribute>()
            {
                new TextAttribute("type", "Feature"),
                new ListAttribute("properties", new List<ObjectAttribute>()
                {
                    new LongAttribute("OBJECTID", 267),
                    new TextAttribute("P_PLADS_NR", "RK 1"),
                    new TextAttribute("INDKOERSEL_VEJ", "Torvegade_ Enghavevej"),
                    new TextAttribute("UDKOERSEL_VEJ", "Enghavevej"),
                    new TextAttribute("EJER", "Institution"),
                    new TextAttribute("ANTAL_PLADSER", "92"),
                    new TextAttribute("HANDICAP_PLADSER", "2"),
                    new TextAttribute("TIDSZONE", "3 timer"),
                    new TextAttribute("BELAEGNING", "Asfalt"),
                    new TextAttribute("AFMAERKNING", "Hvid"),
                    new TextAttribute("ANTAL_LYSMASTER", "s20"),
                    new TextAttribute("URL", "https://gisext.rksk.dk/pdf/PPlads/RK_1_Torvegade_Enghavevej.pdf"),
                    new TextAttribute("BEMAERKNINGER", "kommunen holder_aftaler?"),
                    new TextAttribute("GlobalID", "{A0864547-1F2E-43F4-87F5-34B5ECACEC1A}"),
                    new NullAttribute("created_user"),
                    new NullAttribute("created_date"),
                    new TextAttribute("last_edited_user", "RKSK"),
                    new DateAttribute("last_edited_date", new DateTime(2019,12,17,7,23,46)),
                    new NullAttribute("Brugernavn"),
                    new DoubleAttribute("ShapeSTArea", 3950.336602756815),
                    new DoubleAttribute("ShapeSTLength", 437.8971282813771),
                }),
                new ListAttribute("geometry", new List<ObjectAttribute>()
                {
                    new TextAttribute("type", "Polygon"),
                    new ListAttribute("coordinates", coordinates),
                }),
            });
            return res;
        }

        internal static List<IntermediateObject> ConvertListToIntermediateObject(List<ObjectAttribute> list)
        {
            var res = new List<IntermediateObject>();
            foreach (var attribute in list)
                res.Add(new IntermediateObject((List<ObjectAttribute>) attribute.Value));
            return res;
        }

        internal static IntermediateObject GetIntermediateObject(ListAttribute features)
        {
            List<ObjectAttribute> list = new List<ObjectAttribute>();
            list.Add(features);
            return new IntermediateObject(list);
        }
        internal static IntermediateObject GetIntermediateObject(List<ObjectAttribute> objects)
        {
            return new IntermediateObject(objects);
        }

        internal static List<ObjectAttribute> GetListOfObjectsAttributes(int amount, Func<ObjectAttribute> getObjectAttribute)
        {
            var res = new List<ObjectAttribute>();
            for (int i = 0; i < amount; i++)
            {
                res.Add(getObjectAttribute());
            }
            return res;
        }

        internal static List<ObjectAttribute> GetListOfObjectsAttributes(int amount, Func<int, ObjectAttribute> getObjectAttribute)
        {
            var res = new List<ObjectAttribute>();
            for (int i = 0; i < amount; i++)
            {
                res.Add(getObjectAttribute(i));
            }
            return res;
        }
    }
}
