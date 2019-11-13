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
        public ShoppinlistFunctionality(string _dest)
        {
            dest = _dest;
        }

        public string productString;
        string newProduct;
        HttpClient Http = new HttpClient();
        string dest;
        public List<AddedProduct> itemList = new List<AddedProduct>();
        HttpResponseMessage response = new HttpResponseMessage();

        public async Task<HttpResponseMessage> GetProductsOnStart()
        {
            productString = await Http.GetStringAsync("https://localhost:44325/" + dest);

            itemList = JsonConvert.DeserializeObject<List<AddedProduct>>(productString);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public double CompletePrice()
        {
            double result = 0;

            foreach (var item in itemList)
            {
                result += item.Price * item.AmountOfItem;
            }
            return result;
        }

        public async Task<HttpResponseMessage> AddProductToList(string name, string amount, double price)
        {
            // *Insert search method here*
            var response = new HttpResponseMessage();

            AddedProduct newItem = new AddedProduct()
            {
                Name = name,
                Amount = amount,
                Price = price,
                Id = itemList.Count()+1
            };

            itemList.Add(newItem);

            productString = JsonConvert.SerializeObject(itemList[itemList.Count - 1]);
            var content = new StringContent(productString, Encoding.UTF8, "application/json");
            response = await Http.PostAsync("https://localhost:44325/" + dest, content);

            string result = response.Content.ReadAsStringAsync().Result;

            newProduct = string.Empty;

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async Task<HttpResponseMessage> AddProductToList(AddedProduct newItem, string dest)
        {
            var response = new HttpResponseMessage();

            string Email = "Test";          // Email Should be Input!
            int EmailLength = Email.Length; // Email Length

            productString = JsonConvert.SerializeObject(newItem);
            productString = EmailLength.ToString() + "|" + Email + productString;
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
            response = await Http.PostAsync("https://localhost:44325/" + dest, content);

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

        // NEW AND IMPROVED CODE BELOW

        public async Task<HttpResponseMessage> OnStart()
        {
            productString = await Http.GetStringAsync("https://localhost:44325/" + dest);

            itemList = JsonConvert.DeserializeObject<List<AddedProduct>>(productString);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public double CalculatePrice()
        {
            double result = 0;

            foreach (var item in itemList)
            {
                result += item.Price * item.AmountOfItem;
            }
            return result;
        }

        public async Task<HttpResponseMessage> AddProductString(string name, string amount, double price, string email)
        {
            var response = new HttpResponseMessage();

            AddedProduct newItem = new AddedProduct()
            {
                Name = name,
                Amount = amount,
                Price = price,
                Id = itemList.Count() + 1
            };

            await AddProductItem(newItem, email);

            //itemList.Add(newItem);

            //productString = JsonConvert.SerializeObject(itemList[itemList.Count - 1]);
            //var content = new StringContent(productString, Encoding.UTF8, "application/json");
            //response = await Http.PostAsync("https://localhost:44325/" + dest, content);

            //string result = response.Content.ReadAsStringAsync().Result;

            //newProduct = string.Empty;

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async Task<HttpResponseMessage> AddProductItem(AddedProduct newItem, string email)
        {
            var response = new HttpResponseMessage();

            email = "Test";
            int EmailLength = email.Length;

            productString = JsonConvert.SerializeObject(newItem);

            productString = EmailLength.ToString() + "|" + email + productString;

            await SendToApi(productString);

            //var content = new StringContent(productString, Encoding.UTF8, "application/json");
            //response = await Http.PostAsync("https://localhost:44325/" + dest, content);

            //string result = response.Content.ReadAsStringAsync().Result;

            //newProduct = string.Empty;

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async Task<HttpResponseMessage> AddProductItem(AddedProduct newItem, string email, string newDest)
        {
            var response = new HttpResponseMessage();

            email = "Test";
            int EmailLength = email.Length;

            productString = JsonConvert.SerializeObject(newItem);

            productString = EmailLength.ToString() + "|" + email + productString;

            await SendToApi(productString, newDest);

            //var content = new StringContent(productString, Encoding.UTF8, "application/json");
            //response = await Http.PostAsync("https://localhost:44325/" + dest, content);

            //string result = response.Content.ReadAsStringAsync().Result;

            //newProduct = string.Empty;

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async void AddList(List<AddedProduct> list, string email)
        {
            var response = new HttpResponseMessage();

            email = "Test";
            int EmailLength = email.Length;

            productString = JsonConvert.SerializeObject(list);

            productString = EmailLength.ToString() + "|" + email + productString;

            await SendToApi(productString);
        }

        async Task<HttpResponseMessage> SendToApi(string productString)
        {
            var content = new StringContent(productString, Encoding.UTF8, "application/json");
            response = await Http.PostAsync("https://localhost:44325/" + dest, content);

            string result = response.Content.ReadAsStringAsync().Result;

            newProduct = string.Empty;

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        async Task<HttpResponseMessage> SendToApi(string productString, string newDest)
        {
            var content = new StringContent(productString, Encoding.UTF8, "application/json");
            response = await Http.PostAsync("https://localhost:44325/" + newDest, content);

            string result = response.Content.ReadAsStringAsync().Result;

            newProduct = string.Empty;

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async void AddItemToStorage(int id, string newDest, string email)
        {
            foreach (var item in itemList)
            {
                if (item.Id == id)
                {
                    item.TimeAdded = DateTime.Now.ToString();
                    item.State = "Full";
                    response = await AddProductItem(item, newDest, email);

                    break;
                }
            }

        }

        public void AddListToStorage(List<AddedProduct> list, string dest, string email)
        {
            foreach (var item in list)
            {
                item.TimeAdded = DateTime.Now.ToString();
                item.State = "Full";
            }
        }

        public void DeleteList()
        {

        }

        public void DeleteItem()
        {

        }
    }
}
