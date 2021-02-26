using System;
using VMS.Core.Entities;

namespace VMS.Api.Models.Groups
{
    public class GroupListItem
    {
        public int GroupId { get; set; }

        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LatestUpdate { get; set; }

        public static GroupListItem GetFrom(Group group)
        {
            return new GroupListItem
            {
                GroupId = group.GroupId,
                Name = group.Name,
                CreatedDate = group.CreatedDate,
                LatestUpdate = group.UpdatedDate
            };
        }
    }
}