namespace EVConnectService.Models.Dtos
{
    // this is used get the login request from admin
    public class LoginRequestAdmin
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}