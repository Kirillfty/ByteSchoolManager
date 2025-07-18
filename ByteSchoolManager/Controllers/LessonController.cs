using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ByteSchoolManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonController : ControllerBase
    {

        private readonly ILessonsRepository _lessonsRepository;

        public LessonController(ILessonsRepository studentRepository)
        {   
            _lessonsRepository = studentRepository;
        }



        
    }
}
