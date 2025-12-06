using Hyper_Radio_API.Models;
using System.ComponentModel.DataAnnotations;

namespace Hyper_Radio_API.DTOs.TrackDTOs
{
    public class TrackDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TrackURL { get; set; }
    }
}