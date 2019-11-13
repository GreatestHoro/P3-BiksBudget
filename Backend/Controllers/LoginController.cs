using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BBCollection.BBObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using BBCollection;
using System.Net.Http;
using System.Net;


namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        // GET: api/Login
        [HttpGet]
        public void Get()
        {
        }

        // GET: api/Login/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            char test = id[id.Length-1];

            if (test.Equals('S') == true)
            {
                return "This is the Storage";
            }
            else if(test.Equals('L') == true)
            {
                return "This is the Shoppinnlist:";
            }
            else
            {
                return "ERROR";
            }
        }

        // POST: api/Login
        [HttpPost]
        public IActionResult Post(string value)
        {
            string buffer;
            DatabaseConnect dbConnect = new DatabaseConnect("localhost", "biksbudgetdb", "root", "BiksBudget123");

            User user = new User();

            HttpRequest request = HttpContext.Request;
            Microsoft.AspNetCore.Http.HttpRequestRewindExtensions.EnableBuffering(request);

            using (var sr = new StreamReader(request.Body))
            {
                buffer = sr.ReadToEnd();
            }

            user = JsonConvert.DeserializeObject<User>(buffer);

            bool exist = dbConnect.CheckUser(user._userName, user._password);

            
            if(exist == true)
            {
                return Ok(ModelState);
                //response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            } else
            {
                return BadRequest(ModelState);
                //var message = string.Format("Product with email " + user._userName + "was not found");
                //response = new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
            }

            //return response;
        }

        // PUT: api/Login/5
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
