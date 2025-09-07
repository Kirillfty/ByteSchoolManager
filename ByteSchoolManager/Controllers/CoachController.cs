using ByteSchoolManager.Features.Coaches.Command.CreateCoach;
using ByteSchoolManager.Features.Coaches.Command.UpdateCoach;
using ByteSchoolManager.Features.Coaches.Query.GetAllCoaches;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ByteSchoolManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoachController : ControllerBase
    {
        private readonly ISender _sender;

        public CoachController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [ProducesResponseType<string>(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateCoach([FromBody] CreateCoachCommand request)
        {
            return Ok(await _sender.Send(request));
        }

        [HttpPut]
        [ProducesResponseType<string>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCoach([FromBody] UpdateCoachCommand request)
        {
            return Ok(await _sender.Send(request));
        }

        [HttpGet]
        [ProducesResponseType<IReadOnlyList<CoachDto>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCoaches()
        {
            return Ok(await _sender.Send(new GetAllCoachesQuery()));
        }
    }
}