namespace Shared.Models.Output
{
    public class GeodataOutput<T> : IntermediateOutput where T : GeoFeature
    {
        public T GeoFeatures { get; set; }
    }
}
