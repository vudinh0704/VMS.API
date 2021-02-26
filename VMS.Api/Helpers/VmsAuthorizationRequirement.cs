using Microsoft.AspNetCore.Authorization;

namespace VMS.Api.Helpers
{
    public class VmsAuthorizationRequirement : IAuthorizationRequirement
    {
        public string FunctionCodes { get; }

        public VmsAuthorizationRequirement(string functionCodes)
        {
            FunctionCodes = functionCodes;
        }
    }
}