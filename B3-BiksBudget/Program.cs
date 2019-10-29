﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net.Http;
using System.IO;
using B3_BiksBudget.Webcrawler;
using BBCollection;

namespace BiksBudget
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseConnect dbConnect = new DatabaseConnect("localhost", "Demonstration", "root", "Bjarke05!");
            dbConnect.InitializeDatabase();
            _ = RecipeCrawl.GetRecipes(1,1000, dbConnect);

            Console.WriteLine("web crawler begins... fear its power");
            Console.ReadLine();
        }
    }
}
