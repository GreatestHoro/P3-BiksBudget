using BBCollection;
using BBCollection.BBObjects;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BBCollection.StoreApi;
using BBCollection.StoreApi.ApiNeeds;
using BBCollection.StoreApi.SallingApi;
using BBCollection.DBHandling;
using BBCollection.DBConncetion;
using B3_BiksBudget.Webcrawler.Assisting_classes;


namespace BBGatherer.Webcrawler
{
    class RecipeCrawl
    {
        #region Webcrawler
        public async Task GetRecipes(int start_page, int Last_page, DatabaseConnect dbConnect)
        {
            AssistingClasses functionality = new AssistingClasses();

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

                                Ingredient ingredient = CreateIngriedient(ind.InnerText, out fatalError, dbConnect, functionality);
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
                            functionality.getCleanFunc().CleanUpPerPerson(PerPerson)));
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
        #endregion

        #region ingrdient Creation
        private Ingredient CreateIngriedient(String ind,out bool fatalError, DatabaseConnect dbConnect, AssistingClasses functionality)
        {
            float amount = functionality.getDetermin().DeterminAmount(ind);
            String unit = functionality.getDetermin().DeterminUnit(ind);
            String name = functionality.getDetermin().DeterminName(ind).Trim();

            name = nameEditing_Evalution(name,out fatalError,dbConnect, functionality);

            return new Ingredient(name.Trim(), unit, amount);
        }

        private string nameEditing_Evalution(string name,out bool fatalError,DatabaseConnect dbConnect, AssistingClasses functionality) 
        {
            if (!String.IsNullOrWhiteSpace(name.Trim()))
            {
                name = functionality.getCleanFunc().NameCleanUp(name);
                Console.WriteLine("INPUT: " + name);
                name = functionality.getRefs().CheckForValidIndgredients(name, dbConnect, out fatalError);
                if (!fatalError)
                {
                    name = functionality.getCleanFunc().EdgeCaseCleanUp(name);
                    Console.WriteLine("OUTPUT: " + name);
                    fatalError = EvaluateName(name);
                }
            }
            else
            {
                fatalError = false;
                name = "none";
            }

            return name;
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
