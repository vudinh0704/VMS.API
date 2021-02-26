using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using VMS.Core.Entities;
using VMS.Core.Helpers;
using VMS.Core.Interfaces.Repositories;

namespace VMS.Infrastructure.Data
{
    public class GroupRepository : BaseRepository<GroupRepository>, IGroupRepository
    {
        private readonly VMSDbContext _dbContext;
        
        public GroupRepository(VMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AnyByIdAsync(int groupId)
        {
            return await _dbContext.Groups.AnyAsync(item => item.GroupId == groupId);
        }

        public async Task<bool> AnyByNameAsync(string name)
        {
            return await _dbContext.Groups.AnyAsync(item => item.Name == name);
        }

        public async Task<bool> AnyByPermissionAsync(int functionId, int groupId)
        {
            return await _dbContext.Permissions.AnyAsync(item => item.FunctionId == functionId && item.GroupId == groupId);
        }

        public async Task<Group> CreateGroupAsync(Group group)
        {
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.SaveChangesAsync();

            return group;
        }

        public async Task<Permission> CreatePermissionAsync(Permission permission)
        {
            await _dbContext.Permissions.AddAsync(permission);
            await _dbContext.SaveChangesAsync();

            return permission;
        }

        public async Task DeletePermissionAsync(int groupId, int functionId)
        {
            await _dbContext.Database.ExecuteSqlInterpolatedAsync($"delete from [Permission] where FunctionId = { functionId } and GroupId = { groupId }");
        }

        public async Task<Group> GetGroupByIdAsync(int groupId)
        {
            return await _dbContext.Groups.AsNoTracking().FirstOrDefaultAsync(item => item.GroupId == groupId);
        }

        public async Task<ItemList<Group>> GetGroupsAsync(string keyword, int page, int pageSize)
        {
            var result = new ItemList<Group> { Page = page, PageSize = pageSize };
            var groups = _dbContext.Groups.AsNoTracking();

            if (!string.IsNullOrEmpty(keyword))
            {
                groups = groups.Where(item => item.Name.Contains(keyword));
            }

            groups = groups.OrderBy(item => item.Name);

            result.Total = await groups.CountAsync();
            result.Items = await GetList(groups, page, pageSize);

            return result;
        }

        public async Task<ItemList<Permission>> GetPermissionsByGroupIdAsync(int groupId, int page, int pageSize)
        {
            var result = new ItemList<Permission> { Page = page, PageSize = pageSize };
            var functionsIsAlive = _dbContext.Functions.AsNoTracking().Select(item => item.FunctionId);
            var permissions = _dbContext.Permissions.AsNoTracking().Where(item => item.GroupId == groupId && functionsIsAlive.Contains(item.FunctionId));

            permissions = permissions.OrderBy(item => item.FunctionId);

            result.Total = await permissions.CountAsync();
            result.Items = await GetList(permissions, page, pageSize);

            return result;
        }

        public async Task UpdateGroupAsync(Group group)
        {
            _dbContext.Entry(group).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}