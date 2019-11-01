using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Backend.Controllers
{
    public class TestProduct
    {
        List<CoopProduct> stuff = new List<CoopProduct>
        {
            new CoopProduct{Navn = "Kyllingebryst", Navn2 = "100g", Ean = "1233", Pris = 100.00, VareHierakiId = 2525},
            new CoopProduct{Navn = "Kyllingepålæg", Navn2 = "80g", Ean = "0239", Pris = 15.00, VareHierakiId = 5525},
            new CoopProduct{Navn = "Mælk", Navn2 = "1000g", Ean = "1293", Pris = 9.99, VareHierakiId = 2125},
            new CoopProduct{Navn = "Mel", Navn2 = "2000g", Ean = "1533", Pris = 20.00, VareHierakiId = 2540}
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

            //return new string[] { "value1", "value2" };
        }

        // GET: api/Shoppinglist/5
        [HttpGet("{id}")]
        public void Get([FromBody]string _test)
        {
            //return "value";
            productData = JsonConvert.DeserializeObject<List<CoopProduct>>(_test);
        }

        // POST: api/Shoppinglist
        [Route("api/Shoppinglist/save")]
        [HttpPost]
        public void Post([FromBody] string value)
        {
            productData = JsonConvert.DeserializeObject<List<CoopProduct>>(value);
            Console.WriteLine("stuff");
        }

        // PUT: api/Shoppinglist/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
