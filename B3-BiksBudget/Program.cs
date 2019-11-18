using BBGatherer.StoreApi.CoopApi;
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
            //dh.GenerateDatabase();
            dh.GenerateData();
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

        public void GenerateData()
        {

            /*CoopDoStuff tryCoop = new CoopDoStuff("d0b9a5266a2749cda99d4468319b6d9f");

            List<CoopProduct> coopProducts = tryCoop.CoopFindEverythingInStore("24073");

            int count = 0;
            foreach (CoopProduct c in coopProducts)
            {
                count++;
                Console.WriteLine(count);
                dbConnect.AddProduct(new Product("F" + c.Ean, c.Navn, c.Navn2, c.Pris, "", "Fakta"));
            }

            coopProducts = tryCoop.CoopFindEverythingInStore("2096");

            count = 0;
            foreach (CoopProduct c in coopProducts)
            {
                count++;
                Console.WriteLine(count);
                dbConnect.AddProduct(new Product("B" + c.Ean, c.Navn, c.Navn2, c.Pris, "", "SuperBrugsen"));
            }


            /*RecipeCrawl WebRunner = new RecipeCrawl();
            _ = WebRunner.GetRecipes(100, 1200, dbConnect);

            Console.WriteLine("web runner begins... fear its power");
            Console.ReadLine();*/
        }

        public void TestCollection()
        {
            dbConnect.AddUser("Test6", "Test", "BB");

            List<Product> testList = new List<Product>();

            Product tProd1 = new Product("S13981601", 5, "Full");
            Product tProd2 = new Product("S14785501", 5, "Full");
            Product tProd3 = new Product("S14937401", 5, "Full");

            testList.Add(tProd1);
            testList.Add(tProd2);
            testList.Add(tProd3);

            //dbConnect.AddListToStorage("Test6", testList);

            //testList.Remove(tProd2);

            //dbConnect.UpdateStorage("Test6", testList);

            Shoppinglist shoppinglist = new Shoppinglist("TestShoppingList", testList);
            List<Shoppinglist> testSL = new List<Shoppinglist>();
            testSL.Add(shoppinglist);


            dbConnect.AddShoppingListsToDatabase("Test6",testSL);
            
            List <Recipe> rec = new List<Recipe>();


            List<Shoppinglist> shoppinglists = dbConnect.GetShoppinglists("Test6");


            foreach (Shoppinglist sl in shoppinglists)
            {
                foreach (Product p in sl._products)
                {
                    Console.WriteLine(p._productName + sl._name);

                }
            }

            dbConnect.DeleteShoppingListFromName("TestShoppingList", "Test6");

            List<Shoppinglist> shoppinglistsTest  = dbConnect.GetShoppinglists("Test6");

            Console.WriteLine(shoppinglistsTest.Count);
        }
    }
}
