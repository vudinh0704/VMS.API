using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VMS.Api.Exceptions;
using VMS.Api.Helpers;
using VMS.Api.Interfaces;
using VMS.Api.Models.Comments;
using VMS.Api.Models.Posts;
using VMS.Core.Entities;
using VMS.Core.Interfaces.Repositories;

namespace VMS.Api.Controllers
{
    [Route("posts")]
    [ApiController]
    public class PostsController : BaseApiController
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPostRepository _postRepository;

        public PostsController(IAccountRepository accountRepository, IPostRepository postRepository, IBaseApiService service) : base(service)
        {
            _accountRepository = accountRepository;
            _postRepository = postRepository;
        }

        /// <summary>
        /// POST /posts/{postId}/comments
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("{postId:int}/comments")]
        [VmsAuthorize(FunctionCodes = "Comment_Full, Comment_Create")]
        public async Task<IActionResult> CreateCommentAsync([FromRoute] int postId, [FromBody] CommentCreateModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Content))
            {
                throw new IsRequiredException("content");
            }

            if (model.Content.Length < 20)
            {
                throw new ContentIsInvalidException();
            }

            if (!await _postRepository.AnyByIdAsync(postId))
            {
                throw new NotFound404Exception("post");
            }

            DateTime now = DateTime.Now;

            var comment = new Comment
            {
                AccountId = CurrentAccountId,
                PostId = postId,
                Content = model.Content,
                CreatedDate = now,
                UpdatedDate = now
            };

            await _postRepository.CreateCommentAsync(comment);

            return Ok(CommentDTO.GetFrom(comment));
        }

        /// <summary>
        /// POST /posts
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("")]
        [VmsAuthorize(FunctionCodes = "Post_Full, Post_Create")]
        public async Task<IActionResult> CreatePostAsync([FromBody] PostCreateModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Content))
            {
                throw new IsRequiredException("content");
            }

            if (model.Content.Length < 20)
            {
                throw new ContentIsInvalidException();
            }

            if (string.IsNullOrWhiteSpace(model.Title))
            {
                throw new IsRequiredException("title");
            }

            if (model.Title.Length > 100)
            {
                throw new TitleIsInvalidException();
            }

            if (await _postRepository.AnyByTitleAsync(model.Title))
            {
                throw new AlreadyExistsException("title");
            }

            DateTime now = DateTime.Now;

            var post = new Post
            {
                AccountId = CurrentAccountId,
                Thumbnail = model.Thumbnail,
                Title = model.Title,
                Author = model.Author,
                Content = model.Content,
                Tags = model.Tags,
                CreatedDate = now,
                UpdatedDate = now
            };

            await _postRepository.CreatePostAsync(post);

            return Ok(PostDTO.GetFrom(post));
        }

        /// <summary>
        /// DELETE /posts/{postId}/comments/{commentId}
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpDelete, Route("{postId:int}/comments/{commentId:int}")]
        [VmsAuthorize(FunctionCodes = "Comment_Full, Comment_Delete_All, Comment_Delete")]
        public async Task<IActionResult> DeleteCommentAsync([FromRoute] int postId, [FromRoute] int commentId)
        {
            var currentFunctionCodes = GetCurrentAccountFunctionCodes();
            var comment = await _postRepository.GetCommentByIdAsync(commentId);

            if (comment == null)
            {
                throw new NotFound404Exception("comment");
            }

            if (!currentFunctionCodes.Contains("Comment_Full"))
            {
                if (!currentFunctionCodes.Contains("Comment_Delete_All"))
                {
                    if (CurrentAccountId != comment.AccountId)
                    {
                        throw new ForbiddenException();
                    }
                }                

                var currentAccount = await _accountRepository.GetAccountByIdAsync(CurrentAccountId);
                var account = await _accountRepository.GetAccountByIdAsync(comment.AccountId);

                if (currentAccount.GroupId > account.GroupId)
                {
                    throw new ForbiddenException(); // the lower the group id, the higher the authority; can only delete the group with authority lower than the current group
                }
            }

            if (!await _postRepository.AnyByIdAsync(postId))
            {
                throw new NotFound404Exception("post");
            }            

            comment.IsDeleted = true;
            comment.UpdatedDate = DateTime.Now;

            await _postRepository.UpdateCommentAsync(comment);

            return Ok(CommentDTO.GetFrom(comment));
        }

        /// <summary>
        /// DELETE /posts/{postId}
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpDelete, Route("{postId:int}")]
        [VmsAuthorize(FunctionCodes = "Post_Full, Post_Delete_All, Post_Delete")]
        public async Task<IActionResult> DeletePostAsync([FromRoute] int postId)
        {
            var currentFunctionCodes = GetCurrentAccountFunctionCodes();
            var post = await _postRepository.GetPostByIdAsync(postId);

            if (post == null)
            {
                throw new NotFound404Exception("post");

            }

            if (!currentFunctionCodes.Contains("Post_Full"))
            {
                if (!currentFunctionCodes.Contains("Post_Delete_All"))
                {
                    if (CurrentAccountId != post.AccountId)
                    {
                        throw new ForbiddenException();
                    }
                }                

                var currentAccount = await _accountRepository.GetAccountByIdAsync(CurrentAccountId);
                var account = await _accountRepository.GetAccountByIdAsync(post.AccountId);

                if (currentAccount.GroupId > account.GroupId)
                {
                    throw new ForbiddenException(); // the lower the group id, the higher the authority; can only delete the group with authority lower than the current group
                }
            }

            post.IsDeleted = true;            
            post.UpdatedDate = DateTime.Now;

            await _postRepository.UpdatePostAsync(post);

            return Ok(PostDTO.GetFrom(post));
        }        

        /// <summary>
        /// GET /posts/{postId}
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet, Route("{postId:int}")]
        public async Task<IActionResult> GetPostByIdAsync([FromRoute] int postId)
        {
            var post = await _postRepository.GetPostByIdAsync(postId);

            if (post == null)
            {
                throw new NotFound404Exception("post");
            }

            return Ok(PostDTO.GetFrom(post));
        }

        /// <summary>
        /// GET /posts
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet, Route("")]
        public async Task<IActionResult> GetPostsAsync([FromQuery] PostGetModel model)
        {
            model.Validate();

            var list = await _postRepository.GetPostsAsync(model.Keyword, model.StartDate, model.EndDate, model.Page, model.PageSize);

            if (list.Items.Count == 0)
            {
                throw new NotFound404Exception("page");
            }

            return Ok(PostList.GetFrom(list));
        }

        /// <summary>
        /// PUT /posts/{postId}/comments/{commentId}
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="commentId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut, Route("{postId:int}/comments/{commentId:int}")]
        [VmsAuthorize(FunctionCodes = "Comment_Full, Comment_Modify")]
        public async Task<IActionResult> UpdateCommentAsync([FromRoute] int postId, [FromRoute]  int commentId, [FromBody] CommentUpdateModel model)
        {
            var comment = await _postRepository.GetCommentByIdAsync(commentId);

            if (!await _postRepository.AnyByIdAsync(postId))
            {
                throw new NotFound404Exception("post");
            }

            if (comment == null)
            {
                throw new NotFound404Exception("comment");
            }

            if (string.IsNullOrWhiteSpace(model.Content))
            {
                throw new IsRequiredException("content");
            }

            if (model.Content.Length < 20)
            {
                throw new ContentIsInvalidException();
            }
            
            // bind data
            comment.Content = model.Content;
            comment.UpdatedDate = DateTime.Now;

            await _postRepository.UpdateCommentAsync(comment);

            return Ok(CommentDTO.GetFrom(comment));
        }

        /// <summary>
        /// PUT /posts/{postId}
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut, Route("{postId:int}")]
        [VmsAuthorize(FunctionCodes = "Post_Full, Post_Modify")]
        public async Task<IActionResult> UpdatePostAsync([FromRoute] int postId, [FromBody] PostUpdateModel model)
        {
            var post = await _postRepository.GetPostByIdAsync(postId);

            if (post == null)
            {
                throw new NotFound404Exception("post");
            }

            if (string.IsNullOrWhiteSpace(model.Content))
            {
                throw new IsRequiredException("content");
            }

            if (model.Content.Length < 20)
            {
                throw new ContentIsInvalidException();
            }

            if (string.IsNullOrWhiteSpace(model.Title))
            {
                throw new IsRequiredException("title");
            }

            if (await _postRepository.AnyByTitleAsync(model.Title) && !post.Title.Equals(model.Title))
            {
                throw new AlreadyExistsException("title");
            }

            if (model.Title.Length > 100)
            {
                throw new TitleIsInvalidException();
            }

            // bind data
            post.Thumbnail = model.Thumbnail;
            post.Title = model.Title;
            post.Author = model.Author;
            post.Content = model.Content;
            post.Tags = model.Tags;
            post.UpdatedDate = DateTime.Now;

            await _postRepository.UpdatePostAsync(post);

            return Ok(PostDTO.GetFrom(post));
        }
    }
}