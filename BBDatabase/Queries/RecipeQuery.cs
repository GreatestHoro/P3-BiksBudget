using BBCollection.BBObjects;
using BBCollection.DBHandling;
using BBCollection.StoreApi;
using BBCollection.StoreApi.ApiNeeds;
using BBCollection.StoreApi.SallingApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBCollection.Queries
{
    public class RecipeQuery
    {
        DatabaseConnect _dc = new DatabaseConnect();
        BearerAccessToken bearerAccessToken = new BearerAccessToken("a6f4495c-ace4-4c39-805c-46071dd536db");
        SallingAPILink _linkMaker = new SallingAPILink();
        Filter<SallingAPIProduct> _productFilter = new Filter<SallingAPIProduct>();
        public int _productsPerLoad { get; set; } = 10;
        public int _productsToMatch { get; set; } = 10;
        public int _loadCount { get; set; } = 0;
        string _prevSearch { get; set; }

        int _productsPerIngredient { get; set; } = 3;

        /// <summary>
        /// Calculates at batch of _productsPerLoad of recipes sorted by price, where the prie for each is calculated
        /// </summary>
        /// <param name="searchTerm">The recipe title searched on</param>
        /// <returns>A list with size of _productsPerLoad, where the price is calculated and the list is returned sorted by price</returns>
        public async Task<List<ComplexRecipe>> CheapestCRecipes(string searchTerm)
        {
            //Get recipes matching searchTerm and Filters
            List<Recipe> recipeList = await Recipes(searchTerm);

            //Make an array of distinct ingredients
            List<string> distinctIngredients = DistinctIngredients(recipeList);

            //Func products matching to the ingredients
            //want to make it multithreaded later
            //Find x sallingAPIProducts per distinct ingredient
            //Dictionary<string, List<Product>> productsDict = await MatchingProducts(distinctIngredients);
            Dictionary<string, List<Product>> productsDict = await MatchingProductsChain(distinctIngredients, "bilka");
            Dictionary<string, List<Product>> productsDictFakta = await MatchingProductsChain(distinctIngredients, "fakta");
            Dictionary<string, List<Product>> productsDictBrugsen = await MatchingProductsChain(distinctIngredients, "brugsen");

            // put into db here


            //Turns the dictionary into a hashtable
            Hashtable productsHashtable = new Hashtable(productsDict);


            List<ComplexRecipe> resultComplexRecipes = new List<ComplexRecipe>();
            //calculate the price of each recipe by calling RecipeCost for each recipe, and create a list of ComplexRecipe objects
            resultComplexRecipes = (from recipe in recipeList
                                    select new ComplexRecipe(recipe._recipeID, recipe._Name,
          recipe._description, recipe._ingredientList, recipe._PerPerson, RecipeCost(productsHashtable, recipe))).ToList();
            //sort the list of ComplexRecipes by price
            resultComplexRecipes.Sort((a, b) => a._complexRecipeComponent.RecipeCost.CompareTo(b._complexRecipeComponent.RecipeCost));

            return resultComplexRecipes;

        }

        public async Task<List<ComplexRecipe>> CheapestRecipeDB(string searchTerm)
        {
            if (_loadCount == 0)
            {
                _prevSearch = searchTerm;

            }

            List<ComplexRecipe> complexRecipes = await _dc.Recipe.GetPriceAsync(searchTerm, _productsPerLoad, _productsPerLoad * _loadCount);

            //List<Recipe> recipes = await _dc.Recipe.GetListAsync(searchTerm);

            _loadCount++;

            return complexRecipes;
        }

        public async Task<Dictionary<string, List<Product>>> GetProductsForRecipe(int recipeID, List<ComplexRecipe> complexRecipes)
        {
            ComplexRecipe cr = complexRecipes.First(x => x._recipeID == recipeID);
            Dictionary<string, List<Product>> dict = new Dictionary<string, List<Product>>();
            List<string> ingList = cr._ingredientList.Select(ingredient => ingredient._ingredientName).ToList();

            Dictionary<string, List<Product>> prodDict = await MatchingProducts(ingList);

            return prodDict;
        }

        /// <summary>
        /// Calculates the price for a recipe
        /// by looking for the price in the hashtable of products previously created
        /// it is assumed that the cheapest product for a given ingredient is the most relevant at first/initial calculation.
        /// Step 1: run through the recipes ingredients
        /// Step 2: - add the price to the total cost
        ///         - add the matching product list to ComplexProduct's dictionary for the product
        /// </summary>
        /// <param name="productsHashtable">the productHashtable which has the dictionary for the batches distinctingredients</param>
        /// <param name="recipe">the recipe to perform the calculation upon</param>
        /// <returns>returns a new ComplexRecipecComponent which has the total recipeCost and a dictionary of products</returns>
        private ComplexRecipeComponent RecipeCost(Hashtable productsHashtable, Recipe recipe)
        {
            double totalCost = 0;
            // CRP = ComplexRecipeComponent
            //ComplexRecipeComponent CRP = new ComplexRecipeComponent();
            Dictionary<string, List<Product>> CRPProductDicts = new Dictionary<string, List<Product>>();
            var recipeIngredientList = recipe._ingredientList.Select(a => a._ingredientName);
            var distinctRecipeIngredients = recipeIngredientList.Distinct();

            foreach (string ingredient in distinctRecipeIngredients)
            {
                if (productsHashtable.ContainsKey(ingredient))
                {
                    List<Product> productList = (List<Product>)productsHashtable[ingredient];
                    if (productList.Any())
                    {
                        totalCost += productList.First()._price;
                        CRPProductDicts.Add(ingredient, productList);
                    }
                }

            }
            return new ComplexRecipeComponent(totalCost, CRPProductDicts);
        }

        /// <summary>
        /// Iterates through the distinctIngredients list. Then adds the the matching products to the value of the dictionary for each distinct ingredient
        /// </summary>
        /// <param name="distinctIngredients">Gets the list of the distinct ingredients</param>
        /// <returns>Return a dictionary with keys being the distinct ingreient names, and the value being a list of matching products</returns>
        private async Task<Dictionary<string, List<Product>>> MatchingProducts(List<string> distinctIngredients)
        {
            Dictionary<string, List<Product>> resDictionary = new Dictionary<string, List<Product>>();

            foreach (string ingredient in distinctIngredients.Distinct().ToList())
            {
                resDictionary.Add(ingredient, await Products(ingredient));
            }
            return resDictionary;
        }

        private async Task<Dictionary<string, List<Product>>> MatchingProductsChain(List<string> distinctIngredients, string chain)
        {
            Dictionary<string, List<Product>> resDictionary = new Dictionary<string, List<Product>>();

            foreach (string ingredient in distinctIngredients.Distinct().ToList())
            {
                List<Product> productList = await Products(ingredient);
                var filteredList = productList.Where(product => product._storeName == chain).ToList();
                filteredList.Sort((p1, p2) => p1._price.CompareTo(p2._price));
                resDictionary.Add(ingredient, filteredList);
            }
            return resDictionary;
        }

        /// <summary>
        /// Searches in the database for relevant products
        /// </summary>
        /// <param name="ingredient">ingredient name to search products for</param>
        /// <returns>the list of matching products searched in the products database with size of _productsToMatch and always at 0 offset</returns>
        private async Task<List<Product>> Products(string ingredient)
        {
            List<Product> resProducts = new List<Product>();
            resProducts = await _dc.Product.GetRange(ingredient, _productsToMatch, 0);
            if (resProducts.Count == 0)
            {
                resProducts = await SlowExsperimentalSearch(ingredient);
            }
            resProducts.Sort((a, b) => a._price.CompareTo(b._price));

            return resProducts;
        }

        private async Task<List<Product>> SlowExsperimentalSearch(string searchterm)
        {
            string[] InduviduelTerms = searchterm.Split(" ");
            ProductHandling pTest = new ProductHandling();
            List<List<Product>> TempListList = new List<List<Product>>();
            List<Product> ProductList = new List<Product>();

            foreach (string s in InduviduelTerms)
            {
                TempListList.Add(await pTest.ReferencesAsync(s));
            }
            TempListList = TempListList.OrderByDescending(x => -x.Count).ToList();
            if (ProductList.Count == 0)
            {
                foreach (List<Product> Plist in TempListList)
                {
                    if (Plist.Count != 0)
                    {
                        ProductList = Plist;
                        break;
                    }
                }
            }
            foreach (Product p in ProductList)
            {
                p._CustomReferenceField = "*";
            }
            return ProductList;
        }

        /// <summary>
        /// Function which finds corresponding recipes to the searchTerm in the database
        /// <para>It checks if the previous serach is the same as the current. If true it it increments the offset to the next un-discovered batch</para>
        /// <para>If not it sets the offset back to zero and resets the _prevsearch to the current search term</para>
        /// </summary>
        ///
        /// <param name="searchTerm">The recipe title searched for</param>
        /// <returns>Returns a list of size  _productsPerLoad at the given offsett</returns>
        private async Task<List<Recipe>> Recipes(string searchTerm)
        {
            if (_loadCount == 0)
            {
                _prevSearch = searchTerm;

            }

            //List<Recipe> recipes = await _dc.Recipe.GetRange(searchTerm, _productsPerLoad, _productsPerLoad * _loadCount);

            List<Recipe> recipes = await _dc.Recipe.GetList(searchTerm);

            _loadCount++;

            return recipes;
        }

        /// <summary>
        /// First step is to create a new list of only the ingredient name
        /// <par>Second step is to use the .Distinct function to get the distinct ingredients only</par>
        /// </summary>
        /// <param name="recipeList">List of queried recipes matching searchTerm</param>
        /// <returns>List of the distinct ingredients in these recipes</returns>
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


    //Flags enum allows for allowing multiple options to be selected simultaneously in a neat way, since each option is represented by the state (1 or 0) of a bit position
    //look up the documentation for more
    [Flags]
    public enum Chain
    {
        none = 0,
        bilka = 1,
        superBrugsen = 2,
        fakta = 4
    }

}
