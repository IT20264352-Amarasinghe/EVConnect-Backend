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

        // GET: api/chargers/id/{id}
        [HttpGet("id/{id}")]
        public IActionResult GetChargerById(string id)
        {
            var charger = _chargerService.GetById(id);
            if (charger == null) return NotFound("Charger not found");
            return Ok(charger);
        }

        // GET: api/chargers/{code}
        [HttpGet("{code}")]
        public IActionResult GetChargerByCode(string code)
        {
            var charger = _chargerService.GetByCode(code);
            if (charger == null) return NotFound("Charger not found");
            return Ok(charger);
        }

        // POST: api/chargers
        [HttpPost]
        public IActionResult CreateCharger([FromBody] Charger charger)
        {
            _chargerService.Create(charger);
            return Ok(charger);
        }

        // POST: api/chargers/batch
        [HttpPost("batch")]
        public IActionResult CreateChargers([FromBody] List<Charger> chargers)
        {
            foreach (var charger in chargers)
            {
                _chargerService.Create(charger);
            }
            return Ok(chargers);
        }
    }
}
