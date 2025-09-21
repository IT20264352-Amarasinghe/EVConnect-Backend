using EVConnectService.Data;
using EVConnectService.Models;
using MongoDB.Driver;

namespace EVConnectService.Services
{
    public class BookingService
    {
        private readonly MongoDbContext _context;

        public BookingService(MongoDbContext context)
        {
            _context = context;
        }

        public List<Booking> GetAll() =>
            _context.Bookings.Find(_ => true).ToList();

        public List<Booking> GetByCustomerNic(string customerNic) =>
_context.Bookings.Find(b => b.CustomerNic == customerNic).ToList();

        public Booking GetById(string id) =>
            _context.Bookings.Find(b => b.Id == id).FirstOrDefault();

        public Booking Create(Booking booking)
        {
            _context.Bookings.InsertOne(booking);
            return booking;
        }

        public void Update(Booking booking) =>
            _context.Bookings.ReplaceOne(b => b.Id == booking.Id, booking);
    }
}
