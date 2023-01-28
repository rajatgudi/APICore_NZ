
using API_NZWalks.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API_NZWalks.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkController : Controller
    {
        private readonly IWalksRepository walksRepository;
        private readonly IMapper mapper;
        public WalkController(IWalksRepository walksRepository, IMapper mapper)
        {
            this.walksRepository = walksRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWalkAsync()
        {
            //Fetch Data from Database -- domain walks
            var walks = await walksRepository.GetAllAsync();

            //Convert Domain walks to DTO Walks

            var walksDto = mapper.Map<List<Models.DTO.Walk>>(walks);
            //return walks DTO 
            return Ok(walksDto);
        }


        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            var walkDomain = await walksRepository.GetAsync(id);

            var walkDto = mapper.Map<Models.DTO.Walk>(walkDomain);

            return Ok(walkDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            //Convert DTO to Domain Request
            var walkDomain = new Models.Domain.Walk
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId,
            };
            //pass domain obj
            walkDomain = await walksRepository.AddASync(walkDomain);

            //Convert Domain to DTO
            var walkDto = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId,
            };

            //Send Response to Client

            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDto.Id }, walkDto);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //Convert DTO to Domain
            var walkDomain = new Models.Domain.Walk
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId,
            };
            //Pass details to Repository Get Domain Obj in repsonse or null
            walkDomain = await walksRepository.UpdateAsync(id, walkDomain);

            //Handle Not FOund'
            if (walkDomain == null)
            {
                return NotFound();
            }
            else
            {
                //Convert domain back to DTO
                var walkDto = new Models.Domain.Walk
                {
                    Id = walkDomain.Id,
                    Length = walkDomain.Length,
                    Name = walkDomain.Name,
                    RegionId = walkDomain.RegionId,
                    WalkDifficultyId = walkDomain.WalkDifficultyId,
                };


                //Return Response
                return Ok(walkDto);
            }

        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync([FromRoute] Guid id)
        {
            //Delete from repository

            var walkDomain = await walksRepository.DeleteAsync(id);
            if (walkDomain == null)
            {
                return NotFound();
            }
            else
            {
                var walkDto = mapper.Map<Models.DTO.Walk>((walkDomain));
                return Ok(walkDto);

            }
        }

    }
}
