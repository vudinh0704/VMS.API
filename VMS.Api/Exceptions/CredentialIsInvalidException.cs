using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace VMS.Api.Exceptions
{
    public class CredentialIsInvalidException : BaseException
    {
        public CredentialIsInvalidException() : base("credential_is_invalid", "credential is invalid")
        {
        }
    }
}