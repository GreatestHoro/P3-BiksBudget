using System;
using System.Collections.Generic;
using System.Text;
using BBCollection;
using BBCollection.StoreApi;
using BBCollection.StoreApi.ApiNeeds;
using BBCollection.StoreApi.SallingApi;
using BBCollection.DBHandling;
using BBCollection.DBConncetion;

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
        private DatabaseConnect dbConnect = new DatabaseConnect("localhost", "biksbudgetDB", "root", "BiksBudget123");

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
#pragma warning disable CS0219 // The variable 'SallingFlag' is assigned but its value is never used
            bool SallingFlag = false;
#pragma warning restore CS0219 // The variable 'SallingFlag' is assigned but its value is never used
            bool flag = false;
            int i = 0;
            List<Product> searchedProducts = dbConnect.GetProducts(searchterm);
            AppliedFiltersList keyWordFilters = new AppliedFiltersList(ToggleWordFilters, wordArray);
            AppliedFiltersList storeNameFilters = new AppliedFiltersList(ToggleStoreFILters, storeArray);

            do
            {
                if (flag) 
                {
                    FilteredList = SearchForProducts(searchterm);
                    flag = false;
                }
                FilteredList = StoreFilter(searchedProducts, storeNameFilters);

                if (keyWordFilters.AppliedFilters.Count != 0)
                {
                    FilteredList = keyWorkFilters(FilteredList, keyWordFilters);
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

            openHttp = new OpenHttp<SallingAPIProductSuggestions>(apiLink, bearerAccessToken.GetBearerToken());
            productSuggestions = openHttp.ReadAndParseAPISingle();

            foreach (SallingAPIProduct products in productSuggestions.Suggestions)
            {
                returnList.Add(new Product(products.id,products.title,products.description,products.price,products.img,"Bilka"));
            }
            return returnList;
        }
        private bool CheckList(List<Product> products, out bool flag) 
        {
            flag = true;
            return products.Count != 0 ? true : false;
        }
        private List<Product> StoreFilter(List<Product> products, AppliedFiltersList stores)
        {
#pragma warning disable CS0219 // The variable 'i' is assigned but its value is never used
            int i = 0;
#pragma warning restore CS0219 // The variable 'i' is assigned but its value is never used
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

        private List<Product> keyWorkFilters(List<Product> products, AppliedFiltersList keywords)
        {
            List<Product> returnProducts = new List<Product>();
            foreach (Product p in products)
            {
                foreach (AppliedFilters a in keywords.AppliedFilters)
                {
                    if (p._productName.Contains(a.name) || p._amount.Contains(a.name))
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

        public AppliedFiltersList(bool[] filterApplied, FilterItem[] filterName)
        {
            if (filterApplied.Length == filterName.Length)
            {
                for (int i = 0; i < filterName.Length; i++)
                {
                    if (filterApplied[i])
                    {
                        AppliedFilters.Add(new AppliedFilters(filterName[i].FilterName));
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


