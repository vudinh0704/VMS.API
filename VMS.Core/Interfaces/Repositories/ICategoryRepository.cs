using System.Threading.Tasks;

namespace VMS.Core.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        public Task<bool> AnyByIdAsync(int categoryId);
    }
}