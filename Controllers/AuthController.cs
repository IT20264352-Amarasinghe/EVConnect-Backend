using Microsoft.AspNetCore.Mvc;
using EVConnectService.Models;
using EVConnectService.Services;
using EVConnectService.Models.Dtos;

namespace EVConnectService.Controllers
{
    // Indicates that this class is an API controller.
    [ApiController]
    // Defines the base route for all endpoints in this controller.
    [Route("api/auth")]
    public class AuthController : BaseController
    {

        private readonly UserService _userService;

        private readonly TokenService _tokenService;


        public AuthController(UserService userService, TokenService tokenService)
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
                return BadRequestError("User with this NIC already exists.");


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
                return UnauthorizedError("Invalid Username");

            // Verify the password using BCrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            if (!isPasswordValid)
                return UnauthorizedError("Invalid Password");

            // Generate JWT token
            var tokenResult = _tokenService.GenerateToken(user);

            // Return token + user info
            return Ok(tokenResult);
        }

        // This method handles HTTP POST requests to the "api/users/login" endpoint.
        [HttpPost("admin/login")]
        // The [FromBody] attribute tells the framework to deserialize the request body into a User object.
        public IActionResult AdminLogin([FromBody] RegisterRequestAdmin request)
        {
            // Finds the first user in the list that matches both the provided NIC and Password.
            // FirstOrDefault returns null if no match is found.
            var user = _userService.GetByEMAIL(request.Email);

            // If the user object is null, it means no match was found.
            if (user == null)
                return UnauthorizedError("Invalid Username");
            if (user.Role != "admin")
                return UnauthorizedError("Invaild Role");

            // Verify the password using BCrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            if (!isPasswordValid)
                return UnauthorizedError("Invalid Password");

            // Generate JWT token
            var tokenResult = _tokenService.GenerateToken(user);

            // Return token + user info
            return Ok(tokenResult);
        }


    }
}
