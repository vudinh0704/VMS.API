namespace VMS.Api.Exceptions
{
    public class PasswordIsInvalidException : BaseException
    {
        public PasswordIsInvalidException() : base("password_is_invalid", "password lengths range from 8 to 20 characters")
        {

        }
    }
}