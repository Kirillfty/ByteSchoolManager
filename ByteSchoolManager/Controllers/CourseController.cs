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
        private readonly ICourseRepository _rep;
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
    }
}
