using System.Threading.Tasks;
using VMS.Core.Entities;
using VMS.Core.Helpers;

namespace VMS.Core.Interfaces.Repositories
{
    public interface IGroupRepository
    {
        Task<bool> AnyByIdAsync(int groupId);

        Task<bool> AnyByNameAsync(string name);

        Task<bool> AnyByPermissionAsync(int functionId, int groupId);

        Task<Group> CreateGroupAsync(Group group);

        Task<Permission> CreatePermissionAsync(Permission permission);

        Task DeletePermissionAsync(int groupId, int functionId);

        Task<Group> GetGroupByIdAsync(int groupId);

        Task<ItemList<Group>> GetGroupsAsync(string keyword, int page, int pageSize);

        Task<ItemList<Permission>> GetPermissionsByGroupIdAsync(int groupId, int page, int pageSize);

        Task UpdateGroupAsync(Group group);
    }
}