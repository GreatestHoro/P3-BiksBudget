using BBCollection;
using BBCollection.BBObjects;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using B3_BiksBudget.Webcrawler.Assisting_classes;


namespace BBGatherer.Webcrawler
{
    class RecipeCrawl
    {
        #region Webcrawler
        /// <summary>
        /// A webcrawler specefictly made to crawl the web site https://www.dk-kogebogen.dk/, i findes the diffrent elements of a recepie page,
        /// such as name of the recipie, ingredients and the desribtion of the dish.
        /// It will also do some prossecing on each found element to clean them up and make them easier to workd with.
        /// </summary>
        /// <param name="start_page">The starting recepie on the website</param>
        /// <param name="Last_page">The last recepie on the web site it should check</param>
        /// <param name="dc">The database that should get the finished recipies loaded into</param>
        /// <returns>It just returns a task as everything is inputed directly into the database while it runs</returns>
        public async Task GetRecipes(int start_page, int Last_page, DatabaseConnect dc)
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

                                Ingredient ingredient = CreateIngriedient(ind.InnerText, out fatalError, dc, functionality);
                                if (!ingredient._ingredientName.Equals("none"))
                                {
                                    IngriedisensList.Add(ingredient);
                                }
                            }
                            if (fatalError) { break; }
                        }

                        if (!fatalError)
                        {
                            await dc.Recipe.AddList(new Recipe
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

        #region ingredient Creation
        /// <summary>
        /// This method Creates and instans of the ingredient object,
        /// it calls method from diffrent classe to attempts to intrepret and edit the names to become more clear.
        /// </summary>
        /// <param name="ind">The raw ingrdient string with no editing or crossrefresing done</param>
        /// <param name="fatalError">A bool which is triggered if the ingredient is determined uninterpretable</param>
        /// <param name="dbConnect">Instace of the database used to save and edit the database</param>
        /// <param name="functionality">A instance of a classe which contains many of the nessesary funtionalitys for the method</param>
        /// <returns>Return a new instance</returns>
        private Ingredient CreateIngriedient(string ind,out bool fatalError, DatabaseConnect dbConnect, AssistingClasses functionality)
        {
            float amount = functionality.getDetermin().DeterminAmount(ind);
            String unit = functionality.getDetermin().DeterminUnit(ind);
            String name = functionality.getDetermin().DeterminName(ind).Trim();

            name = nameEditing_Evalution(name,out fatalError,dbConnect, functionality);

            return new Ingredient(name.Trim(), unit, amount);
        }

        /// <summary>
        /// Method containnig the funtionality that evalute and edite the ingredient name.
        /// </summary>
        /// <param name="name">The raw name</param>
        /// <param name="fatalError">A bool which is triggered if the ingredient is determined uninterpretable</param>
        /// <param name="dbConnect">Instace of the database used to save and edit the database</param>
        /// <param name="functionality">A instance of a classe which contains many of the nessesary funtionalitys for the method</param>
        /// <returns>Returns a string that will be used as the ingrediient name</returns>
        private string nameEditing_Evalution(string name,out bool fatalError,DatabaseConnect dbConnect, AssistingClasses functionality)
        {
            if (!String.IsNullOrWhiteSpace(name.Trim()))
            {
                name = functionality.getCleanFunc().NameCleanUp(name);
                //Console.WriteLine("INPUT: " + name);
                name = functionality.getRefs().CheckForValidIndgredients(name, out fatalError);
                if (!fatalError)
                {
                    name = functionality.getCleanFunc().EdgeCaseCleanUp(name);
                    //Console.WriteLine("OUTPUT: " + name);
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
        /// <summary>
        /// Determines if the finished name is valid.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Gives a true when the name is invalid</returns>
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

        /// <summary>
        /// This method checks if a connection has been established and that the correct elements are present inside it
        /// </summary>
        /// <param name="name">raw html data from the section that contains the name of the recipie</param>
        /// <param name="beskrivels">raw html data from the section that contains the desripbtion of the recipie</param>
        /// <param name="ingredienser">raw html data from the section that contains the ingredients of the recipie</param>
        /// <returns>False means that the webcrawler could not find the nessesary elements on the page</returns>
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
