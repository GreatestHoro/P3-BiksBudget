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
    public class StorageTestList
    {
        List<CoopProduct> StorageTest = new List<CoopProduct>
        {
            new CoopProduct{Navn = "Kyllingebryst", Navn2 = "100g", Ean = "1233", Pris = 100.00, VareHierakiId = 2525, Id = 1, State = "Full", TimeAdded = "07/11/2019 10:37:43"},
            new CoopProduct{Navn = "Kyllingepålæg", Navn2 = "80g", Ean = "0239", Pris = 15.00, VareHierakiId = 5525, Id = 2, State = "Full", TimeAdded = "06/10/2019 22:00:43"},
            new CoopProduct{Navn = "Mælk", Navn2 = "1000g", Ean = "1293", Pris = 9.99, VareHierakiId = 2125, Id = 3, State = "Full", TimeAdded = "06/02/2019 07:27:20"},
            new CoopProduct{Navn = "Mel", Navn2 = "2000g", Ean = "1533", Pris = 20.00, VareHierakiId = 2540, Id = 4, State = "Full", TimeAdded = "06/11/2019 13:01:52"}
        };

        public List<CoopProduct> GetStuff()
        {
            return StorageTest;
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        public StorageTestList StorageTest = new StorageTestList();
        List<CoopProduct> productData;

        // GET: api/Storage
        [HttpGet]
        public string Get()
        {
            productData = StorageTest.GetStuff();

            string jsonRecipes = JsonConvert.SerializeObject(productData);

            return jsonRecipes;
        }

        // GET: api/Storage/5
        [HttpGet("{id}")]
        public string Get(int id, string state)
        {
            productData = StorageTest.GetStuff();

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
            productData = StorageTest.GetStuff();

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
            productData = StorageTest.GetStuff();

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
