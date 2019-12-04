using BBCollection.StoreApi.CoopApi;
using BBGatherer.Webcrawler;
using BBCollection;
using BBCollection.BBObjects;
using System;
using System.Collections.Generic;
using BBCollection.DBConncetion;
using BBCollection.DBHandling;
using System.Threading.Tasks;

namespace BBGatherer
{
    class Program
    {
        static void Main(string[] args)
        {
            
            DataHandling dh = new DataHandling();
            //dh.GenerateDatabase();
            dh.GenerateData(false,true);

            /*try
            {
                dh.TestCollection().Wait();
            } catch(Exception e)
            {
                Console.WriteLine(e);
            }*/
            
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
                _ = WebRunner.GetRecipes(1, 2884, dbConnect);

                Console.WriteLine("web runner begins... fear its power");
                Console.ReadLine();
            }

            //dbConnect.AddProduct(new Product("test","hey","alot",1d,"nope","walmart"));
            //test.InsertIngredientReferenceFromId("tester", "test", new DatabaseInformation("localhost", "biksbudgetDB", "root", "BiksBudget123"));

        }

        public async Task TestCollection()
        {
            DatabaseConnect dc = new DatabaseConnect();

            List<string> strings = new List<String>();

            string str1 = "A";
            string str2 = "B";
            string str3 = "C";

            strings.Add(str1);
            strings.Add(str2);
            strings.Add(str3);

            dc.Recipe.GetReferencesAsync(strings);

            /*int count = 6;
            string check = "";
            string[] three = new string[] { "A", "B", "C"};
            string[] six = new string[] { "A", "B", "C", "D", "E", "F" };

            for (int i = 1; i < six.Length; i++)
            {
                int j = 0;
                while (j < i)
                {
                    check += $"{six[i]} = {six[j]}";
                    j++;
                    if (i != 1)
                    {
                        if (j != i)
                        {
                            check += " AND ";

                        }
                    }
                }
                if (i < six.Length - 1)
                {
                    check += " AND ";
                }
                
            }
            Console.WriteLine(check);*/

        }
    }
}
