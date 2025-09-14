using Microsoft.AspNetCore.Mvc;
using EVConnectService.Models;
using EVConnectService.Services;
using EVConnectService.Models.Dtos;

namespace EVConnectService.Controllers
{
    // Indicates that this class is an API controller.
    [ApiController]
    // Defines the base route for all endpoints in this controller.
    [Route("api/users")]
    public class UsersController : ControllerBase
    {

        private readonly UserService _userService;

        private readonly TokenService _tokenService;


        public UsersController(UserService userService, TokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        // This method handles HTTP POST requests to the "api/users/register" endpoint.
        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            // Checks if a user with the same NIC already exists
            if (_userService.GetByNIC(newUser.NIC) != null)
            {
                var error = new ErrorResponse
                {
                    Message = "User with this NIC already exists.",
                    Code = "USER_EXISTS"
                };
                return BadRequest(error);
            }

            newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
            var createdUser = _userService.Create(newUser);
            // Returns a 200 OK status along with the newly created user object.
            return Ok(createdUser);
        }

        // This method handles HTTP POST requests to the "api/users/login" endpoint.
        [HttpPost("login")]
        // The [FromBody] attribute tells the framework to deserialize the request body into a User object.
        public IActionResult Login([FromBody] RegisterRequest request)
        {
            // Finds the first user in the list that matches both the provided NIC and Password.
            // FirstOrDefault returns null if no match is found.
            var user = _userService.GetByNIC(request.NIC);

            // If the user object is null, it means no match was found.
            if (user == null)
            {
                var error = new ErrorResponse
                {
                    Message = "Invalid Username",
                    Code = "Invalid_Credentials"
                };
                // Return a 401 Unauthorized status with a message.
                return Unauthorized(error);
            }

            // Verify the password using BCrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            if (!isPasswordValid)
            {
                var error = new ErrorResponse
                {
                    Message = "Invalid Password",
                    Code = "Invalid_Credentials"
                };
                // Return a 401 Unauthorized status with a message.
                return Unauthorized(error);
            }

            // Generate JWT token
            var tokenResult = _tokenService.GenerateToken(user);

            // Return token + user info
            return Ok(tokenResult);
        }

        // This method handles HTTP PUT requests
        // The "{nic}" is a route parameter that captures the NIC from the URL.
        [HttpPut("deactivate/{nic}")]
        public IActionResult Deactivate(string nic)
        {
            // Finds the user with the matching NIC.
            var user = _userService.GetByNIC(nic);

            // If no user is found, return a 404 Not Found status.
            if (user == null) return NotFound();

            // Changes the IsActive property of the found user to false.
            _userService.Deactivate(nic);

            // Return a 200 OK status with the updated user object.
            return Ok($"User {nic} deactivated.");
        }
    }
}
