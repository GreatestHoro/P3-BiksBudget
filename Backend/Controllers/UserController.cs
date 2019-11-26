using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BBCollection;
using BBCollection.BBObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Backend.Controllers
{
    [Route("User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [Route("Register")]
        [HttpPost]
        public IActionResult Register(string value)
        {
            string buffer;
            DatabaseConnect dbConnect = new DatabaseConnect();

            User user = new User();

            HttpRequest request = HttpContext.Request;
            Microsoft.AspNetCore.Http.HttpRequestRewindExtensions.EnableBuffering(request);

            using (var sr = new StreamReader(request.Body))
            {
                buffer = sr.ReadToEnd();
            }

            user = JsonConvert.DeserializeObject<User>(buffer);

            string username = user._userName;
            string password = user._password;

            dbConnect.AddUser(username, password);

            if (dbConnect.CheckUser(user._userName, user._password))
            {
                return Ok(ModelState);
                //response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            }
            else
            {
                return BadRequest(ModelState);
                //var message = string.Format("Product with email " + user._userName + "was not found");
                //response = new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
            }

            //return response;
        }
    }
}