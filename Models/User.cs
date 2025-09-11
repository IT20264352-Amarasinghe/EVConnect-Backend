namespace EVChonnectService.Models
{
    public class User
    {
        public string NIC { get; set; }   // Primary Key
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; } = true;
    }
}