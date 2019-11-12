using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BBCollection.BBObjects;
using Newtonsoft.Json;
using BBCollection;
using System.IO;
using Microsoft.AspNetCore.Http;
using System;
using BBGatherer.Queries;

namespace Backend.Controllers
{

    public class DemoData
    {
        public List<Ingredient> in1 = new List<Ingredient> { new Ingredient("kalkun", "kg", 10f) };

        public Recipe kalkun = new Recipe(1, "kalkun", "spis det nu bare!", new List<Ingredient> { new Ingredient("kalkun", "kg", 10f) }, 4);

        public Recipe slik = new Recipe(1, "slik", "spis det om fredagen!", new List<Ingredient> { new Ingredient("sukker+chokolade", "ton", 1f) }, 1);

        public List<Recipe> recipeDatak = new List<Recipe>();

        public DemoData()
        {
            recipeDatak.Add(kalkun);
            recipeDatak.Add(slik);
        }

        public List<Recipe> GetRecipes()
        {
            return recipeDatak;
        }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class recipeController : ControllerBase
    {

        public DemoData demodata = new DemoData();

        // GET: api/recipe
        //[Route("api/recipe/recipeTitle=laks?filter=keto")]
        [HttpGet]
        public string GetRecipes(string recipeTitle = "kaffe", string filter = "all")
        {
            string res = "";
            switch (recipeTitle)
            {
                case "kaffe":
                    res += "kaffe &";
                    break;
                case "laks":
                    res += "laks &";
                    break;
                case "ost":
                    res += "ost &";
                    break;
                default:
                    res += "ingen mad til dig &";
                    break;
            }

            switch (filter)
            {
                case "all":
                    res += "du er normal";
                    break;
                case "keto":
                    res += "det er ok";
                    break;
                case "vegetar":
                    res += "det er skidt";
                    break;
                default:
                    res += "no match!!!";
                    break;
            }

            //RecipeQuery recipeQuery = new RecipeQuery();

            //recipeQuery.CheapestCRecipes(recipeTitle);

            List<Recipe> recipeData = demodata.GetRecipes();

            string jsonRecipes = JsonConvert.SerializeObject(recipeData);

            DatabaseConnect dbConnect = new DatabaseConnect("localhost", "BiksBudgetDB", "root", "BiksBudget123");

            string jsonDBRecipes = JsonConvert.SerializeObject(dbConnect.GetRecipes(recipeTitle));
 

            return jsonDBRecipes;
        }

        // GET: api/recipe/5
        [Route("api/recipe/{search}")]
        [HttpGet]
        public string GetRecipes(string search)
        {
            DatabaseConnect Database = new DatabaseConnect("pass","yoot","root","yeet");
            return JsonConvert.SerializeObject(Database.GetRecipes(search));
        }

        // POST: api/recipe
        //[Route("api/recipe")]
        [HttpPost]
        public void Post()
        {
            string buffer, buffer2;

            HttpRequest request = HttpContext.Request;
            Microsoft.AspNetCore.Http.HttpRequestRewindExtensions.EnableBuffering(request);

            using (var sr = new StreamReader(request.Body))
            {
                buffer = sr.ReadToEnd();

                ////reset
                //Request.Body.Seek(0, SeekOrigin.Begin);

                ////ready to read again
                //buffer2 = sr.ReadToEnd();

            }

            List<Recipe> recipeDataList = JsonConvert.DeserializeObject<List<Recipe>>(buffer);

        }

        // PUT: api/recipe/5
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
