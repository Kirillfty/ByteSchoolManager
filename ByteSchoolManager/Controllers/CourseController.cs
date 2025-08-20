using ByteSchoolManager.Entities;
using ByteSchoolManager.Features.Courses.Create;
using ByteSchoolManager.Features.Courses.GetAll;
using ByteSchoolManager.Features.Courses.UpdateCourseCoach;
using ByteSchoolManager.Features.Courses.UpdateCourseDays;
using ByteSchoolManager.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ByteSchoolManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ISender _sender;

        public record UpdateDateStartCourseRequest(int Id, DateOnly StartDayCourse);

        public record UpdateDateEndCourseRequest(int Id, DateOnly EndDayCourse);

        public record AddStudentCourseRequest(int Id, int[] Students);

        public record UpdateTimeCourseRequest(int Id, TimeOnly TimeOfCourse);

        private readonly ICourseRepository _courseRepository;

        public CourseController(ICourseRepository courseRepository, ISender sender)
        {
            _courseRepository = courseRepository;
            _sender = sender;
        }

        [HttpGet]
        public async Task<List<CourseDto>> GetAll(CancellationToken ct)
        {
            return await _sender.Send(new GetAllCoursesQuery(), ct);
        }

        [HttpPost]
        public async Task<string> Create([FromBody] CreateCourseCommand request, CancellationToken ct)
        {
            return await _sender.Send(request, ct);
        }

        [HttpPatch("start-date")]
        public ActionResult UpdateStartDate([FromBody] UpdateDateStartCourseRequest request)
        {
            var course = _courseRepository.GetById(request.Id);
            if (course == null)
            {
                return NotFound();
            }

            course.DateOfStartCourse = request.StartDayCourse;
            if (_courseRepository.UpdateDayStartCourse(course))
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPatch("end-date")]
        public ActionResult UpdateEndDate([FromBody] UpdateDateEndCourseRequest request)
        {
            var course = _courseRepository.GetById(request.Id);
            if (course == null)
            {
                return NotFound();
            }

            course.DateOfEndCourse = request.EndDayCourse;
            if (_courseRepository.UpdateDayEndCourse(course))
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPatch("days")]
        public async Task<IActionResult> UpdateCourseDays([FromBody] UpdateCourseDaysCommand request, CancellationToken ct)
        {
            return Ok(await _sender.Send(request, ct));
        }

        [HttpPatch("lesson-time")]
        public ActionResult UpdateTime([FromBody] UpdateTimeCourseRequest request)
        {
            Course course = new Course { Id = request.Id, TimeOfLesson = request.TimeOfCourse };

            if (_courseRepository.UpdateTimeOfCourse(course))
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPatch("coach")]
        public async Task<ActionResult<string>> UpdateCoach([FromBody] UpdateCourseCoachCommand request, CancellationToken ct)
        {
            return await _sender.Send(request, ct);
        }

        [HttpPost("add-student-in-course")]
        public ActionResult AddStudentInCourse([FromBody] AddStudentCourseRequest request)
        {
            if (_courseRepository.AddStudentInCourse(request.Id, request.Students) == false)
                return NotFound();

            return Ok();
        }
    }
}