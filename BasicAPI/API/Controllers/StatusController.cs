using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController
    {

        [HttpGet]
        public async Task<ActionResult<dynamic>> Get()
        {
            return new
            {
                Status = "Online"
            };
        }
    }
}
