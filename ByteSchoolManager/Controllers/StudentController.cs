using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ByteSchoolManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    IStudentRepository _repository;

    public StudentController(IStudentRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public List<Student> Get()
    {
        return _repository.GetAll();
    }

    [HttpPost]
    public ActionResult Create([FromBody] Student student)
    {
        if (_repository.Create(student) == null)
        {
            return BadRequest();
        }
        return Ok();
    }
    [HttpPut]
    public ActionResult Update([FromBody] Student student)
    {
        if (_repository.Update(student) == null)
        {
            return BadRequest();
        }
        
        return Ok();
    }
}