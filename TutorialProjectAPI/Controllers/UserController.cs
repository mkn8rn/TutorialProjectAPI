using Microsoft.AspNetCore.Mvc;
using TutorialProjectAPI.Models;
using TutorialProjectAPI.Repositories;
using TutorialProjectAPI.Contexts;


namespace TutorialProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IIdentifiableRepository<UserDB> _userRepository;
        private readonly MainContext _context;

        public UserController(IIdentifiableRepository<UserDB> userRepository, MainContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _userRepository.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserDB user)
        {
            user.Id = Guid.NewGuid();
            await _userRepository.AddAsync(user);
            await _userRepository.SaveAsync();
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UserDB updatedUser)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return NotFound();

            user.Username = updatedUser.Username;
            _userRepository.Update(user);
            await _userRepository.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return NotFound();

            _userRepository.Delete(user);
            await _userRepository.SaveAsync();

            return NoContent();
        }
    }
}
