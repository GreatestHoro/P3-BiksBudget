using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net.Http;

using System.IO;
using B3_BiksBudget.BBObjects;

namespace B3_BiksBudget.Webcrawler
{
    class RecipeCrawl
    {
        public static async Task GetRecipes(int start_page, int Last_page)
        {
            List<Recipe> opskrifter = new List<Recipe>(); //The list that holdes the recipies

            for (int i = start_page; i <= Last_page; i++) //loop that goes from the first page to the last page
            {
                String url = ("https://www.dk-kogebogen.dk/opskrifter/" + i + "/");
                HttpClient HttpClient = new HttpClient();
                string html = await HttpClient.GetStringAsync(url);
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                List<Ingredient> IngriedisensList = new List<Ingredient>();

                HtmlNodeCollection ingredienser = htmlDocument.DocumentNode.SelectNodes("//span[@class][@itemprop]");
                HtmlNodeCollection PerPerson = htmlDocument.DocumentNode.SelectNodes("//span[@itemprop='recipeYield']");
                HtmlNodeCollection Beskrivels = htmlDocument.DocumentNode.SelectNodes("//div[@itemprop]");
                HtmlNodeCollection name = htmlDocument.DocumentNode.SelectNodes("//center");

                Console.WriteLine(CleanUpPerPerson(PerPerson));
                /*if (PerPerson == null)
                {
                    _perPerson = "null";
                    //This is only used in one case when crawling specefickly recipe 1028
                }
                else 
                {
                    _perPerson = PerPerson.ElementAt<HtmlNode>(0).InnerText;
                }*/

                if (Beskrivels.ElementAt<HtmlNode>(0).InnerText.Length == 0)
                {
                    Console.WriteLine("Cannot find recipie continues....");
                }
                else
                {
                    //if (i % 100 == 0)
                    //{
                    Console.WriteLine(i);

                    //}

                    var response = HttpClient.GetAsync(url).Result;

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {

                        foreach (var ind in ingredienser)
                        {
                            IngriedisensList.Add(CreateIngriedient(ind.InnerText));

                            /*testIngriedisens.Add(ind.InnerText);
                            string[] words = ind.InnerText.Split(' ');
                            if (int.TryParse(words[0], out int value))
                            {
                                //Console.WriteLine("Amount: " + words[0] + " " + words[1] + " and Type: " + words[2]);

                            }
                            else
                            {
                                //Console.WriteLine("This is the end");
                            }*/
                        }
                        opskrifter.Add(new Recipe
                            (i,name.ElementAt<HtmlNode>(0).InnerText,
                            Beskrivels.ElementAt<HtmlNode>(0).InnerText,
                            IngriedisensList,
                            CleanUpPerPerson(PerPerson)));
                        /*Console.WriteLine(name.ElementAt<HtmlNode>(0).InnerText);
                        Console.WriteLine(CleanUpPerPerson(PerPerson.ElementAt<HtmlNode>(0).InnerText));*/
                        foreach (Ingredient ind in IngriedisensList) 
                        {
                            Console.WriteLine(ind._IngredientName);
                            Console.WriteLine(ind._Amount);
                            Console.WriteLine(ind._unit);
                        }

                    }
                    else
                    {
                        Console.WriteLine("Connection failed");
                    }
                }
            }
            Console.WriteLine("Procces finished");

        }

        public static float CleanUpPerPerson(HtmlNodeCollection _PerPerson)
        {
            if (_PerPerson != null)
            {
                String PerPerson = _PerPerson.ElementAt<HtmlNode>(0).InnerText;
                String cleanUp = "";
                float numb;
                String[] characters = PerPerson.Split(' ', '&', '-');
                foreach (String c in characters)
                {
                    if (float.TryParse(c, out numb))
                    {
                        return numb;
                    }
                }
                return float.Parse(cleanUp);
            }
            else 
            {
                return 4;
            }
 
        }

        public static Ingredient CreateIngriedient(String ind)
        {
            float amount = DeterminAmount(ind);
            String unit = DeterminUnit(ind);
            String name = DeterminName(ind);

            return new Ingredient(name,unit,amount);
        }

        public static float DeterminAmount(String ingrediens)
        {
            String[] SplitString= ingrediens.Split(' ');
            float Amount;
            foreach (String part in SplitString)
            {
                if (float.TryParse(part, out Amount))
                {
                    return Amount;
                }
            }
            return 0;
        }

        public static String DeterminUnit(String ingrediens)
        {
            String[] SplitString = ingrediens.Split(' ');
            return SplitString[1];
        }

        public static String DeterminName(String ingrediens) 
        {
            String[] SplitString = ingrediens.Split(' ');
            return SplitString[2];
        }





    }
}
