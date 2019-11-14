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

        public async Task<HttpResponseMessage> GetProductsOnStart(string userId)
        {
            productString = await Http.GetStringAsync("https://localhost:44325/" + dest + "/" + userId);

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

        public async Task<HttpResponseMessage> AddProductString(string name, string amount, double price, string userId)
        {
            var response = new HttpResponseMessage();

            AddedProduct newItem = new AddedProduct()
            {
                Name = name,
                Amount = amount,
                Price = price,
                Id = itemList.Count() + 1
            };

            //itemList.Add(newItem);

            ////await AddProductItem(newItem, userId);

            

            //productString = JsonConvert.SerializeObject(itemList[itemList.Count - 1]);
            //var content = new StringContent(productString, Encoding.UTF8, "application/json");
            //response = await Http.PostAsync("https://localhost:44325/" + dest, content);

            //string result = response.Content.ReadAsStringAsync().Result;

            //newProduct = string.Empty;

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async Task<HttpResponseMessage> AddProductItem(AddedProduct newItem, string userId)
        {
            var response = new HttpResponseMessage();

            int userIdLength = userId.Length;

            productString = JsonConvert.SerializeObject(newItem);

            productString = userIdLength.ToString() + "|" + userId + productString;

            ////await SendToApi(productString);

            //var content = new StringContent(productString, Encoding.UTF8, "application/json");
            //response = await Http.PostAsync("https://localhost:44325/" + dest, content);

            //string result = response.Content.ReadAsStringAsync().Result;

            //newProduct = string.Empty;

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async Task<HttpResponseMessage> AddProductItem(AddedProduct newItem, string userId, string newDest)
        {
            var response = new HttpResponseMessage();

            int userIdLength = userId.Length;

            productString = JsonConvert.SerializeObject(newItem);

            productString = userIdLength.ToString() + "|" + userId + productString;

            ////await SendToApi(productString, newDest);

            //var content = new StringContent(productString, Encoding.UTF8, "application/json");
            //response = await Http.PostAsync("https://localhost:44325/" + dest, content);

            //string result = response.Content.ReadAsStringAsync().Result;

            //newProduct = string.Empty;

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async void AddList(string userId)
        {
            var response = new HttpResponseMessage();

            int userIdLength = userId.Length;

            productString = JsonConvert.SerializeObject(itemList);

            productString = userIdLength.ToString() + "|" + userId + productString;

            ////await SendToApi(productString);
        }

        async Task<HttpResponseMessage> SendToApi(string productString)
        {
            var content = new StringContent(productString, Encoding.UTF8, "application/json");
            ////response = await Http.PostAsync("https://localhost:44325/" + dest, content);

            string result = response.Content.ReadAsStringAsync().Result;

            newProduct = string.Empty;

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        async Task<HttpResponseMessage> SendToApi(string productString, string newDest)
        {
            var content = new StringContent(productString, Encoding.UTF8, "application/json");
            ////response = await Http.PostAsync("https://localhost:44325/" + newDest, content);

            string result = response.Content.ReadAsStringAsync().Result;

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async void AddItemToStorage(long id, string newDest, string userId)
        {
            foreach (var item in itemList)
            {
                if (item.Id == id)
                {
                    item.TimeAdded = DateTime.Now.ToString();
                    item.State = "Full";
                    ////response = await AddProductItem(item, newDest, userId);

                    break;
                }
            }

        }

        public async void AddListToStorage(string dest, string userId)
        {
            foreach (var item in itemList)
            {
                item.TimeAdded = DateTime.Now.ToString();
                item.State = "Full";
            }

            int userIdLength = userId.Length;

            productString = JsonConvert.SerializeObject(itemList);

            productString = userIdLength.ToString() + "|" + userId + productString;

            ////await SendToApi(productString, dest);
        }

        public async void DeleteList(string userId)
        {
            var response = new HttpResponseMessage();

            int userIdLength = userId.Length;
            string productString = userIdLength.ToString() + "|" + userId + "[PLS_DELETE]";

            ////await SendToApi(productString);
        }

        public async Task<HttpResponseMessage> DeleteItem(long id, string userId)
        {
            var response = new HttpResponseMessage();
            itemList.Remove(itemList.First(x => x.Id == id));

            //int i = 1;
            //foreach (var product in itemList)
            //{
            //    product.Id = i;
            //    i++;
            //}

            int userIdLength = userId.Length;
            productString = JsonConvert.SerializeObject(itemList);

            productString = userIdLength.ToString() + "|" + userId + productString;

            ////await SendToApi(productString);

            //var content = new StringContent(productString, Encoding.UTF8, "application/json");
            //response = await Http.PostAsync("https://localhost:44325/" + dest, content);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async Task<HttpResponseMessage> ChangeItem(int id, string itemString, string userId)
        {
            int userIdLength = userId.Length;

            itemString = userIdLength.ToString() + "|" + userId + itemString;

            var content = new StringContent(itemString, Encoding.UTF8, "application/json");
            ////HttpResponseMessage responce = await Http.PutAsync("https://localhost:44325/api/Storage/" + id, content);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
