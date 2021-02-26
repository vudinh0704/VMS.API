using System;
using System.ComponentModel.DataAnnotations;

namespace VMS.Core.Entities
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        public string AccountId { get; set; }

        public int PostId { get; set; }

        public string Content { get; set; }

        public string BlockReason { get; set; }

        public int Likes { get; set; }

        public int Loves { get; set; }

        public int Dislikes { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}