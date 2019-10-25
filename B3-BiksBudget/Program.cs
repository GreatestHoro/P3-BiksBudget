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
            DatabaseInformation dbInfo = new DatabaseInformation("localhost", "Test_BB", "root", "Bjarke05!");
            //new InitializeDatabase().start(dbInfo);

            Ingredient in1 = new Ingredient("In_Test1", "Kg", 50);
            Ingredient in2 = new Ingredient("In_Test2", "g", 50);

            List<Ingredient> inList = new List<Ingredient>();

            Recipe recipe = new Recipe(1, "RecipeTest", "Something", inList, 10);

            RecipeHandling rh = new RecipeHandling();

            rh.addRecipe(recipe, dbInfo);

            //_ = RecipeCrawl.GetRecipes(1,38482);

            //Console.WriteLine("web crawler begins... fear its power");
            //Console.ReadLine();
        }
    }
}
