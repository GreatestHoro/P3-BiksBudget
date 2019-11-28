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

        /// <summary>
        /// This method returns the storage for a specific user
        /// </summary>
        /// <param name="_email"></param>
        /// <returns></returns>

        // GET: api/Storage/5
        [HttpGet("{_email}")]
        public string Get(string _email)
        {
            List<Product> storageList = dbConnect.GetStorageFromUsername(_email);

            string jsonStorage = JsonConvert.SerializeObject(storageList);

            return jsonStorage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_email"></param>

        // POST: api/Storage
        [HttpPost("{_email}")]
        public void Post(string _email)
        {
            List<Product> newItem = new List<Product>();
            List<Product> Fromdb = new List<Product>();
            
            string buffer;

            HttpRequest request = HttpContext.Request;
            Microsoft.AspNetCore.Http.HttpRequestRewindExtensions.EnableBuffering(request);

            Email = _email;

            using (var sr = new StreamReader(request.Body))
            {
                buffer = sr.ReadToEnd();
            }

            if (buffer.Substring(0, 1) != "[")
            {
                // If the buffer only contains one item it is not a list
                // and cannot be converted to a list. If [] are added it
                // is a list of one product.
                buffer = "[" + buffer + "]";
            }

            // The list of products to add to storage
            newItem = JsonConvert.DeserializeObject<List<Product>>(buffer);

            if (newItem.Count == 0)
            {
                // If the list of items to add is empty, storage is deleted
                dbConnect.UpdateStorage(Email, newItem);
            }
            else
            {
                // Else the storage in the database is requested
                Fromdb = dbConnect.GetStorageFromUsername(Email);

                // The two lists from the database and the frontend are merged into one
                newItem = newItem.Concat(Fromdb).ToList();

                // Dublicats are handled
                newItem = functionality.HandleDublicats(newItem);

                // The updated list without dublicats are added to storage
                dbConnect.UpdateStorage(Email, newItem);
            }
        }

        /// <summary>
        /// This method is used when items in the storage are changed
        /// in either amount or state
        /// </summary>
        /// <param name="_email"></param>

        // PUT: api/Storage/5
        [HttpPut("{_email}")]
        public void Put(string _email)
        {
            string buffer;
            Product newItem = new Product();
            List<Product> storageList = new List<Product>();

            Email = _email;

            HttpRequest request = HttpContext.Request;
            Microsoft.AspNetCore.Http.HttpRequestRewindExtensions.EnableBuffering(request);

            using (var sr = new StreamReader(request.Body))
            {
                // If the buffer only contains one item it is not a list
                // and cannot be converted to a list. If [] are added it
                // is a list of one product.
                buffer = sr.ReadToEnd();
            }

            // The items in storage in the database is requested
            storageList = dbConnect.GetStorageFromUsername(Email);

            // The item to change
            newItem = JsonConvert.DeserializeObject<Product>(buffer);

            if (newItem._amountleft != 0)
            {
                foreach (Product p in storageList)
                {
                    if (p._id == newItem._id)
                    {
                        // On match, the new attributes are set
                        p._state = newItem._state;
                        p._amountleft = newItem._amountleft;
                        break;
                    }
                }
            }
            else
            {
                // If amountleft is zero, the item is deleted
                newItem._amountleft = 1;
                int i = functionality.FindIdex(storageList, newItem);
                storageList.RemoveAt(i);
            }

            dbConnect.UpdateStorage(Email, storageList);
        }
    }
}


