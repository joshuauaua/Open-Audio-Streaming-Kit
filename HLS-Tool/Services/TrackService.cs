using HLS_Tool.DTOs.TrackDTOs;
using HLS_Tool.Models;
using HLS_Tool.Repositories.TrackRepositories;
using HLS_Tool.Models;
using HLS_Tool.Repositories.TrackRepositories;

namespace HLS_Tool.Services.TrackServices
{
    public class TrackService : ITrackService
    {
        private readonly ITrackRepository _trackRepository;
        public TrackService(ITrackRepository trackRepository)
        {
            _trackRepository = trackRepository;
        }
        public async Task<IEnumerable<TrackDTO>> GetAllTracksAsync()
        {
            var tracks = await _trackRepository.GetAllTracksAsync();
            return tracks.Select(t => new TrackDTO
            {
                Id = t.Id,
                Title = t.Title,
                TrackURL = t.TrackURL,
            });
        }
        public async Task<TrackDTO?> GetTrackByIdAsync(int id)
        {
            var track = await _trackRepository.GetTrackByIdAsync(id);

            if (track == null) return null;

            return new TrackDTO
            {
                Id = track.Id,
                Title = track.Title,
                TrackURL = track.TrackURL,
            };
        }
        public async Task<TrackDTO?> CreateTrackAsync(CreateTrackDTO track)
        {
            var createTrack = new Track
            {
                Title = track.Title,
                TrackURL = track.TrackURL,
            };
            _trackRepository.CreateTrack(createTrack);

            if (await _trackRepository.SaveChangesAsync())
            {
                return new TrackDTO
                {
                    Id = createTrack.Id,
                    Title = createTrack.Title,
                    TrackURL = createTrack.TrackURL,
                };
            }
            return null;
        }
        public async Task<bool> UpdateTrackAsync(int id, UpdateTrackDTO track)
        {
            var existingTrack = await _trackRepository.GetTrackByIdAsync(id);

            if (existingTrack == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(track.Title))
            {
                existingTrack.Title = track.Title;
            }

            if (!string.IsNullOrEmpty(track.TrackURL))
            {
                existingTrack.TrackURL = track.TrackURL;
            }

            _trackRepository.UpdateTrack(existingTrack);
            return await _trackRepository.SaveChangesAsync();
        }
        public async Task<bool> DeleteTrackAsync(int id)
        {
            var track = await _trackRepository.GetTrackByIdAsync(id);

            if (track == null) return false;

            _trackRepository.DeleteTrack(track);
            return await _trackRepository.SaveChangesAsync();
        }
    }
}