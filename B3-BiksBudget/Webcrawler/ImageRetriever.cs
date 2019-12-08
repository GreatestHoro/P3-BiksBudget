using BBCollection;
using BBCollection.BBObjects;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Drawing;
using System.Net;
using System.IO;
using B3_BiksBudget.Webcrawler.Assisting_classes;

namespace BBGatherer.Webcrawler
{
    abstract class ImageRetriever
    {
        string searchterm;

        public async Task<List<string>> GetImageUrls(string searchterm)
        {
            this.searchterm = searchterm;
            string url = CreateLink(assembleSearchName(searchterm));
            HtmlNodeCollection nodes = GetImagePlacement(url).Result;
            List<string> imageLinks = ExtractImageURL(nodes);
            
            return imageLinks;
        }

        public abstract bool AssingItemImage(RecepieProductHelper recepiesProduct);
        private string CreateLink(string Name)
        {
            return ("https://www.google.dk/search?q=" + assembleSearchName(Name) + "&sxsrf=ACYBGNQ4PfY5BhIxB_xA0-2uOBOLIunI8w:1575640228751&source=lnms&tbm=isch&sa=X&ved=2ahUKEwjsooOhlaHmAhXCLlAKHRo-CBUQ_AUoAXoECBQQAw&biw=1536&bih=751");
        }

        private string assembleSearchName(string name)
        {
            return name.Replace(" ", "+");
        }

        private string assemblefilename(string name)
        {
            return name.Replace(" ", "_");
        }
        private async Task<HtmlNodeCollection> GetImagePlacement(string url)
        {
            HttpClient HttpClient = new HttpClient();
            string html = await HttpClient.GetStringAsync(url);
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            Console.WriteLine(htmlDocument);
            return htmlDocument.DocumentNode.SelectNodes("/html[1]/body[1]/table[1]/tbody[1]/tr[1]");
        }

        private List<string> ExtractImageURL(HtmlNodeCollection htmlNodes)
        {
            string Innerhtml = htmlNodes.ElementAt<HtmlNode>(0).InnerHtml;
            string identifier = "encrypted";
            List<string> imageLinks = ExtractLinks(Innerhtml);

            imageLinks = imageLinks.Where(x => x.Contains(identifier)).ToList();
            imageLinks = LinkCleanUp(imageLinks);

            return imageLinks;
        }

        private List<string> ExtractLinks(string str)
        {
            string identifier = "https";
            List<string> elementes = str.Split(" ").ToList();

            elementes = elementes.Where(x => x.Contains(identifier)).ToList();

            return elementes;
        }
        private List<string> LinkCleanUp(List<string> Links)
        {
            List<string> ReturnLinks = new List<string>();
            foreach (string link in Links)
            {
                ReturnLinks.Add(RemoveOutside(link, '"'));
            }

            return ReturnLinks;
        }

        private string RemoveOutside(string link, char limiter)
        {
            bool Flag = false;
            List<char> returnstring = new List<char>();
            foreach (char charater in link)
            {
                if (charater.Equals(limiter))
                {
                    Flag = toggelFlag(Flag);
                }
                else if (Flag)
                {
                    returnstring.Add(charater);
                }
            }

            return new string(returnstring.ToArray());
        }

        private bool toggelFlag(bool Flag)
        {
            return !Flag;
        }

        public void SaveImagesFromLink(List<string> SortedLinks) 
        {
            int i = 0;
            WebClient client = new WebClient();
            foreach (string links in SortedLinks)
            {
                Stream stream = client.OpenRead(links);
                Bitmap bitmap; bitmap = new Bitmap(stream);

                if (bitmap != null)
                {
                    bitmap.Save(@"C:\Users\jeppe\Desktop\Images_BBGathere\"+ assemblefilename(searchterm) +i+++".jpg");
                }

                stream.Flush();
                stream.Close();
                client.Dispose();
            }
        }
    }

    class productImages : ImageRetriever
    {
        public override bool AssingItemImage(RecepieProductHelper recepiesProduct)
        {
            string url = recepiesProduct.GetImage() ?? GetImageUrls(recepiesProduct.GetName()).Result.First();
            recepiesProduct.UpdateImage(url);
            return true;
        }
    }

    class recipeImages : ImageRetriever
    {
        public override bool AssingItemImage(RecepieProductHelper recepiesProduct)
        {
            string url = recepiesProduct.GetImage() ?? GetImageUrls(recepiesProduct.GetName()).Result.First();
            recepiesProduct.UpdateImage(url);
            return true;
        }
    }
}