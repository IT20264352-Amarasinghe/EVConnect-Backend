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
            // Ensure unique index on Charger.Code
            var chargerIndexKeys = Builders<Charger>.IndexKeys.Ascending(c => c.Code);
            var chargerIndexOptions = new CreateIndexOptions { Unique = true };
            var chargerIndexModel = new CreateIndexModel<Charger>(chargerIndexKeys, chargerIndexOptions);
            Chargers.Indexes.CreateOne(chargerIndexModel);
        }

        public IMongoCollection<User> Users =>
            _database.GetCollection<User>("Users");

        public IMongoCollection<Charger> Chargers =>
         _database.GetCollection<Charger>("Chargers");

        public IMongoCollection<Booking> Bookings =>
            _database.GetCollection<Booking>("Bookings");

        public IMongoCollection<Slot> Slots => _database.GetCollection<Slot>("Slots");
    }
}
