using HLS_Tool.Models;
using Microsoft.EntityFrameworkCore;

namespace HLS_Tool.Data
{
    public class HLSToolDbContext : DbContext

    {
        public HLSToolDbContext(DbContextOptions<HLSToolDbContext> options) : base(options)
        {     
        }
        public DbSet<Track> Tracks { get; set; }
    }
}