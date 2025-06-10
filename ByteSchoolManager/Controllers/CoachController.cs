using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ByteSchoolManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CoachController:ControllerBase
    {
        public record CreateCoachRequest(string Name, string PhoneNumber, string Telegram);
        private ICoachRepository _repository;
        public CoachController(ICoachRepository usersRepository)
        {
            _repository = usersRepository;
        }

        [HttpPost]
        public ActionResult CreateCoach([FromBody] CreateCoachRequest coach) {

            Coach c = new Coach { Name = coach.Name, PhoneNumber = coach.PhoneNumber, Telegram = coach.Telegram };
            if (_repository.Create(c) != null)
                return Ok();

            return BadRequest();

        }

        [HttpPut]
        public ActionResult UpdateCoach([FromBody] Coach coach)
        {
            if (_repository.Update(coach) == null)
                return BadRequest();
            
            return Ok();
        }
    }
}
