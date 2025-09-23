using Microsoft.AspNetCore.Mvc;
using EVConnectService.Services;
using Microsoft.AspNetCore.Authorization;

namespace EVConnectService.Controllers
{
    // Indicates that this class is an API controller.
    [ApiController]
    [Authorize] // requires valid JWT
    // Defines the base route for all endpoints in this controller.
    [Route("api/users")]
    public class UsersController : BaseController
    {

        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
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

        [HttpGet("customers")]
        public IActionResult GetCustomers()
        {
            var customers = _userService.GetCustomers();

            // Map to DTO (exclude password)
            var customerDtos = customers.Select(u => new UserDto
            {
                NIC = u.NIC,
                Name = u.Name,
                Email = u.Email,
                Phone = u.Phone,
                Role = u.Role,
                IsActive = u.IsActive
            }).ToList();

            return Ok(customerDtos);
        }
    }
}
