using EVConnectService.Models;
using EVConnectService.Services;
using Microsoft.AspNetCore.Mvc;

namespace EVConnectService.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingController : BaseController
    {
        private readonly BookingService _bookingService;
        private readonly ChargerService _chargerService;
        private readonly SlotService _slotService;

        public BookingController(BookingService bookingService, ChargerService chargerService, SlotService slotService)
        {
            _bookingService = bookingService;
            _chargerService = chargerService;
            _slotService = slotService;
        }

        // POST: api/bookings
        [HttpPost]
        public IActionResult CreateBooking([FromBody] Booking request)
        {
            var charger = _chargerService.GetById(request.ChargerId);
            if (charger == null)
                return NotFoundError("Charger not found.");

            var slot = _slotService.GetById(request.SlotId);
            if (slot == null || slot.ChargerId != request.ChargerId) // make sure slot belongs to charger
                return NotFoundError("Slot not found.");

            if (slot.Status != "Available")
                return BadRequestError("Slot not available."); // should be 400, not 404

            slot.Status = "Booked";
            _slotService.Update(slot);

            var booking = new Booking
            {
                CustomerNic = request.CustomerNic,
                ChargerId = request.ChargerId,
                SlotId = request.SlotId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Status = "Active"
            };

            Booking createdBooking = _bookingService.Create(booking);
            var bookingDto = new BookingDto
            {
                CustomerNic = createdBooking.CustomerNic,
                Charger = _chargerService.GetById(createdBooking.ChargerId),
                Slot = _slotService.GetById(createdBooking.SlotId),
                CreatedAt = createdBooking.CreatedAt,
                UpdatedAt = createdBooking.UpdatedAt,
                Status = createdBooking.Status
            };
            return Ok(bookingDto);
        }

        // GET api/bookings?customerNic=123456789V
        [HttpGet]
        public IActionResult GetBookings([FromQuery] string? customerNic)
        {
            List<Booking> bookings;

            if (!string.IsNullOrEmpty(customerNic))
            {
                bookings = _bookingService.GetByCustomerNic(customerNic);
            }
            else
            {
                bookings = _bookingService.GetAll();
            }

            // Map to DTOs with charger & slot details
            var bookingDtos = bookings.Select(b => new BookingDto
            {
                Id = b.Id,
                CustomerNic = b.CustomerNic,
                Charger = _chargerService.GetById(b.ChargerId),
                Slot = _slotService.GetById(b.SlotId),
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt,
                Status = b.Status
            }).ToList();

            return Ok(bookingDtos);
        }

        // PUT: api/bookings/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateBooking(string id, [FromBody] Booking updateRequest)
        {
            var booking = _bookingService.GetById(id);
            if (booking == null) return NotFoundError("Booking not found");

            // Get old slot
            var oldSlot = _slotService.GetById(booking.SlotId);
            if (oldSlot == null) return NotFoundError("Slot not found");

            // Check update time restriction
            // Combine Date + StartTime into a DateTime
            var slotStartDateTime = oldSlot.Date.Date + oldSlot.StartTime;
            if ((slotStartDateTime - DateTime.UtcNow).TotalHours < 12)
                return BadRequestError("Cannot update less than 12h before start");

            // Release old slot
            oldSlot.Status = "Available";
            _slotService.Update(oldSlot);

            // Validate new slot
            var newSlot = _slotService.GetById(updateRequest.SlotId);
            if (newSlot == null || newSlot.Status != "Available")
                return BadRequestError("New slot not available");

            // Book new slot
            newSlot.Status = "Booked";
            _slotService.Update(newSlot);

            // Update booking
            booking.SlotId = updateRequest.SlotId;
            booking.UpdatedAt = DateTime.UtcNow;
            booking.Status = "Updated";
            _bookingService.Update(booking);

            return Ok(booking);
        }


        // DELETE: api/bookings/{id}
        [HttpDelete("{id}")]
        public IActionResult CancelBooking(string id)
        {
            var booking = _bookingService.GetById(id);
            if (booking == null) return NotFoundError("Booking not found");

            // Get the slot directly from SlotService
            var slot = _slotService.GetById(booking.SlotId);
            if (slot == null) return NotFoundError("Slot not found");

            // Combine Date + StartTime
            var slotStartDateTime = slot.Date.Date + slot.StartTime;

            // Restrict cancellation within 12 hours
            if ((slotStartDateTime - DateTime.UtcNow).TotalHours < 12)
                return BadRequestError("Cannot cancel less than 12h before start");

            // Release the slot
            slot.Status = "Available";
            _slotService.Update(slot);

            // Update booking status
            booking.Status = "Cancelled";
            booking.UpdatedAt = DateTime.UtcNow;
            _bookingService.Update(booking);

            return Ok("Booking cancelled successfully");
        }
    }
}
