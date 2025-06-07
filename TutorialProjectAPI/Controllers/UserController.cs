using Microsoft.AspNetCore.Mvc;
using TutorialProjectAPI.Models;
using TutorialProjectAPI.Repositories;
using TutorialProjectAPI.Contexts;
using TutorialProjectAPI.DTOs;

namespace TutorialProjectAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IdentifiableRepository<UserDB> _userRepository;
        private readonly MainContext _context;

        public UserController(IdentifiableRepository<UserDB> userRepository, MainContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] CreateUserDTO userDto)
        {
            var user = new UserDB
            {
                Id = Guid.NewGuid(),
                Username = userDto.Username
            };

            _userRepository.Insert(user);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetAllAsReadOnly()
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username
                });

            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(Guid id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUsername(Guid id, [FromBody] CreateUserDTO dto)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                return NotFound();

            user.Username = dto.Username;
            _userRepository.Update(user);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(Guid id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                return NotFound();

            _userRepository.Delete(user);
            _context.SaveChanges();
            return NoContent();
        }
    }
}

