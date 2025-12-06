using HLS_Tool.DTOs.TrackDTOs;
using HLS_Tool.Models;

namespace HLS_Tool.Services.TrackServices
{
    public interface ITrackService
    {
        Task<IEnumerable<TrackDTO>> GetAllTracksAsync();
        Task<TrackDTO?> GetTrackByIdAsync(int id);
        Task<TrackDTO> CreateTrackAsync(CreateTrackDTO track);
        Task<bool> UpdateTrackAsync(int id, UpdateTrackDTO track);
        Task<bool> DeleteTrackAsync(int id);
    }
}