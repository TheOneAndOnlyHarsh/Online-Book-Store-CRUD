using BookStore.Model.DTO;
using BookStore.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var loginResponse = await _userRepository.Login(loginRequestDTO);

            if (loginResponse == null)
            {
                return Unauthorized("Invalid name or password");
            }

            return Ok(loginResponse);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register( [FromBody] RegistrationRequestDTO registrationRequestDTO)
        {
            if (!_userRepository.isUniqueUser(registrationRequestDTO.Name))
            {
                return BadRequest("User with the same name already exists");
            }

            var registeredUser = await _userRepository.Register(registrationRequestDTO);

            return StatusCode(201, registeredUser);
        }
    }
}
