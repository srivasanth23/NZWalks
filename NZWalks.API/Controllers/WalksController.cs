using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomeFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    // /api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepo _walkRepo;

        public WalksController(IMapper imapper, IWalkRepo iwalkrepo)
        {
            _mapper = imapper;
            _walkRepo = iwalkrepo;
        }



        // CREATE Walk
        // POST : https://localhost:potnumber/api/walks
        [HttpPost]
        [ValidateModelAttribute]
        public async Task<IActionResult> Create([FromBody] AddrequestWalksDTO addrequestWalksDTO)
        {

            // Map DTO to Domiain model using Automapper
            var walkDomainModel = _mapper.Map<Walk>(addrequestWalksDTO);
            await _walkRepo.CreateAsync(walkDomainModel);

            // Map domain model to DTO and return
            return Ok(_mapper.Map<WalkDTO>(walkDomainModel));

        }


        // GET Walks
        // GET : https://localhost:portnumber/api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true
        [HttpGet]
        public async Task<ActionResult> GetAllWalks([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walksDomainModel = await _walkRepo.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

            // Map Domain Model to DTO 
            return Ok(_mapper.Map<List<WalkDTO>>(walksDomainModel));
        }



        // GET Walks by ID
        // GET : https://localhost:portnumber/api/walks/:id
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetWalkById([FromRoute] Guid id)
        {
            var walksDomainModel = await _walkRepo.GetWalkByIdAsync(id);
            if (walksDomainModel == null)
            {
                return NotFound();
            }

            //Map Domain model to DTO
            return Ok(_mapper.Map<WalkDTO>(walksDomainModel));
        }



        // UPDATE Walks by ID
        // PUT : https://localhost:portnumber/api/walks/:id
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModelAttribute]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDTO updateWalkRequestDTO)
        {

            // Map DTO to Domian model
            var walksDomainModel = _mapper.Map<Walk>(updateWalkRequestDTO);
            walksDomainModel = await _walkRepo.UpdateWalkAsync(id, walksDomainModel);

            if (walksDomainModel == null) { return NotFound(); }

            // Map Domain Model to DTO
            return Ok(_mapper.Map<WalkDTO>(walksDomainModel));

        }


        // DELETE Walks By Id
        // DELETE : https://localhost:portnumber/api/walks/:id
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walksDomainModel = await _walkRepo.DeleteWalkAsync(id);
            if (walksDomainModel == null) { return NotFound(); }

            return Ok(_mapper.Map<WalkDTO>(walksDomainModel));
        }
    }
}
