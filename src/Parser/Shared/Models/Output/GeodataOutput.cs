namespace Shared.Models.Output
{
    public class GeodataOutput<T> : IntermediateOutput where T : GeoFeature
    {
        public List<T> GeoFeatures { get; set; } = new List<T>();
    }
}
