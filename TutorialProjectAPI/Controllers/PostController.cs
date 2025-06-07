using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutorialProjectAPI.Contexts;
using TutorialProjectAPI.DTOs;
using TutorialProjectAPI.Models;
using TutorialProjectAPI.Repositories;

namespace TutorialProjectAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly GenericRepository<Post> _postRepo;
        private readonly MainContext _context;

        public PostController(GenericRepository<Post> postRepo, MainContext context)
        {
            _postRepo = postRepo;
            _context = context;
        }

        [HttpPost]
        public IActionResult CreatePost([FromBody] CreatePostWithTempIdDTO dto)
        {
            var user = _context.Users.Find(dto.UserId);
            if (user == null) return BadRequest("User not found.");

            var post = new Post
            {
                Title = dto.Title,
                Body = dto.Body,
                UserId = dto.UserId
            };

            _postRepo.Insert(post);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetPostById), new { id = post.PostId }, post);
        }

        [HttpGet("{id}")]
        public IActionResult GetPostById(int id)
        {
            var post = _context.Posts
                .Include(p => p.User)
                .Include(p => p.Replies)
                .FirstOrDefault(p => p.PostId == id);

            if (post == null) return NotFound();

            return Ok(post);
        }

        [HttpGet]
        public IActionResult GetPosts()
        {
            var posts = _context.Posts
                .Include(p => p.User)
                .Select(p => new PostDto
                {
                    PostId = p.PostId,
                    Title = p.Title,
                    Body = p.Body,
                    Username = p.User.Username
                })
                .ToList();

            return Ok(posts);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePost(int id, [FromBody] CreatePostWithTempIdDTO dto)
        {
            var post = _context.Posts.Find(id);
            if (post == null) return NotFound();

            post.Title = dto.Title;
            post.Body = dto.Body;
            post.UserId = dto.UserId;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePost(int id)
        {
            var post = _context.Posts.Find(id);
            if (post == null) return NotFound();

            _postRepo.Delete(post);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
