namespace VMS.Api.Exceptions
{
    public class PasswordIsIncorrectException : BaseException
    {
        public PasswordIsIncorrectException() : base("password_is_incorrect", "password is incorrect")
        {

        }
    }
}