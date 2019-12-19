using BBCollection;
using BBCollection.BBObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class recipeController : ControllerBase
    {
        // GET: api/recipe/5
        [Route("api/recipe/{search}")]
        [HttpGet]
        public async Task<string> GetRecipes(string search)
        {
            DatabaseConnect Database = new DatabaseConnect();

            List<Recipe> recipes = await Database.Recipe.GetListAsync(search);

            return JsonConvert.SerializeObject(recipes);
        }

        // POST: api/recipe
        //[Route("api/recipe")]
        [HttpPost]
        public void Post()
        {
            string buffer;

            HttpRequest request = HttpContext.Request;
            Microsoft.AspNetCore.Http.HttpRequestRewindExtensions.EnableBuffering(request);

            using (var sr = new StreamReader(request.Body))
            {
                buffer = sr.ReadToEnd();
            }

            List<Recipe> recipeDataList = JsonConvert.DeserializeObject<List<Recipe>>(buffer);
        }
    }
}
