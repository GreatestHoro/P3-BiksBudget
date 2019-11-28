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
        private DatabaseConnect dbConnect = new DatabaseConnect();

        public Filters(bool[] _ToggleWordFilters, bool[] _ToggleStoreFILters)
        {
            ToggleWordFilters = _ToggleWordFilters;
            ToggleStoreFILters = _ToggleStoreFILters;
            storeArray = stores.GetStoreArray();
            wordArray = keyWords.GetWordArray();
        }

        /// <summary>
        /// Find the products who fit the filters applied, and searchterm
        /// </summary>
        /// <param name="searchterm"></param>
        /// <returns></returns>
        public List<Product> UseTogglefilters(string searchterm)
        {
            List<Product> FilteredProductList = new List<Product>();
            bool flag = false;
            int i = 0;

            // All products in the database who match on the searchterm are loaded in
            List<Product> searchedProducts = dbConnect.GetProducts(searchterm);

            // Returns a list of strings of the storenames of stores enabled
            AppliedFiltersList keyWordFilters = new AppliedFiltersList(ToggleWordFilters, wordArray);

            // Returns a list of string of the keywords enabled
            AppliedFiltersList storeNameFilters = new AppliedFiltersList(ToggleStoreFILters, storeArray);

            //This loop always runs two times. One time is to get all products fitting form the database.
            // The next run is if no results are found, and the Salling api is called to ses if any
            // matches can be found there.
            do
            {
                if (flag && IsBilkTrue(storeNameFilters))
                {
                    // Is called if there are returned no products fitting to the filters
                    // and if bilka is turned on.
                    // It calls the Salling API (Bilka2go) and retuns a list of products.
                    FilteredProductList = SearchForProducts(searchterm);
                    flag = false;
                }

                // Returns all products who are in stores enabled
                FilteredProductList = StoreFilter(searchedProducts, storeNameFilters); 

                if (keyWordFilters.AppliedFilters.Count != 0)
                {
                    // If there are any keyword filters enabled, this returns the products 
                    FilteredProductList = keyWorkFilters(FilteredProductList, keyWordFilters);
                }

                if (CheckList(FilteredProductList, out flag)) 
                {
                    // If the FilteredProductList contains products, it breaks.
                    // Otherwise the loop runs again to search in the Salling api.
                    break;
                }
            } while (i++ <=2);
            return FilteredProductList;
        }

        private List<Product> SearchForProducts(String searchterm)
        {
            BearerAccessToken bearerAccessToken = new BearerAccessToken("fc5aefca-c70f-4e59-aaaa-1c4603607df8");
            SallingAPILink linkMaker = new SallingAPILink();
            SallingAPIProductSuggestions productSuggestions = new SallingAPIProductSuggestions();

            // Creates a link to the Salling api with the searchterm as input
            string apiLink = linkMaker.GetProductAPILink(searchterm);
            OpenHttp<SallingAPIProductSuggestions> openHttp;
            List<Product> returnList = new List<Product>();
            try
            {
                // Opens the Salling API
                openHttp = new OpenHttp<SallingAPIProductSuggestions>(apiLink, bearerAccessToken.GetBearerToken());

                // Parsese the returned Json string into a list of SallingAPIProduct
                productSuggestions = openHttp.ReadAndParseAPISingle();

                if (productSuggestions.Suggestions.Count != 0)
                {
                    foreach (SallingAPIProduct products in productSuggestions.Suggestions)
                    {
                        // Adds all SallingAPIProduct to the returnList and converts them all to the class Product
                        returnList.Add(new Product(products.id, products.title, products.description, products.price, products.img, "Bilka"));
                    }
                }
            }
            catch (WebException e)
            {
                // If there is an error while loading the Salling api.
                Console.WriteLine("This program is expected to throw WebException on successful run." +
                                  "\n\nException Message :" + e.Message);
            }
            
            return returnList;
        }

        /// <summary>
        /// This method decides whether or not the loop should run again.
        /// If the list is empty it runs again, if there are products
        /// it breaks.
        /// </summary>
        /// <param name="products"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        private bool CheckList(List<Product> products, out bool flag) 
        {
            flag = true;
            return (products.Count != 0) ? true : false;
        }

        /// <summary>
        /// This method looks at where all the products coms from.
        /// If they are from a store in the stores input, they
        /// are returned.
        /// </summary>
        /// <param name="products"></param>
        /// <param name="stores">The list of storesnames which are enabled in the filters</param>
        /// <returns></returns>
        private List<Product> StoreFilter(List<Product> products, AppliedFiltersList stores)
        {
            List<Product> returnProducts = new List<Product>();
            foreach (Product p in products)
            {
                foreach (AppliedFilters a in stores.AppliedFilters)
                {
                    if (p._storeName.Equals(a.name))
                    {
                        // If the product checked is from one of the stores enabled,
                        // it is added to the returned list
                        returnProducts.Add(p);
                        break;
                    }
                }
            }
            return returnProducts;
        }


        /// <summary>
        /// This method checks whether or nort Bilka is enabled.
        /// If it is, the Salling api will be called
        /// </summary>
        /// <param name="stores">The list of enabled stores names</param>
        /// <returns></returns>
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


        /// <summary>
        /// This method checks if the keyword is included in the product
        /// If it is, it is included in the returnd list.
        /// </summary>
        /// <param name="products"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        private List<Product> keyWorkFilters(List<Product> products, AppliedFiltersList keywords)
        {
            List<Product> returnProducts = new List<Product>();
            foreach (Product p in products)
            {
                foreach (AppliedFilters a in keywords.AppliedFilters)
                {
                    if (p._productName.ToLower().Contains(a.name) || p._amount.ToLower().Contains(a.name))
                    {
                        // If the keyword is in either the _amout (used for descirption sometimes in the api)
                        // or the _productname it is added.
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
        // A list of strings, used to contain either the keyword filters, or storenames.
        public List<AppliedFilters> AppliedFilters = new List<AppliedFilters>();
        string nameToAdd;

        /// <summary>
        /// The constructor to the AppliedFIltersList class. The class contains a list of AppliedFilters
        /// which is a string. The list will be filled with the names of each Filteritem if it is 
        /// enabled by the user.
        /// </summary>
        /// <param name="filterApplied">An array of bools to indicate each filter</param>
        /// <param name="filterName">An array of FilterItems, containing the filtername, and whether or not it is enabled</param>
        public AppliedFiltersList(bool[] filterApplied, FilterItem[] filterName)
        {
            if (filterApplied.Length == filterName.Length)
            {
                for (int i = 0; i < filterName.Length; i++)
                {
                    if (filterApplied[i])
                    {
                        // If the bool is true, name of the matching index should be saved

                        // The FilterItem class contains a SerchName attribute. This is used if the filter has a different name
                        // than what should be searhed for in the Products.
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


