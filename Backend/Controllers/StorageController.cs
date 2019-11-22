using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using BBCollection;
using BBCollection.BBObjects;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        DatabaseConnect dbConnect = new DatabaseConnect("localhost", "biksbudgetDB", "root", "BiksBudget123");
        List<Product> resultList = new List<Product>();
        List<Product> oldLists = new List<Product>();
        Product AddedProduct = new Product();
        int i = 0;
        string Email;

        // GET: api/Storage
        [HttpGet]
        public void Get()
        {
        }

        // GET: api/Storage/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            List<Product> storageList = dbConnect.GetStorageFromUsername(id);

            string jsonStorage = JsonConvert.SerializeObject(storageList);

            return jsonStorage;
        }

        // POST: api/Storage
        [HttpPost]
        public void Post(string value)
        {
            Find find = new Find();
            string buffer;
            List<Product> newItem = new List<Product>();
            int pNum;

            HttpRequest request = HttpContext.Request;
            Microsoft.AspNetCore.Http.HttpRequestRewindExtensions.EnableBuffering(request);

            using (var sr = new StreamReader(request.Body))
            {
                buffer = sr.ReadToEnd();
            }

            int indexNumber = buffer.IndexOf("|");
            int number = Convert.ToInt32(buffer.Substring(0, indexNumber));
            if (number >= 10)
            {
                pNum = 2;
            }
            else
            {
                 pNum = 1;
            }

            Email = buffer.Substring(indexNumber.ToString().Length + pNum, number);
            buffer = buffer.Remove(0, indexNumber.ToString().Length + number + pNum);

            if (buffer.Contains("PLS_DELETE"))
            {
                //Delete the entire list
                dbConnect.RemoveFromStorage(Email, newItem);
            }
            else
            {
                List<Product> ListFromDatabase = new List<Product>();
                if (buffer.Substring(0, 1) != "[")
                {
                    //buffer = "[" + buffer + "]";
                    AddedProduct = JsonConvert.DeserializeObject<Product>(buffer);
                    newItem.Add(AddedProduct);
                }
                else
                {
                    newItem = JsonConvert.DeserializeObject<List<Product>>(buffer);
                    dbConnect.DeleteShoppingListFromName("Shoppinglist", Email);
                }

                ListFromDatabase = dbConnect.GetStorageFromUsername(Email);

                foreach (Product p in newItem)
                {
                    ListFromDatabase = find.FindDublicats(ListFromDatabase, p);
                }
                dbConnect.UpdateStorage(Email, ListFromDatabase);
            } 
        }



        // PUT: api/Storage/5
        [HttpPut("{id}")]
        public void Put(string id)
        {
            string buffer;
            Product newItem = new Product();
            List<Product> storageList = new List<Product>();
            int pNum;

            HttpRequest request = HttpContext.Request;
            Microsoft.AspNetCore.Http.HttpRequestRewindExtensions.EnableBuffering(request);

            using (var sr = new StreamReader(request.Body))
            {
                buffer = sr.ReadToEnd();
            }

            int indexNumber = buffer.IndexOf("|");
            int number = Convert.ToInt32(buffer.Substring(0, indexNumber));
            if (number >= 10)
            {
                pNum = 2;
            }
            else
            {
                pNum = 1;
            }

            Email = buffer.Substring(indexNumber.ToString().Length + pNum, number);
            buffer = buffer.Remove(0, indexNumber.ToString().Length + number + pNum);

            storageList = dbConnect.GetStorageFromUsername(Email);
            newItem = JsonConvert.DeserializeObject<Product>(buffer);

            foreach (Product p in storageList)
            {
                if (p._id == newItem._id)
                {
                    p._state = newItem._state;
                    break;
                }
            }

            dbConnect.UpdateStorage(Email, storageList);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        
    }

    class Find
    {
        public List<Product> FindDublicats(List<Product> ReturnList, Product FromFrontendProduct)
        {
            bool isFound = false;

            foreach (Product p in ReturnList)
            {
                if (p._id == FromFrontendProduct._id)
                {
                    p._amountleft += FromFrontendProduct._amountleft;
                    isFound = true;
                    break;
                }
            }

            if (!isFound)
            {
                ReturnList.Add(FromFrontendProduct);
            }

            return ReturnList;
        }
    }
}
