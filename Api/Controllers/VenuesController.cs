using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketMaster.Api.Filter;
using TicketMaster.Api.Model;
using TicketMaster.Business.Interfaces;
using TicketMaster.Data.Model;

namespace TicketMaster.Controllers
{
    [ApiController]
    [Route("/api/[controller]/{id?}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public class VenuesController : ControllerBase
    {


        private readonly IService<Venue> _service;
        private readonly IMapper _mapper;
        private readonly ILogger<VenuesController> _logger;

        public VenuesController(IService<Venue> service,
                                IMapper mapper, ILogger<VenuesController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }


        [HttpGet]
        [Route("/api/[controller]/")]
        public async Task<IEnumerable<VenueJSON>> GetAsync()
        {
            return _mapper.Map<IEnumerable<VenueJSON>>(await _service.GetAllAsync());
        }


        [HttpGet]        
        public async Task<VenueJSON> GetAsync(long id)
        {
            return _mapper.Map<VenueJSON>(await _service.GetAsync(id));
        }

        [HttpPost]               
        public async Task<VenueJSON> Post(VenueJSON venue)
        {
            return _mapper.Map<VenueJSON>(await _service.Add(_mapper.Map<Venue>(venue)));
        }

        [HttpDelete]       
        public async Task<IActionResult> DeleteAsync(long id)
        {
            await _service.Remove(id);
            return Ok();
        }
    }
}
