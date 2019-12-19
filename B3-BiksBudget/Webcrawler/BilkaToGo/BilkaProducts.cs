using BBCollection;
using BBCollection.BBObjects;
using OpenQA.Selenium;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BBGatherer.Webcrawler.BilkaToGo
{
    class BilkaProducts
    {
        ConcurrentDictionary<int, string> _relevantProducts = new ConcurrentDictionary<int, string>();
        DatabaseConnect _dc = new DatabaseConnect();

        public async Task Retrieve()
        {
            await ConfirmRelevance();
            await Task.Run(() => InitiateCrawl());
        }

        private async Task ConfirmRelevance()
        {
            foreach (string ingredientName in await RetrieveIngredientNames())
            {
                if (ingredientName.Length >= 4)
                {
                    Parallel.ForEach(await _dc.Product.GetRelevantLinksAsync(ingredientName), new ParallelOptions { MaxDegreeOfParallelism = 8 }, async (c) =>
                    {
                        if (!_relevantProducts.ContainsKey(c.Key))
                        {
                            string[] linkSplit = Regex.Split(c.Value, "/");
                            string productName = linkSplit[linkSplit.Length - 1].Replace("-", " ").Replace("_", "-");
                            bool exist = await _dc.Product.Exist(productName);
                            Console.WriteLine(c.Value);
                            _relevantProducts.TryAdd(c.Key, c.Value);
                        }
                    });
                }
            }
        }

        private async Task<List<string>> RetrieveIngredientNames()
        {
            return await _dc.Recipe.GetAllIngredientNames();
        }

        private void InitiateCrawl()
        {
            /*IWebDriver driver = new WebDriver(true).Driver();
            driver.Navigate().GoToUrl(@"https://www.bilkatogo.dk/produkt/NOCCO-BCAA-_-MELON-CRUSH-33CL/64711/");
            CrawlProduct(driver);
            driver.Close();*/


            Parallel.ForEach(_relevantProducts, new ParallelOptions { MaxDegreeOfParallelism = 4 }, (c) =>
            {
                try
                {
                    IWebDriver driver = new WebDriver(true).Driver();
                    driver.Navigate().GoToUrl(c.Value);
                    CrawlProduct(driver);
                    driver.Close();
                }
                catch (Exception e)
                {

                }
            });
        }

        private void CrawlProduct(IWebDriver driver)
        {
            IWebElement productInformation = driver.FindElement(By.XPath("//div[@class='bg-white product-page__box h-100']"));

            string[] productInfo = Regex.Split(productInformation.Text, "\r\n");

            IWebElement imgURL = driver.FindElement(By.XPath("//img[@class='img-fluid']"));

            string url = imgURL.GetAttribute("src");

            ProcessProduct(productInfo, url);
        }

        private void ProcessProduct(string[] productInfo, string url)
        {
            try
            {
                string id = "S" + Regex.Split(productInfo[5], ":")[1].Trim();
                string name = productInfo[0];
                string[] postAmount = Regex.Split(Regex.Split(productInfo[1], "/")[0].Trim(), " ");
                string amount = postAmount[0] + " " + postAmount[1];
                double price = Convert.ToDouble(productInfo[productInfo.Length - 2].Replace(" ", "."));
                _dc.Product.Add(new Product(id, name, amount, price, url, "Bilka"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
