using EVConnectService.Models;
using EVConnectService.Services;
using Microsoft.AspNetCore.Mvc;

namespace EVConnectService.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingController : ControllerBase
    {
        private readonly BookingService _bookingService;
        private readonly ChargerService _chargerService;

        public BookingController(BookingService bookingService, ChargerService chargerService)
        {
            _bookingService = bookingService;
            _chargerService = chargerService;
        }

        // POST: api/bookings
        [HttpPost]
        public IActionResult CreateBooking([FromBody] Booking request)
        {
            var charger = _chargerService.GetByCode(request.ChargerCode);
            if (charger == null) return NotFound("Charger not found");

            var slot = charger.Slots.FirstOrDefault(s => s.Id == request.SlotId);
            if (slot == null) return NotFound("Slot not found");
            if (slot.Status != "Available") return BadRequest("Slot not available");

            slot.Status = "Booked";
            _chargerService.Update(charger);

            var booking = new Booking
            {
                CustomerNic = request.CustomerNic,
                ChargerCode = request.ChargerCode,
                SlotId = request.SlotId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Status = "Active"
            };

            _bookingService.Create(booking);
            return Ok(booking);
        }

        // PUT: api/bookings/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateBooking(string id, [FromBody] Booking updateRequest)
        {
            var booking = _bookingService.GetById(id);
            if (booking == null) return NotFound("Booking not found");

            var charger = _chargerService.GetByCode(booking.ChargerCode);
            var slot = charger.Slots.FirstOrDefault(s => s.Id == booking.SlotId);

            if (slot == null) return NotFound("Slot not found");

            if ((slot.StartTime - DateTime.UtcNow).TotalHours < 12)
                return BadRequest("Cannot update less than 12h before start");

            slot.Status = "Available";

            var newSlot = charger.Slots.FirstOrDefault(s => s.Id == updateRequest.SlotId);
            if (newSlot == null || newSlot.Status != "Available")
                return BadRequest("New slot not available");

            newSlot.Status = "Booked";
            booking.SlotId = updateRequest.SlotId;
            booking.UpdatedAt = DateTime.UtcNow;
            booking.Status = "Updated";

            _bookingService.Update(booking);
            _chargerService.Update(charger);

            return Ok(booking);
        }

        // DELETE: api/bookings/{id}
        [HttpDelete("{id}")]
        public IActionResult CancelBooking(string id)
        {
            var booking = _bookingService.GetById(id);
            if (booking == null) return NotFound("Booking not found");

            var charger = _chargerService.GetByCode(booking.ChargerCode);
            var slot = charger.Slots.FirstOrDefault(s => s.Id == booking.SlotId);

            if (slot == null) return NotFound("Slot not found");

            if ((slot.StartTime - DateTime.UtcNow).TotalHours < 12)
                return BadRequest("Cannot cancel less than 12h before start");

            slot.Status = "Available";
            booking.Status = "Cancelled";
            booking.UpdatedAt = DateTime.UtcNow;

            _bookingService.Update(booking);
            _chargerService.Update(charger);

            return Ok("Booking cancelled successfully");
        }
    }
}
