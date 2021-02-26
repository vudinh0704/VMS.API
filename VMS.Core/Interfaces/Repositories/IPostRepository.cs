using System;
using System.Threading.Tasks;
using VMS.Core.Entities;
using VMS.Core.Helpers;

namespace VMS.Core.Interfaces.Repositories
{
    public interface IPostRepository
    {
        public Task<bool> AnyByIdAsync(int postId);

        public Task<bool> AnyByTitleAsync(string title);

        public Task<Comment> CreateCommentAsync(Comment comment);

        public Task<Post> CreatePostAsync(Post post);

        public Task<Comment> GetCommentByIdAsync(int commentId);

        public Task<Post> GetPostByIdAsync(int postId);

        public Task<ItemList<Post>> GetPostsAsync(string keyword, DateTime? startDate, DateTime? endDate, int page, int pageSize);        

        public Task UpdateCommentAsync(Comment comment);

        public Task UpdatePostAsync(Post post);
    }
}