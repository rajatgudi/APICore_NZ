using API_NZWalks.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace API_NZWalks.Data
{
    public class NZWalksDbContext:DbContext
    {
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> options):base(options) 
        {

        }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<WalkDifficulty> WalksDifficulty { get; set; }
    }
}
