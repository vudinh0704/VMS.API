namespace VMS.Api.Exceptions
{
    public class NotFound400Exception : BaseException
    {
        /// <summary>
        /// Use for 400 error status code
        /// </summary>
        public NotFound400Exception(string entity) : base($"{ entity }_not_found", $"{ entity } not found")
        {

        }
    }
}