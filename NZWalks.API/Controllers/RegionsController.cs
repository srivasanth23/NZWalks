using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomeFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    // https://localhostportnumber/api/regins

    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        // Dependency Injection
        private readonly IRegionRepo _regionRepo;
        private readonly IMapper _mapper;

        public RegionsController(IRegionRepo regionRepo, IMapper mapper)
        {
            _regionRepo = regionRepo;
            _mapper = mapper;
        }




        // GET all regions
        // GET : https://localhost:7090/api/regions
        [HttpGet]
        //[ActionName("Get")]
        public async Task<IActionResult> GetAll()
        {
            // Get Data from database - Domain models
            var regions = await _regionRepo.GetAllAsync(); // calling repo

            // Map domain models to DTOs
            //var regionsDto = new List<RegionDTO>();
            //foreach (var region in regions)
            //{
            //    regionsDto.Add(new RegionDTO()
            //    {
            //        Id = region.Id,
            //        Code = region.Code,
            //        Name = region.Name,
            //        RegionImageUrl = region.RegionImageUrl,
            //    });
            //}

            // Using automapper to map domain model to DTOs
            // Return DTOs
            return Ok(_mapper.Map<List<RegionDTO>>(regions)); // success response // 200
        }




        // GET region by Id
        // GET : https://localhost:7090/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")] // as we require id from user we are adding this
        public async Task<IActionResult> GetById(Guid id)
        {
            // Get region domain model from database
            //var regions = _dbContext.Regions.Find(id); // Find only works with int type property well therefore we use FirstOrDefault for all others 
            var regionDomain = await _regionRepo.GetById(id);
            if (regionDomain == null)
            {
                return NotFound();
            }

            // Map domain models to DTOs
            return Ok(_mapper.Map<RegionDTO>(regionDomain));
        }




        // POST : Create a new region
        // POST : https://localhost:portnumber/api/regions
        [HttpPost]
        [ValidateModelAttribute]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            //if (ModelState.IsValid) // Here we are doing model validation by using Annotations
            //{
            // Map or convert DTO to domain model
            //var regionDomainModel = new Region
            //{
            //    Code = addRegionRequestDTO.Code,
            //    Name = addRegionRequestDTO.Name,
            //    RegionImageUrl = addRegionRequestDTO.RegionImageUrl,
            //};
            var regionDomainModel = _mapper.Map<Region>(addRegionRequestDTO);


            // Use DOmain Model to create Region
            regionDomainModel = await _regionRepo.CreateAsync(regionDomainModel);

            // Map domain model again to DTO
            //var regionDTO = new RegionDTO
            //{
            //    Id = regionDomainModel.Id,
            //    Code = addRegionRequestDTO.Code,
            //    Name = addRegionRequestDTO.Name,
            //    RegionImageUrl = addRegionRequestDTO.RegionImageUrl,
            //};
            var regionDTO = _mapper.Map<RegionDTO>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDTO);
            //}
            //else
            //{
            //    return BadRequest();
            //}
        }




        // PUT : Update the existing region
        // PUT : https://localhost:portnumber/api/regions/:id
        [HttpPut]
        [Route("{id:Guid}")] // as we require id from user we are adding this
        [ValidateModelAttribute]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionDTO updateRegionDTO)
        {

            var regionDomainModel = _mapper.Map<Region>(updateRegionDTO);

            regionDomainModel = await _regionRepo.UpdateAsync(id, regionDomainModel);


            if (regionDomainModel == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<RegionDTO>(regionDomainModel));

        }





        // DELETE : Delete a region
        // DELETE : https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await _regionRepo.DeleteAsync(id);

            if (regionDomainModel != null)
            {

                //also we can deelted region back
                var regionDTO = _mapper.Map<RegionDTO>(regionDomainModel);
                return Ok(regionDTO);
            }
            return NotFound();
        }
    }
}
