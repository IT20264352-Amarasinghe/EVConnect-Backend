using Microsoft.AspNetCore.Mvc;
using EVConnectService.Models;
using EVConnectService.Services;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace EVConnectService.Controllers
{
    // Indicates that this class is an API controller.
    [ApiController]
    // Defines the base route for all endpoints in this controller.
    [Route("api/users")]
    public class UsersController : ControllerBase
    {

        private readonly UserService _userService;

        private readonly IConfiguration _configuration;

        public UsersController(IConfiguration configuration, UserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        // This method handles HTTP POST requests to the "api/users/register" endpoint.
        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            // Checks if a user with the same NIC already exists
            if (_userService.GetByNIC(newUser.NIC) != null)
                // If a duplicate is found, return a 400 Bad Request with a message.
                return BadRequest("User with this NIC already exists.");

            newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
            var createdUser = _userService.Create(newUser);
            // Returns a 200 OK status along with the newly created user object.
            return Ok(createdUser);
        }

        // This method handles HTTP POST requests to the "api/users/login" endpoint.
        [HttpPost("login")]
        // The [FromBody] attribute tells the framework to deserialize the request body into a User object.
        public IActionResult Login([FromBody] User loginUser)
        {
            // Finds the first user in the list that matches both the provided NIC and Password.
            // FirstOrDefault returns null if no match is found.
            var user = _userService.GetByNIC(loginUser.NIC);

            // If the user object is null, it means no match was found.
            if (user == null)
                // Return a 401 Unauthorized status with a message.
                return Unauthorized("Invalid credentials");

            // Generate JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
              {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim("NIC", user.NIC),
            new Claim(ClaimTypes.Email, user.Email)
        }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                Token = tokenHandler.WriteToken(token),
                Expiration = token.ValidTo,
                User = new { user.NIC, user.Name, user.Email }
            });
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
