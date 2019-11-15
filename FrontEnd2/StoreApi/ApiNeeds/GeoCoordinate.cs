namespace FrontEnd2
{
    public class GeoCoordinate
    {
        public double _longitude { get; set; }
        public double _latitude { get; set; }

        public GeoCoordinate(double lon, double lat)
        {
            _longitude = lon;
            _latitude = lat;
        }

        public override string ToString()
        {
            return _longitude.ToString() + "," + _latitude.ToString();
        }
    }
}
