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
            dh.GenerateData(false, true);
            dh.TestCollection();
            //ProductSearchLinkConstructer yeet = new ProductSearchLinkConstructer(" øl ", "00", "111111111");
            //Console.WriteLine(yeet.GetURL());
            //Console.ReadLine();
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
        dbConnect.AddUser("Test6", "Test", "BB");

        /*List<Product> testList = new List<Product>();

        Product tProd1 = new Product("F2141400000004", 5, "Full");

        Product tProd2 = new Product("F2141640000000", 5, "Full");
        Product tProd3 = new Product("F4001724019831", 5, "Full");

        testList.Add(tProd1);
        testList.Add(tProd2);
        testList.Add(tProd3);

        dbConnect.AddListToStorage("Test6", testList);

        testList.Remove(tProd2);

        dbConnect.UpdateStorage("Test6", testList);*/

        List<Recipe> res = dbConnect.GetRecipes("Lam");

        foreach (Recipe r in res)
        {
            Console.WriteLine(r._ingredientList.Count);
        }

        /*
        List <Recipe> recipes = new List<Recipe>();
        Console.WriteLine("?????");

        foreach(Recipe r in res)
        {
            Console.WriteLine(r._Name + " AND " + r._ingredientList.Count);
        }



        /*public void TestCollection()
        {


            //Console.WriteLine(dbConnect.checkIfSomethingExist("users", "username", "Test"));

            dbConnect.AddUser("Test6", "Test", "email");
            //Console.WriteLine(dbConnect.CheckUser("Test", "Test"));

        //Console.WriteLine(dbConnect.checkIfSomethingExist("users", "username", "Test"));
        //dbConnect.AddUser("Test6", "Test", "email");
        //Console.WriteLine(dbConnect.CheckUser("Test", "Test"));

        //List<Product> testList = new List<Product>();

        //Product tProd1 = new Product("B2000020000002", 5, "Full");
        //Product tProd2 = new Product("B2000110000004", 5, "Full");
        //Product tProd3 = new Product("B2000060000000", 5, "Full");

        //testList.Add(tProd1);
        //testList.Add(tProd2);
        //testList.Add(tProd3);
        //dbConnect.AddListToStorage("Test6", testList);


            //List<Product> testList = new List<Product>();

            //testList = dbConnect.GetStorageFromUsername("Test3");

            //foreach (Product p in testList)
            //{
            //    Console.WriteLine(p._timeAdded);
            //}
        }*/
    }
        }
    }

