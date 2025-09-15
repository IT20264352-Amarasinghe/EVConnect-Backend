using Microsoft.Extensions.Options;
using MongoDB.Driver;
using EVConnectService.Models;

namespace EVConnectService.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration config)
        {
            var connectionString = config["MongoDB:ConnectionString"];
            var databaseName = config["MongoDB:DatabaseName"];
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<User> Users =>
            _database.GetCollection<User>("Users");

        public IMongoCollection<Charger> Chargers =>
         _database.GetCollection<Charger>("Chargers");

        public IMongoCollection<Booking> Bookings =>
            _database.GetCollection<Booking>("Bookings");
    }
}
