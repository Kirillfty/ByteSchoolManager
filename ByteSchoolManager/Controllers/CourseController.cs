using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Text.Json.Serialization;
using static ByteSchoolManager.Repository.ICourseRepository;

namespace ByteSchoolManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {

        public record UpdateCoachCourseRequest(int id, int coachId);
        public record UpdateDateStartCourseRequest(int id, DateOnly startDayCourse);
        public record UpdateDateEndCourseRequest(int id, DateOnly endDayCourse);
        public record CreateCourseRequest(int[] days,
            TimeOnly timeOfLesson,
            DateOnly DateOfStartCourse,
            DateOnly DateOfEndCourse,
            string Title,
            int CoachId);
        public record UpdateTimeCourseRequest(int id, TimeOnly timeOfCourse);
        public record UpdateDayCourseRequest(int id, int[] days);

        public readonly ICourseRepository _rep;
        public CourseController(ICourseRepository rep)
        {
            _rep = rep;
        }

        [HttpPost("create-course")]
        public ActionResult CreateCoure([FromBody] CreateCourseRequest c)
        {

            Course course = new Course
            {
                TimeOfLesson = c.timeOfLesson,
                DateOfEndCourse = c.DateOfEndCourse,
                DateOfStartCourse = c.DateOfStartCourse,
                DaysOfWeek = DaysHelper.GetDayOfWeek(c.days.Select(u => (DayOfWeek)u).ToArray()),
                Title = c.Title,
                CoachId = c.CoachId
            };
            if (_rep.Create(course) == null)
            {
                return BadRequest();
            }
            else
            {
                return Created();
            }
        }
        [HttpPatch("update-start-date")]
        public ActionResult UpdateStartCourse([FromBody] UpdateDateStartCourseRequest request) {

            var course = _rep.GetById(request.id);
            course.DateOfStartCourse = request.startDayCourse;
            if (_rep.UpdateDayStartCourse(course) == true)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPatch("update-end-date")]
        public ActionResult UpdateEndCourse([FromBody] UpdateDateEndCourseRequest request)
        {

            var course = _rep.GetById(request.id);
            if (course == null) {
                return NotFound();
            }
            course.DateOfEndCourse = request.endDayCourse;
            if (_rep.UpdateDayEndCourse(course) == true)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPatch("update-day-lesson")] //поменять put на patch
        public ActionResult UpdateDayOfWorkedLesson([FromBody] UpdateDayCourseRequest request)
        {
            var course = _rep.GetById(request.id);
            if (course == null)
            {
                return NotFound();
            }
            course.DaysOfWeek = DaysHelper.GetDayOfWeek(request.days.Select(u => (DayOfWeek) u).ToArray());
            if (_rep.UpdateDayOfLesson(course) == true)
            {
                return Ok();
            }
            return BadRequest();

            
        }
        [HttpPatch("update-time-course")]//поменять put на patch
        public ActionResult UpdateTimeInCourse([FromBody] UpdateTimeCourseRequest c)
        {
            Course course = new Course { Id = c.id, TimeOfLesson = c.timeOfCourse };

            if (_rep.UpdateTimeOfCourse(course) == true)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPatch("update-coach-in-course")]//поменять put на patch
        public ActionResult UpdateCoachInCourse([FromBody] UpdateCoachCourseRequest c)//исправь опечатку в названии метода
        {
            Course course = new Course { CoachId = c.coachId, Id = c.id };


            if (_rep.UpdateCoachCourse(course) == true)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet]
        public List<Course> GetAll()
        {
            return _rep.GetAll();
        }




    }
}
