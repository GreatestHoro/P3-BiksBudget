using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace FrontEnd2.Data
{
    class ShoppinlistFunctionality
    {
        public ShoppinlistFunctionality(string _get)
        {
            get = _get;
        }

        public string productString;
        string newProduct;
        HttpClient Http = new HttpClient();
        string get;
        public List<AddedProduct> itemList = new List<AddedProduct>();
        HttpResponseMessage response = new HttpResponseMessage();

        public async Task<HttpResponseMessage> GetProductsOnStart()
        {
            productString = await Http.GetStringAsync("https://localhost:44325/" + get);

            itemList = JsonConvert.DeserializeObject<List<AddedProduct>>(productString);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public double CompletePrice()
        {
            double result = 0;

            foreach (var item in itemList)
            {
                result += item.Price;
            }
            return result;
        }

        public async Task<HttpResponseMessage> AddProductToList(string name, string amount, double price)
        {
            // *Insert search method here*
            var response = new HttpResponseMessage();

            AddedProduct newItem = new AddedProduct() 
            {   Name = name,
                Amount = amount, 
                Price = price, 
                TimeAdded = DateTime.Now.ToString(), 
                State = "Full" 
            };

            itemList.Add(newItem);

            productString = JsonConvert.SerializeObject(itemList[itemList.Count - 1]);
            var content = new StringContent(productString, Encoding.UTF8, "application/json");
            response = await Http.PostAsync("https://localhost:44325/" + get, content);

            string result = response.Content.ReadAsStringAsync().Result;

            newProduct = string.Empty;

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async Task<HttpResponseMessage> AddProductToList(AddedProduct newItem, string dest)
        {
            var response = new HttpResponseMessage();

            productString = JsonConvert.SerializeObject(newItem);
            var content = new StringContent(productString, Encoding.UTF8, "application/json");
            response = await Http.PostAsync("https://localhost:44325/" + dest, content);

            string result = response.Content.ReadAsStringAsync().Result;

            newProduct = string.Empty;

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async Task<HttpResponseMessage> DeleteProduct(int id)
        {
            var response = new HttpResponseMessage();
            itemList.Remove(itemList.First(x => x.Id == id));

            int i = 1;
            foreach (var product in itemList)
            {
                product.Id = i;
                i++;
            }

            productString = JsonConvert.SerializeObject(itemList);
            var content = new StringContent(productString, Encoding.UTF8, "application/json");
            response = await Http.PostAsync("https://localhost:44325/" + get, content);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async void AddProductToStorage(int id, string dest)
        {
            foreach (var item in itemList)
            {
                if (item.Id == id)
                {
                    item.TimeAdded = DateTime.Now.ToString();
                    item.State = "Full";
                    response = await AddProductToList(item, dest);
                    //response = await DeleteProduct(id);
                    
                    break;
                }
            }

            //itemList.Remove(itemList.First(x => x.Id == id));
        }
    }
}
