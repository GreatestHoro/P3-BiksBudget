using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using BBCollection.DBConncetion;

namespace BBCollection.BBObjects
{
    public class ProductSearchLinkConstructer
    {
        ConnectionSettings connectionSettings = new ConnectionSettings();
        private string url;
        public ProductSearchLinkConstructer(String search, String keywordFilter, String storeFilter, int loadCount) 
        {
            search = HttpUtility.UrlEncode(search);
            url = connectionSettings.GetApiLink() + $"api/Search?searchterm={search}&_keywordFilter={keywordFilter}&_storeFilter={storeFilter}&_loadCount={loadCount}";
            
        }

        public string GetURL() 
        {
            return url;
        }

        
    }
}
