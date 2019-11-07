using System;
using System.Collections.Generic;
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
    public class StorageController : ControllerBase
    {
        public TestProduct test = new TestProduct();
        List<CoopProduct> productData;

        // GET: api/Storage
        [HttpGet]
        public string Get()
        {
            productData = test.GetStuff();

            string jsonRecipes = JsonConvert.SerializeObject(productData);

            return jsonRecipes;
        }

        // GET: api/Storage/5
        [HttpGet("{id}")]
        public string Get(int id, string state)
        {
            productData = test.GetStuff();

            foreach (var item in productData)
            {
                if (item.Id == id)
                {
                    item.State = state;
                    break;
                }
            }

            return "value";
        }

        // POST: api/Storage
        [HttpPost]
        public void Post(string value)
        {
            string buffer;
            List<CoopProduct> newItem = new List<CoopProduct>();
            productData = test.GetStuff();

            HttpRequest request = HttpContext.Request;
            Microsoft.AspNetCore.Http.HttpRequestRewindExtensions.EnableBuffering(request);

            using (var sr = new StreamReader(request.Body))
            {
                buffer = sr.ReadToEnd();
            }

            if (buffer.Substring(0, 1) != "[")
            {
                buffer = "[" + buffer + "]";

                newItem = JsonConvert.DeserializeObject<List<CoopProduct>>(buffer);
                productData.Add(newItem[0]);
                newItem.Clear();
            }
            else
            {
                newItem = JsonConvert.DeserializeObject<List<CoopProduct>>(buffer);
                productData = newItem;
            }
        }

        // PUT: api/Storage/5
        [HttpPut("{id}")]
        public void Put(int id, string value)
        {
            string buffer;
            CoopProduct newItem = new CoopProduct();
            productData = test.GetStuff();

            HttpRequest request = HttpContext.Request;
            Microsoft.AspNetCore.Http.HttpRequestRewindExtensions.EnableBuffering(request);

            using (var sr = new StreamReader(request.Body))
            {
                buffer = sr.ReadToEnd();
            }

            newItem = JsonConvert.DeserializeObject<CoopProduct>(buffer);

            foreach (var item in productData)
            {
                if (item.Id == id)
                {
                    item.State = newItem.State;
                    break;
                }
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
