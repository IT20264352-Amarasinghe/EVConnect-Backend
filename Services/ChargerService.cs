using EVConnectService.Data;
using EVConnectService.Models;
using MongoDB.Driver;

namespace EVConnectService.Services
{
    public class ChargerService
    {
        private readonly MongoDbContext _context;

        public ChargerService(MongoDbContext context)
        {
            _context = context;
        }

        public List<Charger> GetAll() =>
            _context.Chargers.Find(_ => true).ToList();

        public Charger GetByCode(string code) =>
            _context.Chargers.Find(c => c.Code == code).FirstOrDefault();

        public Charger GetById(string id) =>
        _context.Chargers.Find(s => s.Id == id).FirstOrDefault();

        public Charger Create(Charger charger)
        {
            _context.Chargers.InsertOne(charger);
            return charger;
        }

        public void Update(Charger charger) =>
            _context.Chargers.ReplaceOne(c => c.Code == charger.Code, charger);
    }
}
