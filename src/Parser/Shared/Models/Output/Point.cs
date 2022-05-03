namespace Shared.Models.Output
{
    public class Point : GeoFeature
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public Point()
        {

        }

        public Point(double longitude, double latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }
    }
}
