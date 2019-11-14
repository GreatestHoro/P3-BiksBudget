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
using IpData;

namespace IP.Data
{
    

    class GetIP
    {
        public IpItem ipItem = new IpItem();

        string key = "3adf07b57437481add9e64472cc055f5";
        string url = "http://api.ipstack.com/check?access_key=";


        public IpItem GetIpData()
        {
            var request = WebRequest.Create(url + key);

            using (WebResponse wrs = request.GetResponse())
            using (Stream stream = wrs.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                var obj = JObject.Parse(json);
            }



            return ipItem;
        }
    }
}
