namespace VMS.Api.Exceptions
{
    public class PageSizeIsInvalidException : BaseException
    {
        public PageSizeIsInvalidException() : base("pageSize_is_invalid", "pageSize must be greater than 0")
        {

        }
    }
}