using System.Collections.Generic;
using System.Linq;
using VMS.Core.Entities;

namespace VMS.Api.Models.Permissions
{
    public class PermissionListItem
    {
        public int FunctionId { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public static PermissionListItem GetFrom(Permission Permission, List<Function> function)
        {
            return new PermissionListItem
            {
                FunctionId = Permission.FunctionId,
                Code = function.FirstOrDefault(item => item.FunctionId == Permission.FunctionId).Code,
                Description = function.FirstOrDefault(item => item.FunctionId == Permission.FunctionId).Description
            };
        }
    }
}