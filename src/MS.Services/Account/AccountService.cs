using AutoMapper;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using MS.Common.IDCode;
using MS.Component.Jwt;
using MS.Component.Jwt.UserClaim;
using MS.DbContexts;
using MS.Entities;
using MS.Models.RequestModel;
using MS.UnitOfWork;
using MS.WebCore;
using MS.WebCore.Core;
using System.Threading.Tasks;

namespace MS.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly JwtService _jwtService;
        private readonly SiteSetting _siteSetting;

        public AccountService(JwtService jwtService, IOptions<SiteSetting> options, IUnitOfWork<MSDbContext> unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claimsAccessor, IStringLocalizer localizer) : base(unitOfWork, mapper, idWorker, claimsAccessor, localizer)
        {
            _jwtService = jwtService;
            _siteSetting = options.Value;
        }

        public async Task<ExecuteResult<UserData>> Login(LoginRequest request)
        {
            var result = await request.LoginValidate(_unitOfWork, _mapper, _siteSetting, _localizer);
            if (result.IsSucceed)
            {
                result.Result.Token = _jwtService.BuildToken(_jwtService.BuildClaims(result.Result));
                return new ExecuteResult<UserData>(result.Result);
            }
            else
            {
                return new ExecuteResult<UserData>(result.Message);
            }
        }

        public async Task<ExecuteResult<User>> GetUserProfile()
        {
            var user = new User
            {
                Id = _claimsAccessor.UserId,
                Account = _claimsAccessor.UserAccount,
                Name = _claimsAccessor.UserName,
                Role = new Role
                {
                    Name = _claimsAccessor.UserRole,
                    DisplayName = _claimsAccessor.UserRoleDisplayName
                }
            };
            return new ExecuteResult<User>(user);
        }



    }
}
