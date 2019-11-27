﻿using BBCollection.BBObjects;
using BBCollection.DBConncetion;
using BBCollection.DBHandling;
using BBCollection.HandleRecipe;
using System;
using System.Collections.Generic;

namespace BBCollection
{
    public class DatabaseConnect
    {
        DatabaseInformation databaseInformation = new DatabaseInformation();

        /* In this section the initalization of the database will be handled */

        public void InitializeDatabase()
        {
            new InitializeDB().start(databaseInformation.GetConnect());
        }

        public void InitializeUserDatabase()
        {
            new InitializeDB().CreateUserDB(databaseInformation.GetConnect());
        }

        public void InitializeStorageDatabase()
        {
            new InitializeDB().CreateStorageDB(databaseInformation.GetConnect());
        }

        public void InitializeShoppinglistDatabase()
        {
            new InitializeDB().CreateSLTables(databaseInformation.GetConnect());
        }

        /* In this section the Product functions will be handled*/

        public void AddProduct(Product product)
        {
            new ProductHandling().Insert(product, databaseInformation.GetConnect());
        }

        public List<Product> GetProducts(string productName)
        {
            ProductHandling productHandling = new ProductHandling();
            List<Product> returnProducts = new List<Product>();
            List<Product> products = productHandling.ListOfProductsFromName(productName, databaseInformation.GetConnect());
            foreach (Product p in products)
            {
                returnProducts.Add(productHandling.GetProductWithReferenceFromId(p._id, databaseInformation));
            }
            return returnProducts;
        }

        public void AddRecipe(Recipe recipe)
        {
            new RecipeToDatabase().CombineRecipe(recipe, databaseInformation.GetConnect());
        }

        public List<Recipe> GetRecipes(string recipeName)
        {
            return new RetrieveFromDatabase().RetrieveRecipeList(recipeName, databaseInformation.GetConnect());
        }

        public List<Ingredient> GetIngredients(int recipeID)
        {
            return new RetrieveFromDatabase().GetIngredientsFromRecipeID(recipeID, databaseInformation.GetConnect());
        }

        /* In this section the Product functions will be handled*/

        /* In this section the user functions will get handled. */

        public void AddUser(string username, string password)
        {
            new UserHandling().InsertUser(username, password, databaseInformation.GetConnect());
        }

        public bool CheckUser(string username, string password)
        {
            return new UserHandling().VerifyUser(username, password, databaseInformation.GetConnect());
        }

        /* Insert Sallingproduct to database */

        public void GenerateSallingDB()
        {
            new InitializeDB().CreateSallingProduct(databaseInformation.GetConnect());
        }


        public void AddSallingProduct(SallingProduct sallingProduct)
        {
            Product convert = new Product("S" + sallingProduct._id, sallingProduct._title, sallingProduct._description, sallingProduct._price, sallingProduct._img, "Bilka");

            new ProductHandling().Insert(convert, databaseInformation.GetConnect());
        }

        /* Storagehandling */
        public void AddListToStorage(string username, List<Product> storage)
        {
            new ProductHandling().AddToStorageFromUsername(username, storage, databaseInformation.GetConnect());
        }

        public void AddOneToStorage(string username, Product product)
        {
            new ProductHandling().AddSingleProductToStorage(username, product, databaseInformation.GetConnect());
        }

        public List<Product> GetStorageFromUsername(string username)
        {
            return new ProductHandling().ProductsInStorageFromUsername(username, databaseInformation.GetConnect());
        }

        public void RemoveFromStorage(string username, List<Product> storage)
        {
            new ProductHandling().RemoveStorageFromUsername(username, databaseInformation.GetConnect());
        }

        public void UpdateStorage(string username, List<Product> storage)
        {
            new ProductHandling().UpdateStorage(username, storage, databaseInformation.GetConnect());
        }

        /* Shoppinglist handling */
        public List<Shoppinglist> GetShoppinglists(string username)
        {
            return new ProductHandling().GetShoppinglistsFromUsername(username, databaseInformation.GetConnect());
        }

        public void AddShoppingListsToDatabase(string username, List<Shoppinglist> shoppingLists)
        {
            new ProductHandling().AddShoppingListsToDB(username, shoppingLists, databaseInformation.GetConnect());
        }

        public void DeleteShoppingListFromName(string shoppingListName, string username)
        {
            new ProductHandling().DeleteShoppingListFromName(shoppingListName, username, databaseInformation.GetConnect());
        }

        public void combineProductsAndIngredients()
        {
            new ProductHandling().GoThroughIngredientsToFindProducts(databaseInformation.GetConnect());
        }

        public List<Product> GetProductsFromIngredientName(string ingredientName)
        {

            return null;
        }

        public void InsertReferenceToProduct(string reference, string productID)
        {
            new ProductHandling().InsertIngredientReferenceFromId(reference, productID, databaseInformation.GetConnect());
        }

        public Product GetProductWithReference(string prodID)
        {
            return new ProductHandling().GetProductWithReferenceFromId(prodID, databaseInformation.GetConnect());
        }

        public List<Product> GetListOfProductsFromReference(string reference)
        {
            return new ProductHandling().GetProductsWhereReferenceIncludesString(reference, databaseInformation.GetConnect());
        }

        public List<Product> GetProductInterval(string name, int limit, int offset)
        {
            return new ProductHandling().GetProductsInterval(name, limit, offset, databaseInformation.GetConnect());
        }

        public List<Recipe> GetRecipesInterval(string name, int limit, int offset)
        {
            return new RetrieveFromDatabase().RetrieveRecipeListInterval(name, limit, offset, databaseInformation.GetConnect());
        }

    }
}
