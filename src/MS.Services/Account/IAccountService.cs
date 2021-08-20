using MS.Component.Jwt.UserClaim;
using MS.Entities;
using MS.Models.RequestModel;
using MS.WebCore.Core;
using System.Threading.Tasks;

namespace MS.Services
{
    public interface IAccountService : IBaseService
    {
        Task<ExecuteResult<UserData>> Login(LoginRequest viewModel);
        Task<ExecuteResult<User>> GetUserProfile();
    }
}