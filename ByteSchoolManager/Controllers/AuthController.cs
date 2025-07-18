using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ClubsBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtCreator _jwtCreator;
        private readonly IUserRepository _userRepository;

        public AuthController(JwtCreator jwtCreator, IUserRepository userRepository)
        {
            _jwtCreator = jwtCreator;
            _userRepository = userRepository;
        }

        public record LoginRequest([Required] string Login, [Required] string Password);

        public record RegisterRequest(
            [Required] string Login,
            [Required] string Password);

        public record TokenPair(string AccessToken, string RefreshToken);

        [HttpPost("login")]
        public ActionResult<TokenPair> Login([FromBody] LoginRequest loginRequest)
        {
            var users = _userRepository.GetAll();

            var user = users.FirstOrDefault(u => u.Login == loginRequest.Login);

            if (user == null)
                return Unauthorized("Username is incorrect");

            if (user.Password != loginRequest.Password)
                return Unauthorized("Password is incorrect");

            string refreshToken = Guid.NewGuid().ToString();

            _userRepository.SetRefreshToken(refreshToken, user.Id);

            string authToken = _jwtCreator.Create(user.Role, user.Id);

            return new TokenPair(authToken, refreshToken);
        }

        [Authorize]
        [HttpGet("check")]
        public ActionResult<string> CheckLogin()
        {
            string userName = HttpContext.User.Identity!.Name!;
            return Ok(userName);

        }

        [HttpPost("register")]
        public ActionResult<TokenPair> Register([FromBody] RegisterRequest registerRequest)
        {
            User newUser = new()
            {

                Login = registerRequest.Login,
                Password = registerRequest.Password,
                Role = "user"
            };
            int? result = _userRepository.Create(newUser);

            if (result == null)
                return BadRequest("User cannot be created");

            newUser.Id = result.Value;

            string refreshToken = Guid.NewGuid().ToString();

            _userRepository.SetRefreshToken(refreshToken, newUser.Id);

            string authToken = _jwtCreator.Create(newUser.Role, newUser.Id);

            return new TokenPair(authToken, refreshToken);
        }

        [HttpGet("refresh/{refreshToken}")]
        public ActionResult<TokenPair> Refresh([FromRoute] string refreshToken)
        {
            var users = _userRepository.GetAll();

            var user = users.FirstOrDefault(u => u.RefreshToken == refreshToken);

            if (user == null)
                return Unauthorized("Refresh token is incorrect");

            string newRefreshToken = Guid.NewGuid().ToString();

            _userRepository.SetRefreshToken(newRefreshToken, user.Id);

            string authToken = _jwtCreator.Create(user.Role, user.Id);

            return new TokenPair(authToken, newRefreshToken);
        }

        [Authorize]
        [HttpGet]
        public ActionResult<List<User>> GetAllUsers()
        {
            return _userRepository.GetAll().ToList();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{userId:int}")]
        public ActionResult DeleteUser([FromRoute] int userId)
        {
            if (_userRepository.Delete(userId))
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}