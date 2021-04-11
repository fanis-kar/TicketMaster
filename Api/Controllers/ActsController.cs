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
    
    public class ActsController : ControllerBase
    {
        private readonly IService<Act> _service;
        private readonly IMapper _mapper;
        private readonly ILogger<ActsController> _logger;

        public ActsController(IService<Act> service,
                                IMapper mapper, ILogger<ActsController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }


        [HttpGet]
        [Route("/api/[controller]/")]
        public async Task<IEnumerable<ActJSON>> GetAsync()
        {
            return _mapper.Map<IEnumerable<ActJSON>>(await _service.GetAllAsync());
        }


        [HttpGet]        
        public async Task<ActJSON> GetAsync(long id)
        {
            return _mapper.Map<ActJSON>(await _service.GetAsync(id));
        }

        [HttpPost]
        public async Task<ActJSON> PostAsync(ActJSON act)
        {
            return _mapper.Map<ActJSON>(await _service.Add(_mapper.Map<Act>(act)));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            await _service.Remove(id);
            return Ok();
        }
    }
}
