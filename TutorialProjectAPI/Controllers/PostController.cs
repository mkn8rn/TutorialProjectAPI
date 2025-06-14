using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutorialProjectAPI.Contexts;
using TutorialProjectAPI.Models;
using TutorialProjectAPI.Repositories;

namespace TutorialProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IdentifiableRepository<PostDB> _postRepository;
        private readonly IdentifiableRepository<ReplyDB> _replyRepository;
        private readonly MainContext _context;

        public PostController(
            IdentifiableRepository<PostDB> postRepository,
            IdentifiableRepository<ReplyDB> replyRepository,
            MainContext context)
        {
            _postRepository = postRepository;
            _replyRepository = replyRepository;
            _context = context;
        }

        [HttpPost]
        public IActionResult CreatePosts([FromBody] List<PostWithRepliesDTO> posts)
        {
            if (posts == null || posts.Count == 0)
                return BadRequest("No posts provided.");

            var createdPosts = new List<PostDB>();

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                foreach (var postDto in posts)
                {
                    // Validate UserId
                    if (!_context.Users.Any(u => u.Id == postDto.UserId))
                        return BadRequest($"User with ID {postDto.UserId} does not exist.");

                    Guid? imageId = null;

                    // Handle image creation or reference
                    if (!string.IsNullOrWhiteSpace(postDto.ImageBase64))
                    {
                        if (postDto.ImageBase64.Length > 136000)
                            return BadRequest("Image exceeds 100kb base64 size limit.");

                        var image = new ImageDB
                        {
                            Id = Guid.NewGuid(),
                            Base64Image = postDto.ImageBase64
                        };
                        _context.Images.Add(image);
                        _context.SaveChanges();
                        imageId = image.Id;
                    }
                    else if (postDto.ImageId.HasValue)
                    {
                        // Check if the referenced image exists
                        if (!_context.Images.Any(i => i.Id == postDto.ImageId.Value))
                            return BadRequest($"Image with ID {postDto.ImageId.Value} does not exist.");
                        imageId = postDto.ImageId.Value;
                    }

                    var post = new PostDB
                    {
                        Id = Guid.NewGuid(),
                        Body = postDto.Body,
                        UserId = postDto.UserId,
                        ImageId = imageId,
                        Replies = new List<ReplyDB>()
                    };

                    _postRepository.Insert(post);
                    _context.SaveChanges(); // Save post to generate PostId

                    if (postDto.Replies != null)
                    {
                        foreach (var replyDto in postDto.Replies)
                        {
                            // Validate UserId for replies
                            if (!_context.Users.Any(u => u.Id == replyDto.UserId))
                                return BadRequest($"User with ID {replyDto.UserId} does not exist.");

                            var reply = new ReplyDB
                            {
                                Id = Guid.NewGuid(),
                                Body = replyDto.Body,
                                UserId = replyDto.UserId,
                                PostId = post.Id
                            };
                            _replyRepository.Insert(reply);
                        }
                    }

                    _context.SaveChanges(); // Save replies
                    createdPosts.Add(post);
                }

                transaction.Commit();
                return Ok(createdPosts);
            }
            catch
            {
                transaction.Rollback();
                return StatusCode(500, "An error occurred while creating posts.");
            }
        }

        [HttpGet]
        public IActionResult GetAllPosts()
        {
            var posts = _context.Posts
                .Include(p => p.Image)
                .Include(p => p.User)
                .Include(p => p.Replies)
                .AsNoTracking()
                .ToList();

            return Ok(posts);
        }

        [HttpGet("{id}")]
        public IActionResult GetPostById(Guid id)
        {
            var post = _context.Posts
                .Include(p => p.Image)
                .Include(p => p.User)
                .Include(p => p.Replies)
                .AsNoTracking()
                .FirstOrDefault(p => p.Id == id);

            if (post == null)
                return NotFound();

            return Ok(post);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePost(Guid id, [FromBody] PostWithRepliesDTO postDto)
        {
            var post = _context.Posts.Include(p => p.Replies).FirstOrDefault(p => p.Id == id);
            if (post == null)
                return NotFound();

            // Validate UserId
            if (!_context.Users.Any(u => u.Id == postDto.UserId))
                return BadRequest($"User with ID {postDto.UserId} does not exist.");

            post.Body = postDto.Body;
            post.UserId = postDto.UserId;

            if (!string.IsNullOrWhiteSpace(postDto.ImageBase64))
            {
                if (postDto.ImageBase64.Length > 136000)
                    return BadRequest("Image exceeds 100kb base64 size limit.");

                var image = new ImageDB
                {
                    Id = Guid.NewGuid(),
                    Base64Image = postDto.ImageBase64
                };
                _context.Images.Add(image);
                _context.SaveChanges();
                post.ImageId = image.Id;
            }
            else if (postDto.ImageId.HasValue)
            {
                if (!_context.Images.Any(i => i.Id == postDto.ImageId.Value))
                    return BadRequest($"Image with ID {postDto.ImageId.Value} does not exist.");
                post.ImageId = postDto.ImageId.Value;
            }
            else
            {
                post.ImageId = null;
            }

            _context.SaveChanges();
            return Ok(post);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePost(Guid id)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == id);
            if (post == null)
                return NotFound();

            _postRepository.Delete(post);
            _context.SaveChanges();
            return Ok();
        }
    }
}