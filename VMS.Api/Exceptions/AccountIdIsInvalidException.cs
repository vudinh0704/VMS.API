namespace VMS.Api.Exceptions
{
    public class AccountIdIsInvalidException : BaseException
    {
        public AccountIdIsInvalidException() : base("accountId_is_invalid", "the maximum length of accountId is 20 characters")
        {

        }
    }
}