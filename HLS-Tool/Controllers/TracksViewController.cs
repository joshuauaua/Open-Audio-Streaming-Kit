using HLS_Tool.DTOs.TrackDTOs;
using HLS_Tool.Services.TrackServices;
using HLS_Tool.DTOs.TrackDTOs;
using HLS_Tool.Services.TrackServices;
using HLS_Tool.DTOs.TrackDTOs;
using HLS_Tool.Services.TrackServices;
using Microsoft.AspNetCore.Mvc;

namespace HLS_Tool.Controllers
{
    public class TracksViewController : Controller
    {
        private readonly ITrackService _trackService;

        public TracksViewController(ITrackService trackService)
        {
            _trackService = trackService;
        }

        // GET: /TracksView
        public async Task<IActionResult> Index()
        {
            var tracks = await _trackService.GetAllTracksAsync();
            return View(tracks);
        }

        // GET: /TracksView/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /TracksView/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTrackDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var created = await _trackService.CreateTrackAsync(dto);

            if (created == null)
            {
                ModelState.AddModelError("", "Track creation failed");
                return View(dto);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}