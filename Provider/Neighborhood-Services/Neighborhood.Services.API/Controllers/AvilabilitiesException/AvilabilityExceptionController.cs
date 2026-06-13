using MediatR;
using Microsoft.AspNetCore.Mvc;
using Neighborhood.Services.Application.AvilabilitiesException.Commands;
using Neighborhood.Services.Application.AvilabilitiesException.DTOs;
using Neighborhood.Services.Application.AvilabilitiesException.Queries;

namespace Neighborhood.Services.API.Controllers.AvilabilitiesException
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvilabilityExceptionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AvilabilityExceptionController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("{technicianId}")]
        public async Task<ActionResult<IReadOnlyList<AvailiabilityExceptionDTO>>> Get(int technicianId)
             => Ok(await _mediator.Send(new GetAvabilityExceptionForSpecificTechQuery(technicianId)));



        [HttpPost("{technicianId}")]
        public async Task<ActionResult<int>> Add(int technicianId, AddAvailabilityExceptionCommand command)
        {
            command.TechnicianId = technicianId;
            return Ok(await _mediator.Send(command));
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Update(int id, UpdateAvailabilityExceptionCommand command)
        {
            command.Id = id;
            return Ok(await _mediator.Send(command));
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            var command = new DeleteAvailabilityExceptionCommand(id);
            return Ok(await _mediator.Send(command));
        }





    }
}
