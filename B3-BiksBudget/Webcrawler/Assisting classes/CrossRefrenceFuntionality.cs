using BBCollection;
using BBCollection.BBObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using BBCollection.StoreApi;
using BBCollection.StoreApi.SallingApi;
using BBCollection.DBHandling;
using BBCollection.DBConncetion;


namespace B3_BiksBudget.Webcrawler.Assisting_classes
{
    class CrossRefrenceFuntionality
    {
        readonly DatabaseConnect dc = new DatabaseConnect();
        specialCombination _combo = new specialCombination();
        ProductRefrenceFuntionality _refs = new ProductRefrenceFuntionality();
        #region(Check if Indgredients)
        public String CheckForValidIndgredients(String name, DatabaseConnect dbConnect, out bool fatalError)
        {
            fatalError = false;
            List<string> matches = new List<string>();
            List<String> Combinations = _combo.GetAllCombinations(name, dbConnect);

            if (Combinations.Count != 0)
            {
                matches = CheckIngredientInDatabase(Combinations, dbConnect);
                /*if (matches.Count == 0)
                {
                    foreach (string Searchterm in Combinations)
                    {
                        if (CheckIngredientsInApi(Searchterm, dbConnect))
                        {
                            matches = (CheckIngredientInDatabase(Combinations, dbConnect));
                        }
                    }
                }*/
            }

            //lav en metode til at gemme det afviste svar
            if (matches.Count() == 0 && !fatalError)
            {
                fatalError = true;
            }

            return Combinations[Combinations.Count - 1];
        }

        
        private List<string> CheckIngredientInDatabase(List<string> Searchterms, DatabaseConnect dbConnect)
        {
            List<string> results = new List<string>();
            foreach (string Searchterm in Searchterms)
            {
                if (CheckCOOPProductsInDatabase(Searchterm.Trim(), dbConnect))
                {
                    results.Add(Searchterm.Trim());
                }
            }

            return results;
        }

        private bool CheckCOOPProductsInDatabase(String Searchterm, DatabaseConnect dbConnect)
        {
            string newRefrence;
            ProductHandling productHandling = new ProductHandling();
            List<Product> ProductsWithRef = _refs.GetProductWithRef(Searchterm, dbConnect);
            foreach (Product p in ProductsWithRef)
            {
                newRefrence = _refs.UpdateProductRefrence(p._CustomReferenceField.Trim(), Searchterm);
                newRefrence = _refs.InterpretAndEditProductRef(newRefrence);
                dc.Product.AddReference(newRefrence, p._id);
            }

            return ProductsWithRef.Count != 0 ? true : false;
        }
 

        private bool CheckIngredientsInApi(string Searchterm, DatabaseConnect dbConnect)
        {
            System.Threading.Thread.Sleep(4000);
            BearerAccessToken bearerAccessToken = new BearerAccessToken("fc5aefca-c70f-4e59-aaaa-1c4603607df8");


            SallingAPILink linkMaker = new SallingAPILink();
            SallingAPIProductSuggestions productSuggestions = new SallingAPIProductSuggestions();
            string apiLink = linkMaker.GetProductAPILink(Searchterm);
            OpenHttp<SallingAPIProductSuggestions> openHttp;


            try
            {
                openHttp = new OpenHttp<SallingAPIProductSuggestions>(apiLink, bearerAccessToken.GetBearerToken());
                productSuggestions = openHttp.ReadAndParseAPISingle();
                if (productSuggestions.Suggestions != null)
                {
                    foreach (var p in productSuggestions.Suggestions)
                    {
                        dc.Product.Add(new Product(p.title, p.id, p.prod_id, p.price, p.description, p.link, p.img));
                    }
                }

            }

            catch (System.Net.WebException)
            {
                Console.WriteLine("Exception found is being handled");
                System.Threading.Thread.Sleep(30000);
                Console.WriteLine("Exception resolved");
                return false;
            }

            if (productSuggestions.Suggestions != null)
            {
                return productSuggestions.Suggestions.Count != 0 ? true : false;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
