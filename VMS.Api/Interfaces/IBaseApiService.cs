using VMS.Core.Interfaces.Repositories;

namespace VMS.Api.Interfaces
{
    public interface IBaseApiService
    {
        IFunctionRepository FunctionRepository { get; }
    }
}