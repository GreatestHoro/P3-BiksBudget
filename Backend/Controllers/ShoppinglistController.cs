using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using BBCollection;
using BBCollection.BBObjects;

namespace Backend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ShoppinglistController : ControllerBase
    {
        DatabaseConnect dbConnect = new DatabaseConnect("localhost", "biksbudgetDB", "root", "BiksBudget123");
        List<Shoppinglist> shoppinglists = new List<Shoppinglist>();
        string Email;
        //DatabaseConnect dbConnect = new DatabaseConnect("localhost", "biksbudgetDB", "root", "BiksBudget123");
        // GET: api/Shoppinglist
        [HttpGet]
        public void Get()
        {

        }

        // GET: api/Storage/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            //List<Product> storageList = dbConnect.GetStorageFromUsername(id);
            shoppinglists = dbConnect.GetShoppinglists(id);

            //List<Product> resultList = ConvertBeforeSending(storageList);

            string jsonStorage = JsonConvert.SerializeObject(shoppinglists);

            return jsonStorage;
        }

        // POST: api/Shoppinglist
        [HttpPost]
        public void Post(String value)
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
                dbConnect.DeleteShoppingListFromName("Shoppinglist", Email);
            }
            else
            {
                if (buffer.Substring(0, 1) != "[")
                {
                    buffer = "[" + buffer + "]";
                }

                newItem = JsonConvert.DeserializeObject<List<Product>>(buffer);

                List<Shoppinglist> toSend = new List<Shoppinglist>();
                toSend = dbConnect.GetShoppinglists(Email);

                if (toSend.Count == 0)
                {
                    toSend.Add(new Shoppinglist("Shoppinglist", newItem));
                }
                else
                {
                    foreach (var item in newItem)
                    {
                        toSend[0]._products.Add(item);

                    }
                }

                dbConnect.AddShoppingListsToDatabase(Email, toSend);

            }

        }

        // PUT: api/Shoppinglist/5
        [HttpPut("{id}")]
        public void Put(int id, string value)
        {
            string buffer;
            Product newItem = new Product();
            //productData = test.GetStuff();

            HttpRequest request = HttpContext.Request;
            Microsoft.AspNetCore.Http.HttpRequestRewindExtensions.EnableBuffering(request);

            using (var sr = new StreamReader(request.Body))
            {
                buffer = sr.ReadToEnd();
            }

            newItem = JsonConvert.DeserializeObject<Product>(buffer);
        }
    }
}
