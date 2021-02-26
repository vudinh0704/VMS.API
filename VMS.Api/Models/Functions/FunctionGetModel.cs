namespace VMS.Api.Models.Functions
{
    public class FunctionGetModel : BaseGetModelWithPagination
    {
        public string Keyword { get; set; }

        public bool? IsActive { get; set; }
    }
}