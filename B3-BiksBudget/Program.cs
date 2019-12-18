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
            DataHandling dh = new DataHandling();
            string settings = PromtForUserInput();

            try
            {
                dh.GenerateDatabase().Wait();
                dh.GenerateData(
                    GetOptionsAt(settings,0), 
                    GetOptionsAt(settings, 1), 
                    GetOptionsAt(settings, 2), 
                    GetOptionsAt(settings, 3), 
                    GetOptionsAt(settings, 4)).Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public static string PromtForUserInput()
        {
            Console.WriteLine("To Choose Gather options create a string of zeros and one, where ¨0¨ means false, and ¨1¨ means true.");
            Console.WriteLine("The options consists of five functionalities that can be toggeled on and off independently:");
            Console.WriteLine("Example of an option string ¨11101¨ (recommended default)");
            Console.WriteLine("1: Getting products from the coop api.");
            Console.WriteLine("2: Crawling Alletiderskogebog and generating recepies ingrdient, and product refs. (Will take a long time)");
            Console.WriteLine("3: Populate links to cheapest products to ingredients");
            Console.WriteLine("4: Deletes the generated product tags. (should not be on by default)");
            Console.WriteLine("5: Autocorret suggestions in product search");
            return Console.ReadLine();
        }

        public static bool GetOptionsAt(string options, int placement) 
        {
            char[] arr = options.ToCharArray();
            if (options.Length > 5) 
            {
                throw new IndexOutOfRangeException();
            }
            return arr[placement].Equals('1') ? true : false;
        }
    }

    public class DataHandling
    {
        public DatabaseConnect dc = new DatabaseConnect();
        public async Task GenerateDatabase()
        {
            await dc.Initialize.Start();
        }

        public async Task GenerateData(bool coop, bool webcrawler, bool generatePrice, bool DeleteRefrences, bool autocomplete)
        {
            ProductHandling productHandling = new ProductHandling();
            if (coop)
            {
                CoopDoStuff tryCoop = new CoopDoStuff("d0b9a5266a2749cda99d4468319b6d9f");

                List<CoopProduct> coopProducts = tryCoop.CoopFindEverythingInStore("24073");
                int count = 0;
                foreach (CoopProduct c in coopProducts)
                {
                    count++;
                    Console.WriteLine(count);
                    await dc.Product.Add(new Product("F" + c.Ean, c.Navn, c.Navn2, c.Pris, "", "Fakta"));
                }
            }

            if (webcrawler)
            {
                RecipeCrawl WebRunner = new RecipeCrawl();
                _ = WebRunner.GetRecipes(100, 2884, dc);

                Console.WriteLine("web runner begins... fear its power");
                Console.ReadKey();
            }

            if (generatePrice)
            {
                await dc.Recipe.GenerateCheapestPL();
                await dc.Recipe.PopulateIngredientLink();
            }

            if (DeleteRefrences)
            {
                List<Product> products = dc.Product.GetList("");

                foreach (Product product in products)
                {
                    _ = dc.Product.AddReference("", product._id);
                }
            }

            if (autocomplete)
            {
                await dc.Product.AddAutocompleteToDB();
            }
        }
    }
}
