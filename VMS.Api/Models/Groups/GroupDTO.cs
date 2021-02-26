using VMS.Core.Entities;
using System;

namespace VMS.Api.Models.Groups
{
    public class GroupDTO
    {
        public int GroupId { get; set; }

        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime LatestUpdate { get; set; }

        public string UpdatedBy { get; set; }

        public static GroupDTO GetFrom(Group group)
        {
            return new GroupDTO
            {
                GroupId = group.GroupId,
                Name = group.Name,
                CreatedDate = group.CreatedDate,
                CreatedBy = group.CreatedBy,
                LatestUpdate = group.UpdatedDate,
                UpdatedBy = group.UpdatedBy
            };
        }
    }
}