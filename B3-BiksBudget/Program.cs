﻿using BBGatherer.StoreApi.CoopApi;
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
            DatabaseConnect dbConnect = new DatabaseConnect("localhost", "Test-1", "root", "yeet");

            dbConnect.InitializeDatabase();

            /*CoopDoStuff tryCoop = new CoopDoStuff("d0b9a5266a2749cda99d4468319b6d9f");

            List<CoopProduct> coopProducts = tryCoop.CoopFindEverythingInStore("24073");
            int count = 0;
            foreach (CoopProduct c in coopProducts)
            {
                count++;
                Console.WriteLine(count);
                dbConnect.AddProduct(new Product(c.Ean, c.Navn, c.Navn2, c.Pris, c.VareHierakiId));
            }*/


            /*List<Recipe> recipes = dbConnect.GetRecipes("lammebov");

            foreach(Recipe r in recipes)
            {
                Console.WriteLine(r._Name);
            }
            */
            //dbConnect.InitializeDatabase();

            _ = RecipeCrawl.GetRecipes(489,491, dbConnect);

            Console.WriteLine("web runner begins... fear its power");
            Console.ReadLine();
        }
    }
}
