using API.Services;
using DataLayer.Abstractions;
using DataModels.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasherService _passwordHasherService;

        private readonly IDBWriter _dbWriter;
        private readonly IDBReader _dbReader;
        private readonly IDBDeleter _dBDeleter;

        public UserController(ITokenService tokenService, IPasswordHasherService passwordHasherService,
            IDBWriter dbWriter, IDBReader dbReader, IDBDeleter dbDeleter)
        {
            _tokenService = tokenService;
            _passwordHasherService = passwordHasherService;
            _dbWriter = dbWriter;
            _dbReader = dbReader;
            _dBDeleter = dbDeleter;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<dynamic>> Register([FromBody] User user)
        {
            try
            {
                // Check if the user sign in data is valid 
                if (!user.HasValidData())
                    return new { Message = "Invalid data." };

                // Check if user does not exist yet
                var userExists = _dbReader.ReadById<User>(user.Email) != null;

                if (userExists)
                    return new { Error = "Email already taken." };
                else
                {
                    // Hash the password
                    var hashedPassword = await _passwordHasherService.HashPassword(user.Password);
                    user.Password = hashedPassword.Hash;
                    user.Salt = hashedPassword.Salt;

                    // Set the user as a default client
                    user.SetAsClient();

                    // Get the coordinates for the user's postal code
                    await user.GetCoordinatesFromPostalCode();

                    // Insert new user in the database
                    var result = await _dbWriter.WriteAsync(new List<User>()
                    {
                        user
                    }, update: false);

                    // Response to the sign in
                    return new
                    {
                        Email = user.Email,
                        Result = result
                    };
                }
            }
            catch (Exception)
            {
                // Log
                return new
                {
                    ErrorMessage = "Unable to Create account.",
                    result = 0
                };
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Login([FromBody]User userInfo)
        {
            // Get the user from the DB
            var databaseUser = _dbReader.ReadById<User>(userInfo.Email);

            // Checks if the user is valid/exists
            var userInputPassword = await _passwordHasherService.HashPassword(userInfo.Password, HashedPassword.SaltToByteArray(databaseUser.Salt));
            if (databaseUser == null || databaseUser.Password != userInputPassword.Hash)
                return new { message = "Invalid Credentials" };

            // Generates the token
            var token = _tokenService.GenerateToken(databaseUser, out var expiryDate);

            // Return the token and other data on the response
            return new
            {
                Email = databaseUser.Email,
                Token = token,
                ExpiryDate = expiryDate
            };
        }

        // Create endpoint to change password

        // Create endpoint to get user info

        [Route("update")]
        [Authorize]
        public async Task<ActionResult<dynamic>> Update([FromBody] User userInfo)
        {
            // Get the user from the DB
            var user = _dbReader.ReadById<User>(userInfo.Email);

            // Checks if the user is valid/exists
            if (user == null || !user.HasValidData())
                return new { message = "Invalid Data." };

            // Check if new password is the same as the current one
            var hashedPassword = await _passwordHasherService.HashPassword(userInfo.Password, HashedPassword.SaltToByteArray(user.Salt));
            if (hashedPassword.Hash != user.Password)
                return Unauthorized();

            // Update user data
            userInfo.Password = hashedPassword.Hash;
            var result = await _dbWriter.WriteAsync(new List<User>
            {
                userInfo
            });

            // Return the token and other data on the response
            return new
            {
                Email = user.Email,
                Result = result
            };
        }

        [Route("delete")]
        [Authorize]
        public async Task<ActionResult<dynamic>> Delete([FromBody] User userInfo)
        {
            // Get the user from the DB
            var user = _dbReader.ReadById<User>(userInfo.Email);

            // Checks if the user is exists
            if (user == null)
                return new { message = "User not found." };

            // Check if authorized
            var hashedPassword = await _passwordHasherService.HashPassword(userInfo.Password, HashedPassword.SaltToByteArray(user.Salt));
            if (hashedPassword.Hash != user.Password)
                return Unauthorized();

            // Update user data
            var result = _dBDeleter.DeleteById<User>(user.Email);

            // Return the token and other data on the response
            return new
            {
                Email = user.Email,
                Result = result
            };
        }


        [HttpGet]
        [Route("anonymous")]
        [AllowAnonymous]
        public string Anonymous() => "Anônimo";

        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public string Authenticated() => String.Format("Autenticado - {0}", User.Identity.Name);

        [HttpGet]
        [Route("employee")]
        [Authorize(Roles = "Client,Admin")]
        public string Employee() => "Funcionário";

        [HttpGet]
        [Route("manager")]
        [Authorize(Roles = "manager")]
        public string Manager() => "Gerente";
    }
}
