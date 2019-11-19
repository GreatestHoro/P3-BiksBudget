using System;
using System.Collections.Generic;
using System.Text;
using BBCollection;
using BBCollection.StoreApi;
using BBCollection.StoreApi.ApiNeeds;
using BBCollection.StoreApi.SallingApi;

namespace BBCollection.BBObjects
{
    public class Filters
    {
        bool[] ToggleWordFilters;
        String[] KeyWords = new string[] { "ØKO", "GLUTENFRI" };
        string[] StoreNames = new string[] {
        "SuperBrugsen",
        "Irma",
        "Kvicly",
        "DagligBrugsen",
        "Bilka",
        "Netto",
        "Føtex",
        "Saling",
        "Fakta"
        };
        bool[] ToggleStoreFILters;
        string[] blacklist;
        private DatabaseConnect dbConnect = new DatabaseConnect("localhost", "biksbudgetDB", "root", "BiksBudget123");

        public Filters(bool[] _ToggleWordFilters, bool[] _ToggleStoreFILters)
        {
            ToggleWordFilters = _ToggleWordFilters;
            ToggleStoreFILters = _ToggleStoreFILters;
        }

        public List<Product> UseTogglefilters(string searchterm)
        {
            List<Product> FilteredList = new List<Product>();
            bool SallingFlag = false;
            bool flag = false;
            int i = 0;
            List<Product> searchedProducts = dbConnect.GetProducts(searchterm);
            AppliedFiltersList keyWordFilters = new AppliedFiltersList(ToggleWordFilters, KeyWords);
            AppliedFiltersList storeNameFilters = new AppliedFiltersList(ToggleStoreFILters, StoreNames);

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
            int i = 0;
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

                //stores.AppliedFilters[i].name.Equals((products.ToArray())[i]._productName);


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

        public AppliedFiltersList(bool[] filterApplied, String[] filterName)
        {
            foreach (var s in filterName)
            {
                foreach (var b in filterApplied)
                {
                    if (b)
                    {
                        AppliedFilters.Add(new AppliedFilters(s));
                    }
                }
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


