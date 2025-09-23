// This DTO is used to send user details to frontend.
public class UserDto
{
    public required string NIC { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required string Role { get; set; }
    public required bool IsActive { get; set; }
}