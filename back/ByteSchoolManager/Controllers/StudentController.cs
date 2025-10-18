using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace ByteSchoolManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    IStudentRepository _repository;
    public record CreateStudentRequest(string Name,string StudentPhoneNumber, string ParentName,string ParentPhoneNumber);
    public record UpdateStudentRequest(int Id,string Name, string StudentPhoneNumber, string ParentName, string ParentPhoneNumber);

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
    public ActionResult Create([FromBody] CreateStudentRequest student)
    {
        Student stud = new Student
        {
            Name = student.Name,
            ParentName = student.ParentName,
            StudentPhoneNumber = student.StudentPhoneNumber,
            ParentPhoneNumber = student.ParentPhoneNumber
        };
        if (_repository.Create(stud) == null)
        {
            return BadRequest();
        }
        return Ok();
    }
    [HttpPut]
    public ActionResult Update([FromBody] UpdateStudentRequest student)
    {
        Student stud = new Student
        {
            Id = student.Id,
            Name = student.Name,
            ParentName = student.ParentName,
            StudentPhoneNumber = student.StudentPhoneNumber,
            ParentPhoneNumber = student.ParentPhoneNumber
        };
        if (_repository.Update(stud) == null)
        {
            return BadRequest();
        }
        
        return Ok();
    }
    [HttpDelete("delete/{id:int}")]
    public ActionResult DeleteStudent([FromRoute] int id) {
        if (_repository.Delete(id))
        {
            return Ok();
        }
        return BadRequest();
    }
    
}