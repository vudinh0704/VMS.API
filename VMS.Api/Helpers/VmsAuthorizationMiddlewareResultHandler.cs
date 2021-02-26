using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace VMS.Api.Helpers
{
    public class VmsAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler DefaultHandler = new AuthorizationMiddlewareResultHandler();

        public async Task HandleAsync(RequestDelegate requestDelegate, HttpContext context,
            AuthorizationPolicy authorizationPolicy, PolicyAuthorizationResult policyAuthorizationResult)
        {
            if (!policyAuthorizationResult.Succeeded)
            {
                context.Response.ContentType = "application/json";

                if (context.User.Identity.IsAuthenticated)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    await context.Response.WriteAsync("{ \"code\": \"forbidden\", \"message\": \"You do not have authority to execute this request!\" }");
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync("{ \"code\": \"invalid_token\", \"message\": \"Invalid token information!\" }");
                }

                return;
            }

            await DefaultHandler.HandleAsync(requestDelegate, context, authorizationPolicy, policyAuthorizationResult);
        }
    }
}