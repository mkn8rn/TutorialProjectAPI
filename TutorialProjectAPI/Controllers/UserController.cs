using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TutorialProjectAPI.Contexts;
using TutorialProjectAPI.Models;
using TutorialProjectAPI.Repositories;

namespace TutorialProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IdentifiableRepository<UserDB> _userRepository;
        private readonly MainContext _context;

        public UserController(
            IdentifiableRepository<UserDB> userRepository,
            MainContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserCreateDTO userDto)
        {
            if (userDto == null)
                return BadRequest("No user data provided.");

            Guid? imageId = null;

            // Handle image creation or reference
            if (!string.IsNullOrWhiteSpace(userDto.ProfileImageBase64))
            {
                if (userDto.ProfileImageBase64.Length > 136000)
                    return BadRequest("Profile image exceeds 100kb base64 size limit.");

                var image = new ImageDB
                {
                    Id = Guid.NewGuid(),
                    Base64Image = userDto.ProfileImageBase64
                };
                _context.Images.Add(image);
                _context.SaveChanges();
                imageId = image.Id;
            }
            else if (userDto.ProfileImageId.HasValue)
            {
                if (!_context.Images.Any(i => i.Id == userDto.ProfileImageId.Value))
                    return BadRequest($"Image with ID {userDto.ProfileImageId.Value} does not exist.");
                imageId = userDto.ProfileImageId.Value;
            }

            var user = new UserDB
            {
                Id = Guid.NewGuid(),
                Username = userDto.Username,
                ProfileImageId = imageId
            };

            _userRepository.Insert(user);
            _context.SaveChanges();

            return Ok(user);
        }

        [HttpPut("{id}")]
        public IActionResult EditUser(Guid id, [FromBody] UserDB user)
        {

            _userRepository.Update(user);
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(Guid id)
        {
            var user = _userRepository.GetById(id);

            _userRepository.Delete(user);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userRepository.GetAllAsReadOnly();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(Guid id)
        {
            var user = _userRepository.GetById(id);


            return Ok(user);
        }
    }
}