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
        string Email;
        public List<Product> itemList = new List<Product>();
        public List<Product> LocalItemList = new List<Product>();
        public List<Product> CombinedList = new List<Product>();
        List<Shoppinglist> shoppinglists = new List<Shoppinglist>();
        HttpResponseMessage response = new HttpResponseMessage();
        List<Product> TempStorageList = new List<Product>();

        public async Task<HttpResponseMessage> GetStorageOnStart(string userId)
        {
            Email = userId;

            productString = await Http.GetStringAsync("https://localhost:44325/" + dest + "/" + userId);

            CombinedList = JsonConvert.DeserializeObject<List<Product>>(productString);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async Task<HttpResponseMessage> GetShoppinglistOnStart(string userId, List<Product> LocalList)
        {
            Email = userId;

            productString = await Http.GetStringAsync("https://localhost:44325/" + dest + "/" + userId);

            shoppinglists = JsonConvert.DeserializeObject<List<Shoppinglist>>(productString);

            if (LocalList.Count > 0)
            {
                LocalItemList = LocalList;
                //LocalItemList = FindDuplicatesInOneList(LocalList);
            }
            if (shoppinglists.Count > 0)
            {
                if (shoppinglists[0]._products.Count > 0)
                {
                    itemList = shoppinglists[0]._products;
                    //itemList = FindDuplicatesInOneList(shoppinglists[0]._products);
                }
            }

            if (itemList.Count > 0 && LocalItemList.Count > 0)
            {
                CombinedList = LocalItemList.Concat(itemList).ToList();
            }
            else if (itemList.Count > 0)
            {
                CombinedList = itemList;
            }
            else
            {
                CombinedList = LocalItemList;
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        //public List<Product> CreateList(List<Product> ReturnedList, List<Product> CompareList)
        //{
        //    if (CompareList.Count > 0)
        //    {
        //        ReturnedList = FindDuplicatesInOneList(CompareList);
        //        return ReturnedList;
        //    }
        //    return null;
        //}

        public List<Product> FindDuplicatesInOneList(List<Product> inputList)
        {
            List<Product> returnList = new List<Product>();
            int id;

            for (int i = 0; i < inputList.Count; i++)
            {
                if (!returnList.Contains(returnList[i]))
                {
                    returnList.Add(returnList[i]);
                }
                else
                {
                    id = FindMatch(inputList, inputList[i]._id);
                    returnList[id]._amountleft += inputList[i]._amountleft;
                }
            }

            return returnList;
        }

        public int FindMatch(List<Product> InputList, string id)
        {
            int result = -1;

            for (int i = 0; i < InputList.Count; i++)
            {
                if (InputList[i]._id == id)
                {
                    result = i;
                    break;
                }
            }

            return result;
        }

        public async Task<HttpResponseMessage> GetShoppinglistOnStart(string userId)
        {
            Email = userId;

            productString = await Http.GetStringAsync("https://localhost:44325/" + dest + "/" + userId);

            shoppinglists = JsonConvert.DeserializeObject<List<Shoppinglist>>(productString);

            if (shoppinglists.Count != 0)
            {
                CombinedList = shoppinglists[0]._products;
            }

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

            foreach (var item in CombinedList)
            {
                result += item._price * item._amountleft;
            }
            result = Math.Round(result, 3);
            return result;
        }

        // NEW AND IMPROVED CODE BELOW

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

            //itemList.Add(newItem);
            await AddProductAsItem(newItem, Email);

            //productString = JsonConvert.SerializeObject(itemList[itemList.Count - 1]);
            //var content = new StringContent(productString, Encoding.UTF8, "application/json");
            //response = await Http.PostAsync("https://localhost:44325/" + dest, content);

            //string result = response.Content.ReadAsStringAsync().Result;

            //newProduct = string.Empty;

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async Task<HttpResponseMessage> AddProductAsItem(Product newItem)
        {
            var response = new HttpResponseMessage();

            int userIdLength = Email.Length;

            CombinedList.Add(newItem);

            //productString = JsonConvert.SerializeObject(newItem);

            //productString = userIdLength.ToString() + "|" + Email + productString;

            ////await SendToApi(productString);

            //var content = new StringContent(productString, Encoding.UTF8, "application/json");
            //response = await Http.PostAsync("https://localhost:44325/" + dest, content);

            //string result = response.Content.ReadAsStringAsync().Result;

            //newProduct = string.Empty;

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async Task<HttpResponseMessage> AddListToShoppinglist(List<Product> sentList)
        {
            foreach (var item in sentList)
            {
                item._timeAdded = DateTime.Now.ToString();
                item._state = "Full";
            }

            int userIdLength = Email.Length;

            productString = JsonConvert.SerializeObject(sentList);

            productString = userIdLength.ToString() + "|" + Email + productString;

            sentList.Clear();

            await SendToApi(productString, dest);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async Task<HttpResponseMessage> AddProductAsItem(Product newItem, string newDest)
        {
            var response = new HttpResponseMessage();

            int userIdLength = Email.Length;

            productString = JsonConvert.SerializeObject(newItem);

            productString = userIdLength.ToString() + "|" + Email + productString;

            ////await SendToApi(productString, newDest);

            //var content = new StringContent(productString, Encoding.UTF8, "application/json");
            //response = await Http.PostAsync("https://localhost:44325/" + dest, content);

            //string result = response.Content.ReadAsStringAsync().Result;

            //newProduct = string.Empty;

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async void AddFuncList()
        {
            var response = new HttpResponseMessage();

            int userIdLength = Email.Length;

            productString = JsonConvert.SerializeObject(CombinedList);

            productString = userIdLength.ToString() + "|" + Email + productString;

            response = await SendToApi(productString);
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

            //string result = response.Content.ReadAsStringAsync().Result;

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async void AddItemToStorage(Product AddedItem, string dest)
        {
            AddedItem._timeAdded = DateTime.Now.ToString();
            AddedItem._state = "Full";

            int userIdLength = Email.Length;

            productString = JsonConvert.SerializeObject(AddedItem);

            productString = userIdLength.ToString() + "|" + Email + productString;

            await SendToApi(productString, dest);

            //await DeleteItem(AddedItem._id);
        }

        public async void AddShoppinlistToStorage(string dest)
        {
            foreach (var item in CombinedList)
            {
                item._timeAdded = DateTime.Now.ToString();
                item._state = "Full" ;
            }

            int userIdLength = Email.Length;

            productString = JsonConvert.SerializeObject(CombinedList);

            productString = userIdLength.ToString() + "|" + Email + productString;

            await SendToApi(productString, dest);
        }

        public async void SaveStorage()
        {
            int userIdLength = Email.Length;

            productString = JsonConvert.SerializeObject(CombinedList);

            productString = userIdLength.ToString() + "|" + Email + productString;

            //CombinedList.Clear();

            await SendToApi(productString);
        }

        public async void DeleteFuncList()
        {
            CombinedList.Clear();
            var response = new HttpResponseMessage();

            int userIdLength = Email.Length;
            string productString = userIdLength.ToString() + "|" + Email + "[PLS_DELETE]";

            await SendToApi(productString);
        }

        public async Task<HttpResponseMessage> DeleteItem(string id)
        {
            var response = new HttpResponseMessage();
            
            CombinedList.Remove(CombinedList.First(x => x._id == id));

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        //public async Task<HttpResponseMessage> ChangeItem(string id, string itemString)
        //{
        //    int userIdLength = Email.Length;

        //    itemString = userIdLength.ToString() + "|" + Email + itemString;

        //    var content = new StringContent(itemString, Encoding.UTF8, "application/json");
        //    ////HttpResponseMessage responce = await Http.PutAsync("https://localhost:44325/api/Storage/" + id, content);

        //    return new HttpResponseMessage(HttpStatusCode.OK);
        //}
    }
}
