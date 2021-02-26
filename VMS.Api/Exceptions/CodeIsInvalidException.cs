namespace VMS.Api.Exceptions
{
    public class CodeIsInvalidException : BaseException
    {
        public CodeIsInvalidException() : base("code_is_invalid", "the maximum length of name is 20 characters")
        {

        }
    }
}