using System;
using System.Web;

namespace FrontEnd2
{
    public enum Brand { bilka, foetex, netto, salling, br, carlsjr, starbucks };

    class SallingAPILink : APILink
    {
        #region Properties
        string _productSuggestionsBaseLink { get; set; }
        string _singleStoreBaseLink { get; set; }
        string _multiStoreBaseLink { get; set; }
        string _geoPositionURL { get; set; }
        GeoCoordinate _geoPosition { get; set; }
        string _radiusURL { get; set; }
        int _radius;
        int Radius
        {
            get { return _radius; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException($"{nameof(value)} must be a positive integer.");
                }
                _radius = value;
            }
        }
        string _pageURL { get; set; }
        int _pageCount;
        int PageCount
        {
            get { return _pageCount; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException($"{nameof(value)} must be a positive integer.");
                }
                _pageCount = value;
            }
        }
        string _perPageURL { get; set; }
        int _perPageCount;
        int PerPageCount
        {
            get { return _perPageCount; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException($"{nameof(value)} must be a positive integer.");
                }
                _perPageCount = value;
            }
        }
        string _brandURL { get; set; }
        string _storeId { get; set; }
        string _cityURL { get; set; }
        string _zipURL { get; set; }
        int _zip;
        int Zip
        {
            get { return _zip; }
            set
            {
                if (!(value >= 1000) || !(value <= 9999))
                {
                    throw new ArgumentOutOfRangeException($"{nameof(value)} must be a >= 1000 and <= 9999.");
                }
                _zip = value;
            }
        }
        #endregion

        public SallingAPILink()
        {
            _productSuggestionsBaseLink = "https://api.sallinggroup.com/v1-beta/product-suggestions/relevant-products?query=";
            _singleStoreBaseLink = "https://api.sallinggroup.com/v2/stores/";
            _multiStoreBaseLink = "https://api.sallinggroup.com/v2/stores?";
            _pageURL = "page=";
            _perPageURL = "&per_page=";
            _radiusURL = "&radius=";
            _zipURL = "&zip=";
            _geoPositionURL = "&geo=";
            _brandURL = "&brand=";
            _cityURL = "&city=";
            _geoPosition = new GeoCoordinate(57.0122721, 9.9917718);
            _pageCount = 1;
            _perPageCount = 10;
            _radius = 10;
        }

        #region Public Functions
        public string GetProductAPILink(string searchWord)
        {
            string encodedSearchWord = HttpUtility.UrlEncode(searchWord);

            return _productSuggestionsBaseLink + encodedSearchWord;
        }

        public string GetSingleStoreAPILink(string storeId)
        {
            return _singleStoreBaseLink + storeId;
        }

        #region MultiStore City Search Functions
        public string GetMultiStoreAPILink(string city, int radiusLimit, Brand brand)
        {
            return GetMultiStoreAPILink(city, radiusLimit) + MultiStoreBrandURLPart(brand);
        }

        public string GetMultiStoreAPILink(string city, int radiusLimit)
        {
            return GetMultiStoreAPILink(city) + MultiStoreRadiusURLPart(radiusLimit);
        }

        public string GetMultiStoreAPILink(string city)
        {
            string cityEncoded = HttpUtility.UrlEncode(city);
            return MultiStoreBaseURL() + _cityURL + cityEncoded;
        }
        #endregion

        #region MultiStore Zip Search Functions
        public string GetMultiStoreAPILink(int zip, int radiusLimit, Brand brand)
        {
            return GetMultiStoreAPILink(zip, radiusLimit) + MultiStoreBrandURLPart(brand);
        }

        public string GetMultiStoreAPILink(int zip, int radiusLimit)
        {
            return GetMultiStoreAPILink(zip) + MultiStoreRadiusURLPart(radiusLimit);
        }

        public string GetMultiStoreAPILink(int zip)
        {
            return MultiStoreBaseURL() + GetZipURL(zip);
        }
        #endregion

        #region MultiStore GeoPosition Search Functions
        public string GetMultiStoreAPILink(GeoCoordinate geoPosition, int radiusLimit, Brand brand)
        {
            return GetMultiStoreAPILink(geoPosition, radiusLimit) + MultiStoreBrandURLPart(brand);
        }

        public string GetMultiStoreAPILink(GeoCoordinate geoPosition, int radiusLimit)
        {
            return GetMultiStoreAPILink(geoPosition) + MultiStoreRadiusURLPart(radiusLimit);
        }

        public string GetMultiStoreAPILink(GeoCoordinate geoPosition)
        {
            return MultiStoreBaseURL() + MultiStoreGeoURLPart(geoPosition);
        }
        #endregion

        #region Page Count And Adjustment Functions
        public void NextMultiStorePage()
        {
            _pageCount++;
        }

        public void PreviousMultiStorePage()
        {
            _pageCount--;
        }

        public void SpecificMultiStorePage(int pageNumber)
        {
            _pageCount = pageNumber;
        }
        #endregion 
        #endregion

        #region Private Functions

        private string MultiStoreBaseURL()
        {
            return _multiStoreBaseLink + MultiStorePageURLPart();
        }

        private string MultiStorePageURLPart()
        {
            return _pageURL + _pageCount + _perPageURL + _perPageCount;
        }

        private string MultiStoreBrandURLPart(Brand brand)
        {
            return _brandURL + brand.ToString();
        }

        private string MultiStoreGeoURLPart(GeoCoordinate geoPosition)
        {
            _geoPosition = geoPosition;
            return _geoPositionURL + HttpUtility.UrlEncode(geoPosition.ToString());
        }

        private string MultiStoreRadiusURLPart(int radiusLimit)
        {
            Radius = radiusLimit;
            return _radiusURL + _radius;

        }
        private string GetZipURL(int zip)
        {
            Zip = zip;
            return _zipURL + _zip;
        }
        #endregion


    }
}
