using System;
using System.Collections.Generic;
using System.Text;
using BBCollection.BBObjects;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;

namespace BBCollection.DBHandling
{
    public class UserStorage
    {
        public UserStorage(string _username)
        {
            username = _username;
            api = new ApiCalls(url, username);
        }

        string username;
        string url = "api/Storage";
        string productString;

        int amount;

        public List<Product> storageList = new List<Product>();
        static Product tempProduct = new Product();

        HttpResponseMessage response = new HttpResponseMessage();
        ApiCalls api;

        public async Task Get()
        {
            productString = await api.Get();

            storageList = JsonConvert.DeserializeObject<List<Product>>(productString);
        }

        public async Task<HttpResponseMessage> DeleteProduct(Product p)
        {
            p._amountleft = 0;

            storageList.Remove(storageList.First(x => x._id == p._id));

            return await EditProduct(p);
        }

        public async Task<HttpResponseMessage> DeleteProduct(Product p, int i)
        {
            p._amountleft -= i;

            if (p._amountleft >= 1)
            {
                return await EditProduct(p);
            }
            else
            {
                return await DeleteProduct(p);
            }
        }

        public async Task<HttpResponseMessage> DeleteStorage()
        {
            storageList.Clear();

            productString = JsonConvert.SerializeObject(new List<Product>());

            return await api.Post(productString);
        }

        public async Task<HttpResponseMessage> EditProduct(Product p)
        {
            productString = JsonConvert.SerializeObject(p);

            return await api.Put(productString);
        }

        public async Task<HttpResponseMessage> AddProduct(Product p)
        {
            p = HelpToAdd(p);

            productString = JsonConvert.SerializeObject(p);

            return await api.Post(productString);
        }

        public async Task<HttpResponseMessage> AddProduct(Product p, int i)
        {
            amount = p._amountleft;
            p = HelpToAdd(p, i);

            productString = JsonConvert.SerializeObject(p);

            p._amountleft = amount;

            return await api.Post(productString);
        }

        public async Task<HttpResponseMessage> AddList(List<Product> listToAdd)
        {
            listToAdd.ForEach(p => p = HelpToAdd(p));

            productString = JsonConvert.SerializeObject(listToAdd);

            return await api.Post(productString);
        }

        #region Private Methods

        private Product HelpToAdd(Product p)
        {
            p._timeAdded = DateTime.Now.ToString();
            p._state = "Full";

            return p;
        }

        private Product HelpToAdd(Product p, int i)
        {
            p._timeAdded = DateTime.Now.ToString();
            p._state = "Full";
            p._amountleft = i;

            return p;
        }

        #endregion
    }
}
