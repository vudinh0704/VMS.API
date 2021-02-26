namespace VMS.Api.Exceptions
{
    public class ContentIsInvalidException : BaseException
    {
        public ContentIsInvalidException() : base("content_is_invalid", "the minimum length of content is 20 characters")
        {

        }
    }
}