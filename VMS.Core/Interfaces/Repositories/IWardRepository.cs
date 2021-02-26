using System.Threading.Tasks;

namespace VMS.Core.Interfaces.Repositories
{
    public interface IWardRepository
    {
        Task<bool> AnyByIdAsync(int wardId);
    }
}