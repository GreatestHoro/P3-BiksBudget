using BBCollection;
using BBCollection.BBObjects;
using BBCollection.DBHandling;
using BBCollection.Queries;
using BBCollection.StoreApi.CoopApi;
using BBGatherer.Webcrawler;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BBGatherer
{
    class Program
    {
        static void Main(string[] args)
        {
            /*productImages productImages = new productImages();
            string name = Console.ReadLine();

            productImages.SaveImagesFromLink(productImages.GetImageUrls(name, "bing").Result);*/

            DataHandling dh = new DataHandling();

            try
            {
                //RecipeQuery recipeQuery = new RecipeQuery();

                //recipeQuery.CheapestCRecipes("").Wait();
                dh.GenerateDatabase().Wait();
                //dh.GenerateData(false, false, false,false,true).Wait();
                dh.TestCollection().Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    public class DataHandling
    {
        public DatabaseConnect dbConnect = new DatabaseConnect();
        public async Task GenerateDatabase()
        {
            await dbConnect.Initialize.Start();
        }

        public async Task GenerateData(bool coop, bool salling, bool generatePrice, bool DeleteRefrences, bool autocomplete)
        {
            ProductHandling test = new ProductHandling();
            //InitializeDB _test = new InitializeDB();

            if (autocomplete)
            {

            }

            if (coop)
            {
                CoopDoStuff tryCoop = new CoopDoStuff("d0b9a5266a2749cda99d4468319b6d9f");

                List<CoopProduct> coopProducts = tryCoop.CoopFindEverythingInStore("24073");
                int count = 0;
                foreach (CoopProduct c in coopProducts)
                {
                    count++;
                    Console.WriteLine(count);
                    await dbConnect.Product.Add(new Product("F" + c.Ean, c.Navn, c.Navn2, c.Pris, "", "Fakta"));
                }
            }
            //_test.UpdateProductTable(new DatabaseInformation("localhost", "biksbudgetDB", "root", "BiksBudget123"));
            if (salling)
            {
                RecipeCrawl WebRunner = new RecipeCrawl();
                _ = WebRunner.GetRecipes(1, 2884, dbConnect);

                Console.WriteLine("web runner begins... fear its power");
                Console.ReadLine();
            }

            //dbConnect.AddProduct(new Product("test","hey","alot",1d,"nope","walmart"));
            //test.InsertIngredientReferenceFromId("tester", "test", new DatabaseInformation("localhost", "biksbudgetDB", "root", "BiksBudget123"));

            if (generatePrice)
            {
                await dbConnect.Recipe.GenerateTotalPriceAsync();
            }

            if (DeleteRefrences)
            {
                var yeet = test.GetList("");
                int i = 0;

                foreach (var yet in yeet)
                {
                    _ = test.AddReference("", yet._id);
                    Console.WriteLine(i++);
                }

            }
        }

        public async Task TestCollection()
        {
            DatabaseConnect dc = new DatabaseConnect();

            await dc.Product.AddAutocompleteToDB();



        }
    }
}
