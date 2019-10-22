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


namespace BiksBudget
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseInformation dbInfo = new DatabaseInformation("localhost", "JJBB", "root", "Bjarke05!");
            new InitializeDatabase().start(dbInfo);


            //_ = recipecrawl.getrecipes(1, 38482);

            //console.writeline("web crawler begins... fear its power");
            //console.readline();
        }
    }
}
