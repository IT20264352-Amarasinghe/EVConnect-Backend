using EVConnectService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize] // requires valid JWT
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
    public IActionResult GetSlots(string chargerId, [FromQuery] DateTime? date)
    {
        List<Slot> slots;

        if (date.HasValue)
        {
            slots = _slotService.GetSlotsByChargerAndDate(chargerId, date.Value);
        }
        else
        {
            slots = _slotService.GetAllSlotsByCharger(chargerId);
        }

        // Always return 200 OK, even if empty
        return Ok(slots ?? new List<Slot>());
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