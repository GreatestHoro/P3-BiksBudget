using BBCollection;
using BBCollection.BBObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;


namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        // POST: api/Login
        [HttpPost]
        public async Task<IActionResult> Post(string value)
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
         

            bool exist = await dbConnect.User.Verify(user._userName, user._password);


            if (exist == true)
            {
                return Ok(ModelState);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
