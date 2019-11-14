using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using BBCollection;
using BBCollection.BBObjects;

namespace Backend.Controllers
{
    public class AddedProduct
    {
        public string Name { get; set; }
        public string Amount { get; set; }
        public double Price { get; set; }
        public long Id { get; set; }
        public string State { get; set; }
        public string TimeAdded { get; set; }
        public string UniqueId { get; set; }
        public string Image { get; set; }
        public string StoreName { get; set; }
        public int AmountOfItem { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        DatabaseConnect dbConnect = new DatabaseConnect("localhost", "biksbudgetDB", "root", "BiksBudget123");
        List<AddedProduct> resultList = new List<AddedProduct>();
        int i = 0;
        string Email;

        // GET: api/Storage
        [HttpGet]
        public string Get()
        {
            List<Product> storageList = dbConnect.GetStorageFromUsername("Test6");

            resultList = ConvertBeforeSending(storageList);

            string jsonRecipes = JsonConvert.SerializeObject(resultList);

            return jsonRecipes;
        }

        // GET: api/Storage/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            List<Product> storageList = dbConnect.GetStorageFromUsername(id);

            List<AddedProduct> resultList = ConvertBeforeSending(storageList);

            string jsonStorage = JsonConvert.SerializeObject(resultList);

            return jsonStorage;

            //productData = StorageTest.GetStuff();

            //foreach (var item in productData)
            //{
            //    if (item.Id == id)
            //    {
            //        item.State = state;
            //        break;
            //    }
            //}

            //return "value";
        }

        private List<AddedProduct> ConvertBeforeSending(List<Product> bbList)
        {
            List<AddedProduct> result = new List<AddedProduct>();
            AddedProduct tempProduct = new AddedProduct();
            int i = 1;

            foreach (var item in bbList)
            {
                item._id = item._id.Remove(0, 1);

                result.Add(new AddedProduct
                {
                    Amount = item._amount,
                    Id = Convert.ToInt64(item._id),
                    State = item._amountleft.ToString(),
                    Image = item._image,
                    Name = item._productName,
                    Price = item._price,
                    StoreName = item._storeName,
                    TimeAdded = item._timeAdded,
                    AmountOfItem = 2 // IMPLEMENT PLS
                });
            }

            return result;
        }

        // POST: api/Storage
        [HttpPost]
        public void Post(string value)
        {
            string buffer;
            List<AddedProduct> newItem = new List<AddedProduct>();
            List<Product> storageList = new List<Product>(); /*dbConnect.GetStorageFromUsername("Test6");*/

            List<AddedProduct> resultList = ConvertBeforeSending(storageList);
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
                resultList.Clear();
            }
            else
            {
                if (buffer.Substring(0, 1) != "[")
                {
                    buffer = "[" + buffer + "]";

                    newItem = JsonConvert.DeserializeObject<List<AddedProduct>>(buffer);
                    resultList.Add(newItem[0]);
                    newItem.Clear();
                }
                else
                {
                    newItem = JsonConvert.DeserializeObject<List<AddedProduct>>(buffer);
                    resultList = newItem;

                }

                if (resultList.Count != 0)
                {
                    resultList.Last().Id = resultList.Count;

                }
            } 
        }

        // PUT: api/Storage/5
        [HttpPut("{id}")]
        public void Put(int id, string value)
        {
            string buffer;
            AddedProduct newItem = new AddedProduct();
            List<Product> storageList = dbConnect.GetStorageFromUsername("Test6");

            List<AddedProduct> resultList = ConvertBeforeSending(storageList);
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

            resultList[id-1] = newItem;
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
