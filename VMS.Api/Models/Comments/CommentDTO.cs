using System;
using VMS.Core.Entities;

namespace VMS.Api.Models.Comments
{
    public class CommentDTO
    {
        public int CommentId { get; set; }

        public string AccountId { get; set; }

        public int PostId { get; set; }

        public string Content { get; set; }

        public int Likes { get; set; }

        public int Loves { get; set; }

        public int Dislikes { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LatestUpdate { get; set; }

        public static CommentDTO GetFrom(Comment comment)
        {
            return new CommentDTO
            {
                CommentId = comment.CommentId,
                AccountId = comment.AccountId,
                PostId = comment.PostId,
                Content = comment.Content,
                Likes = comment.Likes,
                Loves = comment.Loves,
                Dislikes = comment.Dislikes,
                CreatedDate = comment.CreatedDate,
                LatestUpdate = comment.UpdatedDate
            };
        }
    }
}