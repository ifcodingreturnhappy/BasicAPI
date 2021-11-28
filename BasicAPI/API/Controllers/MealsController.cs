using API.Services.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MealsController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Lasanha", "Pizza", "Burger", "Batatas cozidas", "Arroz", 
            "Fruta", "Vinho", "Agua", "Pudim", "Mousse de chocolate"
        };

        private readonly DiscordLogger _logger;

        public MealsController(DiscordLogger discordLogger)
        {
            _logger = discordLogger;
        }

        [HttpGet]
        public async Task<ActionResult<dynamic>> Get()
        {
            var rng = new Random();
            var output = Enumerable.Range(1, 5).Select(index => new
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Meal = Summaries[rng.Next(Summaries.Length)]
            });

            return new
            {
                Meals = output
            };
        }

        //[HttpGet]
        //public async Task<ActionResult<dynamic>> GetAllMeals()
        //{
        //}
    }
}
