using Hyper_Radio_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Hyper_Radio_API.Data
{
    public class HLSToolDbContext : DbContext

    {
        public HLSToolDbContext(DbContextOptions<HLSToolDbContext> options) : base(options)
        {     
        }
        public DbSet<Track> Tracks { get; set; }
    }
}