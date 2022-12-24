using API_NZWalks.Models.Domain;

namespace API_NZWalks.Repositories
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetAllAsync();
    }
}
