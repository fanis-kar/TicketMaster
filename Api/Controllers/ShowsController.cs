using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketMaster.Api.Filter;
using TicketMaster.Api.Model;
using TicketMaster.Business.Interfaces;
using TicketMaster.Business.Services;
using TicketMaster.Data.Model;

namespace TicketMaster.Controllers
{
    [ApiController]
    [Route("/api/[controller]/{id?}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]

    public class ShowsController : ControllerBase
    {


        private readonly IShowService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<ShowsController> _logger;

        public ShowsController(IShowService service,
                                IMapper mapper, ILogger<ShowsController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }


        [HttpGet]
        [Route("/api/[controller]/")]
        public async Task<IEnumerable<ShowJSON>> GetAsync()
        {
            return _mapper.Map<IEnumerable<ShowJSON>>(await _service.GetAllAsync());
        }


        [HttpGet]
        public async Task<ShowJSON> GetAsync(long id)
        {
            return _mapper.Map<ShowJSON>(await _service.GetAsync(id));
        }

        [HttpGet]
        [Route("/api/[controller]/venue/{venueId}")]
        public async Task<ICollection<ShowJSON>> GetByVenue(long venueId)
        {
            return _mapper.Map<ICollection<ShowJSON>>(await _service.GetShowsByVenue(venueId));
        }

        [HttpGet]
        [Route("/api/[controller]/dates/from/{start}/to/{end}")]
        public async Task<ICollection<ShowJSON>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            return _mapper.Map<ICollection<ShowJSON>>(await _service.GetShowsByDateRange(start, end));
        }

        [HttpPost]
        public async Task<ShowJSON> PostAsync(ShowJSON act)
        {
            return _mapper.Map<ShowJSON>(await _service.Add(_mapper.Map<Show>(act)));
        }

        [HttpDelete]
        [Route("/api/shows/{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            await _service.Remove(id);
            return Ok();
        }
    }
}
