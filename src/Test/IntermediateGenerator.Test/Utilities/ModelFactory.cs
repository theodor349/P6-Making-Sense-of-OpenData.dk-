using Shared.Models;
using Shared.Models.ObjectAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntermediateGenerator.Test.Utilities
{
    internal static class ModelFactory
    {
        public static DatasetObject GetDatasetObject(IntermediateObject intermediateObject)
        {
            List<IntermediateObject> list = new List<IntermediateObject>();
            list.Add(intermediateObject);
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
                    new FloatAttribute("ShapeSTArea", 3950.336602756815f),
                    new FloatAttribute("ShapeSTLength", 437.8971282813771f),
                }),
                new ListAttribute("geometry", new List<ObjectAttribute>()
                {
                    new TextAttribute("type", "Polygon"),
                    new ListAttribute("coordinates", new List<ObjectAttribute>()
                    {
                        new ListAttribute("StartArray", new List<ObjectAttribute>()
                        {
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new FloatAttribute("FloatValue", 8.240479957252727f), new FloatAttribute("FloatValue", 56.09079007170635f),  }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new FloatAttribute("FloatValue", 8.240318595897797f), new FloatAttribute("FloatValue", 56.090837686750646f), }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new FloatAttribute("FloatValue", 8.240047779834569f), new FloatAttribute("FloatValue", 56.090904377291984f), }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new FloatAttribute("FloatValue", 8.239825529070766f), new FloatAttribute("FloatValue", 56.09075226701193f),  }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new FloatAttribute("FloatValue", 8.239608726838116f), new FloatAttribute("FloatValue", 56.090590206517284f), }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new FloatAttribute("FloatValue", 8.240022603150605f), new FloatAttribute("FloatValue", 56.09043407736101f),  }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new FloatAttribute("FloatValue", 8.24019233686947f),  new FloatAttribute("FloatValue", 56.09056237798621f),  }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new FloatAttribute("FloatValue", 8.240395238968205f), new FloatAttribute("FloatValue", 56.09046966900493f),  }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new FloatAttribute("FloatValue", 8.240585490036453f), new FloatAttribute("FloatValue", 56.09038274916644f),  }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new FloatAttribute("FloatValue", 8.240779697910575f), new FloatAttribute("FloatValue", 56.0902939934467f),   }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new FloatAttribute("FloatValue", 8.241035223152073f), new FloatAttribute("FloatValue", 56.090177248322696f), }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new FloatAttribute("FloatValue", 8.241072187406699f), new FloatAttribute("FloatValue", 56.09020206775729f),  }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new FloatAttribute("FloatValue", 8.24123737902105f),  new FloatAttribute("FloatValue", 56.090191366335105f), }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new FloatAttribute("FloatValue", 8.24145804337276f),  new FloatAttribute("FloatValue", 56.090481386660706f), }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new FloatAttribute("FloatValue", 8.241567533899898f), new FloatAttribute("FloatValue", 56.09059793116452f),  }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new FloatAttribute("FloatValue", 8.241617303686505f), new FloatAttribute("FloatValue", 56.090584300935404f), }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new FloatAttribute("FloatValue", 8.241815237368847f), new FloatAttribute("FloatValue", 56.09079504913395f),  }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new FloatAttribute("FloatValue", 8.241851540593345f), new FloatAttribute("FloatValue", 56.090833692352234f), }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new FloatAttribute("FloatValue", 8.241681157785298f), new FloatAttribute("FloatValue", 56.09088098333104f),  }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new FloatAttribute("FloatValue", 8.241343630277166f), new FloatAttribute("FloatValue", 56.09052731829657f),  }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new FloatAttribute("FloatValue", 8.241219705532911f), new FloatAttribute("FloatValue", 56.09052280917696f),  }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new FloatAttribute("FloatValue", 8.241138757384064f), new FloatAttribute("FloatValue", 56.09054854466553f),  }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new FloatAttribute("FloatValue", 8.241082890945895f), new FloatAttribute("FloatValue", 56.090491987788184f), }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new FloatAttribute("FloatValue", 8.24068850056752f),  new FloatAttribute("FloatValue", 56.090605738043045f), }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new FloatAttribute("FloatValue", 8.240689451412898f), new FloatAttribute("FloatValue", 56.090610262501514f), }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new FloatAttribute("FloatValue", 8.240634176396334f), new FloatAttribute("FloatValue", 56.09055912707765f),  }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new FloatAttribute("FloatValue", 8.240351930550798f), new FloatAttribute("FloatValue", 56.090678658608425f), }),
                            new ListAttribute("StartArray", new List<ObjectAttribute>() { new FloatAttribute("FloatValue", 8.240479957252727f), new FloatAttribute("FloatValue", 56.09079007170635f),  }),
                        }),
                    }),
                }),
            });
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

        internal static List<ObjectAttribute> GetListOfObjectsAttributes(int amount)
        {
            var res = new List<ObjectAttribute>();
            for (int i = 0; i < amount; i++)
            {
                res.Add(GetRingkøbingSkjernParking());
            }
            return res;
        }
    }
}
