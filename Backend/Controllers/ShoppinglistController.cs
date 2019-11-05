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

namespace Backend.Controllers
{
    public class TestProduct
    {
        List<CoopProduct> stuff = new List<CoopProduct>
        {
            new CoopProduct{Navn = "Kyllingebryst", Navn2 = "100g", Ean = "1233", Pris = 100.00, VareHierakiId = 2525, Id = 1},
            new CoopProduct{Navn = "Kyllingepålæg", Navn2 = "80g", Ean = "0239", Pris = 15.00, VareHierakiId = 5525, Id = 2},
            new CoopProduct{Navn = "Mælk", Navn2 = "1000g", Ean = "1293", Pris = 9.99, VareHierakiId = 2125, Id = 3},
            new CoopProduct{Navn = "Mel", Navn2 = "2000g", Ean = "1533", Pris = 20.00, VareHierakiId = 2540, Id = 4}
        };

        public List<CoopProduct> GetStuff()
        {
            return stuff;
        }
    }

    public class CoopProduct
    {
        public string Ean { get; set; }
        public string Navn { get; set; }
        public string Navn2 { get; set; }
        public double Pris { get; set; }
        public int VareHierakiId { get; set; }
        public int Id { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ShoppinglistController : ControllerBase
    {
        public TestProduct test = new TestProduct();
        List<CoopProduct> productData;

        // GET: api/Shoppinglist
        [HttpGet]
        public string Get()
        {
            productData = test.GetStuff();

            string jsonRecipes = JsonConvert.SerializeObject(productData);

            return jsonRecipes;
        }

        // GET: api/Shoppinglist/5
        //[Route("api/Shoppinglist/{value}")]
        //[HttpGet]
        //public HttpResponseMessage Get(string value)
        //{
        //    var stuff = new HttpResponseMessage(System.Net.HttpStatusCode.OK);

        //    List<CoopProduct> newItem = JsonConvert.DeserializeObject<List<CoopProduct>>(value);

        //    productData.Add(newItem[0]);

        //    return stuff;
        //}

        //// GET: api/Shoppinglist/5
        //[Route("api/Shoppinglist/{id}")]
        //[HttpGet]
        //public string Get(int id)
        //{
        //    productData.Remove(productData.First(x => x.Id == id));

        //    int i = 1;

        //    foreach (var product in productData)
        //    {
        //        product.Id = i;
        //        i++;
        //    }

        //    return "Person nr " + id.ToString();

        //    //productData = JsonConvert.DeserializeObject<List<CoopProduct>>(id);
        //}

        // POST: api/Shoppinglist
        [HttpPost]
        public void Post(String value)
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

        // PUT: api/Shoppinglist/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //    productData = test.GetStuff();

        //    productData.Remove(productData.First(x => x.Id == id));

        //    int i = 1;
        //    foreach (var product in productData)
        //    {
        //        product.Id = i;
        //        i++;
        //    }
        //}
    }
}
