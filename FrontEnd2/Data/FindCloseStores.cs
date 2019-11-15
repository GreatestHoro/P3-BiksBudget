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
        public FindCloseStores(IpItem _ipItem, int _radius)
        {
            latitude = _ipItem.latitude;
            longitude = _ipItem.longitude;
            radius = _radius;
        }

        public CoopStoreApi coopStores = new CoopStoreApi();
        public int radius;
        public double latitude;
        public double longitude;
        public string url;
        public string token = "d0b9a5266a2749cda99d4468319b6d9f";

        public CoopStoreApi GetStore()
        {
            GeoCoordinate geo = new GeoCoordinate(longitude, latitude);
            CoopDoStuff FindStores = new CoopDoStuff(token, geo, radius);

            coopStores = FindStores.CoopCloseStore();
            
            return coopStores;
        }

    }
}
