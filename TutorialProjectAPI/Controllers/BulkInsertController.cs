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
    public class BulkInsertController : ControllerBase
    {
        private readonly GenericRepository<Post> _postRepo;
        private readonly GenericRepository<Reply> _replyRepo;
        private readonly MainContext _context;

        public BulkInsertController(GenericRepository<Post> postRepo, GenericRepository<Reply> replyRepo, MainContext context)
        {
            _postRepo = postRepo;
            _replyRepo = replyRepo;
            _context = context;
        }

        [HttpPost]
        public IActionResult BulkInsert([FromBody] BulkInsertRequestDTO dto)
        {
            var incomingPosts = dto.Posts
                .GroupBy(p => new
                {
                    Title = (p.Title ?? "").Trim().ToLower(),
                    Body = (p.Body ?? "").Trim().ToLower(),
                    p.UserId
                })
                .Select(g => g.First())
                .ToList();

            var userIds = incomingPosts.Select(p => p.UserId).Distinct().ToList();
            var existingPosts = _context.Posts
                .Where(p => userIds.Contains(p.UserId))
                .ToList();

            var tempIdToDbId = new Dictionary<int, int>();
            var postEntitiesToInsert = new List<Post>();

            foreach (var postDto in incomingPosts)
            {
                bool exists = existingPosts.Any(ep =>
                    ep.Title.Trim().ToLower() == postDto.Title.Trim().ToLower() &&
                    ep.Body.Trim().ToLower() == postDto.Body.Trim().ToLower() &&
                    ep.UserId == postDto.UserId);

                if (!exists)
                {
                    var post = new Post
                    {
                        Title = postDto.Title,
                        Body = postDto.Body,
                        UserId = postDto.UserId
                    };
                    postEntitiesToInsert.Add(post);
                }
            }

            if (postEntitiesToInsert.Any())
            {
                _postRepo.AddRange(postEntitiesToInsert);
                _context.SaveChanges();
            }

            foreach (var post in postEntitiesToInsert)
            {
                var dtoMatch = incomingPosts.First(p =>
                    p.Title.Trim().ToLower() == post.Title.Trim().ToLower() &&
                    p.Body.Trim().ToLower() == post.Body.Trim().ToLower() &&
                    p.UserId == post.UserId);

                tempIdToDbId[dtoMatch.TempId] = post.PostId;
            }

            var replyEntities = dto.Replies
                .Where(r => tempIdToDbId.ContainsKey(r.PostTempId))
                .Select(r => new Reply
                {
                    Text = r.Body,
                    UserId = r.UserId,
                    PostId = tempIdToDbId[r.PostTempId]
                })
                .ToList();

            if (replyEntities.Any())
            {
                _replyRepo.AddRange(replyEntities);
                _context.SaveChanges();
            }

            return Ok(new
            {
                PostsInserted = postEntitiesToInsert.Count,
                RepliesInserted = replyEntities.Count
            });
        }
    }
}
