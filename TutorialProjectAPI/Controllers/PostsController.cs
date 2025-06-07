using Microsoft.AspNetCore.Mvc;
using TutorialProjectAPI.Models;
using TutorialProjectAPI.Repositories;

namespace TutorialProjectAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IIdentifiableRepository<PostDB> _posts;
        private readonly IIdentifiableRepository<ReplyDB> _replies;

        public PostsController(IIdentifiableRepository<PostDB> posts,
                               IIdentifiableRepository<ReplyDB> replies)
        {
            _posts = posts;
            _replies = replies;
        }

        // ───────────────────────────────────────────────────────────────────
        // GET: api/posts
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _posts.GetAllAsync());

        // ───────────────────────────────────────────────────────────────────
        // GET: api/posts/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var post = await _posts.GetByIdAsync(id);
            return post is null ? NotFound() : Ok(post);
        }

        // ───────────────────────────────────────────────────────────────────
        // POST: api/posts               (single post, optional replies)
        [HttpPost]
        public async Task<IActionResult> Create(PostCreateDto dto)
        {
            var post = new PostDB
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                Body = dto.Body
            };

            if (dto.Replies?.Any() == true)
            {
                post.Replies = dto.Replies.Select(r => new ReplyDB
                {
                    Id = Guid.NewGuid(),
                    Body = r.Body,
                    UserId = r.UserId,
                    PostId = post.Id
                }).ToList();
            }

            await _posts.AddAsync(post);
            await _posts.SaveAsync();

            return CreatedAtAction(nameof(Get), new { id = post.Id }, post);
        }

        // ───────────────────────────────────────────────────────────────────
        // POST: api/posts/bulk          (array of posts with nested replies)
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkCreate(List<PostCreateDto> list)
        {
            foreach (var dto in list)
            {
                var p = new PostDB
                {
                    Id = Guid.NewGuid(),
                    UserId = dto.UserId,
                    Body = dto.Body,
                    Replies = dto.Replies?.Select(r => new ReplyDB
                    {
                        Id = Guid.NewGuid(),
                        Body = r.Body,
                        UserId = r.UserId
                    }).ToList() ?? new List<ReplyDB>()
                };

                // ensure PostId is set on each reply
                foreach (var reply in p.Replies)
                    reply.PostId = p.Id;

                await _posts.AddAsync(p);
            }

            await _posts.SaveAsync();
            return Ok();
        }

        // ───────────────────────────────────────────────────────────────────
        // PUT: api/posts/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, PostCreateDto dto)
        {
            var post = await _posts.GetByIdAsync(id);
            if (post is null) return NotFound();

            post.Body = dto.Body;
            await _posts.SaveAsync();
            return NoContent();
        }

        // ───────────────────────────────────────────────────────────────────
        // DELETE: api/posts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var post = await _posts.GetByIdAsync(id);
            if (post is null) return NotFound();

            _posts.Delete(post);
            await _posts.SaveAsync();
            return NoContent();
        }
    }
}
