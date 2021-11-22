using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationController : ControllerBase
    {
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<dynamic>> Register(/*[FromBody] User user*/)
        {
            return new
            {
            };
        }
    }
}
