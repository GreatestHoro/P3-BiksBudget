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
            DatabaseConnect dbConnect = new DatabaseConnect("localhost", "biksbudgetDB", "root", "BiksBudget123");



            dbConnect.InitializeDatabase();
            dbConnect.GenerateSallingDB();
            dbConnect.InitializeUserDatabase();


            dbConnect.AddUser("Test", "Test", "email");
            Console.WriteLine(dbConnect.CheckUser("Test", "Test"));

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
                dbConnect.AddProduct(new Product("B" + c.Ean, c.Navn, c.Navn2, c.Pris, "", "Brugsen"));
            }
*/

            RecipeCrawl WebRunner = new RecipeCrawl();
            _ = WebRunner.GetRecipes(620, 1200, dbConnect);

            Console.WriteLine("web runner begins... fear its power");
            Console.ReadLine();
        }
    }
}
