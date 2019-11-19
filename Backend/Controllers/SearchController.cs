using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BBCollection.BBObjects;
using BBGatherer.StoreApi;
using Newtonsoft.Json;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        // GET: api/Search/5
        [HttpGet("{searchterm}")]
        public string Get(string searchterm,string _keywordFilter,string _storeFilter)
        {
            bool[] keywordFilter = GetFilters(_keywordFilter);
            bool[] storeFilter = GetFilters(_storeFilter);
            Filters filters = new Filters(keywordFilter, storeFilter);


            
            return JsonConvert.SerializeObject(filters.UseTogglefilters(searchterm));
        }

        
        private List<Product> CheckDatabases() { return null;  }
        private List<Product> CheckSallingAPI() { return null; }


        public bool[] GetFilters(string filters)
        {
            char[] chars = filters.ToCharArray();
            bool[] results = new bool[filters.Length];
            int i = 0;
            foreach (char c in chars)
            {
                if (c.Equals("1"))
                {
                    results[i++] = true;
                }
                else
                {
                    results[i++] = false;
                }

            }
            return results;
        }
    }
}
