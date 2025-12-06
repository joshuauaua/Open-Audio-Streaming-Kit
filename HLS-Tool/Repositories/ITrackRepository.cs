using HLS_Tool.Models;

namespace HLS_Tool.Repositories.TrackRepositories
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