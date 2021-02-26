using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;
using VMS.Core.Interfaces.Repositories;
using static VMS.Api.AppSettings;

namespace VMS.Api.Helpers
{
    public class VmsAuthorizationHandler : AuthorizationHandler<VmsAuthorizationRequirement>
    {
        private readonly IFunctionRepository _functionRepository;

        public VmsAuthorizationHandler(IFunctionRepository functionRepository)
        {
            _functionRepository = functionRepository;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, VmsAuthorizationRequirement requirement)
        {
            try
            {
                var accountId = context.User.FindFirst(c => c.Type == VmsClaimTypes.AccountId && c.Issuer == AppSettings.JwtIssuer).Value;

                // todo: checking banned token
                if (requirement.FunctionCodes == string.Empty)
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }

                var functionIds = context.User.FindFirst(c => c.Type == VmsClaimTypes.FunctionIds && c.Issuer == AppSettings.JwtIssuer)
                                            .Value.SplitByCommonChars();

                if (functionIds.Length == 0)
                {
                    context.Fail();
                    return Task.CompletedTask;
                }

                var apiFunctionCodes = requirement.FunctionCodes.SplitByCommonChars();

                foreach (var apiFunctionCode in apiFunctionCodes)
                {
                    var functionCode = _functionRepository.GetFunctionByCodeAsync(apiFunctionCode).GetAwaiter().GetResult();

                    if (functionCode != null && functionIds.Contains(functionCode.FunctionId.ToString()))
                    {
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }
                }

                context.Fail();
            }
            catch (Exception)
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }

        public override Task HandleAsync(AuthorizationHandlerContext context)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            return base.HandleAsync(context);
        }
    }
}