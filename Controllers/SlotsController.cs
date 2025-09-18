using EVConnectService.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/slots")]
public class SlotsController : BaseController
{
    private readonly SlotService _slotService;

    public SlotsController(SlotService slotService)
    {
        _slotService = slotService;
    }

    // GET api/slots/{chargerId}?date=2025-09-18
    [HttpGet("{chargerId}")]
    public IActionResult GetSlots(string chargerId, [FromQuery] DateTime date)
    {
        var slots = _slotService.GetSlotsByChargerAndDate(chargerId, date);

        if (slots == null || !slots.Any())
            return NotFoundError("No slots found for this charger and date.");

        return Ok(slots);
    }

    // POST api/slots
    [HttpPost]
    public IActionResult CreateSlot([FromBody] Slot slot)
    {
        _slotService.Create(slot);
        return Ok(slot);
    }

    // POST: api/slots/batch
    [HttpPost("batch")]
    public IActionResult CreateSlots([FromBody] List<Slot> slots)
    {
        foreach (var slot in slots)
        {
            _slotService.Create(slot);
        }
        return Ok(slots);
    }
}