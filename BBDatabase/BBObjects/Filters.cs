using System;
using System.Collections.Generic;
using System.Text;
using BBCollection;
using BBGatherer.StoreApi;

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
            List<Product> searchedProducts = dbConnect.GetProducts(searchterm);
            AppliedFiltersList keyWordFilters = new AppliedFiltersList(ToggleWordFilters, KeyWords);
            AppliedFiltersList storeNameFilters = new AppliedFiltersList(ToggleStoreFILters, StoreNames);

            do
            {

                FilteredList = StoreFilter(searchedProducts, storeNameFilters);



                if (keyWordFilters.AppliedFilters.Count != 0)
                {
                    FilteredList = keyWorkFilters(FilteredList, keyWordFilters);
                }


            } while (CheckList(FilteredList,out flag) || !SallingFlag || flag);

            return FilteredList;
        }
        private void SearchForProducts()
        {
            BearerAccessToken bearerAccessToken = new BearerAccessToken("fc5aefca-c70f-4e59-aaaa-1c4603607df8");


            SallingAPILink linkMaker = new SallingAPILink();
            SallingAPIProductSuggestions productSuggestions = new SallingAPIProductSuggestions();
            string apiLink = linkMaker.GetProductAPILink(Searchterm);
            OpenHttp<SallingAPIProductSuggestions> openHttp;


            openHttp = new OpenHttp<SallingAPIProductSuggestions>(apiLink, bearerAccessToken.GetBearerToken());
            productSuggestions = openHttp.ReadAndParseAPISingle();

        }
        private bool CheckList(List<Product> products, out bool flag) 
        {
            flag = true;
            return products.Count == 0 ? true : false;
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


