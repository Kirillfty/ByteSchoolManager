using ByteSchoolManager.Entities;
using ByteSchoolManager.Features.Courses.Create;
using ByteSchoolManager.Features.Courses.Delete;
using ByteSchoolManager.Features.Courses.EditCourseStudents;
using ByteSchoolManager.Features.Courses.GetAll;
using ByteSchoolManager.Features.Courses.UpdateCourseCoach;
using ByteSchoolManager.Features.Courses.UpdateCourseDates;
using ByteSchoolManager.Features.Courses.UpdateCourseDays;
using ByteSchoolManager.Features.Courses.UpdateCourseTime;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ByteSchoolManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ISender _sender;
        
        public CourseController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            return Ok(await _sender.Send(new GetAllCoursesQuery(), ct));
        }
        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteCourseCommand { Id = id };
            await _sender.Send(command);

            return NoContent();
        }




        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCourseCommand request, CancellationToken ct)
        {
            return Ok(await _sender.Send(request, ct));
        }

        [HttpPatch("dates")]
        public async Task<IActionResult> UpdateDates([FromBody] UpdateCourseDatesCommand request,
            CancellationToken ct)
        {
            return Ok(await _sender.Send(request, ct));
        }

        [HttpPatch("days")]
        public async Task<IActionResult> UpdateDays([FromBody] UpdateCourseDaysCommand request,
            CancellationToken ct)
        {
            return Ok(await _sender.Send(request, ct));
        }

        [HttpPatch("time")]
        public async Task<IActionResult> UpdateTime([FromBody] UpdateCourseTimeCommand request,
            CancellationToken ct)
        {
            return Ok(await _sender.Send(request, ct));
        }

        [HttpPatch("coach")]
        public async Task<IActionResult> UpdateCoach([FromBody] UpdateCourseCoachCommand request,
            CancellationToken ct)
        {
            return Ok(await _sender.Send(request, ct));
        }

        [HttpPatch("students")]
        public async Task<IActionResult> UpdateStudents([FromBody] EditCourseStudentsCommand request,
            CancellationToken ct)
        {
            return Ok(await _sender.Send(request, ct));
        }
    }
}