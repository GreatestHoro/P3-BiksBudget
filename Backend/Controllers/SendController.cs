using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Net.Http;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendController : ControllerBase
    {
        // GET: api/Send/5
        [HttpGet("{Serialized}")]
        public string Get(String Serialized)
        {
            return Serialized;
        }
        
        // Post: api/Send/5

        [HttpPost]
        [Route(nameof(Post))]
        public void Post([FromBody]String Serialized)
        {
            
        }

    }
}
