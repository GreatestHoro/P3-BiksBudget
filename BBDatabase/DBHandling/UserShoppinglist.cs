﻿using System;
using System.Collections.Generic;
using System.Text;
using BBCollection.BBObjects;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace BBCollection.DBHandling
{
    public class UserShoppinglist
    {
        public UserShoppinglist(string _username)
        {
            username = _username;
            api = new ApiCalls(url, username);
        }

        public UserShoppinglist() { }

        public enum Filter
        {
            All,
            Active,
            Completed,
        }

        public Filter filter = Filter.All;
        string username;
        string url = "api/Shoppinglist";
        string shoppinglistString;

        int amount;

        public List<Product> shoppinglist = new List<Product>();
        public Dictionary<string, List<Product>> shoppinglistDict = new Dictionary<string, List<Product>>();
        Product tempProduct = new Product();
        List<Shoppinglist> shoppinglistFromAPi = new List<Shoppinglist>();
        ApiCalls api;

        public void SetFilter(Filter filter)
        {
            this.filter = filter; 
        }

        public IEnumerable<Product> Products
        {
            get
            {
                var query = from p in shoppinglist select p;

                if (filter == Filter.Active)
                    query = from q in query where !q._completed select q;

                if (filter == Filter.Completed)
                    query = from q in query where q._completed select q;

                return query;
            }
        }

        public void CheckAll(bool isChecked)
        {
            shoppinglist.ForEach(x => x._completed = isChecked);
        }

        public async Task GetWhenLoggedIn()
        {
            shoppinglistString = await api.Get();

            if (StringExists(shoppinglistString))
            {
                shoppinglistFromAPi = JsonConvert.DeserializeObject<List<Shoppinglist>>(shoppinglistString);

                if (ShoppinglistProductExists())
                {
                    shoppinglist = shoppinglistFromAPi[0]._products;
                }
            }
        }

        public void GetWhenNotLoggedIn(string localStorage)
        {
            if (StringExists(localStorage))
            {
                shoppinglist = JsonConvert.DeserializeObject<List<Product>>(localStorage);
            }
        }

        public async Task<HttpResponseMessage> DeleteAndSaveList()
        {
            DeleteList();

            return await Save();
        }

        public void DeleteList()
        {
            shoppinglist.Clear();
        }

        public void ClearItems()
        {
            shoppinglist.RemoveAll(x => x._completed);
            filter = Filter.All;
        }

        public void DeleteItem(Product p)
        {
            shoppinglist.Remove(shoppinglist.First(x => x._id == p._id));
        }

        public void DecrementProduct(Product p)
        {
            p._amountleft--;

            if (p._amountleft <= 0)
            {
                DeleteItem(p);
            }
            else
            {
                shoppinglist.Where(x => x._id == p._id).Select(x => x._amountleft = p._amountleft);
            }
        }

        public async Task<HttpResponseMessage> Save()
        {
            shoppinglistString = JsonConvert.SerializeObject(shoppinglistFromAPi[0]._products);

            return await api.Post(shoppinglistString);
        }

        public async Task<HttpResponseMessage> AddProduct(Product p)
        {
            amount = p._amountleft;
            p._amountleft = 1;

            shoppinglistString = JsonConvert.SerializeObject(p);

            p._amountleft = amount;

            return await api.Put(shoppinglistString);
        }

        public async Task<HttpResponseMessage> AddList(List<Product> inputList)
        {
            shoppinglistString = JsonConvert.SerializeObject(inputList);

            return await api.Put(shoppinglistString);
        }

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

            foreach (Product item in shoppinglist)
            {
                result += FindSubtotal(item);
            }

            return result;
        }

        #region Private Methods

        private bool IsLoggedIn()
        {
            return String.IsNullOrEmpty(username);
        }

        private bool ShoppinglistExists()
        {
            if (shoppinglistFromAPi.Count != 0)
            {
                return ProductsExist(shoppinglistFromAPi[0]._products);
            }
            else
            {
                return false;
            }
        }

        private bool ShoppinglistProductExists()
        {
            if (shoppinglistFromAPi.Count > 0)
            {
                if (ProductsExist(shoppinglistFromAPi[0]._products))
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        private bool ProductsExist(List<Product> inputList)
        {
            return inputList.Count > 0;
        }

        private bool StringExists(string inputString)
        {
            return !String.IsNullOrEmpty(inputString);
        }

        #endregion
    }
}
