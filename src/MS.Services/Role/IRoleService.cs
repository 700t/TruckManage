using MS.Entities;
using MS.Models.RequestModel;
using MS.WebCore.Core;
using System.Threading.Tasks;

namespace MS.Services
{
    public interface IRoleService : IBaseService
    {
        Task<ExecuteResult<Role>> Create(RoleRequest viewModel);
        Task<ExecuteResult> Update(RoleRequest viewModel);
        Task<ExecuteResult> Delete(RoleRequest viewModel);
    }
}
