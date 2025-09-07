using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ByteSchoolManager.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPut]
    public ActionResult Update([FromBody] User user)
    {
        if (_userRepository.Update(user) == null)
        {
            return BadRequest();
        }
        return Ok();
    }
}