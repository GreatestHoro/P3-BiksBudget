using BBCollection.BBObjects;
using BBCollection.DBConncetion;
using BBCollection.DBHandling;
using BBCollection.HandleRecipe;
using System.Collections.Generic;

namespace BBCollection
{
    public class DatabaseConnect
    {
        DatabaseInformation dbInfo = null;
        public DatabaseConnect(string sName, string dBName, string dBUser, string dBPw)
        {
            dbInfo = new DatabaseInformation(sName, dBName, dBUser, dBPw);
        }

        /* In this section the Recipe functions will be handled*/

        public void AddRecipe(Recipe recipe)
        {
            new RecipeToDatabase().CombineRecipe(recipe, dbInfo.GetConnect());
        }

        public List<Recipe> GetRecipes(string recipeName)
        {
            return new RetrieveFromDatabase().RetrieveRecipeList(recipeName, dbInfo.GetConnect());
        }

        public List<Ingredient> GetIngredients(int recipeID)
        {
            return new RetrieveFromDatabase().GetIngredientsFromRecipeID(recipeID, dbInfo.GetConnect());
        }

        /* In this section the Product functions will be handled*/ 

        public void AddProduct(Product product)
        {
            new ProductHandling().Insert(product, dbInfo.GetConnect());
        }

        public List<Product> GetProducts(string productName)
        {
            return new ProductHandling().ListOfProductsFromName(productName, dbInfo.GetConnect());
        } 

        /* In this section the initalization of the database will be handled */

        public void InitializeDatabase()
        {
            new InitializeDB().start(dbInfo.GetConnect());
        }
    }
}
