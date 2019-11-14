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
                            if (!ind.InnerText.Contains(':'))
                            {
                                Ingredient ingredient = CreateIngriedient(ind.InnerText, out fatalError, dbConnect);
                                if (!ingredient._ingredientName.Equals("none")) 
                                {
                                    IngriedisensList.Add(ingredient);
                                }
                            }
                            if (fatalError) { break; }
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



        private Ingredient CreateIngriedient(String ind,out bool fatalError, DatabaseConnect dbConnect)
        {
            float amount = DeterminAmount(ind);
            String unit = DeterminUnit(ind);
            String name = DeterminName(ind).Trim();
            name = NameCleanUp(name);
            Console.WriteLine("INPUT: "+name);
            
            if (!String.IsNullOrWhiteSpace(name.Trim()))
            {
                name = CheckForValidIndgredients(name, dbConnect, out fatalError);
                name = EdgeCaseCleanUp(name);
                Console.WriteLine("OUTPUT: " + name);
                fatalError = EvaluateName(name);
            }
            else 
            {
                fatalError = false;
                name = "none";
            }

            return new Ingredient(name.Trim(), unit, amount);
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
            return CheckCOOPProductsInDatabase(Searchterm, dbConnect) || CheckSallingProductsInDatabase(Searchterm,dbConnect);
        }

        private bool CheckCOOPProductsInDatabase(String Searchterm, DatabaseConnect dbConnect) 
        {
            List<Product> Products = dbConnect.GetProducts(Searchterm);

            return Products.Count != 0 ? true : false;
        }

        private bool CheckSallingProductsInDatabase(String Searchterm, DatabaseConnect dbConnect) 
        {
            List<SallingProduct> sallingP = dbConnect.GetSallingProduct(Searchterm);
            return sallingP.Count != 0 ? true : false;
        }

        private bool CheckIngredientsInApi(string Searchterm, DatabaseConnect dbConnect) 
        {
            System.Threading.Thread.Sleep(4000);
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
            
            catch (System.Net.WebException)
            {
               Console.WriteLine("Exception found is being handled");
               System.Threading.Thread.Sleep(30000);
               Console.WriteLine("Exception resolved");
                return false;
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
            List<char> _char = new List<char>() {',', '.', '+', '-', '!', '?','&','%'};
            List<string> splitString = new List<string>() {"eller","i","med"};
            List<string> removeSub = new List<string>() { "evt" };

            name = RemoveParentheses(name);
            foreach (string s in splitString) 
            {
                name = RemoveEverythingAfter(name, s);
            }
            foreach (char c in _char)
            {
                name = RemoveCharFromString(name, c);
            }
            foreach (string s in removeSub)
            {
                name = RemoveSubstring(name, s);
            }

            
            
     
            name = name.ToLower();

            return name.Trim();

        }

        private String EdgeCaseCleanUp(String name) 
        {
            name = new String(name.Where(c => c != '-' && (c < '0' || c > '9')).ToArray());
            List<String> Substring = new List<string>() {"u","dl", "ca", "á", "g", "kg", "til"};
            foreach (String s in Substring)
            {
                name = RemoveSubstring(name.Trim(), s);
            }

            if (name.Trim().Equals("i tern") || name.Trim().Equals("i") || name.Trim().Equals("med")) 
            {
                name = "";
            }
            return name;
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
            String[] Words = name.Split(" ");
            string ReturnString = "";
            foreach (string w in Words) 
            {
                if (!w.Equals(substring))
                {
                    ReturnString = ReturnString + w +" ";
                }
            }

            return ReturnString;
        }

        private string RemoveEverythingAfter(string _string, string RemoveStart) 
        {
            String[] Words = _string.Split(" ");
            String _return = "";
            bool KeyFlag = false;
            foreach (String s in Words) 
            {
                if (s.Equals(RemoveStart)) 
                {
                    KeyFlag = true;
                }
                if (!KeyFlag) 
                {
                    _return = _return + s + " ";
                }
            }
            return _return;
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

        private String RemoveCharFromString(string _string, char _char) 
        {
            Char[] charArray = _string.ToCharArray();
            string returnString = "";

            foreach (Char c in charArray) 
            {
                if (c != _char) 
                {
                    returnString = returnString + c;
                }
            }

            return returnString;
        }
        #endregion

        #region(Process critical checks)
        private bool EvaluateName(String name)
        {
            String[] SplitString = name.Split(" ");
            bool fatalError = false;

            if (SplitString.Length > 4 && !fatalError)
            {
                fatalError = true;
            }
            else
            {
                fatalError = false;
            }

            if (name.Equals("") && !fatalError)
            {
                fatalError = true;
            }
            else 
            {
                fatalError = false;
            }

            if (String.IsNullOrWhiteSpace(name.Trim()) && !fatalError)
            {
                fatalError = true;
            }
            else
            {
                fatalError = false;
            }
            
            return fatalError;
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
        #endregion
    }
    
}
