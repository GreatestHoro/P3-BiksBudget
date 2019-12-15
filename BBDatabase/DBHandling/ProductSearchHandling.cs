using BBCollection.BBObjects;
using BBCollection.DBHandling;
using BBCollection.Queries;
using Newtonsoft.Json;
using Blazorise.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Net.Http;
using Microsoft.AspNetCore.Components;

namespace BBCollection.DBHandling
{
    public class ProductSearchHandling
    {
        
        ProductHandling ph = new ProductHandling();
        List<Product> itemList = new List<Product>();
        HttpClient Http = new HttpClient();
        SortBy sort = new SortBy();
        string[] prodName;
        string productString;
        string tempSearchword = "";
        int _loadCount = 0;


        #region Autocomplete
        public async Task<string[]> InitializeAutocorrect(bool[] enabledKeywords, bool[] enabledStores)
        {
            itemList = await CallApiForProducts(tempSearchword, enabledKeywords, enabledStores);

            prodName = await ph.AddRefCol();

            return prodName;
        }

        public IEnumerable<AutocompleteSearch> InitializeRecommender(string[] prodName)
        {
            return Enumerable.Range(1, prodName.Length)
            .Select(x => new AutocompleteSearch { textField = prodName[x - 1], valueField = x });
        }

        public bool[] SetBoolArray(List<FilterItem> filterList)
        {
            return filterList.Select(x => x.IsEnabled).ToArray();
        }

        #endregion

        public async Task<List<Product>> CallApiForProducts(string searchTerm, bool[] keywordArray, bool[] storeArray)
        {
            ProductSearchLinkConstructer link = new ProductSearchLinkConstructer(searchTerm, ConvertBoolArrToStr(keywordArray), ConvertBoolArrToStr(storeArray), _loadCount);

            productString = await Http.GetStringAsync(link.GetURL());

            return JsonConvert.DeserializeObject<List<Product>>(productString);
        }

         
        #region Filters

        /// <summary>
        /// When the api is called, the url is created. The url cannot take a bool array as input
        /// and therefore this array is converted to a string of 1 and 0.
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public string ConvertBoolArrToStr(bool[] arr)
        {
            char[] returnChar = new char[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                returnChar[i] = arr[i] ? '1' : '0';
            }
            return new string(returnChar);
        }

        public List<Product> DecideOrderFilter(int filterName, List<Product> itemList)
        {
            switch (filterName)
            {
                case (int)SortNames.Relevance:
                    itemList = sort.SortByMostRelevant(itemList);
                    break;
                case (int)SortNames.LowestPrice:
                    itemList = sort.SortByPriceLH(itemList);
                    break;
                case (int)SortNames.HeigestPrice:
                    itemList = sort.SortBypriceHL(itemList);
                    break;
                case (int)SortNames.ProductAZ:
                    itemList = sort.SortByNameAZ(itemList);
                    break;
                case (int)SortNames.ProductZA:
                    itemList = sort.SortByNameZA(itemList);
                    break;
                case (int)SortNames.StoreAZ:
                    itemList = sort.SortByStoreAZ(itemList);
                    break;
                case (int)SortNames.StoreZA:
                    itemList = sort.SortByStoreZA(itemList);
                    break;
            }

            return itemList;
        }
        #endregion
    }
}
