using System.Linq;
using VMS.Core.Entities;
using VMS.Core.Helpers;

namespace VMS.Api.Models.Groups
{
    public class GroupList : ItemList<GroupListItem>
    {
        public static GroupList GetFrom(ItemList<Group> list)
        {
            return new GroupList
            {
                Total = list.Total,
                Page = list.Page,
                PageSize = list.PageSize,
                Items = list.Items.Select(item => GroupListItem.GetFrom(item)).ToList()
            };
        }
    }
}