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
using BBCollection.DBHandling;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        DatabaseConnect dbConnect = new DatabaseConnect();
        List<Product> resultList = new List<Product>();
        ControllerFuncionality functionality = new ControllerFuncionality();
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
        [HttpPost("{value}")]
        public void Post(string value)
        {
            string buffer;
            List<Product> newItem = new List<Product>();
            List<Product> Fromdb = new List<Product>();

            HttpRequest request = HttpContext.Request;
            Microsoft.AspNetCore.Http.HttpRequestRewindExtensions.EnableBuffering(request);

            Email = value;

            using (var sr = new StreamReader(request.Body))
            {
                buffer = sr.ReadToEnd();
            }

            if (buffer.Substring(0, 1) != "[")
            {
                buffer = "[" + buffer + "]";
            }

            newItem = JsonConvert.DeserializeObject<List<Product>>(buffer);

            if (newItem.Count == 0)
            {
                dbConnect.UpdateStorage(Email, newItem);
            }
            else
            {
                Fromdb = dbConnect.GetStorageFromUsername(Email);
                newItem = newItem.Concat(Fromdb).ToList();
                newItem = functionality.HandleDublicats(newItem);
                dbConnect.UpdateStorage(Email, newItem);
            }
        }



        // PUT: api/Storage/5
        [HttpPut("{id}")]
        public void Put(string id)
        {
            string buffer;
            Product newItem = new Product();
            List<Product> storageList = new List<Product>();

            Email = id;

            HttpRequest request = HttpContext.Request;
            Microsoft.AspNetCore.Http.HttpRequestRewindExtensions.EnableBuffering(request);

            using (var sr = new StreamReader(request.Body))
            {
                buffer = sr.ReadToEnd();
            }

            storageList = dbConnect.GetStorageFromUsername(Email);
            newItem = JsonConvert.DeserializeObject<Product>(buffer);

            if (newItem._amountleft != 0)
            {
                foreach (Product p in storageList)
                {
                    if (p._id == newItem._id)
                    {
                        p._state = newItem._state;
                        p._amountleft = newItem._amountleft;
                        break;
                    }
                }
            }
            else
            {
                newItem._amountleft = 1;
                int i = functionality.FindIdex(storageList, newItem);
                storageList.RemoveAt(i);
            }

            dbConnect.UpdateStorage(Email, storageList);
        }



        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}


