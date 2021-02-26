using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using VMS.Core.Interfaces.Repositories;

namespace VMS.Infrastructure.Data
{
    public class CategoryRepository : BaseRepository<CategoryRepository>, ICategoryRepository
    {
        private readonly VMSDbContext _dbContext;

        public CategoryRepository(VMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AnyByIdAsync(int categoryId)
        {
            return await _dbContext.Categories.AnyAsync(item => item.CategoryId.Equals(categoryId));
        }
    }
}