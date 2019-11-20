namespace BBCollection.StoreApi.CoopApi
{
    public class CoopAPILinks
    {
        string _baseUrl { get; set; }
        string _radiusUrl { get; set; }
        string _latitudeUrl { get; set; }
        GeoCoordinate _geoPosition { get; set; }
        string _longtitudeUrl { get; set; }
        string _productUrl { get; set; }
        int _inputRadius { get; set; }

        public CoopAPILinks()
        {
            _baseUrl = "https://api.cl.coop.dk/";
            _radiusUrl = "storeapi/v1/stores/find/radius/";
            _latitudeUrl = "?latitude=";
            //_geoPosition = new GeoCoordinate(12.3688172, 55.7290673); // Copenhagen
            //_geoPosition = new GeoCoordinate(10.2039, 56.1629); // Aarhus
            _geoPosition = new GeoCoordinate(9.9217, 57.0488); // Aalborg
            _longtitudeUrl = "&longitude=";
            _productUrl = "productapi/v1/product/";
        }

        public string GetRadiusLink(int inputRadius)
        {
            string _url = _baseUrl + _radiusUrl + inputRadius + _latitudeUrl + _geoPosition._latitude + _longtitudeUrl + _geoPosition._longitude;

            return _url;
        }

        public string GetProductLinke(string storeNumber)
        {
            string _url = _baseUrl + _productUrl + storeNumber;

            return _url;
        }
    }
}
