using AutoMapper;
namespace API_NZWalks.Profiles
{
    public class RegionProfile:Profile
    {
        public RegionProfile()
        {
            CreateMap<Models.Domain.Region,Models.DTO.Region>()
                .ReverseMap();
        }
    }
}
