using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class CoordinateReferenceSystem
    {
        // General
        public bool IsUtm { get; set; }
        public bool IsWgs84 { get; set; }
        public bool CoordsAreSwappedBefore { get; set; }
        public bool CoordsAreSwappedAfter { get; set; }

        // UTM
        public string? GeodeticCrs => IsUtm ? "utm" : null;
        public string? UtmZoneLetter { get; set; } = null;
        public int? UtmZoneNumber { get; set; } = null;

        public CoordinateReferenceSystem()
        {

        }

        public CoordinateReferenceSystem(string utmLetter, int utmNumber)
        {
            IsUtm = true;
            UtmZoneLetter = utmLetter;
            UtmZoneNumber = utmNumber;
        }

        public CoordinateReferenceSystem(string urnString)
        {
            if (urnString.Contains("CRS84", StringComparison.InvariantCultureIgnoreCase))
                IsWgs84 = true;
            else if (urnString.Contains("EPSG", StringComparison.InvariantCultureIgnoreCase))
                HandleEpsg(urnString);
            else
                SetUtm(urnString);
        }

        private void HandleEpsg(string urnString)
        {
            if (urnString.Contains("4326", StringComparison.InvariantCultureIgnoreCase))
                IsWgs84 = true;
            else
                SetUtm(urnString);
        }

        private void SetUtm(string urnString)
        {
            IsUtm = true;
            if (urnString.Contains("25832", StringComparison.InvariantCultureIgnoreCase))
            {
                UtmZoneNumber = 32;
                UtmZoneLetter = "N";
            }
            else if (urnString.Contains("25833", StringComparison.InvariantCultureIgnoreCase))
            {
                UtmZoneNumber = 33;
                UtmZoneLetter = "N";
            }
        }

        public CoordinateReferenceSystem(bool isWgs84)
        {
            IsWgs84 = isWgs84;
        }

        public CoordinateReferenceSystem(bool isWgs84, bool coordsAreSwapped)
        {
            IsWgs84 = isWgs84;
            CoordsAreSwappedBefore = coordsAreSwapped;
        }
    }
}
