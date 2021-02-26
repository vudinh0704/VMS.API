namespace VMS.Api.Exceptions
{
    public class ForbiddenException : BaseException
    {
        public ForbiddenException() : base("forbidden", "you do not have authority to execute this request")
        {

        }
    }
}