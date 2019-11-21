using BBCollection.StoreApi.CoopApi;
using BBGatherer.Webcrawler;
using BBCollection;
using BBCollection.BBObjects;
using System;
using System.Collections.Generic;

namespace BBGatherer
{
    class Program
    {
        static void Main(string[] args)
        {
            DataHandling dh = new DataHandling();
            dh.GenerateDatabase();
            //dh.GenerateData(false, true);
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

            CoopDoStuff tryCoop = new CoopDoStuff("d0b9a5266a2749cda99d4468319b6d9f");

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


            RecipeCrawl WebRunner = new RecipeCrawl();
            _ = WebRunner.GetRecipes(100, 1200, dbConnect);

            Console.WriteLine("web runner begins... fear its power");
            Console.ReadLine();
        }

        public void TestCollection()
        {
            dbConnect.AddUser("Test6", "Test");

            List<Product> testList = new List<Product>();

            Product tProd1 = new Product("F2141400000004", 5, "Full");

            Product tProd2 = new Product("F2141640000000", 5, "Full");
            Product tProd3 = new Product("F4001724019831", 5, "Full");

            testList.Add(tProd1);
            testList.Add(tProd2);
            testList.Add(tProd3);

            Shoppinglist shoppinglist = new Shoppinglist("TestShoppingList",testList);

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

            dbConnect.DeleteShoppingListFromName("","Test6");
        }
    }
}
