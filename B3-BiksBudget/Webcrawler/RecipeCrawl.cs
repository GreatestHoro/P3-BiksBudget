using BBCollection;
using BBCollection.BBObjects;
using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BBGatherer.StoreApi.SallingApi;
using BBGatherer.StoreApi;



namespace BBGatherer.Webcrawler
{
    class RecipeCrawl
    {
        public async Task GetRecipes(int start_page, int Last_page, DatabaseConnect dbConnect)
        {
            List<Recipe> opskrifter = new List<Recipe>(); //The list that holdes the recipies

            for (int i = start_page; i <= Last_page; i++) //loop that goes from the first page to the last page
            {
                String url = ("https://www.dk-kogebogen.dk/opskrifter/" + i + "/");
                HttpClient HttpClient = new HttpClient();
                string html = await HttpClient.GetStringAsync(url);
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);
                bool validity;
                bool fatalError = false;


                List<Ingredient> IngriedisensList = new List<Ingredient>();

                HtmlNodeCollection ingredienser = htmlDocument.DocumentNode.SelectNodes("//span[@class][@itemprop]");
                HtmlNodeCollection PerPerson = htmlDocument.DocumentNode.SelectNodes("//span[@itemprop='recipeYield']");
                HtmlNodeCollection Beskrivels = htmlDocument.DocumentNode.SelectNodes("//div[@itemprop]");
                HtmlNodeCollection name = htmlDocument.DocumentNode.SelectNodes("//center");

                Console.WriteLine(i);
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
                                Ingredient ingredient = CreateIngriedient(ind.InnerText, out validity, out fatalError,dbConnect);
                                if (validity)
                                {
                                    IngriedisensList.Add(ingredient);
                                }
                            }
                            if (fatalError) { break; }
                            //Console.WriteLine(ind.InnerText);
                        }

                        if (!fatalError)
                        {
                            dbConnect.AddRecipe(new Recipe
                            (i, name.ElementAt<HtmlNode>(0).InnerText,
                            Beskrivels.ElementAt<HtmlNode>(0).InnerText,
                            IngriedisensList,
                            CleanUpPerPerson(PerPerson)));
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

        private float CleanUpPerPerson(HtmlNodeCollection _PerPerson)
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

        private Ingredient CreateIngriedient(String ind, out bool validity, out bool fatalError, DatabaseConnect dbConnect)
        {
            float amount = DeterminAmount(ind);
            String unit = DeterminUnit(ind);
            String name = DeterminName(ind).Trim();
            name = NameCleanUp(name);
           /* name =*/ CheckForValidIndgredients("hakkede krydderurter yeet", dbConnect);

            validity = EvaluateName(name, out fatalError);

            return new Ingredient(name, unit, amount);
        }
        private void CheckForValidIndgredients(String name, DatabaseConnect dbConnect)
        {
            String[] str = name.Split(" ");
            int CombinationSize = 1;
            int Start = 0;

            while (CombinationSize <= str.Length) 
            {
                while (Start <= str.Length - CombinationSize)
                {
                    Console.WriteLine(GetCombination(str, CombinationSize, Start++));
                }
                Start = 0;
                CombinationSize++;
            }
            
            Console.WriteLine("finish");
            
            

        }
        private string GetCombination(string[] str, int size,int i) 
        {
            String ReturnString = "";
            foreach (String word in Combination(str, size, i))
            {
                ReturnString = ReturnString +" "+ word;
            }
            return ReturnString;
        }

        private List<String> Combination(string[] str, int size, int i) 
        {
            List<String> Comb = new List<string>();
            List<String> ReturnArray = new List<string>();
            if (size == 0)
            { 
                return ReturnArray;
            }
            else
            {
                ReturnArray.Add(str[i]);
                
                Comb = Combination(str, --size, ++i);
                foreach (String strList in Comb) 
                {
                    ReturnArray.Add(strList);
                }
                return ReturnArray;

            }
        }
        private bool CheckIngredient(String Searchterm,DatabaseConnect dbConnect) 
        {
            List<Product> Products = dbConnect.GetProducts(Searchterm);
            bool IfExist = false;
            if (Products.Count != 0) 
            {
                IfExist = true;
            }
            return IfExist;
        }

        private float DeterminAmount(String ingrediens)
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

        private String DeterminUnit(String ingrediens)
        {
            String[] SplitString = ingrediens.Split(' ');
            return SplitString[1];
        }

        private String DeterminName(String ingrediens)
        {
            String[] SplitString = ingrediens.Split(' ');
            String ReturnString = "";
            for (int i = 2; i < SplitString.Length; i++)
            {
                ReturnString = ReturnString + " " + SplitString[i];

            }
            return ReturnString;
        }

        private bool EvaluateName(String name,out bool fatalError) 
        {
            //Subject to big changes
            String[] SplitString = name.Split(" ");
            if (SplitString.Length > 2)
            {
                //Console.WriteLine(name +" "+ SplitString.Length);
                fatalError = true;
            }
            else 
            {
                fatalError = false;
            }

            if (String.IsNullOrWhiteSpace(name))
            {
                return false;
            }
            else 
            {
                return true;
            }
        }

        private String NameCleanUp(String name) 
        {
            name = RemoveParentheses(name);
            name = RemoveSubstring(name, "evt. ");
            name = name.ToLower();

            return name.Trim();

        }
        private string RemoveSubstring(String name,String substring) 
        {
            if (name.Contains(substring)) 
            {
                name.Replace(substring, "");
            }
            return name;
        }

        private String RemoveParentheses(string name) 
        {
            char[] chars = name.ToCharArray();
            bool ParenthesesFlag = false;
            String _string = "";

            foreach (char character in chars) 
            {
                if (ParenthesesFlag)
                {
                    if (character.Equals(')')) 
                    {
                        ParenthesesFlag = false;
                    }
                }
                else if (character.Equals('('))
                {
                    ParenthesesFlag = true;
                }
                else 
                {
                    _string += character;
                }
            }
            return _string;
        }

        private bool CheckIfPageFound(HtmlNodeCollection name, HtmlNodeCollection beskrivels, HtmlNodeCollection ingredienser)
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
