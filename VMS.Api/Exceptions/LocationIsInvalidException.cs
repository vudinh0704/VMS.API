namespace VMS.Api.Exceptions
{
    public class LocationIsInvalidException : BaseException
    {
        public LocationIsInvalidException() : base("location_is_invalid", "the maximum length of location is 100 characters")
        {

        }
    }
}