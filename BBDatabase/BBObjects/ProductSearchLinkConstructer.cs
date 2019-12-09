using BBCollection.DBConncetion;
using System;
using System.Web;

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
