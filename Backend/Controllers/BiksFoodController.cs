using System;
using System.Collections.Generic;
using BBCollection.BBObjects;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BiksFoodController : ControllerBase
    {
        private string buffer;
        private List<Product> Storage = new List<Product>();
        EmptyFridgeFuntionality emptyFridgeFuntionality;

        //GET: api/BiksFood
        [HttpGet]
        public Task<List<WeightedRecipies>> Get(string option)
        {
            emptyFridgeFuntionality = new EmptyFridgeFuntionality(Storage);
            return emptyFridgeFuntionality.GetRecepies();
        }

        // POST: api/BiksFood
        [HttpPost]
        public void Post()
        {
            HttpRequest request = HttpContext.Request;
            Microsoft.AspNetCore.Http.HttpRequestRewindExtensions.EnableBuffering(request);

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

            Storage = JsonConvert.DeserializeObject<List<Product>>(buffer);
        }

    }
}
