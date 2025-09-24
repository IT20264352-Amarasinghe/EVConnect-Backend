using EVConnectService.Data;
using EVConnectService.Models;
using MongoDB.Driver;

namespace EVConnectService.Services
{
    // Service class for handling booking-related operations.
    public class BookingService
    {
        private readonly MongoDbContext _context;

        // Constructor for the BookingService.
        public BookingService(MongoDbContext context)
        {
            _context = context;
        }

        // Retrieves all bookings from the database.
        public List<Booking> GetAll() =>
            _context.Bookings.Find(_ => true).ToList();

        // Retrieves all bookings made by a specific customer.
        public List<Booking> GetByCustomerNic(string customerNic) =>
_context.Bookings.Find(b => b.CustomerNic == customerNic).ToList();

        //Retrieves a single booking by its unique identifier.
        public Booking GetById(string id) =>
            _context.Bookings.Find(b => b.Id == id).FirstOrDefault();

        // Creates a new booking in the database.
        public Booking Create(Booking booking)
        {
            _context.Bookings.InsertOne(booking);
            return booking;
        }

        // Updates an existing booking in the database.
        public void Update(Booking booking) =>
            _context.Bookings.ReplaceOne(b => b.Id == booking.Id, booking);
    }
}
