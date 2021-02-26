using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VMS.Api.Exceptions;
using VMS.Api.Helpers;
using VMS.Api.Interfaces;
using VMS.Api.Models.Functions;
using VMS.Api.Models.Groups;
using VMS.Api.Models.Permissions;
using VMS.Core.Entities;
using VMS.Core.Interfaces.Repositories;

namespace VMS.Api.Controllers
{
    [Route("groups")]
    [ApiController]
    public class GroupsController : BaseApiController
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IFunctionRepository _functionRepository;
        private readonly IGroupRepository _groupRepository;        

        public GroupsController(IAccountRepository accountRepository, IFunctionRepository functionRepository, IGroupRepository groupRepository, IBaseApiService service) : base(service)
        {
            _accountRepository = accountRepository;
            _functionRepository = functionRepository;
            _groupRepository = groupRepository;
        }

        /// <summary>
        /// POST /groups
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("")]
        [VmsAuthorize(FunctionCodes = "Group_Full, Group_Create")]
        public async Task<IActionResult> CreateGroupAsync([FromBody] GroupCreateModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                throw new IsRequiredException("name");
            }            

            if (model.Name.Length > 150)
            {
                throw new NameIsInvalidException();
            }

            if (await _groupRepository.AnyByNameAsync(model.Name))
            {
                throw new AlreadyExistsException("name");
            }

            DateTime now = DateTime.Now;

            var accountId = CurrentAccountId;

            var group = new Group
            {
                Name = model.Name,
                CreatedDate = now,
                CreatedBy = accountId,
                UpdatedDate = now,
                UpdatedBy = accountId
            };

            await _groupRepository.CreateGroupAsync(group);

            return Ok(GroupDTO.GetFrom(group));
        }

        /// <summary>
        /// POST /groups/{groupId}/functions
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("{groupId:int}/functions")]
        [VmsAuthorize(FunctionCodes = "Group_Full, Group_Modify_All")]
        public async Task<IActionResult> CreatePermissionAsync([FromRoute] int groupId, [FromBody] PermissionCreateModel model)
        {
            var currentAccount = await _accountRepository.GetAccountByIdAsync(CurrentAccountId);

            if (currentAccount.GroupId > groupId)
            {
                throw new ForbiddenException(); // the lower the group id, the higher the authority; can only delete the group with authority lower than the current group
            }

            var group = await _groupRepository.GetGroupByIdAsync(groupId);
            var function = await _functionRepository.GetFunctionByIdAsync(model.FunctionId);

            if (group == null)
            {
                throw new NotFound404Exception("group");
            }

            if (function == null)
            {
                throw new NotFound400Exception("function");
            }

            if (await _groupRepository.AnyByPermissionAsync(model.FunctionId, groupId))
            {
                throw new AlreadyExistsException("function");
            }

            var permission = new Permission
            {
                FunctionId = model.FunctionId,
                GroupId = groupId                
            };

            await _groupRepository.CreatePermissionAsync(permission);

            return Ok(FunctionDTO.GetFrom(function));
        }

        /// <summary>
        /// DELETE /groups/{groupId}
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpDelete, Route("{groupId:int}")]
        [VmsAuthorize(FunctionCodes = "Group_Full, Group_Delete")]
        public async Task<IActionResult> DeleteGroupAsync([FromRoute] int groupId)
        {
            var group = await _groupRepository.GetGroupByIdAsync(groupId);
            var currentAccount = await _accountRepository.GetAccountByIdAsync(CurrentAccountId);

            if (currentAccount.GroupId > groupId)
            {
                throw new ForbiddenException(); // the lower the group id, the higher the authority; can only delete the group with authority lower than the current group
            }

            if (group == null)
            {
                throw new NotFound404Exception("group");
            }

            group.IsDeleted = true;
            group.UpdatedDate = DateTime.Now;
            group.UpdatedBy = CurrentAccountId;            

            await _groupRepository.UpdateGroupAsync(group);

            return Ok(GroupDTO.GetFrom(group));
        }

        /// <summary>
        /// DELETE /groups/{groupId}/functions/{functionId}
        /// </summary>
        /// <param name="functionId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpDelete, Route("{groupId:int}/functions/{functionId:int}")]
        [VmsAuthorize(FunctionCodes = "Group_Full, Group_Modify_All")]
        public async Task<IActionResult> DeletePermissionAsync([FromRoute] int groupId, [FromRoute] int functionId)
        {
            var currentAccount = await _accountRepository.GetAccountByIdAsync(CurrentAccountId);

            if (currentAccount.GroupId > groupId)
            {
                throw new ForbiddenException(); // the lower the group id, the higher the authority; can only delete the group with authority lower than the current group
            }

            var group = await _groupRepository.GetGroupByIdAsync(groupId);
            var function = await _functionRepository.GetFunctionByIdAsync(functionId);

            if (group == null)
            {
                throw new NotFound404Exception("group");
            }

            if (function == null || !(await _groupRepository.AnyByPermissionAsync(functionId, groupId)))
            {
                throw new NotFound404Exception("function");
            }

            await _groupRepository.DeletePermissionAsync(functionId, groupId);

            return Ok(FunctionDTO.GetFrom(function));
        }

        /// <summary>
        /// GET /groups/{groupId}
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpGet, Route("{groupId:int}")]
        [VmsAuthorize(FunctionCodes = "Group_Full, Group_Read_All, Group_Read")]
        public async Task<IActionResult> GetGroupByIdAsync([FromRoute] int groupId)
        {
            var currentFunctionCodes = GetCurrentAccountFunctionCodes();

            if (!currentFunctionCodes.Contains("Group_Full") && !currentFunctionCodes.Contains("Group_Read_All"))
            {
                var currentAccount = await _accountRepository.GetAccountByIdAsync(CurrentAccountId);

                if (currentAccount.GroupId != groupId)
                {
                    throw new ForbiddenException();
                }
            }

            var group = await _groupRepository.GetGroupByIdAsync(groupId);

            if (group == null)
            {
                throw new NotFound404Exception("group");
            }

            return Ok(GroupDTO.GetFrom(group));
        }

        /// <summary>
        /// GET /groups
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet, Route("")]
        [VmsAuthorize(FunctionCodes = "Group_Full, Group_Read_All")]
        public async Task<IActionResult> GetGroupsAsync([FromQuery] GroupGetModel model)
        {
            model.Validate();

            var list = await _groupRepository.GetGroupsAsync(model.Keyword, model.Page, model.PageSize);

            if (list.Items.Count == 0)
            {
                throw new NotFound404Exception("page");
            }

            return Ok(GroupList.GetFrom(list));
        }

        /// <summary>
        /// GET /groups/{groupId}/functions
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet, Route("{groupId:int}/functions")]
        [VmsAuthorize(FunctionCodes = "Group_Full, Group_Read_All, Group_Read")]
        public async Task<IActionResult> GetPermissionsByGroupIdAsync([FromRoute] int groupId, [FromQuery] PermissionGetModel model)
        {
            var currentFunctionCodes = GetCurrentAccountFunctionCodes();

            if (!currentFunctionCodes.Contains("Group_Full") && !currentFunctionCodes.Contains("Group_Read_All"))
            {
                var currentAccount = await _accountRepository.GetAccountByIdAsync(CurrentAccountId);

                if (currentAccount.GroupId != groupId)
                {
                    throw new ForbiddenException();
                }
            }

            model.Validate();

            var permissions = await _groupRepository.GetPermissionsByGroupIdAsync(groupId, model.Page, model.PageSize);
            var functions = await _functionRepository.GetFunctionsAsync(null, null, 1, int.MaxValue);

            if (permissions.Items.Count == 0)
            {
                throw new NotFound404Exception("page");
            }

            return Ok(PermissionList.GetFrom(permissions, functions.Items));
        }

        /// <summary>
        /// PUT /groups/{groupId}
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut, Route("{groupId:int}")]
        [VmsAuthorize(FunctionCodes = "Group_Full, Group_Modify")]
        public async Task<IActionResult> UpdateGroupAsync([FromRoute] int groupId, [FromBody] GroupUpdateModel model)
        {
            var currentAccount = await _accountRepository.GetAccountByIdAsync(CurrentAccountId);

            if (currentAccount.GroupId > groupId)
            {
                throw new ForbiddenException(); // the lower the group id, the higher the authority; can only delete the group with authority lower than the current group
            }

            var group = await _groupRepository.GetGroupByIdAsync(groupId);

            if (group == null)
            {
                throw new NotFound404Exception("group");
            }            

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                throw new IsRequiredException("name");
            }            

            if (model.Name.Length > 50)
            {
                throw new NameIsInvalidException();
            }

            if (await _groupRepository.AnyByNameAsync(model.Name) && !group.Name.Equals(model.Name))
            {
                throw new AlreadyExistsException("name");
            }

            // bind data
            group.Name = model.Name;
            group.UpdatedDate = DateTime.Now;
            group.UpdatedBy = CurrentAccountId;

            await _groupRepository.UpdateGroupAsync(group);

            return Ok(GroupDTO.GetFrom(group));
        }
    }
}