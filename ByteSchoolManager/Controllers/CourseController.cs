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

        public record UpdateCoachCourseRequest(int id,int coachId); //поля в рекордах с большой буквы
        public record CreateCourseRequest(Course.DayOfWeek day, //не day а days, лучше сделай чтобы передался массив дней недели, ибо enum flags на js нету
            TimeOnly timeOfLesson, 
            DateOnly DateOfStartCourse,
            DateOnly DateOfEndCourse, 
            string Title, 
            int CoachId);//поля в рекордах с большой буквы
        public record UpdateTimeCourseRequest(int id, TimeOnly timeOfCourse);//поля в рекордах с большой буквы
        public record UpdateDayCourseRequest(int id, Course.DayOfWeek day);//поля в рекордах с большой буквы, не day а days, лучше сделай чтобы передался массив дней недели, ибо enum flags на js нету

        public ICourseRepository _rep; //добавить public readonly, изменить название поля на _repository (буквы если что бесплатные)
        public CourseController(ICourseRepository rep)
        {
            _rep = rep;
        }

        [HttpPost]
        public ActionResult CreateCoure([FromBody] CreateCourseRequest c) { //исправь опечатку в названии метода, поменять название 'c' на 'request'
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
        [HttpPatch("update-day-lesson")] //поменять put на patch
        public ActionResult UpdateDayOfWorkedLesson([FromBody]UpdateDayCourseRequest c)
        {
            Course course = new Course { Id = c.id, DaysOfWeek = c.day};
            if (_rep.UpdateDayOfLesson(course) == true)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPatch("update-time-course")]//поменять put на patch
        public ActionResult UpdateTimeinCourse([FromBody]UpdateTimeCourseRequest c)
        {
            Course course = new Course { Id = c.id,TimeOfLesson = c.timeOfCourse };

            if (_rep.UpdateTimeOfCourse(course) == true)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPatch]//поменять put на patch
        public ActionResult UpdateCoachInCourse([FromBody] UpdateCoachCourseRequest c)//исправь опечатку в названии метода
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
