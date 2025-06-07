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
    public class ReplyController : ControllerBase
    {
        private readonly GenericRepository<Reply> _replyRepo;
        private readonly MainContext _context;

        public ReplyController(GenericRepository<Reply> replyRepo, MainContext context)
        {
            _replyRepo = replyRepo;
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateReply([FromBody] CreateReplyWithTempPostIdDTO dto)
        {
            var user = _context.Users.Find(dto.UserId);
            if (user == null) return BadRequest("User not found.");

            var post = _context.Posts.Find(dto.PostTempId);
            if (post == null) return BadRequest("Post not found.");

            var reply = new Reply
            {
                Text = dto.Body,
                UserId = dto.UserId,
                PostId = post.PostId
            };

            _replyRepo.Insert(reply);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetReplyById), new { id = reply.CommentId }, reply);
        }

        [HttpGet("{id}")]
        public IActionResult GetReplyById(int id)
        {
            var reply = _context.Replies
                .Include(r => r.User)
                .Include(r => r.Post)
                .FirstOrDefault(r => r.CommentId == id);

            if (reply == null) return NotFound();

            return Ok(reply);
        }

        [HttpGet]
        public IActionResult GetReplies()
        {
            var replies = _context.Replies
                .Include(r => r.User)
                .Select(r => new ReplyDto
                {
                    CommentId = r.CommentId,
                    Text = r.Text,
                    Username = r.User.Username,
                    PostId = r.PostId
                })
                .ToList();

            return Ok(replies);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateReply(int id, [FromBody] CreateReplyWithTempPostIdDTO dto)
        {
            var reply = _context.Replies.Find(id);
            if (reply == null) return NotFound();

            reply.Text = dto.Body;
            reply.UserId = dto.UserId;
            reply.PostId = dto.PostTempId;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReply(int id)
        {
            var reply = _context.Replies.Find(id);
            if (reply == null) return NotFound();

            _replyRepo.Delete(reply);
            _context.SaveChanges();
            return NoContent();
        }
    }
}

