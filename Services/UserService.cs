using EVConnectService.Data;
using EVConnectService.Models;
using MongoDB.Driver;

namespace EVConnectService.Services
{
    public class UserService
    {
        private readonly MongoDbContext _context;

        public UserService(MongoDbContext context)
        {
            _context = context;
        }

        public List<User> GetAll() =>
            _context.Users.Find(_ => true).ToList();

        public User GetByNIC(string nic) =>
            _context.Users.Find(u => u.NIC == nic).FirstOrDefault();

        public User Create(User user)
        {
            _context.Users.InsertOne(user);
            return user;
        }

        public void Deactivate(string nic)
        {
            var user = GetByNIC(nic);
            if (user != null)
            {
                user.IsActive = false;
                _context.Users.ReplaceOne(u => u.NIC == nic, user);
            }
        }

        public User Login(string nic, string password) =>
            _context.Users.Find(u => u.NIC == nic && u.Password == password).FirstOrDefault();
    }
}
