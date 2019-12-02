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

        // GET: api/Shoppinglist
        [HttpGet]
        public void Get()
        {
        }


        /// <summary>
        /// Requests the shoppinglist from a user
        /// Returns it to the frontend as a string.
        /// </summary>
        /// <param name="_email"></param>
        /// <returns></returns>

        // GET: api/Storage/5
        [HttpGet("{_email}")]
        public async Task<string> Get(string _email)
        {
            shoppinglists = await dbConnect.Shoppinglist.GetList(_email);

            string jsonStorage = JsonConvert.SerializeObject(shoppinglists);

            return jsonStorage;
        }

        /// <summary>
        /// The new shoppinglist to store in the database is send
        /// and is stored in the user.
        /// </summary>
        /// <param name="_email">The name of the user</param>

        // POST: api/Shoppinglist
        [HttpPost("{_email}")]
        public async void Post(String _email)
        {
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

            newItems = JsonConvert.DeserializeObject<List<Product>>(buffer);

            if (newItems.Count == 0)
            {
                // If an empty list is posted, the shoppinglist named "shoppinglist" is deleted.
                await dbConnect.Shoppinglist.Delete("Shoppinglist", Email);
            }
            else
            {
                // If there is a list to add to the shoppingst, the already existing shoppinglist is
                // requested from the database.
                toSend = await dbConnect.Shoppinglist.GetList(Email);

                if (toSend.Count == 0)
                {
                    // If the shoppinglist in the database is emtpy, a new is created, and the input
                    // list from frontend is added.
                    toSend.Add(new Shoppinglist("Shoppinglist", newItems));
                }
                else
                {
                    // If the shoppinglist exists the products are deleted and the new ones are deleted
                    toSend[0]._products.Clear();
                    foreach (var item in newItems)
                    {
                        toSend[0]._products.Add(item);
                    } 
                }

                await dbConnect.Shoppinglist.Delete("Shoppinglist", Email);
                await dbConnect.Shoppinglist.AddList(Email, toSend);
            }
        }

        /// <summary>
        /// The put method is called when a list of products
        /// or one product is added to the shoppinglist, 
        /// rather than replaced.
        /// </summary>
        /// <param name="_email"></param>

        // PUT: api/Shoppinglist/5
        [HttpPut("{_email}")]
        public async void PutQuick(string _email)
        {
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

            // The products to add to shoppinglist
            newItems = JsonConvert.DeserializeObject<List<Product>>(buffer);

            // The products already in the shoppinglist
            toSend = await dbConnect.Shoppinglist.GetList(Email);

            if (toSend.Count == 0)
            {
                // If there is no shoppinglist, all products are added to a new shoppinglist
                toSend.Add(new Shoppinglist("Shoppinglist", newItems));
            }
            else
            {
                // Else all products are added to the existing shoppinglist
                foreach (Product item in newItems)
                {
                    toSend[0]._products.Add(item);
                }
            }

            // The dublicats in the shoppinglist are sorted out
            toSend[0]._products = funcionality.HandleDublicats(toSend[0]._products);

            await dbConnect.Shoppinglist.Delete("Shoppinglist", Email);
            await dbConnect.Shoppinglist.AddList(Email, toSend);
        }
    }
}
