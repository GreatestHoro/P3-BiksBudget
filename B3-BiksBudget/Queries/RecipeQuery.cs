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
using B3_BiksBudget.BBGatherer.Queries;

namespace BBGatherer.Queries
{
    public class RecipeQuery
    {
        DatabaseConnect _dbConnect = new DatabaseConnect();
        BearerAccessToken bearerAccessToken = new BearerAccessToken("a6f4495c-ace4-4c39-805c-46071dd536db");
        SallingAPILink _linkMaker = new SallingAPILink();
        Filter<SallingAPIProduct> _productFilter = new Filter<SallingAPIProduct>();
        int _productsPerIngredient { get; set; } = 3;

        // Cheapest Complex Recipes
        public List<ComplexRecipe> CheapestCRecipes(string searchTerm)
        {
            //Get recipes matching searchTerm and Filters
            List<Recipe> recipeList = Recipes(searchTerm);

            //Make an array of distinct ingredients
            List<string> distinctIngredients = DistinctIngredients(recipeList);

            //Finc products matching to the ingredients
            //want to make it multithreaded later
            //Find x sallingAPIProducts per distinct ingredient
            Dictionary<string, List<Product>> productsDict = MatchingProducts(distinctIngredients);
            Hashtable productsHashtable = new Hashtable(productsDict);

            List<ComplexRecipe> resultComplexRecipes = new List<ComplexRecipe>();

            //calculate the price of each recipe, and creat a list of ComplexRecipe objects
            resultComplexRecipes = (from recipe in recipeList select new ComplexRecipe(recipe._recipeID, recipe._Name,
                                    recipe._description, recipe._ingredientList, recipe._PerPerson, RecipeCost(productsHashtable, recipe))).ToList();

            resultComplexRecipes.Sort((a, b) => a._complexRecipeComponent._recipeCost.CompareTo(b._complexRecipeComponent._recipeCost));

            return resultComplexRecipes;

        }

        // idea to return complex object with productSuggestions and total cost 

        private ComplexRecipeComponent RecipeCost(Hashtable productsHashtable, Recipe recipe)
        {            
            double totalCost = 0;
            // CRP = ComplexRecipeComponent
            //ComplexRecipeComponent CRP = new ComplexRecipeComponent();
            Dictionary<string, List<Product>> CRPProductDicts = new Dictionary<string, List<Product>>();
            var recipeIngredientList = recipe._ingredientList.Select(a => a._ingredientName);
            var distinctRecipeIngredients = recipeIngredientList.Distinct();

            foreach (var ingredient in distinctRecipeIngredients)
            {
                if (productsHashtable.ContainsKey(ingredient))
                {
                    var productList = (List<Product>)productsHashtable[ingredient];
                    if (productList.Any())
                    {
                        totalCost += productList.First()._price;
                        CRPProductDicts.Add(ingredient, productList);
                    }
                }

            }
            return new ComplexRecipeComponent(totalCost, CRPProductDicts);
        }

        private Dictionary<string, List<Product>> MatchingProducts(List<string> distinctIngredients)
        {
            Dictionary<string, List<Product>> resDictionary = new Dictionary<string, List<Product>>();

            foreach(string ingredient in distinctIngredients)
            {                
                resDictionary.Add(ingredient, matchingProducts(ingredient));
            }
            return resDictionary;
        }

        // Input: ingredient to search products for
        // Output: the list of matching products searched in the products database 
        private List<Product> matchingProducts(string ingredient)
        {
            List<Product> resProducts = new List<Product>();
            resProducts = _dbConnect.GetProducts(ingredient);
            resProducts.Sort((a,b) => a._price.CompareTo(b._price));

            return resProducts;
        }

        private List<Recipe> Recipes(string searchTerm)
        {
            return _dbConnect.GetRecipes(searchTerm);
        }

        // Input: list of queried recipes matching searchTerm
        // Output: list of the distinct ingredients in these recipes
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
