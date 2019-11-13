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
    public class ShoppinglistTestList
    {
        List<AddedProduct> StorageTest = new List<AddedProduct>
        {
            new AddedProduct{ Name = "Kylling", Amount = "200g", Id = 1, State = "Full", TimeAdded = "07/11/2019 10:37:43", Price = 27.00, AmountOfItem = 1, StoreName = "Fakta" },
            new AddedProduct{ Name = "Oksekød", Amount = "500g", Id = 2, State = "Full", TimeAdded = "06/10/2019 22:00:43", Price = 36.00, AmountOfItem = 1, StoreName = "Fakta" },
            new AddedProduct{ Name = "Laks", Amount = "280g", Id = 3, State = "Full", TimeAdded = "06/02/2019 07:27:20", Price = 55.00, AmountOfItem = 1, StoreName = "DagliBrugsen" },
            new AddedProduct{ Name = "Lammebov", Amount = "1000g", Id = 4, State = "Full", TimeAdded = "06/11/2019 13:01:52", Price = 270.00, AmountOfItem = 1, StoreName = "Føtex" }        
        };

        public List<AddedProduct> GetStuff()
        {
            return StorageTest;
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
        public string State { get; set; }
        public string TimeAdded { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ShoppinglistController : ControllerBase
    {
        public ShoppinglistTestList test = new ShoppinglistTestList();
        List<AddedProduct> productData;

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
            List<AddedProduct> newItem = new List<AddedProduct>();
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

                newItem = JsonConvert.DeserializeObject<List<AddedProduct>>(buffer);
                productData.Add(newItem[0]);
                newItem.Clear();
            }
            else
            {
                newItem = JsonConvert.DeserializeObject<List<AddedProduct>>(buffer);
                productData = newItem;
            }

            if (productData.Count != 0)
            {
                productData.Last().Id = productData.Count;

            }
        }

        // PUT: api/Shoppinglist/5
        [HttpPut("{id}")]
        public void Put(int id, string value)
        {
            string buffer;
            AddedProduct newItem = new AddedProduct();
            productData = test.GetStuff();

            HttpRequest request = HttpContext.Request;
            Microsoft.AspNetCore.Http.HttpRequestRewindExtensions.EnableBuffering(request);

            using (var sr = new StreamReader(request.Body))
            {
                buffer = sr.ReadToEnd();
            }

            newItem = JsonConvert.DeserializeObject<AddedProduct>(buffer);

            foreach (var item in productData)
            {
                if (item.Id == id)
                {
                    item.Amount = newItem.Amount;
                    
                    break;
                }
            }
        }

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
