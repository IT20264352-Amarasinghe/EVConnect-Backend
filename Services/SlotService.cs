using EVConnectService.Data;
using EVConnectService.Models;
using MongoDB.Driver;

namespace EVConnectService.Services
{
    public class SlotService
    {
        private readonly MongoDbContext _context;

        public SlotService(MongoDbContext context)
        {
            _context = context;
        }

        public List<Slot> GetAllSlotsByCharger(string chargerId) =>
    _context.Slots
        .Find(s => s.ChargerId == chargerId)
        .SortBy(s => s.Date)
        .ThenBy(s => s.StartTime)
        .ToList();

        public List<Slot> GetSlotsByChargerAndDate(string chargerId, DateTime date) =>
            _context.Slots
                .Find(s => s.ChargerId == chargerId && s.Date == date.Date)
                .SortBy(s => s.StartTime)
                .ToList();

        public Slot Create(Slot slot)
        {
            _context.Slots.InsertOne(slot);
            return slot;
        }

        public Slot GetById(string id) =>
            _context.Slots.Find(s => s.Id == id).FirstOrDefault();

        public void Update(Slot slot) =>
            _context.Slots.ReplaceOne(s => s.Id == slot.Id, slot);

        public void Delete(string id) =>
            _context.Slots.DeleteOne(s => s.Id == id);
    }
}