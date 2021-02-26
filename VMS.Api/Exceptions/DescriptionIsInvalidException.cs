namespace VMS.Api.Exceptions
{
    public class DescriptionIsInvalidException : BaseException
    {
        public DescriptionIsInvalidException() : base("description_is_invalid", "the minimum length of description is 20 characters")
        {

        }
    }
}