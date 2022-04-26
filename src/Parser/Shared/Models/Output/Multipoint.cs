namespace Shared.Models.Output
{
    public class Multipoint : GeoFeature
    {
        public List<Point> Points { get; set; } = new List<Point>();
    }
}
