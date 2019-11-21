using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using System.Web;
using Json.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using FrontEnd2.Data;

namespace FrontEnd2.Data
{
    public class FindCloseStores
    {
        public FindCloseStores(GeoCoordinate geo, int _radius)
        {
            latitude = geo._latitude;
            longitude = geo._longitude;
            radius = _radius;
            RadiusInKM = _radius / 1000;
        }

        public CoopStoreApi coopStores = new CoopStoreApi();
        public int radius;
        public double latitude;
        public double longitude;
        public string url;
        //public string token = "d0b9a5266a2749cda99d4468319b6d9f";
        BearerAccessToken bearerAccessToken = new BearerAccessToken("a6f4495c-ace4-4c39-805c-46071dd536db");
        List<UnifiedAPIStore> _unifiedAPIStores = new List<UnifiedAPIStore>();
        public string token = "f0cabde6bb8d4bd78c28270ee203253f";
        
        public int RadiusInKM {
            get => _radiusInKM;
            set
            {
                if ((radius / 1000 < 1))
                {
                    _radiusInKM = 1;
                }
                else
                {
                    _radiusInKM = radius / 1000; 
                }
            }
        }
        int _radiusInKM;


        public List<UnifiedAPIStore> GetStore()
        {
            GeoCoordinate geo = new GeoCoordinate(longitude, latitude);
            CoopDoStuff FindStores = new CoopDoStuff(token, geo, radius);

            coopStores = FindStores.CoopCloseStore();

            List<UnifiedAPIStore> unifiedCoopStores = UnifiyCoopStores(coopStores);

            if (radius != 0)
            {
                List<UnifiedAPIStore> unifiedSallingStores = GetUnifiedSallingStores();
                _unifiedAPIStores.AddRange(unifiedSallingStores);
            }

            _unifiedAPIStores.AddRange(unifiedCoopStores);
            

            return _unifiedAPIStores;
        }

        // get a CoopAPIStore and turn it into a list of unifiedstores
        private List<UnifiedAPIStore> UnifiyCoopStores(CoopStoreApi coopStores)
        {
            List<UnifiedAPIStore> unifiedCoopStores = (from store in coopStores.Data
                                                          select new UnifiedAPIStore(store.RetailGroupName, store.RetailGroupName,
                                                          new GeoCoordinate(store.Location.coordinates[0], store.Location.coordinates[1]),
                                                          store.Zipcode.ToString(), store.City, store.Address, StoreChain.coopChain)).ToList();

            return unifiedCoopStores;

        }

        private List<UnifiedAPIStore> GetUnifiedSallingStores()
        {
            SallingAPILink linkMaker = new SallingAPILink();
            GeoCoordinate geoCoordinate = new GeoCoordinate(latitude, longitude);
            int radiusInKM = radius / 1000;
            string apiLink = linkMaker.GetMultiStoreAPILink(geoCoordinate, RadiusInKM);
            List<SallingAPIStore> sallingStores = new List<SallingAPIStore>();

            OpenHttp<SallingAPIStore> sallingStoreHttp = new OpenHttp<SallingAPIStore>(apiLink, bearerAccessToken.GetBearerToken());
            try
            {
                sallingStores = sallingStoreHttp.ReadAndParseAPI();
            }
            catch (ArgumentNullException e)
            {
                // No Salling Stores Nearby
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            // Turn Salling Stores into UnifiedAPIStore
            List<UnifiedAPIStore> unifiedSallingStores = (from store in sallingStores 
                                       select new  UnifiedAPIStore(store.name, store.brand,
                                       new GeoCoordinate(store.coordinates[0], store.coordinates[1]),
                                       store.address.zip, store.address.city, store.address.ToString(), StoreChain.sallingChain)).ToList();

            return unifiedSallingStores;

        }
    }
}
