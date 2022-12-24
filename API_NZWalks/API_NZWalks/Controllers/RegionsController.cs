using API_NZWalks.Models.Domain;
using API_NZWalks.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API_NZWalks.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        public RegionsController(IRegionRepository regionRepository,IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            var regions = await regionRepository.GetAllAsync();

            //return Regions DTO
            //var regionsDto = new List<Models.DTO.Region>();
            //regions.ToList().ForEach(region =>
            //{
            //        var regionDTO=new Models.DTO.Region()
            //        {
            //            Id= region.Id,
            //            Name = region.Name, 
            //            Area= region.Area,  
            //            Code=region.Code,   
            //            Lat= region.Lat,    
            //            Long= region.Long,  
            //            Population = region.Population,
            //        };

            //    regionsDto.Add(regionDTO);
            //});
            var regionsDto = mapper.Map<List<Models.DTO.Region>>(regions);
            return Ok(regionsDto);
        }
    }
}
