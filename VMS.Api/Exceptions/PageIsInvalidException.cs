namespace VMS.Api.Exceptions
{
    public class PageIsInvalidException : BaseException
    {
        public PageIsInvalidException() : base("page_is_invalid", "page must be greater than 0")
        {

        }
    }
}