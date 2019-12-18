using BBCollection;
using BBGatherer.Webcrawler.BilkaToGo;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BBGatherer.Webcrawler.BilkaToGo
{
    public class LinkCreation
    {
        readonly DatabaseConnect _dc = new DatabaseConnect();
        
        public void IterateOverStringParallel(String alphabet)
        {
            Parallel.ForEach(alphabet, new ParallelOptions { MaxDegreeOfParallelism = 4 }, (c) =>
            {
                IWebDriver driver = new WebDriver(false).Driver();
                driver.Navigate().GoToUrl("https://www.bilkatogo.dk/");
                CrawlLetter(c, driver);
                driver.Close();
            });
        }

        private void CrawlLetter(char c, IWebDriver driver)
        {
            Console.WriteLine("Crawling: " + c);
            Dictionary<int, string> productLinks = new Dictionary<int, string>();
            IWebElement searchInput = driver.FindElement(By.XPath("//input[@type='search']"));
            searchInput.SendKeys($"{c}");

            IWebElement searchButton = driver.FindElement(By.XPath("//button[@type='submit']"));
            searchButton.SendKeys(Keys.Enter);

            searchInput.SendKeys(Keys.Backspace);

            bool notAtEnd = true;
            while (notAtEnd)
            {
                try
                {
                    IWebElement moreProductsButton = driver.FindElement(By.XPath("//button[@class='btn btn-dark border-radius my-3']"));
                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    js.ExecuteScript("arguments[0].scrollIntoView();", moreProductsButton);
                    moreProductsButton.Click();
                }
                catch (NoSuchElementException e)
                {
                    Console.WriteLine("End of: " + c);
                    IList<IWebElement> webElements = driver.FindElements(By.XPath("//div[@data-productid]"));
                    AddLinksToDB(ProcessProductLinks(productLinks, webElements));
                    notAtEnd = false;
                }
            }
            driver.Navigate().Refresh();
        }

        private Dictionary<int, string> ProcessProductLinks(Dictionary<int, string> productLinks, IList<IWebElement> webElements)
        {
            foreach (IWebElement element in webElements)
            {
                int productID = Convert.ToInt32(element.GetAttribute("data-productid"));

                string[] name = element.Text.Split("\n");
                
                productLinks.Add(productID, name[0]);
            }
            return productLinks;
        }

        private void AddLinksToDB(Dictionary<int, string> productLinks)
        {
            foreach (KeyValuePair<int, string> kp in productLinks)
            {
                _dc.Product.AddLink(kp.Key, CreateLink(kp.Key, kp.Value));
            }

        }

        private string CreateLink(int productID, string productName)
        {
            return $"https://www.bilkatogo.dk/produkt/{productName.Replace("-", "_").Replace(" ", "-")}/{productID}/";
        }
    }
}
