using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Monitor_App.Web.Data;
using Monitor_App.Web.Models;

namespace Monitor_App.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LocationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostLocation([FromBody] LocationData locationData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Locations.Add(locationData);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLocation", new { id = locationData.Id }, locationData);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocationData>>> GetLocations()
        {
            return await _context.Locations.ToListAsync();
        }

        [HttpGet("{deviceId}")]
        public async Task<ActionResult<IEnumerable<LocationData>>> GetDeviceLocations(string deviceId)
        {
            return await _context.Locations
                .Where(l => l.DeviceId == deviceId)
                .OrderByDescending(l => l.Timestamp)
                .ToListAsync();
        }
    }
}
