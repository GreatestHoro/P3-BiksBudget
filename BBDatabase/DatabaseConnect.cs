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

        public void InitializeUserDatabase()
        {
            new InitializeDB().CreateUserDB(dbInfo.GetConnect());
        }

        public void InitializeStorageDatabase()
        {
            new InitializeDB().CreateStorageDB(dbInfo.GetConnect());
        }

        /* In this section the user functions will get handled. */

        public void AddUser(string username, string password, string email)
        {
            new UserHandling().InsertUser(username, password, dbInfo.GetConnect());
        }

        public bool CheckUser(string username, string password)
        {
            return new UserHandling().VerifyUser(username, password, dbInfo.GetConnect());
        }

        /* Insert Sallingproduct to database */

        public void GenerateSallingDB()
        {
            new InitializeDB().CreateSallingProduct(dbInfo.GetConnect());
        }


        public void AddSallingProduct(SallingProduct sallingProduct)
        {
            Product convert = new Product("B" + sallingProduct._id, sallingProduct._title, sallingProduct._description, sallingProduct._price, sallingProduct._img, "Bilka");

            new ProductHandling().Insert(convert, dbInfo.GetConnect());
        }

        //public List<SallingProduct> GetSallingProduct(string nameToSearch)
        //{
        //    return new ProductHandling().ListOfSallingProductsFromName(nameToSearch, dbInfo.GetConnect());
        //}

        /* Storagehandling */
        public void AddListToStorage(string username, List<Product> storage)
        {
            new ProductHandling().AddToStorageFromUsername(username, storage, dbInfo.GetConnect());
        }

        public void AddOneToStorage(string username, Product product)
        {
            new ProductHandling().AddSingleProductToStorage(username, product, dbInfo.GetConnect());
        }

        public List<Product> GetStorageFromUsername(string username)
        {
            return new ProductHandling().ProductsInStorageFromUsername(username, dbInfo.GetConnect());
        }

        public void UpdateStorage(string username, List<Product> storage)
        {
            new ProductHandling().RemoveStorageFromUsername(username, dbInfo.GetConnect());
            new ProductHandling().AddToStorageFromUsername(username, storage, dbInfo.GetConnect());
        }
    }
}
