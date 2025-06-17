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
        public record CreateCourseRequest(Course.DayOfWeek day,
            TimeOnly timeOfLesson, 
            DateOnly DateOfStartCourse,
            DateOnly DateOfEndCourse, 
            string Title, 
            int CoachId);
        public record UpdateTimeCourseRequest(int id, TimeOnly timeOfCourse);
        public record UpdateDayCourseRequest(int id, Course.DayOfWeek day);

        public ICourseRepository _rep;
        public CourseController(ICourseRepository rep)
        {
            _rep = rep;
        }

        [HttpPost]
        public ActionResult CreateCoure([FromBody] CreateCourseRequest c) {
            Course course = new Course {TimeOfLesson = c.timeOfLesson,
                DateOfEndCourse = c.DateOfEndCourse,
                DateOfStartCourse = c.DateOfStartCourse,
                DaysOfWeek = c.day,
                Title = c.Title,
                CoachId = c.CoachId
            };
            if (_rep.Create(course) == null)
            {
                return BadRequest();
            }
            else {
                return Created();
            }
        }
        [HttpPut("update-day-lesson")]
        public ActionResult UpdateDayOfWorkedLesson([FromBody]UpdateDayCourseRequest c)
        {
            Course course = new Course { Id = c.id, DaysOfWeek = c.day};
            if (_rep.UpdateDayOfLesson(course) == true)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPut("update-time-course")]
        public ActionResult UpdateTimeinCourse([FromBody]UpdateTimeCourseRequest c)
        {
            Course course = new Course { Id = c.id,TimeOfLesson = c.timeOfCourse };

            if (_rep.UpdateTimeOfCourse(course) == true)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPut]
        public ActionResult UpdateCoachinCourse([FromBody] UpdateCoachCourseRequest c)
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
