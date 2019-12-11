using System;
using System.Collections.Generic;
using BBCollection.BBObjects;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using BBCollection.DBHandling;
using System.Net.Http;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BiksFoodController : ControllerBase
    {
        private string buffer;
        private List<Product> Storage = new List<Product>();
        HttpResponseMessage respons = new HttpResponseMessage();
        EmptyFridgeFuntionality emptyFridgeFuntionality;
        List<WeightedRecipies> WeightedRecipies = new List<WeightedRecipies>();

        //GET: api/BiksFood
        [HttpGet("{id}")]
        public async Task<string> Get(string id)
        {
            ShoppinlistFunctionality shoppinlistFunctionality = new ShoppinlistFunctionality("api/Storage");
            respons = await shoppinlistFunctionality.GetStorageOnStart(id);
            Storage = shoppinlistFunctionality.CombinedList;
            emptyFridgeFuntionality = new EmptyFridgeFuntionality(Storage);
            WeightedRecipies = await emptyFridgeFuntionality.GetRecepies();

            string Recipies = JsonConvert.SerializeObject(OnlySendBackACoupleOfThings(10));
            return Recipies;
        }

        public List<WeightedRecipies> OnlySendBackACoupleOfThings(int x)
        {
            List<WeightedRecipies> Mads_Is_Wrong = new List<WeightedRecipies>();
            Mads_Is_Wrong.AddRange(WeightedRecipies.GetRange(0, x));
            return Mads_Is_Wrong;
        }

        // POST: api/BiksFood
        //[HttpPost]
        //public Task<List<WeightedRecipies>> Post()
        //{
        //    HttpRequest request = HttpContext.Request;
        //    Microsoft.AspNetCore.Http.HttpRequestRewindExtensions.EnableBuffering(request);

        //    using (var sr = new StreamReader(request.Body))
        //    {
        //        buffer = sr.ReadToEnd();
        //    }

        //    if (buffer.Substring(0, 1) != "[")
        //    {
        //        // If the buffer only contains one item it is not a list
        //        // and cannot be converted to a list. If [] are added it
        //        // is a list of one product.
        //        buffer = "[" + buffer + "]";
        //    }

        //    Storage = JsonConvert.DeserializeObject<List<Product>>(buffer);

        //    emptyFridgeFuntionality = new EmptyFridgeFuntionality(Storage);
        //    return emptyFridgeFuntionality.GetRecepies();
        //}

    }
}
