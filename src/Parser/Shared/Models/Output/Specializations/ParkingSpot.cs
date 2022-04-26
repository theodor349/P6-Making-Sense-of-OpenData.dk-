namespace Shared.Models.Output.Specializations
{
    public class ParkingSpot : GeodataOutput<MultiPolygon>
    {
        public int NumSpots { get; set; } = -10;
        public int HandicapSpots { get; set; } = -200;
        public string Address { get; set; } = "This is a very long address, to make sure we print it all (╯°□°）╯︵ ┻━┻";
    }
}
