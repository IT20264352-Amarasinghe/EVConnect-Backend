namespace EVConnectService.Models.Dtos
{
    public class RegisterRequest
    {
        public required string NIC { get; set; }
        public required string Password { get; set; }
    }
}