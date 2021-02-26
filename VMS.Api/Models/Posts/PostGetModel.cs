namespace VMS.Api.Models.Posts
{
    public class PostGetModel : BaseGetModelWithDateAndPagination
    {
        public string Keyword { get; set; }
    }
}