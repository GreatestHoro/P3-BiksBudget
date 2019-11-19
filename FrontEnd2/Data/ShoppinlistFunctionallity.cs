using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using BBCollection.BBObjects;

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
        public List<Product> itemList = new List<Product>();
        List<Shoppinglist> shoppinglists = new List<Shoppinglist>();
        HttpResponseMessage response = new HttpResponseMessage();
        List<Product> TempStorageList = new List<Product>();

        public async Task<HttpResponseMessage> GetProductsOnStart()
        {
            productString = await Http.GetStringAsync("https://localhost:44325/" + dest);

            itemList = JsonConvert.DeserializeObject<List<Product>>(productString);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async Task<HttpResponseMessage> GetProductsOnStart(string userId)
        {
            productString = await Http.GetStringAsync("https://localhost:44325/" + dest + "/" + userId);

            itemList = JsonConvert.DeserializeObject<List<Product>>(productString);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async Task<HttpResponseMessage> GetShoppinglistOnStart(string userId)
        {
            productString = await Http.GetStringAsync("https://localhost:44325/" + dest + "/" + userId);

            shoppinglists = JsonConvert.DeserializeObject<List<Shoppinglist>>(productString);

            itemList = shoppinglists[0]._products;

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public double FindSubtotal(Product item)
        {
            double result;

            result = item._price * item._amountleft;
            result = Math.Round(result, 3);

            return result;
        }


        public double CompletePrice()
        {
            double result = 0;

            foreach (var item in itemList)
            {
                result += item._price * item._amountleft;
            }
            result = Math.Round(result, 3);
            return result;
        }

        // NEW AND IMPROVED CODE BELOW

        public async Task<HttpResponseMessage> OnStart()
        {
            productString = await Http.GetStringAsync("https://localhost:44325/" + dest);

            itemList = JsonConvert.DeserializeObject<List<Product>>(productString);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public double CalculatePrice()
        {
            double result = 0;

            foreach (var item in itemList)
            {
                result += item._price * item._amountleft;
            }
            return result;
        }

        public async Task<HttpResponseMessage> AddProductAsString(string name, string amount, double price, string id, string email)
        {
            var response = new HttpResponseMessage();

            Product newItem = new Product()
            {
                _productName = name,
                _amount = amount,
                _price = price,
                _id = id
            };

            //itemList.Add(newItem);
            await AddProductAsItem(newItem, email);



            //productString = JsonConvert.SerializeObject(itemList[itemList.Count - 1]);
            //var content = new StringContent(productString, Encoding.UTF8, "application/json");
            //response = await Http.PostAsync("https://localhost:44325/" + dest, content);

            //string result = response.Content.ReadAsStringAsync().Result;

            //newProduct = string.Empty;

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async Task<HttpResponseMessage> AddProductAsItem(Product newItem, string email)
        {
            var response = new HttpResponseMessage();

            int userIdLength = email.Length;

            productString = JsonConvert.SerializeObject(newItem);

            productString = userIdLength.ToString() + "|" + email + productString;

            ////await SendToApi(productString);

            //var content = new StringContent(productString, Encoding.UTF8, "application/json");
            //response = await Http.PostAsync("https://localhost:44325/" + dest, content);

            //string result = response.Content.ReadAsStringAsync().Result;

            //newProduct = string.Empty;

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async Task<HttpResponseMessage> AddProductAsItem(Product newItem, string email, string newDest)
        {
            var response = new HttpResponseMessage();

            int userIdLength = email.Length;

            productString = JsonConvert.SerializeObject(newItem);

            productString = userIdLength.ToString() + "|" + email + productString;

            ////await SendToApi(productString, newDest);

            //var content = new StringContent(productString, Encoding.UTF8, "application/json");
            //response = await Http.PostAsync("https://localhost:44325/" + dest, content);

            //string result = response.Content.ReadAsStringAsync().Result;

            //newProduct = string.Empty;

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async void AddFuncList(string email)
        {
            var response = new HttpResponseMessage();

            int userIdLength = email.Length;

            productString = JsonConvert.SerializeObject(itemList);

            productString = userIdLength.ToString() + "|" + email + productString;

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

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async void AddItemToStorage(Product AddedItem, string email)
        {
            TempStorageList.Add(AddedItem);
            DeleteItem(AddedItem._id, email);
        }

        public async void AddShoppinlistToStorage(string dest, string email)
        {
            foreach (var item in TempStorageList)
            {
                itemList.Add(item);
            }

            foreach (var item in itemList)
            {
                item._timeAdded = DateTime.Now.ToString();
                item._state = "Full" ;
            }

            int userIdLength = email.Length;

            productString = JsonConvert.SerializeObject(itemList);

            productString = userIdLength.ToString() + "|" + email + productString;

            itemList.Clear();

            await SendToApi(productString, dest);
        }

        public async void DeleteFuncList(string email)
        {
            itemList.Clear();
            var response = new HttpResponseMessage();

            int userIdLength = email.Length;
            string productString = userIdLength.ToString() + "|" + email + "[PLS_DELETE]";

            ////await SendToApi(productString);
        }

        public async Task<HttpResponseMessage> DeleteItem(string id, string userId)
        {
            var response = new HttpResponseMessage();
            itemList.Remove(itemList.First(x => x._id == id));

            int userIdLength = userId.Length;
            productString = JsonConvert.SerializeObject(itemList);

            productString = userIdLength.ToString() + "|" + userId + productString;

            ////await SendToApi(productString);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async Task<HttpResponseMessage> ChangeItem(string id, string itemString, string userId)
        {
            int userIdLength = userId.Length;

            itemString = userIdLength.ToString() + "|" + userId + itemString;

            var content = new StringContent(itemString, Encoding.UTF8, "application/json");
            ////HttpResponseMessage responce = await Http.PutAsync("https://localhost:44325/api/Storage/" + id, content);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
