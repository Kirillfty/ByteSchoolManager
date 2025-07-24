using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ByteSchoolManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CoachController : ControllerBase
    {
        public record CreateCoachRequest(string Name, string PhoneNumber, string Telegram, int userId);
        public record UpdateCoachRequest(int Id, string Name, string PhoneNumber, string Telegram);
        private ICoachRepository _repository;
        private IUserRepository _userRepository;
        public CoachController(ICoachRepository usersRepository)
        {
            _repository = usersRepository;
        }

        [HttpPost]
        public ActionResult CreateCoach([FromBody] CreateCoachRequest request)
        {

            Coach coach = new Coach { Name = request.Name, PhoneNumber = request.PhoneNumber, Telegram = request.Telegram, UserId = request.userId };
            if (_repository.Create(coach) != null)
            {
                if (_userRepository.UpdateRoleUser(request.userId, UserRole.Coach))
                {
                    return Ok();
                }
            }

            return BadRequest();

        }

        [HttpPut]
        public ActionResult UpdateCoach([FromBody] UpdateCoachRequest request)
        {
            Coach coach = new Coach { Name = request.Name, Id = request.Id, PhoneNumber = request.PhoneNumber, Telegram = request.Telegram };

            if (_repository.Update(coach) == false)
                return BadRequest();

            return Ok();
        }
        [HttpGet]
        public List<Coach> GetAllCoaches()
        {
            return _repository.GetAll();
        }
    }
}
