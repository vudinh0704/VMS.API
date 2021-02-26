using System;
using System.Net;

namespace VMS.Api.Exceptions
{
    public class BaseException : Exception
    {
        public string Code { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public BaseException(string code, string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(message)
        {
            Code = code;
            StatusCode = statusCode;
        }
    }
}