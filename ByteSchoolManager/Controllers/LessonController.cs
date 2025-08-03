using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using ByteSchoolManager.Responces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ByteSchoolManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonController : ControllerBase
    {
        private readonly ILessonsRepository _lessonsRepository;
        private readonly ICoachRepository _coachRepository;
        private readonly ICourseRepository _courseRepository;
        
        public record MoveLessonRequest(int LessonId, DateTime Date);

        public record ReplaceCoachInLessonRequest(int LessonId, int CoachId);

        public LessonController(ILessonsRepository studentRepository,
            ICoachRepository coachRepository,
            ICourseRepository courseRepository)
        {
            _lessonsRepository = studentRepository;
            _coachRepository = coachRepository;
            _courseRepository = courseRepository;
        }

        [HttpGet("{id:int}")]
        public GetLessonByIdResponce? GetLessonByIdWithStudents([FromRoute] int id)
        {
            return _lessonsRepository.GetByIdWithStudents(id);
        }

        [Authorize(Roles = UserRole.Coach)]
        [HttpGet("by-day/{dayOfWeak:int}")]
        public List<GetLessonsInDayResponce> GetLassonInDay([FromRoute] int dayOfWeak)
        {
            var userId = HttpContext.User.Identity.Name;
            var coach = _coachRepository.GetByUserId(int.Parse(userId));

            return _courseRepository.GetLessonsInDays(coach.Id, dayOfWeak);
        }
        
        [HttpPatch("move")]
        public ActionResult ResheduleLesson([FromBody] MoveLessonRequest request)
        {
            if (_lessonsRepository.RescheduleLesson(request.LessonId, request.Date) == false)
                return NotFound();

            return Ok();
        }

        [HttpPatch("replace-coach")]
        public ActionResult ResheduleCoachInLesson([FromBody] ReplaceCoachInLessonRequest request)
        {
            if (_lessonsRepository.RescheduleCoachInLesson(request.LessonId, request.CoachId) == false)
                return NotFound();

            return Ok();
        }
    }
}