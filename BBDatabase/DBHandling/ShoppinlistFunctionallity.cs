using BBCollection.BBObjects;
using BBCollection.DBConncetion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BBCollection.DBHandling
{
    public class ShoppinlistFunctionality
    {

        #region Fields
        public ShoppinlistFunctionality(string _dest)
        {
            dest = _dest;
        }

        readonly ConnectionSettings connectionSettings = new ConnectionSettings();

        public string productString;
        string dest;
        string Email;

        public List<Product> itemList = new List<Product>();
        public List<Product> LocalItemList = new List<Product>();
        public List<Product> CombinedList = new List<Product>();
        public List<Shoppinglist> shoppinglists = new List<Shoppinglist>();
        public List<Product> TempStorageList = new List<Product>();

        ControllerFuncionality features = new ControllerFuncionality();
        HttpResponseMessage response = new HttpResponseMessage();
        HttpClient Http = new HttpClient();
        #endregion

        #region FindPrice

        /// <summary>
        /// Finds the complete price for a product based on how many of them there is.
        /// </summary>
        /// <param name="p"></param>
        /// <returns>Complete price for one item</returns>
        public double FindSubtotal(Product p)
        {
            return (double)p._price * (double)p._amountleft;
        }

        /// <summary>
        /// Finds the complete price for a list of products
        /// </summary>
        /// <returns>Complete price for the list</returns>
        public double CompletePrice()
        {
            double result = 0;

            foreach (Product item in CombinedList)
            {
                result += FindSubtotal(item);
            }

            return result;
        }
        #endregion

        #region Shoppinglist

        /// <summary>
        /// Loads the local storage into the shoppinglist.
        /// This is used when the user is not logged in.
        /// Is only done if the localstorage consists of any Products.
        /// </summary>
        /// <param name="LocalStorageList"></param>
        public void GetShoppinglistWhileNotLoggedIn(List<Product> LocalStorageList)
        {
            if (LocalStorageList.Count != 0)
            {
                CombinedList = LocalStorageList;
                CombinedList = features.HandleDublicats(CombinedList);
                CombinedList = features.SetAllPicture(CombinedList);
            }
        }

        /// <summary>
        /// Calls API to get the users shoppinglist based on userid
        /// </summary>
        /// <param name="userId">The unique id for each user</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetShoppinglistOnStart(string userId)
        {
            Email = userId;

            productString = await Http.GetStringAsync(connectionSettings.GetApiLink() + dest + "/" + userId);

            shoppinglists = JsonConvert.DeserializeObject<List<Shoppinglist>>(productString);

            if (shoppinglists.Count > 0)
            {
                if (shoppinglists[0]._products.Count > 0)
                {
                    CombinedList = shoppinglists[0]._products;
                    CombinedList = features.HandleDublicats(CombinedList);
                    CombinedList = features.SetAllPicture(CombinedList);
                }
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// The entire list in the shoppinglist is sent sent
        /// to the api, and saved in the database as a string.
        /// </summary>
        public async Task<HttpResponseMessage> SaveShoppinglist()
        {
            var response = new HttpResponseMessage();

            productString = JsonConvert.SerializeObject(CombinedList);

            if (String.IsNullOrEmpty(productString))
            {
                productString = "[]";
            }

            response = await SendToApi(productString);

            return response;
        }

        /// <summary>
        /// Add a list to the shoppinglist.
        /// Is called when an entire recipe is added.
        /// </summary>
        /// <param name="sentList">The list to add</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> QuickaddListToShoppinglist(List<Product> sentList)
        {
            productString = JsonConvert.SerializeObject(sentList);

            response = await PutToApi(productString);

            return response;
        }

        /// <summary>
        /// Add one item to the shoppinglist
        /// Is called when you add one product from at reciple at a time
        /// or when you search for a specific product
        /// </summary>
        /// <param name="item">the amount to add</param>
        /// <param name="actualAmout">how many you have added. is resat so you add one at a time</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> QuickaddItemToShoppinglist(Product item)
        {
            productString = JsonConvert.SerializeObject(item);

            response = await PutToApi(productString);

            return response;
        }

        public async Task<HttpResponseMessage> QuickaddItemToShoppinglist(Product item, string dist)
        {
            productString = JsonConvert.SerializeObject(item);

            response = await PutToApi(productString,dist);

            return response;
        }

        #endregion

        #region Storage

        /// <summary>
        /// Calls the api to get the storage for the specific user on start
        /// </summary>
        /// <param name="userId">The unique id to verify the user</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetStorageOnStart(string userId)
        {
            Email = userId;

            productString = await Http.GetStringAsync(connectionSettings.GetApiLink() + dest + "/" + userId);

            CombinedList = JsonConvert.DeserializeObject<List<Product>>(productString);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }


        /// <summary>
        /// Clears the storage list in frontend, and sends the empty
        /// list to the api, to clear the storage
        /// </summary>
        public async Task<HttpResponseMessage> DeleteStorage()
        {
            CombinedList.Clear();
            var response = new HttpResponseMessage();

            productString = JsonConvert.SerializeObject(CombinedList);

            response = await SendToApi(productString);

            return response;
        }


        /// <summary>
        /// Is used when an attribute of an item is changed.
        /// This could either be the state of the item (full, amost full ect.)
        /// or the amount left.
        /// </summary>
        /// <param name="p">The product when a changed attribute</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> ChangeItemInStorage(Product p)
        {
            productString = JsonConvert.SerializeObject(p);

            response = await PutToApi(productString);

            return response;
        }

        /// <summary>
        /// This method is called in the shoppinglist when an item is 
        /// added to the storage.
        /// This method adds all amounts of one product.
        /// </summary>
        /// <param name="p">The product to add</param>
        public async Task<HttpResponseMessage> AddItemToStorage(Product p)
        {
            p = HelpToAdd(p);

            productString = JsonConvert.SerializeObject(p);

            response = await SendToApi(productString, "api/Storage");

            return response;
        }

        /// <summary>
        /// This method is called in the shoppinglist when an item is
        /// added to the storage.
        /// This method will only add one of the selected item.
        /// </summary>
        /// <param name="p"></param>
        public async Task<HttpResponseMessage> AddOneItemToStorage(Product p)
        {
            int realAmountLeft = p._amountleft;

            // Adds the relecant attributes to the item (state and timeAdded).
            p = HelpToAdd(p);
            p._amountleft = 1;

            productString = JsonConvert.SerializeObject(p);

            p._amountleft = realAmountLeft;

            response = await SendToApi(productString, "api/Storage");

            return response;
        }

        /// <summary>
        /// Is used when a product is added to storage.
        /// It adds the relevant attributes used in storage.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        Product HelpToAdd(Product p)
        {
            p._timeAdded = DateTime.Now.ToString();
            p._state = "Full";

            return p;
        }

        /// <summary>
        /// Adds the entire shoppinglist to storage
        /// </summary>
        public async Task<HttpResponseMessage> AddShoppinlistToStorage()
        {
            foreach (Product p in CombinedList)
            {
                // Adds the relevant attributes for storage to the product
                HelpToAdd(p);
            }

            productString = JsonConvert.SerializeObject(CombinedList);

            response = await SendToApi(productString, "api/Storage");

            return response;
        }

        #endregion

        #region SendToApi

        /// <summary>
        /// Sends a string to the api. This string is a list of products.
        /// </summary>
        /// <param name="productString"></param>
        /// <returns></returns>
        async Task<HttpResponseMessage> SendToApi(string productString)
        {
            var content = new StringContent(productString, Encoding.UTF8, "application/json");
            response = await Http.PostAsync(connectionSettings.GetApiLink() + dest + "/" + Email, content);

            return response;
        }

        /// <summary>
        /// Send a string to the api.
        /// This method is called when it is send to storage, therfore another destination.
        /// </summary>
        /// <param name="productString"></param>
        /// <param name="newDest"></param>
        /// <returns></returns>
        async Task<HttpResponseMessage> SendToApi(string productString, string newDest)
        {
            var content = new StringContent(productString, Encoding.UTF8, "application/json");
            response = await Http.PostAsync(connectionSettings.GetApiLink() + newDest + "/" + Email, content);

            return response;
        }

        /// <summary>
        /// Sends a string to the api.
        /// This method is called when the shoppinglist should not be replaced
        /// but products should be added instead.
        /// </summary>
        /// <param name="productString"></param>
        /// <returns></returns>
        async Task<HttpResponseMessage> PutToApi(string productString)
        {
            var content = new StringContent(productString, Encoding.UTF8, "application/json");
            response = await Http.PutAsync(connectionSettings.GetApiLink() + dest + "/" + Email, content);

            return response;
        }

        async Task<HttpResponseMessage> PutToApi(string productString, string dist)
        {
            var content = new StringContent(productString, Encoding.UTF8, "application/json");
            response = await Http.PutAsync(connectionSettings.GetApiLink() + dist + "/" + Email, content);

            return response;
        }
        #endregion
    }
}
