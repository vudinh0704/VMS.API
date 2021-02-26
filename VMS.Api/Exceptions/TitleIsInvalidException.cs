namespace VMS.Api.Exceptions
{
    public class TitleIsInvalidException : BaseException
    {
        public TitleIsInvalidException() : base("title_is_invalid!", "the maximum length of title is 100 characters")
        {

        }
    }
}