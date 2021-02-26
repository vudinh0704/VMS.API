namespace VMS.Api.Exceptions
{
    public class PhoneIsInvalidException : BaseException
    {
        public PhoneIsInvalidException() : base("phone_is_invalid", "phone is invalid")
        {

        }
    }
}