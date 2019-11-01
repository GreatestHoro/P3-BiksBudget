using BBCollection;
using BBCollection.BBObjects;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BBGatherer.Webcrawler
{
    class RecipeCrawl
    {
        public static async Task GetRecipes(int start_page, int Last_page, DatabaseConnect dbConnect)
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

                if (!CheckIfPageFound(name, Beskrivels, ingredienser))
                {
                    Console.WriteLine("Cannot find recipie continues....");
                }
                else
                {
                    var response = HttpClient.GetAsync(url).Result;

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {

                        foreach (var ind in ingredienser)
                        {
                            if (!ind.InnerText.Contains(':'))
                            {
                                IngriedisensList.Add(CreateIngriedient(ind.InnerText));
                            }

                            //Console.WriteLine(ind.InnerText);
                        }

                        dbConnect.AddRecipe(new Recipe
                            (i, name.ElementAt<HtmlNode>(0).InnerText,
                            Beskrivels.ElementAt<HtmlNode>(0).InnerText,
                            IngriedisensList,
                            CleanUpPerPerson(PerPerson)));
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
            String name = DeterminName(ind).Trim();

            if () 
            {
            
            }

            return new Ingredient(name, unit, amount);
        }

        public static float DeterminAmount(String ingrediens)
        {
            String[] SplitString = ingrediens.Split(' ');
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
            String ReturnString = "";
            for (int i = 2; i < SplitString.Length; i++)
            {
                ReturnString = ReturnString + " " + SplitString[i];

            }
            return ReturnString;
        }

        public static bool EvaluateName(String name) 
        {
        
        }

        public static bool CheckIfPageFound(HtmlNodeCollection name, HtmlNodeCollection beskrivels, HtmlNodeCollection ingredienser)
        {
            if (name == null || beskrivels == null || ingredienser == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
