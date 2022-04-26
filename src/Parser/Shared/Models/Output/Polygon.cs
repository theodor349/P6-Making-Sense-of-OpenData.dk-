namespace Shared.Models.Output
{
    public class Polygon : GeoFeature
    {
        public List<Point> Coordinates { get; set; } = new List<Point>();
    }
}
