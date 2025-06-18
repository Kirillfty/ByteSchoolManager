using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ByteSchoolManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        public record UpdateCoachCourseRequest(int Id, int CoachId);

        public record UpdateDateStartCourseRequest(int Id, DateOnly StartDayCourse);

        public record UpdateDateEndCourseRequest(int Id, DateOnly EndDayCourse);

        public record CreateCourseRequest(
            int[] Days,
            TimeOnly TimeOfLesson,
            DateOnly DateOfStartCourse,
            DateOnly DateOfEndCourse,
            string Title,
            int CoachId
        );

        public record UpdateTimeCourseRequest(int Id, TimeOnly TimeOfCourse);

        public record UpdateDayCourseRequest(int Id, int[] Days);

        private readonly ICourseRepository _courseRepository;

        public CourseController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        [HttpGet]
        public List<Course> GetAll()
        {
            return _courseRepository.GetAll();
        }

        [HttpPost]
        public ActionResult Create([FromBody] CreateCourseRequest request)
        {
            Course course = new Course
            {
                TimeOfLesson = request.TimeOfLesson,
                DateOfEndCourse = request.DateOfEndCourse,
                DateOfStartCourse = request.DateOfStartCourse,
                DaysOfWeek = DaysHelper.GetDayOfWeek(request.Days.Select(i => (DayOfWeek)i).ToArray()),
                Title = request.Title,
                CoachId = request.CoachId
            };
            if (_courseRepository.Create(course) == null)
            {
                return BadRequest();
            }
            else
            {
                return Created();
            }
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

        [HttpPatch("lesson-days")]
        public ActionResult UpdateLessonDays([FromBody] UpdateDayCourseRequest request)
        {
            var course = _courseRepository.GetById(request.Id);
            if (course == null)
            {
                return NotFound();
            }

            course.DaysOfWeek = DaysHelper.GetDayOfWeek(request.Days.Select(i => (DayOfWeek)i).ToArray());
            if (_courseRepository.UpdateDayOfLesson(course))
            {
                return Ok();
            }

            return BadRequest();
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
        public ActionResult UpdateCoach([FromBody] UpdateCoachCourseRequest request)
        {
            Course course = new Course { CoachId = request.CoachId, Id = request.Id };


            if (_courseRepository.UpdateCoachCourse(course))
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}