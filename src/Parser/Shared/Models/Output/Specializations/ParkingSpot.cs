namespace Shared.Models.Output.Specializations
{
    public class ParkingSpot : GeodataOutput<MultiPolygon>
    {
        public int NumSpots { get; set; }
        public int HandicapSpots { get; set; }
        public int Address { get; set; }
    }
}
