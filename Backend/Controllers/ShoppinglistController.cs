using System;
using System.Collections.Generic;
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
        public int Id;
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
        [Route("Shop/{id}")]
        [HttpGet]
        public string Get(int id)
        {


            return "Person nr " + id.ToString();

            //productData = JsonConvert.DeserializeObject<List<CoopProduct>>(id);
        }

        // POST: api/Shoppinglist
        [HttpPost]
        public void Post([FromBody]String value)
        {


            //HttpResponseMessage response = new HttpResponseMessage();
            //string stuff = value.ToString();

            //productData = JsonConvert.DeserializeObject<List<CoopProduct>>(stuff);

            //return Ok();
        }

        // PUT: api/Shoppinglist/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public HttpResponseMessage Delete(int id)
        {
            HttpResponseMessage pls = new HttpResponseMessage();

            productData.Remove(productData.First(x => x.Id == id));

            //int i = 1;

            //foreach (var product in productData)
            //{
            //    product.Id = i;
            //    i++;
            //}

            return pls;
        }
    }
}
