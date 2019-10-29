using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Backend.Controllers
{

    public class Recipe
    {
        public string _title { get; set; }
        public Recipe(string title)
        {
            _title = title;
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class recipeController : ControllerBase
    {
        // GET: api/recipe
        //[Route("api/recipe/{gender:string}")]

        Recipe recipe = new Recipe("default");

        [HttpGet]
        public string GetRecipes(string recipeTitle = "kaffe", string filter = "all")
        {

            string res = "{ ";
            switch(recipeTitle)
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

            switch(filter)
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

            recipe._title = res;

            string str = JsonConvert.SerializeObject(recipe);


            return str;
        }

        // GET: api/recipe/5
        [Route("api/recipe/{id}")]
        [HttpGet]
        public string GetRecipes(int id)
        {
            return "value";
        }

        // POST: api/recipe
        [HttpPost]
        public void Post([FromBody] string value)
        {
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
