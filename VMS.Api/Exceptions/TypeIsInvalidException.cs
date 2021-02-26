namespace VMS.Api.Exceptions
{
    public class TypeIsInvalidException : BaseException
    {
        public TypeIsInvalidException() : base("type_is_invalid", "type is invalid")
        {

        }
    }
}