using HLS_Tool.Data;
using HLS_Tool.Models;
using Microsoft.EntityFrameworkCore;
namespace HLS_Tool.Repositories.TrackRepositories
{
    public class TrackRepository : ITrackRepository
    {
        private readonly HLSToolDbContext _context;
        public TrackRepository(HLSToolDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Track>> GetAllTracksAsync()
        {
            return await _context.Tracks.ToListAsync();
        }
        public async Task<Track?> GetTrackByIdAsync(int id)
        {
            return await _context.Tracks.FindAsync(id);
        }

        public void CreateTrack(Track track)
        {
            _context.Tracks.Add(track);
        }
        public void UpdateTrack(Track track)
        {
            _context.Tracks.Update(track);
        }

        public void DeleteTrack(Track track)
        {
            _context.Remove(track);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}