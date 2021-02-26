using System;
using System.ComponentModel.DataAnnotations;

namespace VMS.Core.Entities
{
    public class Post
    {
        [Key]
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

        public int Status { get; set; } = 0; // 0 - disapproved, 1 - approved

        public bool IsDeleted { get; set; } = false;

        public string SearchContent { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}