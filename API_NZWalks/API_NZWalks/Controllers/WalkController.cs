
using API_NZWalks.Models.DTO;
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
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        public WalkController(IWalksRepository walksRepository, IRegionRepository regionRepository, IMapper mapper)
        {
            this.walksRepository = walksRepository;
            this.regionRepository = regionRepository;
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
            //Validate
            if (!(await ValidateAddWalkAsync(addWalkRequest)))
            {
                return BadRequest(ModelState);
            }

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
            //Validate
            if (!(await ValidateUpdateWalkAsync(updateWalkRequest)))
            {
                return BadRequest(ModelState);
            }
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


        #region Private Methods

        private async Task<bool> ValidateAddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            if (addWalkRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest), $"Add Walk Data is Required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(addWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(addWalkRequest.Name), $"{nameof(addWalkRequest.Name)} cannot be empty or whitespace.");
            }
            if (addWalkRequest.Length < 0)
            {
                ModelState.AddModelError(nameof(addWalkRequest.Length), $"{nameof(addWalkRequest.Length)} should be greater than zero.");
            }

            //Check RegionId is Valid
            var region = await regionRepository.GetAsync(addWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId), $"{nameof(addWalkRequest.RegionId)} is Invalid.");
            }

            //Add Validation for Walk Difficulty...
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            if (updateWalkRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest), $"Add Walk Data is Required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(updateWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Name), $"{nameof(updateWalkRequest.Name)} cannot be empty or whitespace.");
            }
            if (updateWalkRequest.Length < 0)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Length), $"{nameof(updateWalkRequest.Length)} should be greater than zero.");
            }

            //Check RegionId is Valid
            var region = await regionRepository.GetAsync(updateWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId), $"{nameof(updateWalkRequest.RegionId)} is Invalid.");
            }

            //Add Validation for Walk Difficulty...
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
