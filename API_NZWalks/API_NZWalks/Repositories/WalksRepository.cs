using API_NZWalks.Data;
using API_NZWalks.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;

namespace API_NZWalks.Repositories
{
    public class WalksRepository : IWalksRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;
        public WalksRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Walk> AddASync(Walk walk)
        {

            //Assign New ID
            walk.Id = Guid.NewGuid();

            await nZWalksDbContext.AddAsync(walk);
            await nZWalksDbContext.SaveChangesAsync();

            return walk;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await nZWalksDbContext.Walks.Include(x => x.Region).Include(x => x.WalkDifficulty).ToListAsync();
        }
        public async Task<Walk> GetAsync(Guid id)
        {
            return await nZWalksDbContext.Walks.Include(x => x.Region).Include(x => x.WalkDifficulty).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await nZWalksDbContext.Walks.FindAsync(id);
            if (existingWalk != null)
            {
                existingWalk.Length = walk.Length;
                existingWalk.Name = walk.Name;
                existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
                existingWalk.RegionId = walk.RegionId;
                await nZWalksDbContext.SaveChangesAsync();
                return existingWalk;
            }
            return null;
        }
        public async Task<Walk> DeleteAsync(Guid id)
        {
            var existingWalk = await nZWalksDbContext.Walks.FindAsync(id);
            if (existingWalk == null)
            {
                return null;
            }
            nZWalksDbContext.Walks.Remove(existingWalk);
            await nZWalksDbContext.SaveChangesAsync();
            return existingWalk;
        }
    }
}
