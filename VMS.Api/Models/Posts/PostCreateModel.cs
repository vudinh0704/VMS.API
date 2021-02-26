namespace VMS.Api.Models.Posts
{
    public class PostCreateModel
    {
        public string Thumbnail { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Content { get; set; }

        public string Tags { get; set; }
    }
}