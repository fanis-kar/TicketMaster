using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketMaster.Api.Filter;
using TicketMaster.Api.Model;
using TicketMaster.Business.Interfaces;
using TicketMaster.Business.Services;

namespace TicketMaster.Controllers
{
    [ApiController]    
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<TicketsController> _logger;

        public TicketsController(ITicketService service,
                                IMapper mapper, ILogger<TicketsController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }


        [HttpGet]
        [Route("/api/tickets/")]
        public async Task<IEnumerable<TicketJSONReply>> GetAsync()
        {
            return _mapper.Map<IEnumerable<TicketJSONReply>>(await _service.GetAllAsync());
        }


        [HttpGet]
        [Route("/api/[controller]/{id}")]
        public async Task<TicketJSONReply> GetAsync(long id)
        {
            return _mapper.Map<TicketJSONReply>(await _service.GetAsync(id));
        }

        [HttpGet]
        [Route("api/[controller]/show/{showId}")]
        public async Task<ICollection<TicketJSONReply>> GetByShowAsync(long showId)
        {
            return _mapper.Map<ICollection<TicketJSONReply>>(await _service.GetByShow(showId));
        }

        [HttpPost]
        [Route("api/[controller]/")]
        public async Task<ICollection<TicketJSONReply>> PostAsync(TicketJSONRequest request)
        {
            return _mapper.Map<ICollection<TicketJSONReply>>(await _service.ReserveTickets(
                                                                           request.ShowId,
                                                                           request.Quantity,
                                                                           request.Price));
        }

        [HttpDelete]
        [Route("api/[controller]/{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            await _service.Remove(id);
            return Ok();
        }        
    }
}
