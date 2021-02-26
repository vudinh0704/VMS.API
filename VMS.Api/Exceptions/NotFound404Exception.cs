namespace VMS.Api.Exceptions
{
    public class NotFound404Exception : BaseException
    {
        /// <summary>
        /// Use for 404 error status code
        /// </summary>
        public NotFound404Exception(string entity) : base($"{ entity }_not_found", $"entity not found", System.Net.HttpStatusCode.NotFound)
        {
        }
    }
}