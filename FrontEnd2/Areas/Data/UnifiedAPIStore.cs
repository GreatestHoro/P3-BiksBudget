using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;


namespace FrontEnd2.Data
{
    /// <summary>
    /// Class to combine coop and salling store object to 1 type by the member fields in the class below
    /// </summary>
    public class UnifiedAPIStore
    {
        public string _storeName { get; set; }
        public string _brand { get; set; }
        public GeoCoordinate _storeLocation = new GeoCoordinate(0, 0);
        public string _zip { get; set; }
        public string _city { get; set; }
        public string _address { get; set; }
        public int _chainID { get; set; }
        public string _logoURL { get; set; }
        
        public UnifiedAPIStore(string storename, string brand, GeoCoordinate geoCoordinate, string zip, string city, string address, StoreChain storeChain)
        {
            _storeName = storename;
            _brand = brand;
            _storeLocation._latitude = geoCoordinate._latitude;
            _storeLocation._longitude = geoCoordinate._longitude;
            _zip = zip;
            _city = city;
            _address = address;
            _chainID = (int)storeChain;
            _logoURL = LogoURL(brand);
            
        }

        /// <summary>
        /// gets the path to the logo given a brand
        /// </summary>
        /// <param name="brandName">name of a store brand</param>
        /// <returns>a path to the logo given a brand</returns>
        public string LogoURL(string brandName)
        {
            // removes redundant characters from the brand name, so it matches the Brand enum
            string processedBrandName = brandName.Replace("'", "");
            StoreBrand storeBrand;
            int storeBrandID;

            // try to pass the brand name string to a Brand enum
            if (Enum.TryParse(processedBrandName, out storeBrand))
            {
                storeBrandID = (int)storeBrand;
            } else
            {
                return "";
            }           
            
            // checks and returns the relevant path
            switch (storeBrandID)
            {
                case (int)StoreBrand.bilka:
                    return "/Pictures/salling-store-logos/bilka.png";
                case (int)StoreBrand.foetex:
                    return "/Pictures/salling-store-logos/foetex.png";
                case (int)StoreBrand.netto:
                    return "/Pictures/salling-store-logos/netto.png";
                case (int)StoreBrand.salling:
                    return "/Pictures/salling-store-logos/salling.png";
                case (int)StoreBrand.Brugsen:
                    return "/Pictures/coop-store-logos/brugsen.png";
                case (int)StoreBrand.DagliBrugsen:
                    return "/Pictures/coop-store-logos/daglibrugsen.png";
                case (int)StoreBrand.Fakta:
                    return "/Pictures/coop-store-logos/fakta.png";
                case (int)StoreBrand.Kvickly:
                    return "/Pictures/coop-store-logos/kvickly.png";
                case (int)StoreBrand.SuperBrugsen:
                    return "/Pictures/coop-store-logos/superbrugsen.png";
                default:
                    return "";
            }
        }
    }
    /// <summary>
    /// enum to check which store chain it is
    /// </summary>
    public enum StoreChain 
    { 
        sallingChain, 
        coopChain,
    }

    /// <summary>
    /// enum to check the brand name
    /// </summary>
    public enum StoreBrand {
        Kvickly,
        SuperBrugsen,
        DagliBrugsen,
        Brugsen,
        Fakta,
        foetex,
        netto,
        salling,
        bilka,
    }

}
