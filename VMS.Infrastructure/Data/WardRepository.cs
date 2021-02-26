using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using VMS.Core.Interfaces.Repositories;

namespace VMS.Infrastructure.Data
{
    public class WardRepository : BaseRepository<WardRepository>, IWardRepository
    {
        private readonly VMSDbContext _dbContext;

        public WardRepository(VMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AnyByIdAsync(int wardId)
        {
            return await _dbContext.Wards.AnyAsync(item => item.WardId == wardId);
        }
    }
}