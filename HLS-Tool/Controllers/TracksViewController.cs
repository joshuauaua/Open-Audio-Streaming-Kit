using HLS_Tool.DTOs.TrackDTOs;
using HLS_Tool.Services.TrackServices;
using HLS_Tool.Services.UploadServices;
using Microsoft.AspNetCore.Mvc;

namespace HLS_Tool.Controllers
{
    public class TracksViewController : Controller
    {
        private readonly ITrackService _trackService;
        private readonly BlobService _blob;
        private readonly HlsConverterService _converter;
        private readonly ILogger<TracksViewController> _logger;

        public TracksViewController(
            ITrackService trackService,
            BlobService blob,
            HlsConverterService converter,
            ILogger<TracksViewController> logger)
        {
            _trackService = trackService;
            _blob = blob;
            _converter = converter;
            _logger = logger;
        }

        // GET: / TracksView
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
            if (dto.File == null || dto.File.Length == 0)
            {
                ModelState.AddModelError("", "Please select a file to upload.");
                return View(dto);
            }

            var file = dto.File;
            var trackFolder = Path.GetFileNameWithoutExtension(file.FileName);

            try
            {
                // 1️⃣ Upload MP3 to Blob
                await using var ms = file.OpenReadStream();
                var mp3Url = await _blob.UploadFileAsync(ms, $"{trackFolder}/{file.FileName}", file.ContentType ?? "audio/mpeg");

                // 2️⃣ Download locally for conversion
                var localPath = Path.Combine(Path.GetTempPath(), file.FileName);
                await _blob.DownloadFileAsync($"{trackFolder}/{file.FileName}", localPath);

                // 3️⃣ Convert to HLS
                var hlsDir = Path.Combine(Path.GetTempPath(), trackFolder);
                Directory.CreateDirectory(hlsDir);
                await _converter.ConvertToHlsAsync(localPath, hlsDir);

                // 4️⃣ Upload HLS segments
                var hlsUrls = await _blob.UploadDirectoryAsync(hlsDir, trackFolder);

                // 5️⃣ Cleanup local files
                Directory.Delete(hlsDir, true);
                System.IO.File.Delete(localPath);

                // 6️⃣ Set TrackURL for DB
                dto.TrackURL = hlsUrls.FirstOrDefault(x => x.EndsWith(".m3u8"));
                dto.Title = dto.Title ?? trackFolder;

                // 7️⃣ Insert into DB
                var createdTrack = await _trackService.CreateTrackAsync(dto);
                if (createdTrack == null)
                {
                    ModelState.AddModelError("", "Failed to create track in the database.");
                    return View(dto);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Track upload failed");
                ModelState.AddModelError("", $"Error uploading track: {ex.Message}");
                return View(dto);
            }
        }
    }
}