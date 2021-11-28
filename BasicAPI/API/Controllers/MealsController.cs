using API.Services.Logging;
using DataLayer.Abstractions;
using DataModels.Products;
using Microsoft.AspNetCore.Authorization;
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
        private readonly DiscordLogger _logger;

        private readonly IDBReader _dbReader;
        private readonly IDBWriter _dbWriter;
        private readonly IDBDeleter _dbDeleter;

        public MealsController(DiscordLogger discordLogger, IDBReader dBReader, IDBWriter dBWriter, IDBDeleter dBDeleter)
        {
            _logger = discordLogger;

            _dbReader = dBReader;
            _dbWriter = dBWriter;
            _dbDeleter = dBDeleter;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<dynamic>> Get()
        {
            // Get all the meals from the db
            var allMeals = _dbReader.ReadAll<Meal>();

            return new
            {
                Meals = allMeals
            };
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<dynamic>> Post([FromBody] Meal meal)
        {
            // Check if data to add to db is valid
            if (!meal.HasValidData())
                return new
                {
                    Status = "Meal data is invalid",
                    Result = 0
                };

            // Check if meal with the same name already exists
            var existingMeal = _dbReader.ReadById<Meal>(meal.Title);
            if (existingMeal != null)
                return new
                {
                    Status = "Meal already exists. Choose different name.",
                    Result = 0
                };

            // Meal does not not yet exist, add it to the db
            var result = await _dbWriter.WriteAsync(meal, update: false);

            // Return result
            return new
            {
                Result = result
            };
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<dynamic>> Put([FromBody] Meal meal)
        {
            // Check if data to add to db is valid
            if (!meal.HasValidData())
                return new
                {
                    Status = "Meal data is invalid",
                    Result = 0
                };

            // Upsert meal to db
            var result = await _dbWriter.WriteAsync(meal, update: true);

            // Return result
            return new
            {
                Result = result
            };
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<dynamic>> Delete([FromBody] Meal meal)
        {
            // Check if the meal provided has a valid title
            if (!meal.IsTitleValid())
                return new
                {
                    Status = "Meal title is not valid.",
                    Result = 0
                };

            // Try to delete target meal from db
            var result = _dbDeleter.DeleteById<Meal>(meal.Title);

            return new
            {
                Result = result
            };
        }


        [HttpGet]
        [Route("tags")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<dynamic>> GetTags()
        {
            // Get all meal tags from the db
            var tags = _dbReader.ReadAll<MealTags>()?.Select(x => x.Tag)
                                                     .ToList();

            return new
            {
                Tags = tags
            };
        }

        [HttpPut]
        [Route("tags")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<dynamic>> UpdateTags([FromBody] List<MealTags> newTags)
        {
            // Get current tags from db
            var currentTags = _dbReader.ReadAll<MealTags>();

            // Upsert new ones in the database
            var upsertResult = await _dbWriter.WriteManyAsync(newTags);
            if (upsertResult < 0)
                return new { Result = upsertResult };

            // Get the items to delete from the database
            var toDelete = currentTags.Where(p => newTags.All(p2 => p2.Tag != p.Tag)).ToList();

            // Delete the ones in excess
            int deletionResult = 1;
            foreach (var mealTag in toDelete)
            {
                deletionResult = _dbDeleter.DeleteById<MealTags>(mealTag.Tag);
                if (deletionResult <= 0)
                    return new { Result = deletionResult };
            }

            return new
            {
                Result = deletionResult
            };
        }

        [HttpGet]
        [Route("types")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<dynamic>> GetMealTypes()
        {
            // Get all meal tags from the db
            var mealTypes = _dbReader.ReadAll<MealTypes>()?.Select(x => x.Type)
                                                            .ToList();

            return new
            {
                Types = mealTypes
            };
        }

        [HttpPut]
        [Route("types")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<dynamic>> UpdateMealTypes([FromBody] List<MealTypes> newTypes)
        {
            // Get current tags from db
            var currentMeals = _dbReader.ReadAll<MealTypes>();

            // Upsert new ones in the database
            var upsertResult = await _dbWriter.WriteManyAsync(newTypes);
            if (upsertResult < 0)
                return new { Result = upsertResult };

            // Get the items to delete from the database
            var toDelete = currentMeals.Where(p => newTypes.All(p2 => p2.Type != p.Type)).ToList();

            // Delete the ones in excess
            int deletionResult = 1;
            foreach (var mealType in toDelete)
            {
                deletionResult = _dbDeleter.DeleteById<MealTypes>(mealType.Type);
                if (deletionResult <= 0)
                    return new { Result = deletionResult };
            }

            return new
            {
                Result = deletionResult
            };
        }
    }
}
