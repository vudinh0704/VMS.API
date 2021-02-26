namespace VMS.Api.Exceptions
{
    public class EmailIsInvalidException : BaseException
    {
        public EmailIsInvalidException() : base("email_is_invalid", "email is invalid")
        {

        }
    }
}