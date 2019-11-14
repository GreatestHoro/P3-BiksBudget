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
    public class AddedProduct
    {
        public string Name { get; set; }
        public string Amount { get; set; }
        public double Price { get; set; }
        public int Id { get; set; }
        public string State { get; set; }
        public string TimeAdded { get; set; }
        public string UniqueId { get; set; }
        public string Image { get; set; }
        public string StoreName { get; set; }
        public int AmountOfItem { get; set; }
    }

    public class StorageTestList
    {
        List<AddedProduct> StorageTest = new List<AddedProduct>
        {
            new AddedProduct{Name = "Kylling", Amount = "200g", Id = 1, State = "Full", TimeAdded = "07/11/2019 10:37:43", Price = 27.00, AmountOfItem = 1 },
            new AddedProduct{Name = "Oksekød", Amount = "500g", Id = 2, State = "Full", TimeAdded = "06/10/2019 22:00:43", Price = 36.00, AmountOfItem = 4 },
            new AddedProduct{Name = "Laks", Amount = "280g", Id = 3, State = "Full", TimeAdded = "06/02/2019 07:27:20", Price = 55.00, AmountOfItem = 10 },
            new AddedProduct{Name = "Lammebov", Amount = "1000g", Id = 4, State = "Full", TimeAdded = "06/11/2019 13:01:52", Price = 270.00, AmountOfItem = 2 }        
        };

        public List<AddedProduct> GetStuff()
        {
            return StorageTest;
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        public StorageTestList StorageTest = new StorageTestList();
        List<AddedProduct> productData;
        int i = 0;
        string Email;

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
            List<AddedProduct> newItem = new List<AddedProduct>();
            productData = StorageTest.GetStuff();
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
                //Delete the entire list
                productData.Clear();
            }
            else
            {
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
        }

        // PUT: api/Storage/5
        [HttpPut("{id}")]
        public void Put(int id, string value)
        {
            string buffer;
            AddedProduct newItem = new AddedProduct();
            productData = StorageTest.GetStuff();
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


            newItem = JsonConvert.DeserializeObject<AddedProduct>(buffer);

            productData[id-1] = newItem;
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
