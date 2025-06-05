using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ByteSchoolManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoachController:ControllerBase
    {
        private ICoachRepository _repository;
        public CoachController(ICoachRepository usersRepository)
        {
            _repository = usersRepository;
        }

        [HttpPost]
        public ActionResult CreateCoach([FromBody] Coach coach) {
            if (_repository.Create(coach) != null)
                return Ok();

            return BadRequest();

        }
    }
}
