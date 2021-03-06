﻿using BBCollection;
using BBCollection.BBObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(string value)
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

            bool result = await dbConnect.User.Verify(user._userName, user._password);
            if (result)
            {
                return BadRequest(ModelState);
            }

            string username = user._userName;
            string password = user._password;

            await dbConnect.User.Add(username, password);

            return Ok(ModelState);
        }
    }
}