using EVConnectService.Data;
using EVConnectService.Models;
using MongoDB.Driver;

namespace EVConnectService.Services
{
    // Service class for handling User-related operations.
    public class UserService
    {
        // Private field to hold an instance of the MongoDbContext
        private readonly MongoDbContext _context;

        // Constructor for the UserService.
        public UserService(MongoDbContext context)
        {
            _context = context;
        }

        // Retrieves all users from the database.
        public List<User> GetAll() =>
            _context.Users.Find(_ => true).ToList();

        // Retrieves a single user by their National Identity Card (NIC).
        public User GetByNIC(string nic) =>
            _context.Users.Find(u => u.NIC == nic).FirstOrDefault();

        // Retrieves a single user by their email address.
        public User GetByEMAIL(string email) =>
      _context.Users.Find(u => u.Email == email).FirstOrDefault();

        // Creates a new user in the database.
        public User Create(User user)
        {
            _context.Users.InsertOne(user);
            return user;
        }

        // Deactivates a user by setting their IsActive status to false.
        public void Deactivate(string nic)
        {
            var user = GetByNIC(nic);
            if (user != null)
            {
                user.IsActive = false;
                _context.Users.ReplaceOne(u => u.NIC == nic, user);
            }
        }

        // Retrieves a list of all users with the "customer" role.
        public List<User> GetCustomers() =>
    _context.Users.Find(u => u.Role.ToLower() == "customer").ToList();

    }
}
