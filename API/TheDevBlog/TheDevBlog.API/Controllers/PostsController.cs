using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheDevBlog.API.Data;
using TheDevBlog.API.Models.DTO;
using TheDevBlog.API.Models.Entities;

namespace TheDevBlog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : Controller
    {
        private readonly TheDevBlogDbContext dbContext;

        public PostsController(TheDevBlogDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await dbContext.Posts.ToListAsync();

            return Ok(posts);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetPostById")]
        public async Task<IActionResult> GetPostById(Guid id)
        {
            var post = await dbContext.Posts.FirstOrDefaultAsync(x => x.Id == id);

            if (post != null)
            {
                return Ok(post);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(AddPostRequest addPostRequest)
        {
            // convert DTO to entity

            var post = new Post()
            {
                Title = addPostRequest.Title,
                Content = addPostRequest.Content,
                Author = addPostRequest.Author,
                FeaturedImageUrl = addPostRequest.Title,
                PublishDate = addPostRequest.PublishDate,
                UpdatedDate = addPostRequest.UpdatedDate,
                Summary = addPostRequest.Summary,
                UrlHandle = addPostRequest.UrlHandle,
                Visible = addPostRequest.Visible,
            };

            post.Id = Guid.NewGuid();
            await dbContext.Posts.AddAsync(post);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, post);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdatePost([FromRoute]Guid id, UpdatePostRequest updatePostRequest)
        {
            var post = new Post()
            {
                Title = updatePostRequest.Title,
                Content = updatePostRequest.Content,
                Author = updatePostRequest.Author,
                FeaturedImageUrl = updatePostRequest.Title,
                PublishDate = updatePostRequest.PublishDate,
                UpdatedDate = updatePostRequest.UpdatedDate,
                Summary = updatePostRequest.Summary,
                UrlHandle = updatePostRequest.UrlHandle,
                Visible = updatePostRequest.Visible,
            };

            // check if exists
            var existingPost = await dbContext.Posts.FindAsync(id);

            if(existingPost != null)
            {
                post.Id = existingPost.Id;
            }
        }
    }
}
