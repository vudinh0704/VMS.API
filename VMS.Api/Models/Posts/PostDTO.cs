using System;
using VMS.Core.Entities;

namespace VMS.Api.Models.Posts
{
    public class PostDTO
    {
        public int PostId { get; set; }

        public string AccountId { get; set; }

        public string Thumbnail { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Content { get; set; }

        public string Tags { get; set; }

        public int Likes { get; set; }

        public int Loves { get; set; }

        public int Dislikes { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LatestUpdate { get; set; }

        public static PostDTO GetFrom(Post post)
        {
            return new PostDTO
            {
                PostId = post.PostId,
                AccountId = post.AccountId,
                Thumbnail = post.Thumbnail,
                Title = post.Title,
                Author = post.Author,
                Content = post.Content,
                Tags = post.Tags,
                Likes = post.Likes,
                Loves = post.Loves,
                Dislikes = post.Dislikes,
                CreatedDate = post.CreatedDate,
                LatestUpdate = post.UpdatedDate
            };
        }
    }
}