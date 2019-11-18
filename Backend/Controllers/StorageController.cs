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
    //public class AddedProduct
    //{
    //    public string Name { get; set; }
    //    public string Amount { get; set; }
    //    public double Price { get; set; }
    //    public long Id { get; set; }
    //    public string State { get; set; }
    //    public string TimeAdded { get; set; }
    //    public string UniqueId { get; set; }
    //    public string Image { get; set; }
    //    public string StoreName { get; set; }
    //    public int AmountOfItem { get; set; }
    //}

    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        DatabaseConnect dbConnect = new DatabaseConnect("localhost", "biksbudgetDB", "root", "BiksBudget123");
        List<Product> resultList = new List<Product>();
        int i = 0;
        string Email;

        // GET: api/Storage
        [HttpGet]
        public void Get()
        {
            //List<Product> storageList = dbConnect.GetStorageFromUsername("Test6");

            //resultList = ConvertBeforeSending(storageList);

            //string jsonRecipes = JsonConvert.SerializeObject(resultList);

            //return jsonRecipes;
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
                if (buffer.Substring(0, 1) != "[")
                {
                    buffer = "[" + buffer + "]";

                    newItem = JsonConvert.DeserializeObject<List<Product>>(buffer);
                    dbConnect.UpdateStorage(Email, newItem);
                }
                else
                {
                    newItem = JsonConvert.DeserializeObject<List<Product>>(buffer);

                    dbConnect.UpdateStorage(Email, newItem);

                }
            } 
        }

        // PUT: api/Storage/5
        [HttpPut("{id}")]
        public void Put(int id, string value)
        {
            string buffer;
            Product newItem = new Product();
            List<Product> storageList = dbConnect.GetStorageFromUsername("Test6");

            //List<AddedProduct> resultList = ConvertBeforeSending(storageList);
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


            newItem = JsonConvert.DeserializeObject<Product>(buffer);

            resultList[id-1] = newItem;
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
