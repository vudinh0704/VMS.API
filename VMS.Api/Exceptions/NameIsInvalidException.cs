namespace VMS.Api.Exceptions
{
    public class NameIsInvalidException : BaseException
    {
        public NameIsInvalidException() : base("name_is_invalid", "the maximum length of name is 50 characters")
        {

        }
    }
}