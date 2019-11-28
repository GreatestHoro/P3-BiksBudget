using System;
using System.Collections.Generic;
using System.Text;
using BBCollection;
using BBCollection.StoreApi;
using BBCollection.StoreApi.ApiNeeds;
using BBCollection.StoreApi.SallingApi;
using BBCollection.DBHandling;
using BBCollection.DBConncetion;
using System.Net;

namespace BBCollection.BBObjects
{
    public class Filters
    {   
        StoreFilterList stores = new StoreFilterList();
        WordFilterList keyWords = new WordFilterList();
        bool[] ToggleWordFilters;
        bool[] ToggleStoreFILters;
        FilterItem[] storeArray;
        FilterItem[] wordArray;
        private DatabaseConnect dc = new DatabaseConnect();

        public Filters(bool[] _ToggleWordFilters, bool[] _ToggleStoreFILters)
        {
            ToggleWordFilters = _ToggleWordFilters;
            ToggleStoreFILters = _ToggleStoreFILters;
            storeArray = stores.GetStoreArray();
            wordArray = keyWords.GetWordArray();
        }

        public List<Product> UseTogglefilters(string searchterm)
        {
            List<Product> FilteredList = new List<Product>();
            bool flag = false;
            int i = 0;
            List<Product> searchedProducts = dc.Product.GetList(searchterm);
            AppliedFiltersList keyWordFilters = new AppliedFiltersList(ToggleWordFilters, wordArray);
            AppliedFiltersList storeNameFilters = new AppliedFiltersList(ToggleStoreFILters, storeArray);

            do
            {
                if (flag && IsBilkTrue(storeNameFilters))
                {
                    FilteredList = SearchForProducts(searchterm);
                    flag = false;
                }
                FilteredList = StoreFilter(searchedProducts, storeNameFilters);

                if (keyWordFilters.AppliedFilters.Count != 0)
                {
                    FilteredList = KeyWorkFilters(FilteredList, keyWordFilters);
                }
                if (CheckList(FilteredList, out flag)) 
                {
                    break;
                }
            } while (i++ <=2);

            return FilteredList;
        }

        private List<Product> SearchForProducts(String searchterm)
        {
            BearerAccessToken bearerAccessToken = new BearerAccessToken("fc5aefca-c70f-4e59-aaaa-1c4603607df8");
            SallingAPILink linkMaker = new SallingAPILink();
            SallingAPIProductSuggestions productSuggestions = new SallingAPIProductSuggestions();
            string apiLink = linkMaker.GetProductAPILink(searchterm);
            OpenHttp<SallingAPIProductSuggestions> openHttp;
            List<Product> returnList = new List<Product>();
            try
            {
                openHttp = new OpenHttp<SallingAPIProductSuggestions>(apiLink, bearerAccessToken.GetBearerToken());
                productSuggestions = openHttp.ReadAndParseAPISingle();

                if (productSuggestions.Suggestions.Count != 0)
                {
                    foreach (SallingAPIProduct products in productSuggestions.Suggestions)
                    {
                        returnList.Add(new Product(products.id, products.title, products.description, products.price, products.img, "Bilka"));
                    }
                }
            }
            catch (WebException e)
            {
                Console.WriteLine("This program is expected to throw WebException on successful run." +
                                  "\n\nException Message :" + e.Message);
            }
            
            return returnList;
        }
        private bool CheckList(List<Product> products, out bool flag) 
        {
            flag = true;
            return (products.Count != 0) ? true : false;
        }

        private List<Product> StoreFilter(List<Product> products, AppliedFiltersList stores)
        {
            List<Product> returnProducts = new List<Product>();
            foreach (Product p in products)
            {
                foreach (AppliedFilters a in stores.AppliedFilters)
                {
                    if (p._storeName.Equals(a.name))
                    {
                        returnProducts.Add(p);
                        break;
                    }
                }
            }
            return returnProducts;
        }

        private bool IsBilkTrue(AppliedFiltersList stores)
        {
            foreach (var item in stores.AppliedFilters)
            {
                if (item.name == "Bilka")
                {
                    return true;
                }
            }
            return false;
        }

        private List<Product> KeyWorkFilters(List<Product> products, AppliedFiltersList keywords)
        {
            List<Product> returnProducts = new List<Product>();
            foreach (Product p in products)
            {
                foreach (AppliedFilters a in keywords.AppliedFilters)
                {
                    if (p._productName.ToLower().Contains(a.name) || p._amount.ToLower().Contains(a.name))
                    {
                        returnProducts.Add(p);
                        break;
                    }
                }
            }
            return returnProducts;
        }
    }

    class AppliedFiltersList
    {
        public List<AppliedFilters> AppliedFilters = new List<AppliedFilters>();
        string nameToAdd;

        public AppliedFiltersList(bool[] filterApplied, FilterItem[] filterName)
        {
            if (filterApplied.Length == filterName.Length)
            {
                for (int i = 0; i < filterName.Length; i++)
                {
                    if (filterApplied[i])
                    {
                        nameToAdd = String.IsNullOrEmpty(filterName[i].SearchName) ? filterName[i].FilterName : filterName[i].SearchName;
                        AppliedFilters.Add(new AppliedFilters(nameToAdd));
                    }
                }
            }
            else 
            {
                throw new SystemException("bools and the length of options were not equal");
            }
        }

        public AppliedFiltersList(String[] filterName)
        {
            foreach (var s in filterName)
            {
                AppliedFilters.Add(new AppliedFilters(s));
            }
        }
    }

    class AppliedFilters
    {
        public string name { get; set; }

        public AppliedFilters(string name)
        {
            this.name = name;
        }
    }
}


