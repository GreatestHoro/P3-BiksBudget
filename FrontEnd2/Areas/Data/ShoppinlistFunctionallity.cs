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
        string dest;
        string Email;

        public List<Product> itemList = new List<Product>();
        public List<Product> LocalItemList = new List<Product>();
        public List<Product> CombinedList = new List<Product>();
        public List<Shoppinglist> shoppinglists = new List<Shoppinglist>();
        public List<Product> TempStorageList = new List<Product>();

        HttpResponseMessage response = new HttpResponseMessage();
        HttpClient Http = new HttpClient();

        #region AddProduct
        public async Task<HttpResponseMessage> AddProductAsItem(Product newItem, string newDest)
        {
            var response = new HttpResponseMessage();

            int userIdLength = Email.Length;

            productString = JsonConvert.SerializeObject(newItem);

            productString = userIdLength.ToString() + "|" + Email + productString;

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async Task<HttpResponseMessage> AddProductAsString(string name, string amount, double price, string id)
        {
            var response = new HttpResponseMessage();

            Product newItem = new Product()
            {
                _productName = name,
                _amount = amount,
                _price = price,
                _id = id
            };

            await AddProductAsItem(newItem, Email);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async Task<HttpResponseMessage> AddProductAsItem(Product newItem)
        {
            var response = new HttpResponseMessage();

            int userIdLength = Email.Length;

            CombinedList.Add(newItem);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
        #endregion

        #region FindPrice

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

            foreach (var item in CombinedList)
            {
                result += item._price * item._amountleft;
            }
            result = Math.Round(result, 3);
            return result;
        }
        #endregion

        #region Shoppinglist

        public async Task<HttpResponseMessage> GetShoppinglistWhileNotLoggedIn(List<Product> LocalStorageList)
        {
            if (LocalStorageList.Count != 0)
            {
                CombinedList = LocalStorageList;
                CombinedList = HandleDublicats(CombinedList);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async Task<HttpResponseMessage> GetShoppinglistOnStart(string userId)
        {
            Email = userId;

            productString = await Http.GetStringAsync("https://localhost:44325/" + dest + "/" + userId);

            shoppinglists = JsonConvert.DeserializeObject<List<Shoppinglist>>(productString);

            if (shoppinglists.Count > 0)
            {
                if (shoppinglists[0]._products.Count > 0)
                {
                    CombinedList = shoppinglists[0]._products;
                    CombinedList = HandleDublicats(CombinedList);
                }
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public List<Product> HandleDublicats(List<Product> inputList)
        {
            bool isFound = false;
            List<Product> uniqueList = new List<Product>();
            List<Product> dublicateList = new List<Product>();

            foreach (Product i in inputList)
            {
                isFound = false;
                foreach (Product u in uniqueList)
                {
                    if (i._id == u._id)
                    {
                        dublicateList.Add(i);
                        isFound = true;
                        break;
                    }
                }
                if (isFound == false)
                {
                    uniqueList.Add(i);
                }
            }

            foreach (Product d in dublicateList)
            {
                foreach (Product i in uniqueList)
                {
                    if (d._id == i._id)
                    {
                        i._amountleft += d._amountleft;
                        break;
                    }
                }
            }

            return uniqueList;
        }

        public async Task<HttpResponseMessage> QuickaddListToShoppinglist(List<Product> sentList)
        {
            productString = JsonConvert.SerializeObject(sentList);

            response =  await PutToApi(productString);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async Task<HttpResponseMessage> QuickaddItemToShoppinglist(Product item, int actualAmout)
        {
            productString = JsonConvert.SerializeObject(item);

            item._amountleft = actualAmout;

            await PutToApi(productString);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async Task<HttpResponseMessage> AddListToShoppinglist(List<Product> sentList, string newDest)
        {
            productString = JsonConvert.SerializeObject(sentList);

            sentList.Clear();

            await SendToApi(productString, newDest);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        #endregion

        #region Storage
        public async Task<HttpResponseMessage> GetStorageOnStart(string userId)
        {
            Email = userId;

            productString = await Http.GetStringAsync("https://localhost:44325/" + dest + "/" + userId);

            CombinedList = JsonConvert.DeserializeObject<List<Product>>(productString);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async void AddItemToStorage(Product AddedItem, string dest)
        {
            AddedItem = HelpToAdd(AddedItem);

            productString = JsonConvert.SerializeObject(AddedItem);

            await SendToApi(productString, dest);
        }

        public async void AddOneItemToStorage(Product AddedItem, string dest)
        {
            int realAmountLeft = AddedItem._amountleft;
            AddedItem = HelpToAdd(AddedItem);
            AddedItem._amountleft = 1;

            productString = JsonConvert.SerializeObject(AddedItem);

            AddedItem._amountleft = realAmountLeft;

            await SendToApi(productString, dest);
        }

        Product HelpToAdd(Product item)
        {
            item._timeAdded = DateTime.Now.ToString();
            item._state = "Full";

            return item;
        }

        public async void AddShoppinlistToStorage(string dest)
        {
            foreach (var item in CombinedList)
            {
                item._timeAdded = DateTime.Now.ToString();
                item._state = "Full" ;
            }

            productString = JsonConvert.SerializeObject(CombinedList);

            await SendToApi(productString, dest);
        }

        public async void SaveStorage()
        {
            productString = JsonConvert.SerializeObject(CombinedList);

            await SendToApi(productString);
        }

        #endregion

        #region SendToApi
        async Task<HttpResponseMessage> SendToApi(string productString)
        {
            var content = new StringContent(productString, Encoding.UTF8, "application/json");
            response = await Http.PostAsync("https://localhost:44325/" + dest + "/" + Email, content);

            string result = response.Content.ReadAsStringAsync().Result;

            newProduct = string.Empty;

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        async Task<HttpResponseMessage> SendToApi(string productString, string newDest)
        {
            var content = new StringContent(productString, Encoding.UTF8, "application/json");
            response = await Http.PostAsync("https://localhost:44325/" + newDest + "/" + Email, content);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
        
        async Task<HttpResponseMessage> PutToApi(string productString)
        {
            var content = new StringContent(productString, Encoding.UTF8, "application/json");
            response = await Http.PutAsync("https://localhost:44325/api/Shoppinglist/" + Email, content);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }


        #endregion

        public async void AddFuncList()
        {
            var response = new HttpResponseMessage();

            productString = JsonConvert.SerializeObject(CombinedList);

            response = await SendToApi(productString);
        }

        public async void DeleteFuncList()
        {
            CombinedList.Clear();
            var response = new HttpResponseMessage();

            productString = JsonConvert.SerializeObject(CombinedList);

            await SendToApi(productString);
        }

        public async Task<HttpResponseMessage> DeleteItem(string id)
        {
            var response = new HttpResponseMessage();

            CombinedList.Remove(CombinedList.First(x => x._id == id));
            productString = JsonConvert.SerializeObject(CombinedList);

            await SendToApi(productString);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async Task<HttpResponseMessage> ChangeItem(Product item)
        {
            string itemString;

            itemString = JsonConvert.SerializeObject(item);

            var content = new StringContent(itemString, Encoding.UTF8, "application/json");
            
            HttpResponseMessage responce = await Http.PutAsync("https://localhost:44325/api/Storage/" + Email, content);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }


}
}
