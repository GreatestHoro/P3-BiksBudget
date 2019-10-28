using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net.Http;
using System.IO;
using B3_BiksBudget.BBDatabase;
using B3_BiksBudget.Webcrawler;
using B3_BiksBudget.BBObjects;

namespace BiksBudget
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseInformation dbInfo = new DatabaseInformation("localhost", "Demonstration", "root", "JeppeJonHoltYeetYeet1999Peterbandsholm?");
            new InitializeDatabase().start(dbInfo);


            _ = RecipeCrawl.GetRecipes(1,1000, dbInfo);

            Console.WriteLine("web crawler begins... fear its power");
            Console.ReadLine();
        }
    }
}
