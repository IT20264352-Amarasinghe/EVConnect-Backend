using EVConnectService.Data;
using EVConnectService.Models;
using MongoDB.Driver;

namespace EVConnectService.Services
{
    // Service class for handling Charger-related operations.
    public class ChargerService
    {
        // Private field to hold an instance of the MongoDbContext.
        private readonly MongoDbContext _context;

        // Constructor for the ChargerService.
        public ChargerService(MongoDbContext context)
        {
            _context = context;
        }

        // Retrieves all chargers from the database.
        public List<Charger> GetAll() =>
            _context.Chargers.Find(_ => true).ToList();

        // Retrieves a single charger by its unique code.
        public Charger GetByCode(string code) =>
            _context.Chargers.Find(c => c.Code == code).FirstOrDefault();

        // Retrieves a single charger by its unique identifier (Id).
        public Charger GetById(string id) =>
        _context.Chargers.Find(s => s.Id == id).FirstOrDefault();

        // Creates a new charger in the database.
        public Charger Create(Charger charger)
        {
            _context.Chargers.InsertOne(charger);
            return charger;
        }

        // Updates an existing charger in the database.
        public void Update(Charger charger) =>
            _context.Chargers.ReplaceOne(c => c.Code == charger.Code, charger);
    }
}
