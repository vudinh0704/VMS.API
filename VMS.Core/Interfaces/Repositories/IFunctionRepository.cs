using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Core.Entities;
using VMS.Core.Helpers;

namespace VMS.Core.Interfaces.Repositories
{
    public interface IFunctionRepository
    {
        public Task<bool> AnyByCodeAsync(string code);

        public Task<bool> AnyByDescriptionAsync(string description);

        public Task<bool> AnyByIdAsync(int functionId);

        public Task<Function> CreateFunctionAsync(Function function);

        Task<Function> GetFunctionByCodeAsync(string code);

        public Task<Function> GetFunctionByIdAsync(int functionId);

        public Task<ItemList<Function>> GetFunctionsAsync(string keyword, bool? isActive, int page, int pageSize);

        Task<List<Function>> GetFunctionsByIdsAsync(List<int> functionIds);

        public Task UpdateFunctionAsync(Function function);
    }
}