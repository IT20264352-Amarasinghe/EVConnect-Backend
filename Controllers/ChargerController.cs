using EVConnectService.Models;
using EVConnectService.Services;
using Microsoft.AspNetCore.Mvc;

namespace EVConnectService.Controllers
{
    [ApiController]
    [Route("api/chargers")]
    public class ChargerController : ControllerBase
    {
        private readonly ChargerService _chargerService;

        public ChargerController(ChargerService chargerService)
        {
            _chargerService = chargerService;
        }

        // GET: api/chargers
        [HttpGet]
        public IActionResult GetAllChargers()
        {
            return Ok(_chargerService.GetAll());
        }

        // GET: api/chargers/{code}
        [HttpGet("{code}")]
        public IActionResult GetChargerByCode(string code)
        {
            var charger = _chargerService.GetByCode(code);
            if (charger == null) return NotFound("Charger not found");
            return Ok(charger);
        }

        // GET: api/chargers/{code}/slots
        [HttpGet("{code}/slots")]
        public IActionResult GetSlotsByCharger(string code)
        {
            var charger = _chargerService.GetByCode(code);
            if (charger == null) return NotFound("Charger not found");
            return Ok(charger.Slots);
        }

        // POST: api/chargers
        [HttpPost]
        public IActionResult CreateCharger([FromBody] Charger charger)
        {
            _chargerService.Create(charger);
            return Ok(charger);
        }
    }
}
