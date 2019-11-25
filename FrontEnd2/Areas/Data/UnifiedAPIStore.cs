using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;


namespace FrontEnd2.Data
{
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
        public string LogoURL(string brandName)
        {
            string processedBrandName = brandName.Replace("'", "");
            StoreBrand storeBrand;
            int storeBrandID;

            if (Enum.TryParse(processedBrandName, out storeBrand))
            {
                storeBrandID = (int)storeBrand;
            } else
            {
                return "";
            }           
            
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
    public enum StoreChain 
    { 
        sallingChain, 
        coopChain,
    }

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
