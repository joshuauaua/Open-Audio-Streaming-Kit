using Hyper_Radio_API.DTOs.TrackDTOs;
using Hyper_Radio_API.Models;

namespace Hyper_Radio_API.Services.TrackServices
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