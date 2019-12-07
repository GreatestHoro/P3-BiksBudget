using BBCollection;
using BBCollection.BBObjects;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using B3_BiksBudget.Webcrawler.Assisting_classes;

namespace B3_BiksBudget.Webcrawler
{
    class ImageRetriever
    {
        public struct RecepiesProduct 
        {
            public Recipe recipe;
            public Product Product;
        } 
        public abstract bool AssingItemImage(RecepiesProduct recepiesProduct,string url) { return false; }
        private async Task<string> GetImageUrl(string Name) 
        {
            String url = ("https://www.google.dk/search?q="+Name+"&sxsrf=ACYBGNQ4PfY5BhIxB_xA0-2uOBOLIunI8w:1575640228751&source=lnms&tbm=isch&sa=X&ved=2ahUKEwjsooOhlaHmAhXCLlAKHRo-CBUQ_AUoAXoECBQQAw&biw=1536&bih=751");
            return url;
        }

        private async Task<HtmlNodeCollection> GetImagePlacement(string url) 
        {
            HttpClient HttpClient = new HttpClient();
            string html = await HttpClient.GetStringAsync(url);
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            return htmlDocument.DocumentNode.SelectNodes("//span[@class][@itemprop='recipeIngredient']");
        }
    }

    class productImages : ImageRetriever
    {
        public override bool AssingItemImage(RecepiesProduct recepiesProduct,string url) 
        {
            recepiesProduct.Product._image = recepiesProduct.Product._image ?? url;

            return true;
        }
    }

    class recipeImages : ImageRetriever
    {
        public override bool AssingItemImage(RecepiesProduct recepiesProduct, string url)
        {
            recepiesProduct.recipe.image = recepiesProduct.recipe.image ?? url;
            return true;
        }
    }
}
