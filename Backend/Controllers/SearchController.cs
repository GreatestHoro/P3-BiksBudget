using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BBCollection.BBObjects;
using BBCollection.StoreApi;
using Newtonsoft.Json;


namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        Filters filters = new Filters();
        /// <summary>
        /// Returns a list of products based on the filters applied.
        /// </summary>
        /// <param name="searchterm">The word the user serches for</param>
        /// <param name="_keywordFilter">A string consisting of bool values for each keyword filter</param>
        /// <param name="_storeFilter">A string consisting of bool values for each store filter</param>
        /// <returns></returns> 

        // GET: api/Search?searchterm=øl&_keywordFilter=00&_storeFilter=111111111&_loadCount=0
        [HttpGet]
        public async Task<string> GetProducts(string searchterm = "carlsberg",string _keywordFilter = "00",string _storeFilter="111111111", int _loadCount = 0)
        {
            bool[] keywordFilter = GetFilters(_keywordFilter);
            bool[] storeFilter = GetFilters(_storeFilter);
            filters.UpdateFilters(keywordFilter, storeFilter);
            filters._loadCount = _loadCount;
            return JsonConvert.SerializeObject(await filters.UseTogglefilters(searchterm));
        }

        /// <summary>
        /// Transforms the string input, which consists of bool values, to a bool array.
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public bool[] GetFilters(string filters)
        {
            // converts the inputstring to a char array
            char[] chars = filters.ToCharArray();

            bool[] results = new bool[filters.Length];
            int i = 0;

            foreach (char c in chars)
            {
                if (c.Equals('1'))
                {
                    results[i++] = true;
                }
                else if (c.Equals('0'))
                {
                    results[i++] = false;
                }
                else 
                {
                    throw new SystemException("Error in generated link(was not 0 or 1)");
                }
            }
            return results;
        }
    }
}
