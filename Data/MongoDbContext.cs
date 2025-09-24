using Microsoft.Extensions.Options;
using MongoDB.Driver;
using EVConnectService.Models;

// This manages the database connection and provides access to the various collections.
namespace EVConnectService.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration config)
        {
            // Retrieve MongoDB connection string and database name
            var connectionString = config["MongoDB:ConnectionString"];
            var databaseName = config["MongoDB:DatabaseName"];
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
            // Ensure unique index on Charger.Code
            var chargerIndexKeys = Builders<Charger>.IndexKeys.Ascending(c => c.Code);
            var chargerIndexOptions = new CreateIndexOptions { Unique = true };
            var chargerIndexModel = new CreateIndexModel<Charger>(chargerIndexKeys, chargerIndexOptions);
            Chargers.Indexes.CreateOne(chargerIndexModel);
        }

        // Public property to get the 'Users' collection.
        public IMongoCollection<User> Users =>
            _database.GetCollection<User>("Users");

        // Public property to get the 'Chargers' collection.
        public IMongoCollection<Charger> Chargers =>
         _database.GetCollection<Charger>("Chargers");

        // Public property to get the 'Bookings' collection.
        public IMongoCollection<Booking> Bookings =>
            _database.GetCollection<Booking>("Bookings");

        // Public property to get the 'Slots' collection.
        public IMongoCollection<Slot> Slots => _database.GetCollection<Slot>("Slots");
    }
}
