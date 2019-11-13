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
            //dh.GenerateData();
            dh.TestCollection();
        }
    }

    public class DataHandling
    {
        public DatabaseConnect dbConnect = new DatabaseConnect("localhost", "biksbudgetDB", "root", "BiksBudget123");
        public void GenerateDatabase()
        {
            dbConnect.InitializeDatabase();
            dbConnect.GenerateSallingDB();
            dbConnect.InitializeUserDatabase();
            dbConnect.InitializeStorageDatabase();
        }

        public void GenerateData()
        {

            CoopDoStuff tryCoop = new CoopDoStuff("d0b9a5266a2749cda99d4468319b6d9f");

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
                dbConnect.AddProduct(new Product("B" + c.Ean, c.Navn, c.Navn2, c.Pris, "", "Brugsen"));
            }


            RecipeCrawl WebRunner = new RecipeCrawl();
            _ = WebRunner.GetRecipes(620, 1200, dbConnect);

            Console.WriteLine("web runner begins... fear its power");
            Console.ReadLine();
        }

        public void TestCollection()
        {


            //Console.WriteLine(dbConnect.checkIfSomethingExist("users", "username", "Test"));

            /*dbConnect.AddUser("Test6", "Test", "email");
            Console.WriteLine(dbConnect.CheckUser("Test", "Test"));

            List<Product> testList = new List<Product>();

            Product tProd1 = new Product("B2000020000002", 500);
            Product tProd2 = new Product("B2000110000004", 300);
            Product tProd3 = new Product("B2000060000000", 500);

            testList.Add(tProd1);
            testList.Add(tProd2);
            testList.Add(tProd3);

            dbConnect.AddToStorage("Test6", testList);*/

            List<Product> testList = new List<Product>();

            testList = dbConnect.GetStorageFromUsername("Test3");

            foreach (Product p in testList)
            {
                Console.WriteLine(p._timeAdded);
            }
        }
    }
}
