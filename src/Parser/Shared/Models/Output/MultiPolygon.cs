namespace Shared.Models.Output
{
    public class MultiPolygon : GeoFeature
    {
        public List<Polygon> Polygons { get; set; } = new List<Polygon>();
    }
}
