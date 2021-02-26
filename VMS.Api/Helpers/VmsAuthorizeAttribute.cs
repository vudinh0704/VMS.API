using Microsoft.AspNetCore.Authorization;

namespace VMS.Api.Helpers
{
    internal class VmsAuthorizeAttribute : AuthorizeAttribute
    {
        const string POLICY_PREFIX = "Vms";

        public string FunctionCodes
        {
            get
            {
                return Policy == null ? string.Empty : Policy[POLICY_PREFIX.Length..];
            }
            set
            {
                Policy = $"{POLICY_PREFIX}{value}";
            }
        }
    }
}