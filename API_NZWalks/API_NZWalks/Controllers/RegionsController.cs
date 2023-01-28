using API_NZWalks.Models.Domain;
using API_NZWalks.Models.DTO;
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
        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
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
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var result = await regionRepository.GetAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            var regionDto = mapper.Map<Models.DTO.Region>(result);
            return Ok(regionDto);

        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            //Request to Domain Model

            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Name = addRegionRequest.Name,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Population = addRegionRequest.Population,

            };

            //Pass details to Repository
            region = await regionRepository.AddAsync(region);

            //Convert back to DTO
            var regionDto = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population,
            };

            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDto.Id }, regionDto);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            // Get Region from Database

            var region = await regionRepository.DeleteAsync(id);

            //If null NotFound
            if (region == null)
            {
                return NotFound();
            }

            //If Found and Deleted
            //Convert response back to DTO

            var regionDto = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population,
            };

            //Return Ok Response
            return Ok(regionDto);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute]Guid id,[FromBody]Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            //COnvert DTO to Domain
            var region = new Models.Domain.Region()
            {
                Name = updateRegionRequest.Name,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Population = updateRegionRequest.Population,

            };

            //Update Region using repository

            region = await regionRepository.UpdateAsync(id, region);

            //If null not found
            if(region== null)
            {
                return NotFound();
            }

            //not null Convert Domain to DTO
            var regionDto = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population,
            };

            //Return OK Response
            return Ok(regionDto);
        }
    }
}
