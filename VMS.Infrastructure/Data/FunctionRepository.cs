using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Core.Entities;
using VMS.Core.Helpers;
using VMS.Core.Interfaces.Repositories;

namespace VMS.Infrastructure.Data
{
    public class FunctionRepository : BaseRepository<FunctionRepository>, IFunctionRepository
    {
        private readonly VMSDbContext _dbContext;

        public FunctionRepository(VMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AnyByIdAsync(int functionId)
        {
            return await _dbContext.Functions.AnyAsync(item => item.FunctionId == functionId);
        }

        public async Task<bool> AnyByCodeAsync(string code)
        {
            return await _dbContext.Functions.AnyAsync(item => item.Code == code);
        }

        public async Task<bool> AnyByDescriptionAsync(string description)
        {
            return await _dbContext.Functions.AnyAsync(item => item.Description == description);
        }

        public async Task<Function> CreateFunctionAsync(Function function)
        {
            await _dbContext.Functions.AddAsync(function);
            await _dbContext.SaveChangesAsync();

            return function;
        }

        public async Task<Function> GetFunctionByCodeAsync(string code)
        {
            return await _dbContext.Functions.AsNoTracking().FirstOrDefaultAsync(item => item.Code == code);
        }

        public async Task<Function> GetFunctionByIdAsync(int functionId)
        {
            return await _dbContext.Functions.AsNoTracking().FirstOrDefaultAsync(item => item.FunctionId == functionId);
        }

        public async Task<ItemList<Function>> GetFunctionsAsync(string keyword, bool? isActive, int page, int pageSize)
        {
            var result = new ItemList<Function> { Page = page, PageSize = pageSize };
            var functions = _dbContext.Functions.AsNoTracking();

            if (isActive.HasValue)
            {
                functions = functions.Where(item => item.IsActive == isActive.Value);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                functions = functions.Where(item => item.Code.Contains(keyword) || item.Description.Contains(keyword));
            }

            functions = functions.OrderBy(item => item.Code);

            result.Total = await functions.CountAsync();            
            result.Items = await GetList(functions, page, pageSize);

            return result;
        }

        public async Task<List<Function>> GetFunctionsByIdsAsync(List<int> functionIds)
        {
            return await _dbContext.Functions.AsNoTracking().Where(item => functionIds.Contains(item.FunctionId)).ToListAsync();
        }

        public async Task UpdateFunctionAsync(Function function)
        {
            _dbContext.Entry(function).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}