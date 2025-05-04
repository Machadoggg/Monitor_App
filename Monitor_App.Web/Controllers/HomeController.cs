using Microsoft.AspNetCore.Mvc;
using Monitor_App.Web.Models;
using System.Diagnostics;

namespace Monitor_App.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;

        public HomeController(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://tumvcapi.com/api/"); // Cambiar por tu URL base
        }

        public async Task<IActionResult> Index()
        {
            var devices = await _context.Locations
                .GroupBy(l => l.DeviceId)
                .Select(g => g.Key)
                .ToListAsync();

            return View(devices);
        }

        public async Task<IActionResult> DeviceLocations(string deviceId)
        {
            var locations = await _context.Locations
                .Where(l => l.DeviceId == deviceId)
                .OrderByDescending(l => l.Timestamp)
                .Take(50)
                .ToListAsync();

            ViewBag.DeviceId = deviceId;
            return View(locations);
        }

        public async Task<IActionResult> MapView(string deviceId)
        {
            var locations = await _context.Locations
                .Where(l => l.DeviceId == deviceId)
                .OrderByDescending(l => l.Timestamp)
                .Take(20)
                .ToListAsync();

            ViewBag.DeviceId = deviceId;
            return View(locations);
        }
    }
}
