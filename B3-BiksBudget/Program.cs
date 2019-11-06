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
            DatabaseConnect dbConnect = new DatabaseConnect("localhost", "BiksBudgetDB", "root", "BiksBudget123");

            //dbConnect.AddUser("Test3", "Test", "Test");

            dbConnect.InitializeDatabase();
            /*dbConnect.InitializeUserDatabase();

            CoopDoStuff tryCoop = new CoopDoStuff("d0b9a5266a2749cda99d4468319b6d9f");

            List<CoopProduct> coopProducts = tryCoop.CoopFindEverythingInStore("24073");

            int count = 0;
            foreach (CoopProduct c in coopProducts)
            {
                dbConnect.AddProduct(new Product(c.Ean, c.Navn, c.Navn2, c.Pris, c.VareHierakiId));
            }
            */
            RecipeCrawl WebRunner = new RecipeCrawl();
            _ = WebRunner.GetRecipes(1, 1000, dbConnect);

            Console.WriteLine("web runner begins... fear its power");
            Console.ReadLine();
        }
    }
}
