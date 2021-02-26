namespace VMS.Api.Exceptions
{
    public class IsRequiredException : BaseException
    {
        /// <summary>
        /// Use for 400 error status code
        /// </summary>
        public IsRequiredException(string entity) : base($"{ entity }_is_required", $"{ entity } is required")
        {

        }
    }
}