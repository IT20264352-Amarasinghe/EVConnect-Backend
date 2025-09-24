using EVConnectService.Data;
using EVConnectService.Models;
using MongoDB.Driver;

namespace EVConnectService.Services
{
    // Service class for managing charging slots.
    public class SlotService
    {

        // A read-only field to store the MongoDB context.
        private readonly MongoDbContext _context;

        // The MongoDB context is injected into this service.
        public SlotService(MongoDbContext context)
        {
            _context = context;
        }

        // Retrieves all charging slots for a specific charger, sorted by date and then start time.
        public List<Slot> GetAllSlotsByCharger(string chargerId) =>
    _context.Slots
        .Find(s => s.ChargerId == chargerId)
        .SortBy(s => s.Date)
        .ThenBy(s => s.StartTime)
        .ToList();

        // Retrieves all charging slots for a specific charger on a given date.
        public List<Slot> GetSlotsByChargerAndDate(string chargerId, DateTime date) =>
            _context.Slots
                .Find(s => s.ChargerId == chargerId && s.Date == date.Date)
                .SortBy(s => s.StartTime)
                .ToList();

        // Creates a new charging slot in the database.
        public Slot Create(Slot slot)
        {
            _context.Slots.InsertOne(slot);
            return slot;
        }

        // Retrieves a single charging slot by its unique identifier
        public Slot GetById(string id) =>
            _context.Slots.Find(s => s.Id == id).FirstOrDefault();

        // Updates an existing charging slot.
        public void Update(Slot slot) =>
            _context.Slots.ReplaceOne(s => s.Id == slot.Id, slot);

        // Deletes a charging slot from the database by its unique identifier.
        public void Delete(string id) =>
            _context.Slots.DeleteOne(s => s.Id == id);
    }
}