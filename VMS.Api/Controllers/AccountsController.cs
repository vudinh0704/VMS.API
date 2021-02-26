using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VMS.Api.Exceptions;
using VMS.Api.Helpers;
using VMS.Api.Interfaces;
using VMS.Api.Models.Accounts;
using VMS.Core;
using VMS.Core.Entities;
using VMS.Core.Interfaces.Repositories;
using static VMS.Api.AppSettings;

namespace VMS.Api.Controllers
{
    [Route("accounts")]
    [ApiController]
    public class AccountsController : BaseApiController
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IFunctionRepository _functionRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IWardRepository _wardRepository;

        public AccountsController(
            IAccountRepository accountRepository,
            IFunctionRepository functionRepository,
            IGroupRepository groupRepository,
            IWardRepository wardRepository,
            IBaseApiService service) : base(service)
        {
            _accountRepository = accountRepository;
            _functionRepository = functionRepository;
            _groupRepository = groupRepository;
            _wardRepository = wardRepository;
        }

        /// <summary>
        /// POST /accounts/login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody] AccountLoginModel model)
        {
            var account = await _accountRepository.GetAccountByIdAsync(model.AccountId);

            if (account == null)
            {
                throw new NotFound400Exception("account");
            }

            if (!await _accountRepository.AnyByAccountAsync(model.AccountId, model.Password))
            {
                throw new PasswordIsIncorrectException();
            }

            var permissions = await _groupRepository.GetPermissionsByGroupIdAsync(account.GroupId, 1, int.MaxValue);
            StringBuilder functionIds = new StringBuilder();
            List<string> functionCodes = new List<string>();

            if (permissions.Total > 0)
            {
                int functionId = permissions.Items[0].FunctionId;
                var functionCode = await _functionRepository.GetFunctionByIdAsync(functionId);

                functionIds.Append(functionId);
                functionCodes.Add(functionCode.Code);

                for (int i = 1; i < permissions.Items.Count; i++)
                {
                    functionId = permissions.Items[i].FunctionId;
                    functionCode = await _functionRepository.GetFunctionByIdAsync(functionId);

                    if (functionCode != null)
                    {
                        functionCodes.Add(functionCode.Code);
                        functionIds.Append(',').Append(permissions.Items[i].FunctionId);
                    }
                }
            }

            // create token
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.JwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(VmsClaimTypes.Name, account.Name),
                new Claim(VmsClaimTypes.AccountId, account.AccountId),
                new Claim(VmsClaimTypes.FunctionIds, functionIds.ToString())
            };

            var token = new JwtSecurityToken(AppSettings.JwtIssuer, null, claims, null, DateTime.Now.AddDays(AppSettings.JwtExpirationDays), credentials);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new AccountAuthenticateResponse { AccountId = account.AccountId, Token = tokenString, FunctionCodes = functionCodes });
        }

        /// <summary>
        /// GET accounts/authenticate
        /// </summary>
        /// <param name="authKey"></param>
        /// <returns></returns>
        [HttpGet, Route("authenticate")]
        public async Task<IActionResult> Authenticate(string authKey)
        {
            var authToken = new JwtSecurityToken(authKey);
            string authAccountId = authToken.Claims.First(item => item.Type == VmsClaimTypes.Name).Value;

            // get account info
            var account = await _accountRepository.GetAccountByIdAsync(authAccountId);

            if (account == null)
            {
                throw new CredentialIsInvalidException();
            }

            var permissions = await _groupRepository.GetPermissionsByGroupIdAsync(account.GroupId, 1, int.MaxValue);
            StringBuilder functionIds = new StringBuilder();
            List<string> functionCodes = new List<string>();

            if (permissions.Total > 0)
            {
                int functionId = permissions.Items[0].FunctionId;
                var functionCode = await _functionRepository.GetFunctionByIdAsync(functionId);

                functionIds.Append(functionId);
                functionCodes.Add(functionCode.Code);

                for (int i = 1; i < permissions.Items.Count; i++)
                {
                    functionId = permissions.Items[i].FunctionId;
                    functionCode = await _functionRepository.GetFunctionByIdAsync(functionId);

                    if (functionCode != null)
                    {
                        functionCodes.Add(functionCode.Code);
                        functionIds.Append(',').Append(permissions.Items[i].FunctionId);
                    }
                }
            }

            // create token
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.JwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(VmsClaimTypes.Name, account.Name),
                new Claim(VmsClaimTypes.AccountId, account.AccountId),
                new Claim(VmsClaimTypes.FunctionIds, functionIds.ToString())
            };

            var token = new JwtSecurityToken(AppSettings.JwtIssuer, null, claims, null, DateTime.Now.AddDays(AppSettings.JwtExpirationDays), credentials);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new AccountAuthenticateResponse { AccountId = account.AccountId, Token = tokenString, FunctionCodes = functionCodes });
        }

        /// <summary>
        /// PUT /accounts/{accountId}/groups
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut, Route("{accountId}/groups")]
        [VmsAuthorize(FunctionCodes = "Account_Full")]
        public async Task<IActionResult> AssignAccountAsync([FromRoute] string accountId, [FromBody] AccountAssignModel model)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);

            if (account == null)
            {
                throw new NotFound404Exception("account");
            }

            if (!await _groupRepository.AnyByIdAsync(model.GroupId))
            {
                throw new NotFound404Exception("group");
            }

            // bind date
            account.GroupId = model.GroupId;
            account.UpdatedDate = DateTime.Now;

            await _accountRepository.AssignAccountAsync(account);

            return Ok(AccountDTO.GetFrom(account));
        }

        /// <summary>
        /// PUT /accounts/{accountId}/ban
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpPut, Route("{accountId}/ban")]
        [VmsAuthorize(FunctionCodes = "Account_Full, Account_Modify_All")]
        public async Task<IActionResult> BanAccountAsync([FromRoute] string accountId)
        {
            var currentFunctionCodes = GetCurrentAccountFunctionCodes();
            var account = await _accountRepository.GetAccountByIdAsync(accountId);

            if (account == null)
            {
                throw new NotFound404Exception("account");
            }

            if (!currentFunctionCodes.Contains("Account_Full"))
            {
                var currentAccount = await _accountRepository.GetAccountByIdAsync(CurrentAccountId);                

                if (currentAccount.GroupId > account.GroupId)
                {
                    throw new ForbiddenException();
                }
            }            

            // bind data
            account.GroupId = 4; // group: visitor
            account.IsActive = false;

            await _accountRepository.UpdateAccountAsync(account);

            return Ok(AccountDTO.GetFrom(account));
        }

        /// <summary>
        /// POST /accounts
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("")]
        public async Task<IActionResult> CreateAccountAsync([FromBody] AccountCreateModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                throw new IsRequiredException("password");
            }

            if (model.Password.Length < 8 || model.Password.Length > 20)
            {
                throw new PasswordIsInvalidException();
            }

            if (model.Name != null)
            {
                if (model.Name.Length > 50)
                {
                    throw new NameIsInvalidException();
                }
            }            

            if (model.BirthDate.HasValue)
            {
                if (model.BirthDate.Value.Year < Constants.MinBirthDate.Year || model.BirthDate.Value.Year > DateTime.Now.Year - Constants.MinAge)
                {
                    throw new BirthDateIsInvalidException();
                }
            }            

            if (model.Email != null)
            {
                if (!model.Email.IsEmail())
                {
                    throw new EmailIsInvalidException();
                }

                if (await _accountRepository.AnyByEmailAsync(model.Email))
                {
                    throw new AlreadyExistsException("email");
                }
            }            

            if (model.Phone != null)
            {
                if (!model.Phone.IsMobile())
                {
                    throw new PhoneIsInvalidException();
                }

                if (await _accountRepository.AnyByPhoneAsync(model.Phone))
                {
                    throw new AlreadyExistsException("phone");
                }
            }            

            if (string.IsNullOrWhiteSpace(model.AccountId))
            {
                throw new IsRequiredException("accountId");
            }

            if (model.AccountId.Length > 20)
            {
                throw new AccountIdIsInvalidException();
            }

            if (await _accountRepository.AnyByIdAsync(model.AccountId))
            {
                throw new AlreadyExistsException("account");
            }

            if (model.WardId.HasValue)
            {
                if (!await _wardRepository.AnyByIdAsync(model.WardId.Value))
                {
                    throw new NotFound400Exception("ward");
                }
            }            

            var now = DateTime.Now;

            var account = new Account
            {
                AccountId = model.AccountId,
                GroupId = 4,
                WardId = model.WardId.HasValue ? model.WardId.Value : 0,
                Password = model.Password,
                Name = model.Name != null ? model.Name : null,
                Gender = model.Gender.HasValue ? model.Gender.Value : null,
                BirthDate = model.BirthDate.HasValue ? model.BirthDate : null,
                Address = model.Address != null ? model.Address : null,
                Email = model.Email != null ? model.Email : null,
                Phone = model.Phone != null ? model.Phone : null,
                CreatedDate = now,
                UpdatedDate = now
            };

            await _accountRepository.CreateAccountAsync(account);

            return Ok(AccountDTO.GetFrom(account));
        }

        /// <summary>
        /// DELETE /accounts/{accountId}
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpDelete, Route("{accountId}")]
        [VmsAuthorize(FunctionCodes = "Account_Full, Account_Delete_All, Account_Delete")]
        public async Task<IActionResult> DeleteAccountAsync([FromRoute] string accountId)
        {
            var currentFunctionCodes = GetCurrentAccountFunctionCodes();
            var account = await _accountRepository.GetAccountByIdAsync(accountId);

            if (account == null)
            {
                throw new NotFound404Exception("account");
            }

            if (!currentFunctionCodes.Contains("Account_Full"))
            {
                if (!currentFunctionCodes.Contains("Account_Delete_All"))
                {
                    if (accountId != CurrentAccountId)
                    {
                        throw new ForbiddenException();
                    }
                }

                var currentAccount = await _accountRepository.GetAccountByIdAsync(CurrentAccountId);

                if (currentAccount.GroupId > account.GroupId)
                {
                    throw new ForbiddenException(); // the lower the group id, the higher the authority; can only delete the group with authority lower than the current group
                }
            }            

            account.IsDeleted = true;
            account.UpdatedDate = DateTime.Now;

            await _accountRepository.UpdateAccountAsync(account);

            return Ok(AccountDTO.GetFrom(account));
        }

        /// <summary>
        /// GET /accounts/{accountId}
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet, Route("{accountId}")]
        [VmsAuthorize(FunctionCodes = "Account_Full, Account_Read_All, Account_Read")]
        public async Task<IActionResult> GetAccountByIdAsync([FromRoute] string accountId)
        {
            var currentFunctionCodes = GetCurrentAccountFunctionCodes();

            if (!currentFunctionCodes.Contains("Account_Full") && !currentFunctionCodes.Contains("Account_Read_All"))
            {
                if (accountId != CurrentAccountId)
                {
                    throw new ForbiddenException();
                }
            }

            var account = await _accountRepository.GetAccountByIdAsync(accountId);

            if (account == null)
            {
                throw new NotFound404Exception("account");
            }

            return Ok(AccountDTO.GetFrom(account));
        }

        /// <summary>
        /// GET /accounts
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet, Route("")]
        [VmsAuthorize(FunctionCodes = "Account_Full, Account_Read_All")]
        public async Task<IActionResult> GetAccountsAsync([FromQuery] AccountGetModel model)
        {
            model.Validate();

            var list = await _accountRepository.GetAccountsAsync(model.Keyword, model.Gender, model.IsActive, model.StartDate, model.EndDate, model.Page, model.PageSize);

            if (list.Items.Count == 0)
            {
                throw new NotFound404Exception("page");
            }

            return Ok(AccountList.GetFrom(list));
        }

        /// <summary>
        /// PUT /accounts/{accountId}/unban
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpPut, Route("{accountId}/unban")]
        [VmsAuthorize(FunctionCodes = "Account_Full, Account_Modify_All")]
        public async Task<IActionResult> UnbanAccountAsync([FromRoute] string accountId)
        {
            var currentFunctionCodes = GetCurrentAccountFunctionCodes();
            var account = await _accountRepository.GetAccountByIdAsync(accountId);

            if (account == null)
            {
                throw new NotFound404Exception("account");
            }

            if (!currentFunctionCodes.Contains("Account_Full"))
            {
                var currentAccount = await _accountRepository.GetAccountByIdAsync(CurrentAccountId);

                if (currentAccount.GroupId > account.GroupId)
                {
                    throw new ForbiddenException(); // the lower the group id, the higher the authority; can only delete the group with authority lower than the current group
                }
            }            

            // bind data
            account.GroupId = 3; // group: user
            account.IsActive = true;

            await _accountRepository.UpdateAccountAsync(account);

            return Ok(AccountDTO.GetFrom(account));
        }

        /// <summary>
        /// PUT /accounts/{accountId}
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut, Route("{accountId}")]
        [VmsAuthorize(FunctionCodes = "Account_Full, Account_Modify")]
        public async Task<IActionResult> UpdateAccountAsync([FromRoute] string accountId, [FromBody] AccountUpdateModel model)
        {
            var functions = GetCurrentAccountFunctionCodes();

            if (!functions.Contains("Account_Full"))
            {
                if (accountId != CurrentAccountId)
                {
                    throw new ForbiddenException();
                }                
            }

            var account = await _accountRepository.GetAccountByIdAsync(accountId);

            if (account == null)
            {
                throw new NotFound404Exception("account");
            }

            if (model.Password != null)
            {
                if (model.Password.Length < 8 || model.Password.Length > 20)
                {
                    throw new PasswordIsInvalidException();
                }
            }            

            if (model.Name != null)
            {
                if (model.Name.Length > 50)
                {
                    throw new NameIsInvalidException();
                }
            }            

            if (model.BirthDate.HasValue)
            {
                if (model.BirthDate.Value.Year < Constants.MinBirthDate.Year || model.BirthDate.Value.Year > DateTime.Now.Year - Constants.MinAge)
                {
                    throw new BirthDateIsInvalidException();
                }
            }            

            if (model.Email != null)
            {
                if (!model.Email.IsEmail())
                {
                    throw new EmailIsInvalidException();
                }

                if (model.Email != account.Email && await _accountRepository.AnyByEmailAsync(model.Email))
                {
                    throw new AlreadyExistsException("email");
                }
            }            

            if (model.Phone != null)
            {
                if (!model.Phone.IsMobile())
                {
                    throw new PhoneIsInvalidException();
                }

                if (model.Phone != account.Phone && await _accountRepository.AnyByPhoneAsync(model.Phone))
                {
                    throw new AlreadyExistsException("phone");
                }
            }

            if (model.WardId.HasValue)
            {
                if (!await _wardRepository.AnyByIdAsync(model.WardId.Value))
                {
                    throw new NotFound400Exception("ward");
                }
            }            

            // bind data
            account.WardId = model.WardId.HasValue ? model.WardId.Value : account.WardId;
            account.Password = model.Password != null ? model.Password : account.Password;
            account.Name = model.Name != null ? model.Name : account.Name;
            account.Gender = model.Gender.HasValue ? model.Gender.Value : account.Gender;
            account.BirthDate = model.BirthDate.HasValue ? model.BirthDate : account.BirthDate;
            account.Address = model.Address != null ? model.Address : account.Address;
            account.Email = model.Email != null ? model.Email : account.Email;
            account.Phone = model.Phone != null ? model.Phone : account.Phone;
            account.Avatar = model.Avatar != null ? model.Avatar : account.Avatar;
            account.Description = model.Description != null ? model.Description : account.Description;            
            account.UpdatedDate = DateTime.Now;
            
            await _accountRepository.UpdateAccountAsync(account);

            return Ok(AccountDTO.GetFrom(account));
        }
    }
}