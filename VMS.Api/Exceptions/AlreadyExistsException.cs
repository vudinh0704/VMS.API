namespace VMS.Api.Exceptions
{
    public class AlreadyExistsException : BaseException
    {
        /// <summary>
        /// Use for 400 error status code
        /// </summary>
        public AlreadyExistsException(string entity) : base($"{ entity }_already_exists", $"{ entity } already exists")
        {

        }
    }
}