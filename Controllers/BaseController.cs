using Microsoft.AspNetCore.Mvc;

public abstract class BaseController : ControllerBase
{
    // 404 Not Found
    protected IActionResult NotFoundError(string message)
    {
        return NotFound(new ErrorResponse { Message = message });
    }

    // 400 Bad Request
    protected IActionResult BadRequestError(string message)
    {
        return BadRequest(new ErrorResponse { Message = message });
    }

    // 401 Unauthorized
    protected IActionResult UnauthorizedError(string message)
    {
        return Unauthorized(new ErrorResponse { Message = message });
    }

    // 500 Internal Server Error (optional)
    protected IActionResult ServerError(string message)
    {
        return StatusCode(500, new ErrorResponse { Message = message });
    }
}
