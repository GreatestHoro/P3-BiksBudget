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
using BBCollection.DBHandling;

namespace Backend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ShoppinglistController : ControllerBase
    {
        DatabaseConnect dbConnect = new DatabaseConnect();
        ControllerFuncionality funcionality = new ControllerFuncionality();
        List<Shoppinglist> shoppinglists = new List<Shoppinglist>();
        List<Shoppinglist> toSend = new List<Shoppinglist>();
        List<Product> newItems = new List<Product>();

        string buffer;
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
            shoppinglists = dbConnect.GetShoppinglists(id);

            string jsonStorage = JsonConvert.SerializeObject(shoppinglists);

            return jsonStorage;
        }

        // POST: api/Shoppinglist
        [HttpPost("{value}")]
        public void Post(String value)
        {
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

            newItems = JsonConvert.DeserializeObject<List<Product>>(buffer);

            if (newItems.Count == 0)
            {
                dbConnect.DeleteShoppingListFromName("Shoppinglist", Email);
            }
            else
            {
                toSend = dbConnect.GetShoppinglists(Email);

                if (toSend.Count == 0)
                {
                    toSend.Add(new Shoppinglist("Shoppinglist", newItems));
                }
                else
                {
                    toSend[0]._products.Clear();
                    foreach (var item in newItems)
                    {
                        toSend[0]._products.Add(item);

                    } 
                }

                dbConnect.DeleteShoppingListFromName("Shoppinglist", Email);
                dbConnect.AddShoppingListsToDatabase(Email, toSend);
                
            }

        }

        // PUT: api/Shoppinglist/5
        [HttpPut("{id}")]
        public void PutQuick(string id)
        {
            HttpRequest request = HttpContext.Request;
            Microsoft.AspNetCore.Http.HttpRequestRewindExtensions.EnableBuffering(request);

            Email = id;

            using (var sr = new StreamReader(request.Body))
            {
                buffer = sr.ReadToEnd();
            }

            if (buffer.Substring(0, 1) != "[")
            {
                buffer = "[" + buffer + "]";
            }

            newItems = JsonConvert.DeserializeObject<List<Product>>(buffer);

            toSend = dbConnect.GetShoppinglists(Email);

            if (toSend.Count == 0)
            {
                toSend.Add(new Shoppinglist("Shoppinglist", newItems));
            }
            else
            {
                foreach (Product item in newItems)
                {
                    toSend[0]._products.Add(item);
                }
            }
            toSend[0]._products = funcionality.HandleDublicats(toSend[0]._products);

            dbConnect.DeleteShoppingListFromName("Shoppinglist", Email);
            dbConnect.AddShoppingListsToDatabase(Email, toSend);
        }
    }
}
