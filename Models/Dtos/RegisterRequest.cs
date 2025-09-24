namespace EVConnectService.Models.Dtos
{
    // this is the register request structure
    public class RegisterRequest
    {
        public required string NIC { get; set; }
        public required string Password { get; set; }
    }
}