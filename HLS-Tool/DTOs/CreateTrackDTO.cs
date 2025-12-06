namespace HLS_Tool.DTOs.TrackDTOs
{
    public class CreateTrackDTO
    {
        public string Title { get; set; }
        public string? TrackURL { get; set; }
        public IFormFile? File { get; set; } = default!;
    }
}