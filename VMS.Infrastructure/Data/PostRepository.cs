using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using VMS.Core.Entities;
using VMS.Core.Helpers;
using VMS.Core.Interfaces.Repositories;

namespace VMS.Infrastructure.Data
{
    public class PostRepository : BaseRepository<PostRepository>, IPostRepository
    {
        private readonly VMSDbContext _dbContext;

        public PostRepository(VMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AnyByIdAsync(int postId)
        {
            return await _dbContext.Posts.AnyAsync(item => item.PostId == postId);
        }

        public async Task<bool> AnyByTitleAsync(string title)
        {
            return await _dbContext.Posts.AnyAsync(item => item.Title == title);
        }

        public async Task<Comment> CreateCommentAsync(Comment comment)
        {
            await _dbContext.Comments.AddAsync(comment);
            await _dbContext.SaveChangesAsync();

            return comment;
        }

        public async Task<Post> CreatePostAsync(Post post)
        {
            await _dbContext.Posts.AddAsync(post);
            await _dbContext.SaveChangesAsync();

            return post;
        }

        public async Task<Comment> GetCommentByIdAsync(int commentId)
        {
            return await _dbContext.Comments.AsNoTracking().FirstOrDefaultAsync(item => item.CommentId == commentId);
        }

        public async Task<Post> GetPostByIdAsync(int postId)
        {
            return await _dbContext.Posts.AsNoTracking().FirstOrDefaultAsync(item => item.PostId == postId);
        }

        public async Task<ItemList<Post>> GetPostsAsync(string keyword, DateTime? startDate, DateTime? endDate, int page, int pageSize)
        {
            var result = new ItemList<Post> { Page = page, PageSize = pageSize };
            var posts = _dbContext.Posts.AsNoTracking();

            if (!string.IsNullOrEmpty(keyword))
            {
                posts = posts.Where(item => item.SearchContent.Contains(keyword));
            }

            endDate = endDate.Value.AddDays(1);

            posts = posts.Where(item => item.CreatedDate >= startDate.Value && item.CreatedDate < endDate.Value);

            posts = posts.OrderBy(item => item.CreatedDate);

            result.Total = await posts.CountAsync();
            result.Items = await GetList(posts, page, pageSize);

            return result;
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            _dbContext.Entry(comment).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdatePostAsync(Post post)
        {
            _dbContext.Entry(post).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}