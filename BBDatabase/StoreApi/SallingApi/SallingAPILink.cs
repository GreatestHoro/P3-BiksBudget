using System;
using System.Web;

namespace BBCollection.StoreApi.SallingApi
{
    /// <summary>
    /// Enumeration to hold the brand/store name of the stores available
    /// </summary>
    public enum Brand { bilka, foetex, netto, salling, br, carlsjr, starbucks };
    /// <summary>
    /// The concrete class for a sallingAPILink of a APILink
    /// Has the needed methods and flexibilities to call their product API and store API through multiple ways
    /// </summary>
    public class SallingAPILink : APILink
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

        #region Constructors
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
        #endregion

        #region Public Functions
        /// <summary>
        /// Generates a salling api product search link
        /// </summary>
        /// <param name="searchWord">the product being search</param>
        /// <returns>returns a salling api product search link</returns>
        public string GetProductAPILink(string searchWord)
        {
            string encodedSearchWord = HttpUtility.UrlEncode(searchWord.ToLower());

            return _productSuggestionsBaseLink + encodedSearchWord;
        }
        /// <summary>
        /// Generates a salling api link to search for a single store by its id
        /// </summary>
        /// <param name="storeId">the specific salling stor ID</param>
        /// <returns>returns a salling api link to search for a single store by its id</returns>
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
            string cityEncoded = HttpUtility.UrlEncode(city.ToLower());
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
        /// <summary>
        /// Generates a link to search for multiple salling stores by geoPosition allowing to filter by radius and brand
        /// </summary>
        /// <param name="geoPosition">geoLocation of the user/position to search for stores from</param>
        /// <param name="radiusLimit">the max distance to store in km</param>
        /// <param name="brand">a salling store brand name to only return results matching the specific brand</param>
        /// <returns>a a link to search for multiple salling stores by geoPosition allowing to filter by radius and brand</returns>
        public string GetMultiStoreAPILink(GeoCoordinate geoPosition, int radiusLimit, Brand brand)
        {
            return GetMultiStoreAPILink(geoPosition, radiusLimit) + MultiStoreBrandURLPart(brand);
        }

        /// <summary>
        /// Generates a link to search for multiple salling stores by geoPosition allowing to filter by radius
        /// </summary>
        /// <param name="geoPosition">geoLocation of the user/position to search for stores from</param>
        /// <param name="radiusLimit">the max distance to store in km</param>
        /// <returns>a link to search for multiple salling stores by geoPosition allowing to filter by radius</returns>
        public string GetMultiStoreAPILink(GeoCoordinate geoPosition, int radiusLimit)
        {
            return GetMultiStoreAPILink(geoPosition) + MultiStoreRadiusURLPart(radiusLimit);
        }
        /// <summary>
        /// Generates a link to search for multiple salling stores by geoPosition by using the default filter of 5 km
        /// </summary>
        /// <param name="geoPosition">geoLocation of the user/position to search for stores from</param>
        /// <returns>a link to search for multiple salling stores by geoPosition allowing to filter by radius</returns>
        public string GetMultiStoreAPILink(GeoCoordinate geoPosition)
        {
            return MultiStoreBaseURL() + MultiStoreGeoURLPart(geoPosition);
        }
        #endregion

        #region Page Count And Adjustment Functions
        /// <summary>
        /// The functions in this region are for manipulating/adjusting the page being searched in the API
        /// </summary>
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
        /// <summary>
        /// The Functions below alow for various ways to filter/search for multiple stores such as by geoLocation, radius, city or zip number
        /// </summary>
        /// <returns>Each returns the filtering part of the sallin api link to search for salling store with its applied filters</returns>
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
            return _geoPositionURL + HttpUtility.UrlEncode(geoPosition.ToString().ToLower());
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
