using ByteSchoolManager.Entities;
using ByteSchoolManager.Features.Lessons.GetLessonsByDay;
using ByteSchoolManager.Features.Lessons.GetLessonWithStudents;
using ByteSchoolManager.Features.Lessons.MarkLesson;
using ByteSchoolManager.Repository;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ByteSchoolManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonController : ControllerBase
    {
        private readonly ILessonsRepository _lessonsRepository;
        private readonly ISender _sender;

        public record MoveLessonRequest(int LessonId, DateTime Date);

        public record ReplaceCoachInLessonRequest(int LessonId, int CoachId);

        public LessonController(ILessonsRepository studentRepository,
            ISender sender)
        {
            _lessonsRepository = studentRepository;
            _sender = sender;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetLessonWithStudents([FromRoute] int id)
        {
            return Ok(await _sender.Send(new GetLessonWithStudentsQuery(id)));
        }

        [Authorize(Roles = UserRole.Coach)]
        [HttpGet("by-day/{dayOfWeak:int}")]
        public async Task<IActionResult> GetLessonsByDay([FromRoute] int dayOfWeak)
        {
            var userId = int.Parse(HttpContext.User.Identity.Name);

            return Ok(await _sender.Send(new GetLessonsByDayQuery(dayOfWeak, userId)));
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

        [HttpPost("mark")]
        public async Task<IActionResult> MarkLesson([FromBody] MarkLessonCommand command)
        {
            return Ok(await _sender.Send(command));
        }
    }
}