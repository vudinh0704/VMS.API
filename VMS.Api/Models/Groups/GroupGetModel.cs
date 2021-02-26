namespace VMS.Api.Models.Groups
{
    public class GroupGetModel : BaseGetModelWithPagination
    {
        public string Keyword { get; set; }
    }
}