using Microsoft.AspNetCore.Mvc;
using TutorialProjectAPI.Contexts;
using TutorialProjectAPI.Models;
using TutorialProjectAPI.Repositories;
using System.Collections.Generic;

namespace TutorialProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReplyController : ControllerBase
    {
        private readonly IdentifiableRepository<ReplyDB> _replyRepository;
        private readonly MainContext _context;

        public ReplyController(
            IdentifiableRepository<ReplyDB> replyRepository,
            MainContext context)
        {
            _replyRepository = replyRepository;
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateReplies([FromBody] List<ReplyCreateDTO> replies)
        {
            if (replies == null || replies.Count == 0)
                return BadRequest("No replies provided.");

            var createdReplies = new List<ReplyDB>();

            foreach (var dto in replies)
            {
                // Bad practice for now...
                // Check if Post exists
                if (!_context.Posts.Any(p => p.Id == dto.PostId))
                    return BadRequest($"Post with ID {dto.PostId} does not exist.");

                // Check if User exists
                if (!_context.Users.Any(u => u.Id == dto.UserId))
                    return BadRequest($"User with ID {dto.UserId} does not exist.");

                var reply = new ReplyDB
                {
                    Id = Guid.NewGuid(),
                    Body = dto.Body,
                    UserId = dto.UserId,
                    PostId = dto.PostId
                };
                _replyRepository.Insert(reply);
                createdReplies.Add(reply);
            }

            _context.SaveChanges();
            return Ok(createdReplies);
        }

        [HttpGet]
        public IActionResult GetAllReplies()
        {
            var replies = _replyRepository.GetAllAsReadOnly();
            return Ok(replies);
        }

        [HttpGet("{id}")]
        public IActionResult GetReplyById(Guid id)
        {
            var reply = _replyRepository.GetById(id);
            return Ok(reply);
        }

        [HttpPut("{id}")]
        public IActionResult EditReply(Guid id, [FromBody] ReplyDB reply)
        {
            _replyRepository.Update(reply);
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReply(Guid id)
        {
            var reply = _replyRepository.GetById(id);
            _replyRepository.Delete(reply);
            _context.SaveChanges();
            return Ok();
        }
    }
}