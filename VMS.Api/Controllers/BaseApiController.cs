using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using VMS.Api.Helpers;
using VMS.Api.Interfaces;
using VMS.Core.Interfaces.Repositories;
using static VMS.Api.AppSettings;

namespace VMS.Api.Controllers
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        internal readonly IFunctionRepository _functionRepository;

        public BaseApiController(IBaseApiService service)
        {
            _functionRepository = service.FunctionRepository;
        }

        public NotFoundObjectResult NotFoundResult()
        {
            return base.NotFound(new { code = "not_found", message = "Not found!" });
        }

        #region ===== CurrentAccount =====
        private bool? _isAuthenticated;
        private string _accountId;
        private string _userName;
        private List<string> _currentAccountFunctionCodes;

        public bool IsAuthenticated
        {
            get
            {
                return _isAuthenticated ??= HttpContext.User.Identity.IsAuthenticated;
            }
        }

        public string CurrentAccountId
        {
            get
            {
                if (IsAuthenticated)
                {
                    return _accountId ??= HttpContext.User.FindFirst(VmsClaimTypes.AccountId).Value;
                }

                return null;
            }
        }

        public string CurrentUserName
        {
            get
            {
                if (IsAuthenticated)
                {
                    return _userName ??= HttpContext.User.FindFirst(VmsClaimTypes.Name).Value;
                }

                return string.Empty;
            }
        }

        public List<string> GetCurrentAccountFunctionCodes()
        {
            if (_currentAccountFunctionCodes == null)
            {
                var ids = HttpContext.User.FindFirst(VmsClaimTypes.FunctionIds).Value.SplitByCommonChars().Select(item => Convert.ToInt32(item)).ToList();
                var list = _functionRepository.GetFunctionsByIdsAsync(ids).GetAwaiter().GetResult();
                _currentAccountFunctionCodes = list.Select(item => item.Code).ToList();
            }

            return _currentAccountFunctionCodes;
        }
        #endregion
    }
}