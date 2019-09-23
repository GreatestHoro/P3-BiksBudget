using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net.Http;
using System.IO;


namespace BiksBudget
{
    class Program
    {
        

        static void Main(string[] args)
        {
            _ = GetOpskrift();
            Console.WriteLine("end of yet");
            Console.ReadLine();
        }

        private static async Task GetOpskrift()
        {
            
            List<Opskrift> opskrifter = new List<Opskrift>();
            
            for (int i = 1; i <= 38482; i++)
            {
            hej:
                //int i = 456;
                var url = ("https://www.dk-kogebogen.dk/opskrifter/"+ i +"/");
                HttpClient HttpClient = new HttpClient();
                string html = await HttpClient.GetStringAsync(url);
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);
                List<string> testIngriedisens = new List<string>();

                var ingredienser = htmlDocument.DocumentNode.SelectNodes("//span[@class][@itemprop]");
                var Beskrivels = htmlDocument.DocumentNode.SelectNodes("//div[@itemprop]");
                var name = htmlDocument.DocumentNode.SelectNodes("//center");
                if (Beskrivels.ElementAt<HtmlNode>(0).InnerText.Length == 0)
                {
                    i++;
                    goto hej;
                }
                
                Console.WriteLine(ingredienser.Count+ " " + i + " item");
                var response = HttpClient.GetAsync(url).Result;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    
                    int k = 0;
                    foreach (var ind in ingredienser)
                    {
                        testIngriedisens.Add(ind.InnerText);
                    }
                    opskrifter.Add(new Opskrift(name.ElementAt<HtmlNode>(0).InnerText, Beskrivels.ElementAt<HtmlNode>(0).InnerText, testIngriedisens));
                    
                }
                else
                {
                    Console.WriteLine("Connection failed");
                }
            }
            Opskrift.SaveRecipie(opskrifter);
            Console.WriteLine("yeet");
        }
    }
}
