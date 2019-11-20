using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace BBCollection.BBObjects
{
    public class ProductSearchLinkConstructer
    {
        private string url;
        public ProductSearchLinkConstructer(String search, String keywordFilter, String storeFilter) 
        {
            search = HttpUtility.UrlEncode(search);
            url = $"https://localhost:44325/api/Search?searchterm={search}&_keywordFilter={keywordFilter}&_storeFilter={storeFilter}";
            
        }

        public string GetURL() 
        {
            return url;
        }

        
    }
}
