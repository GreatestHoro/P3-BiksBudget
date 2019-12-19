using BBCollection;
using BBCollection.BBObjects;
using BBCollection.StoreApi;
using BBCollection.StoreApi.SallingApi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace B3_BiksBudget.Webcrawler.Assisting_classes
{
    /// <summary>
    /// This class holds the funtionality of cross refrencing the ingredient names to attempt to determin wiche word inside the name is the actual ingrdient name.
    /// </summary>
    public class CrossRefrenceFuntionality
    {
        readonly DatabaseConnect dc = new DatabaseConnect();
        specialCombination _combo = new specialCombination();
        ProductRefrenceFuntionality _refs = new ProductRefrenceFuntionality();
        #region(Check if Indgredients)
        /// <summary>
        /// This method takes a string and splits it up in words and linguisticly combine it into new string, these string are then cross refrences with the database.
        /// </summary>
        /// <param name="name">The string going into the funtion</param>
        /// <param name="fatalError">bool that will be true if the name coundt find any matches</param>
        /// <returns>returns the longest of the succesful strings</returns>
        public String CheckForValidIndgredients(String name, out bool fatalError)
        {
            fatalError = false;
            List<string> matches = new List<string>();
            List<String> Combinations = _combo.GetAllCombinations(name);

            if (Combinations.Count != 0)
            {
                matches = CheckIngredientInDatabase(Combinations);
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
            if (matches.Count() == 0 && !fatalError)
            {
                fatalError = true;
            }

            return Combinations[Combinations.Count - 1];
        }

        /// <summary>
        /// Takes a string of searchterms check each for a result in the database if found add them to a return list
        /// </summary>
        /// <param name="Searchterms">A list of searchterms</param>
        /// <returns>retruns a list of string that only include the search terms that yielded a result in the database</returns>
        private List<string> CheckIngredientInDatabase(List<string> Searchterms)
        {
            List<string> results = new List<string>();
            foreach (string Searchterm in Searchterms)
            {
                if (CheckCOOPProductsInDatabase(Searchterm.Trim()))
                {
                    results.Add(Searchterm.Trim());
                }
            }

            return results;
        }
        /// <summary>
        /// Simple method that calls the database on a searchterms and update products custome ref field
        /// </summary>
        /// <param name="Searchterm"></param>
        /// <returns>returns bool based on wheter or not any products are found to match the serchterm in the database</returns>
        private bool CheckCOOPProductsInDatabase(String Searchterm)
        {
            string newRefrence;
            List<Product> ProductsWithRef = new List<Product>();
            if (!string.IsNullOrWhiteSpace(Searchterm))
            {
                ProductsWithRef = dc.Product.GetList(Searchterm);
                //Bjarkes secound name method
                foreach (Product p in ProductsWithRef)
                {
                    newRefrence = _refs.CheckRefrenceContent(p._CustomReferenceField);
                    newRefrence = _refs.UpdateProductRefrence(newRefrence, Searchterm);
                    newRefrence = _refs.InterpretAndEditProductRef(newRefrence);
                    dc.Product.AddReference(newRefrence, p._id);
                    //productImages.AssingItemImage(new RecepieProductHelper(p),"bing");
                }
            }
            else
            {
                return false;
            }

            return ProductsWithRef.Count != 0 ? true : false;
        }

        /// <summary>
        /// If a searchterms yields no results in the databae then this methos is used to check for results in the salling api(currently disabled because of technical issus on sallings side)
        /// </summary>
        /// <param name="Searchterm"></param>
        /// <returns>returns bool based on wheter or not any products are found to match the serchterm</returns>
        private bool CheckIngredientsInApi(string Searchterm)
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
