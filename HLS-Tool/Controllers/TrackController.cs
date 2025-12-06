using HLS_Tool.DTOs.TrackDTOs;
using HLS_Tool.Services.UploadServices;
using HLS_Tool.DTOs.TrackDTOs;
using HLS_Tool.Services.TrackServices;
using HLS_Tool.Services.UploadServices;
using Microsoft.AspNetCore.Mvc;

namespace HLS_Tool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TracksController : ControllerBase
    {
        
        //DI container of services (Blob upload/download and HLS Converter)
        private readonly BlobService _blob;
        private readonly HlsConverterService _converter;
        private readonly ILogger<TracksController> _logger;
        
        private readonly ITrackService _trackService;
        public TracksController(ITrackService trackService, BlobService blob, HlsConverterService converter, ILogger<TracksController> logger)
        {
            _trackService = trackService;
            _blob = blob;
            _converter = converter;
            _logger = logger;
            
        }
        
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrackDTO>>> GetAllTracks()
        {
            var tracks = await _trackService.GetAllTracksAsync();
            return Ok(tracks);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TrackDTO>> GetTrackById(int id)
        {
            var track = await _trackService.GetTrackByIdAsync(id);
            if (track == null)
            {
                return NotFound();
            }
            return Ok(track);
        }
        
        
         [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    
    
    public async Task<IActionResult> UploadTrack([FromForm] CreateTrackDTO dto)
    { 
        var file = dto.File;
        
        //Checks if upload exists
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded or file already exists" +
                              "");

        //Creates a trackfolder based on the name of the file
        var trackFolder = Path.GetFileNameWithoutExtension(file.FileName);

        try
        {
            //Upload MP3 to blob under its own subfolder
            await using var ms = file.OpenReadStream();
            var mp3Url = await _blob.UploadFileAsync(ms, $"{trackFolder}/{file.FileName}", file.ContentType ?? "audio/mpeg");
            _logger.LogInformation("Uploaded MP3: {Url}", mp3Url);

            // Download locally for conversion
            var localPath = Path.Combine(Path.GetTempPath(), file.FileName);
            await _blob.DownloadFileAsync($"{trackFolder}/{file.FileName}", localPath);

            // Convert to HLS
            var hlsDir = Path.Combine(Path.GetTempPath(), trackFolder);
            Directory.CreateDirectory(hlsDir);
            await _converter.ConvertToHlsAsync(localPath, hlsDir);

            // Upload HLS files (segments + m3u8)
            var hlsUrls = await _blob.UploadDirectoryAsync(hlsDir, trackFolder);

            // Cleanup files on local to save storage etc
            Directory.Delete(hlsDir, true);
            System.IO.File.Delete(localPath);

            dto.TrackURL = $"https://hyperradioblobstorage.blob.core.windows.net/tracks/{trackFolder}";
            
            var createdTrack = await _trackService.CreateTrackAsync(dto);
            if (createdTrack == null)
            {
                return StatusCode(500, "Upload failed");
            }
            //Return the mp3, hls playlist (i.e. m3u8 file) and chunks destinations on Blob
            return Ok(new
            {
                OriginalMp3 = mp3Url,
                HlsPlaylist = hlsUrls.FirstOrDefault(x => x.EndsWith(".m3u8")),
                HlsSegments = hlsUrls.Where(x => x.EndsWith(".ts")).ToList()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Track upload failed");
            return StatusCode(500, $"Upload failed: {ex.Message}");
        }
    }
        
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTrack(int id, [FromBody] UpdateTrackDTO track)
        {
            var result = await _trackService.UpdateTrackAsync(id, track);
            if (!result) 
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTrack(int id)
        {
            var result = await _trackService.DeleteTrackAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}