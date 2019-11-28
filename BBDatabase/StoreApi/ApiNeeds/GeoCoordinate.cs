namespace BBCollection.StoreApi
{
    /// <summary>
    /// Class to hold coordinate position by longitude and latitude
    /// </summary>
    public class GeoCoordinate
    {
        public double _longitude { get; set; }
        public double _latitude { get; set; }

        public GeoCoordinate(double lon, double lat)
        {
            _longitude = lon;
            _latitude = lat;
        }
        public GeoCoordinate(string lon, string lat)
        {
            _longitude = double.Parse(lon);
            _latitude = double.Parse(lat);
        }

        public override string ToString()
        {
            return _longitude.ToString() + "," + _latitude.ToString();
        }
    }
}
