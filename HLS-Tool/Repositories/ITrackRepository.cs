using Hyper_Radio_API.Models;

namespace Hyper_Radio_API.Repositories.TrackRepositories
{
    public interface ITrackRepository
    {
        Task<IEnumerable<Track>> GetAllTracksAsync();
        Task<Track?> GetTrackByIdAsync(int id);
        void CreateTrack(Track track);
        void UpdateTrack(Track track);
        void DeleteTrack(Track track);
        Task<bool> SaveChangesAsync();
    }
}