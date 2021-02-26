using System.Collections.Generic;
using System.Linq;
using VMS.Core.Entities;
using VMS.Core.Helpers;

namespace VMS.Api.Models.Permissions
{
    public class PermissionList : ItemList<PermissionListItem>
    {
        public static PermissionList GetFrom(ItemList<Permission> list, List<Function> functions)
        {
            return new PermissionList
            {
                Total = list.Total,
                Page = list.Page,
                PageSize = list.PageSize,
                Items = list.Items.Select(item => PermissionListItem.GetFrom(item, functions)).ToList()
            };
        }
    }
}