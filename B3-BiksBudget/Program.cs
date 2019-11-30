using BBCollection.StoreApi.CoopApi;
using BBGatherer.Webcrawler;
using BBCollection;
using BBCollection.BBObjects;
using System;
using System.Collections.Generic;
using BBCollection.DBConncetion;
using BBCollection.DBHandling;

namespace BBGatherer
{
    class Program
    {
        static void Main(string[] args)
        {
            DataHandling dh = new DataHandling();

            

            dh.GenerateDatabase();
            dh.GenerateData(false, true);
            //dh.TestCollection();
        }
    }

    public class DataHandling
    {
        public DatabaseConnect dbConnect = new DatabaseConnect();
        public void GenerateDatabase()
        {
            
        }

        public void GenerateData(bool coop, bool salling)
        {
            ProductHandling test = new ProductHandling();
            //InitializeDB _test = new InitializeDB();
            if (coop) {
                CoopDoStuff tryCoop = new CoopDoStuff("d0b9a5266a2749cda99d4468319b6d9f");

                List<CoopProduct> coopProducts = tryCoop.CoopFindEverythingInStore("24073");



                int count = 0;
                coopProducts.AddRange(tryCoop.CoopFindEverythingInStore("2096"));

                count = 0;
                foreach (CoopProduct c in coopProducts)
                {
                    count++;
                    Console.WriteLine(count);
                    //dbConnect.AddProduct(new Product("B" + c.Ean, c.Navn, c.Navn2, c.Pris, "", "SuperBrugsen"));
                }
            }
            //_test.UpdateProductTable(new DatabaseInformation("localhost", "biksbudgetDB", "root", "BiksBudget123"));
            if (salling == true)
            {
                RecipeCrawl WebRunner = new RecipeCrawl();
                _ = WebRunner.GetRecipes(75, 30000, dbConnect);

                Console.WriteLine("web runner begins... fear its power");
                Console.ReadLine();
            }

            //dbConnect.AddProduct(new Product("test","hey","alot",1d,"nope","walmart"));
            //test.InsertIngredientReferenceFromId("tester", "test", new DatabaseInformation("localhost", "biksbudgetDB", "root", "BiksBudget123"));

        }

        public void TestCollection()
        {

        }
    }
}
