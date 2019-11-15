using BBCollection;
using BBCollection.BBObjects;
using BBGatherer.StoreApi;
using BBGatherer.StoreApi.SallingApi;
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
                bool fatalError = false;


                List<Ingredient> IngriedisensList = new List<Ingredient>();

                HtmlNodeCollection ingredienser = htmlDocument.DocumentNode.SelectNodes("//span[@class][@itemprop='recipeIngredient']");
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
                            //Console.WriteLine(ind.InnerText);
                            if (!ind.InnerText.Contains(':'))
                            {
                                Ingredient ingredient = CreateIngriedient(ind.InnerText, out fatalError, dbConnect);
                                    IngriedisensList.Add(ingredient);
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
                        else 
                        {
                            Console.WriteLine("Recipie determined false..");
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



        private Ingredient CreateIngriedient(String ind, out bool fatalError, DatabaseConnect dbConnect)
        {
            float amount = DeterminAmount(ind);
            String unit = DeterminUnit(ind);
            String name = DeterminName(ind).Trim();
            name = NameCleanUp(name);
            
            name = CheckForValidIndgredients(name, dbConnect,out fatalError);

            

            return new Ingredient(name, unit, amount);
        }
        #region(Check if Indgredients)
        private String CheckForValidIndgredients(String name, DatabaseConnect dbConnect, out bool fatalError)
        {
            fatalError = false;
            String[] str = name.Split(" ");
            List<String> AllMacthes = new List<string>();
            AllMacthes.Add("");
            String IndgrdientName;
            bool IngredientFlag = false;
            int CombinationSize = 1;
            int Start = 0;
            
            while (CombinationSize <= str.Length)
            {
                while (Start <= str.Length - CombinationSize)
                {
                    IndgrdientName = GetCombination(str, CombinationSize, Start++);
                    IngredientFlag = CheckIngredient(IndgrdientName,dbConnect);

                    if (IngredientFlag)
                    {
                        AllMacthes.Add(IndgrdientName);
                    }
                }
                Start = 0;
                CombinationSize++;
            }
            if (AllMacthes.Count() == 0)
            {
                fatalError = true;
            }
            Console.WriteLine(AllMacthes[AllMacthes.Count-1]);
            return AllMacthes[AllMacthes.Count - 1];
        }
        private string GetCombination(string[] str, int size, int i)
        {
            String ReturnString = "";
            foreach (String word in Combination(str, size, i))
            {
                ReturnString = ReturnString + " " + word;
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
        private bool CheckIngredient(String Searchterm, DatabaseConnect dbConnect)
        {
            return   CheckIngredientInDatabase(Searchterm, dbConnect) || CheckIngredientsInApi(Searchterm.Trim(),dbConnect);
        }

        private bool CheckIngredientInDatabase(String Searchterm, DatabaseConnect dbConnect)
        {
            return CheckCOOPProductsInDatabase(Searchterm, dbConnect);// || CheckSallingProductsInDatabase(Searchterm, dbConnect);
        }

        private bool CheckCOOPProductsInDatabase(String Searchterm, DatabaseConnect dbConnect) 
        {
            List<Product> Products = dbConnect.GetProducts(Searchterm);

            return Products.Count != 0 ? true : false;
        }

        //private bool CheckSallingProductsInDatabase(String Searchterm, DatabaseConnect dbConnect)
        //{
        //    List<SallingProduct> sallingP = dbConnect.GetSallingProduct(Searchterm);
        //    return sallingP.Count != 0 ? true : false;
        //}

        private bool CheckIngredientsInApi(string Searchterm, DatabaseConnect dbConnect) 
        {
            System.Threading.Thread.Sleep(2000);
            BearerAccessToken bearerAccessToken = new BearerAccessToken("5c9040ad-6229-477f-8123-64d281c76768");


            SallingAPILink linkMaker = new SallingAPILink();
            SallingAPIProductSuggestions productSuggestions = new SallingAPIProductSuggestions();
            string apiLink = linkMaker.GetProductAPILink(Searchterm);
            OpenHttp<SallingAPIProductSuggestions> openHttp;


            try
            {
                openHttp = new OpenHttp<SallingAPIProductSuggestions>(apiLink, bearerAccessToken.GetBearerToken());
                productSuggestions = openHttp.ReadAndParseAPISingle();
                foreach (var p in productSuggestions.Suggestions)
                {
                    dbConnect.AddSallingProduct(new SallingProduct(p.title,p.id,p.prod_id,p.price,p.description,p.link,p.img));
                }
            }
            catch(System.Net.WebException)
            {
               Console.WriteLine("Exception found is being handled");
               System.Threading.Thread.Sleep(30000);
               Console.WriteLine("Exception resolved");
               return CheckIngredientsInApi(Searchterm,dbConnect);
            }

            
            return productSuggestions.Suggestions.Count != 0?true:false;
            }
        #endregion

        #region(Determin ingredient values)
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
        #endregion

        #region(String Cleanups)
        private String NameCleanUp(String name) 
        {
            name = RemoveParentheses(name);
            name = RemoveSubstring(name, "evt. ");
            //name = RemoveSubstring(name, "med ");
            name = name.ToLower();

            return name.Trim();

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
        #endregion

        #region(remove)
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
        #endregion

        #region(Process critical checks)
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
        #endregion
    }
    
}
