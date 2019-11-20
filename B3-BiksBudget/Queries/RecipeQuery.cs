using BBCollection.BBObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;
using BBCollection.StoreApi.SallingApi;
using BBCollection.StoreApi;
using BBCollection.StoreApi.ApiNeeds;
using BBCollection;
using System.Threading.Tasks;

namespace BBGatherer.Queries
{
    public class RecipeQuery
    {
        DatabaseConnect _dbConnect = new DatabaseConnect("localhost", "BiksBudgetDB", "root", "BiksBudget123");
        BearerAccessToken bearerAccessToken = new BearerAccessToken("a6f4495c-ace4-4c39-805c-46071dd536db");
        SallingAPILink _linkMaker = new SallingAPILink();
        Filter<SallingAPIProduct> _productFilter = new Filter<SallingAPIProduct>();
        int _productsPerIngredient { get; set; } = 3;

        // Cheapest Complex Recipes
        public List<Recipe> CheapestCRecipes(string searchTerm)
        {
            //Get recipes matching searchTerm and Filters
            List<Recipe> recipeList = Recipes(searchTerm);

            //Make an array of distinct ingredients
            List<string> distinctIngredients = DistinctIngredients(recipeList);

            //Finc products matching to the ingredients
            //want to make it multithreaded later
            //Find x sallingAPIProducts per distinct ingredient
            Dictionary<string, List<SallingAPIProduct>> sallingProductsDict = SallingMatchingProducts(distinctIngredients, _productsPerIngredient);
            Hashtable sallingProductsHashtable = new Hashtable(sallingProductsDict);

            return recipeList;

        }

        private Dictionary<string, List<SallingAPIProduct>> SallingMatchingProducts(List<string> distinctIngredients, int _productsPerIngredient)
        {
            Dictionary<string, List<SallingAPIProduct>> resDictionary = new Dictionary<string, List<SallingAPIProduct>>();
            foreach(string ingredient in distinctIngredients)
            {
                resDictionary.Add(ingredient, SallingProducts(ingredient, _productsPerIngredient));
            }
            return resDictionary;
        }

        private List<SallingAPIProduct> SallingProducts(string ingredient, int _productsPerIngredient)
        {
            string apiProductLink = _linkMaker.GetProductAPILink(ingredient);
            OpenHttp<SallingAPIProductSuggestions> openHttp = new OpenHttp<SallingAPIProductSuggestions>(apiProductLink, bearerAccessToken.GetBearerToken());
            SallingAPIProductSuggestions allMatchingProducts = new SallingAPIProductSuggestions();
            allMatchingProducts = openHttp.ReadAndParseAPISingle();
         
            //sort by price | cheap --> expensive
            allMatchingProducts.Suggestions.Sort((a, b) => a.price.CompareTo(b.price));

            // return the _productsPerIngredient cheapest products
            return allMatchingProducts.Suggestions.Take(_productsPerIngredient).ToList();
        }

        private List<Recipe> Recipes(string searchTerm)
        {
            return _dbConnect.GetRecipes(searchTerm);
        }
        private List<string> DistinctIngredients(List<Recipe> recipeList)
        {
            List<string> resIngredients = new List<string>();

            foreach (Recipe recipe in recipeList)
            {
                //procject List of Ingredient objects to a list of their name
                //Add that range ^^ to resIngredients 
                resIngredients.AddRange(recipe._ingredientList.Select((ingredient) => ingredient._ingredientName));
            }

            // return distinct ingredients
            List<string> distinctIngredients = resIngredients.Distinct().ToList();
        
            return distinctIngredients;

        }
    }
}
