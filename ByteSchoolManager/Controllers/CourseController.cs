using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using Microsoft.AspNetCore.Mvc;
using static ByteSchoolManager.Repository.ICourseRepository;

namespace ByteSchoolManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {

        public record UpdateCoachCourseRequest(int id,int coachId);
        public record UpdateTimeCourseRequest(int id, TimeOnly timeOfCourse);
        public record UpdateDayCourseRequest(int id, DayOfWeek dayOfCourse);
        public ICourseRepository _rep;
        public CourseController(ICourseRepository rep)
        {
            _rep = rep;
        }

        [HttpPost]
        public ActionResult CreateCoure([FromBody] Course c) {

            if (_rep.Create(c) == null)
            {
                return BadRequest();
            }
            else {
                return Created();
            }
        }
        [HttpPut("update-day-lesson")]
        public ActionResult UpdateDayOfWorkedLesson(Course c)
        {
           
            if (_rep.UpdateDayOfLesson(c) == true)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPut("update-time-course")]
        public ActionResult UpdateTimeinCourse(UpdateTimeCourseRequest c)
        {
            Course course = new Course { Id = c.id,TimeOfLesson = c.timeOfCourse };

            if (_rep.UpdateTimeOfCourse(course) == true)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPut]
        public ActionResult UpdateDayCoachinCourse(UpdateCoachCourseRequest c)
        {
            Course course = new Course { CoachId = c.coachId,Id = c.id };
            

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
