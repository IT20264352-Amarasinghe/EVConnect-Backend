using Microsoft.AspNetCore.Mvc;
using EVConnectService.Models;

namespace EVConnectService.Controllers
{
    // Indicates that this class is an API controller.
    [ApiController]
    // Defines the base route for all endpoints in this controller.
    [Route("api/users")]
    public class UsersController : ControllerBase
    {

        // A static in-memory list to simulate a database. This is not for production.
        private static List<User> users = new List<User>();

        // This method handles HTTP POST requests to the "api/users/register" endpoint.
        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            // Checks if a user with the same NIC already exists
            if (users.Any(u => u.NIC == user.NIC))
                // If a duplicate is found, return a 400 Bad Request with a message.
                return BadRequest("User with this NIC already exists.");

            // Adds the new user object to our in-memory list.
            users.Add(user);
            // Returns a 200 OK status along with the newly created user object.
            return Ok(user);
        }

        // This method handles HTTP POST requests to the "api/users/login" endpoint.
        [HttpPost("login")]
        // The [FromBody] attribute tells the framework to deserialize the request body into a User object.
        public IActionResult Login([FromBody] User loginUser)
        {
            // Finds the first user in the list that matches both the provided NIC and Password.
            // FirstOrDefault returns null if no match is found.
            var user = users.FirstOrDefault(u => u.NIC == loginUser.NIC && u.Password == loginUser.Password);
            
            // If the user object is null, it means no match was found.
            if (user == null)
                // Return a 401 Unauthorized status with a message.
                return Unauthorized("Invalid credentials");

            // If a user is found, return a 200 OK status with the user object.
            return Ok(user);
        }

        // This method handles HTTP PUT requests
        // The "{nic}" is a route parameter that captures the NIC from the URL.
        [HttpPut("deactivate/{nic}")]
        public IActionResult Deactivate(string nic)
        {
            // Finds the user with the matching NIC.
            var user = users.FirstOrDefault(u => u.NIC == nic);
            
            // If no user is found, return a 404 Not Found status.
            if (user == null) return NotFound();

            // Changes the IsActive property of the found user to false.
            user.IsActive = false;
            
            // Return a 200 OK status with the updated user object.
            return Ok(user);
        }
    }
}
