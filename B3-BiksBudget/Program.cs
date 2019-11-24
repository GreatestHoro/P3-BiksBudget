using BBCollection.StoreApi.CoopApi;
using BBGatherer.Webcrawler;
using BBCollection;
using BBCollection.BBObjects;
using System;
using System.Collections.Generic;
using BBCollection.DBConncetion;

namespace BBGatherer
{
    class Program
    {
        static void Main(string[] args)
        {
            DataHandling dh = new DataHandling();
            //dh.GenerateDatabase();
            //dh.GenerateData(true, false);
            dh.TestCollection();
        }
    }

    public class DataHandling
    {
        public DatabaseConnect dbConnect = new DatabaseConnect("localhost", "biksbudgetDB", "root", "BiksBudget123");
        public void GenerateDatabase()
        {
            dbConnect.InitializeDatabase();
            dbConnect.InitializeUserDatabase();
            dbConnect.InitializeStorageDatabase();
            dbConnect.InitializeShoppinglistDatabase();
        }

        public void GenerateData(bool coop, bool salling)
        {

            CoopDoStuff tryCoop = new CoopDoStuff("f0cabde6bb8d4bd78c28270ee203253f");

            List<CoopProduct> coopProducts = tryCoop.CoopFindEverythingInStore("24073");

            //oopDoStuff tryCoop = new CoopDoStuff("d0b9a5266a2749cda99d4468319b6d9f");

            int count = 0;
            coopProducts = tryCoop.CoopFindEverythingInStore("2096");

            count = 0;
            foreach (CoopProduct c in coopProducts)
            {
                count++;
                Console.WriteLine(count);
                dbConnect.AddProduct(new Product("B" + c.Ean, c.Navn, c.Navn2, c.Pris, "", "SuperBrugsen"));
            }


            if (salling == true)
            {
                RecipeCrawl WebRunner = new RecipeCrawl();
                _ = WebRunner.GetRecipes(100, 1200, dbConnect);

                Console.WriteLine("web runner begins... fear its power");
                Console.ReadLine();
            }
        }

        public void TestCollection()
        {
            dbConnect.GetRecipes("is");

/*

            //dbConnect.combineProductsAndIngredients();

            dbConnect.AddUser("Test6", "Test");

            dbConnect.DeleteShoppingListFromName("Shoppinglist", "Test6");

            List<Product> testList = new List<Product>();

            Product tProd1 = new Product(null, "TestCust", 5, "Full");
            Product tProd2 = new Product(null, "TestCust2", 5, "Full");
            Product tProd3 = new Product(null, "TestCust3", 5, "Full");

            Product tProdid1 = new Product("B2000060000000", null, 5, "Full");
            Product tProdid2 = new Product("B2000320000009", null, 5, "Full");
            Product tProdid3 = new Product("B2000570000002", null, 5, "Full");



            testList.Add(tProdid1);
            testList.Add(tProdid2);
            testList.Add(tProdid3);
            testList.Add(tProd1);
            testList.Add(tProd2);
            testList.Add(tProd3);

            dbConnect.AddListToStorage("Test6", testList);

            List<Product> products = dbConnect.GetStorageFromUsername("Test6");

            Console.WriteLine(products.Count);
            foreach (Product p in products)
            {
                Console.WriteLine(p._customname);
                Console.WriteLine(p._id);

            }

            Shoppinglist shoppinglist = new Shoppinglist("TestShoppingList", testList);

            List<Shoppinglist> ShoppingList2 = new List<Shoppinglist>();
            ShoppingList2.Add(shoppinglist);

            dbConnect.AddShoppingListsToDatabase("Test6", ShoppingList2);

            List<Shoppinglist> shoppinglists = dbConnect.GetShoppinglists("Test6");

            List<Recipe> recipes = new List<Recipe>();



            foreach (Shoppinglist sl in shoppinglists)
            {
                foreach (Product p in sl._products)
                {
                    Console.WriteLine("???");
                    Console.WriteLine(p._productName + sl._name);

                }
            }

            dbConnect.DeleteShoppingListFromName("", "Test6");*/
        }
    }
}
