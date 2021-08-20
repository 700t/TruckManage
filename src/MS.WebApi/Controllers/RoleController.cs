using Microsoft.AspNetCore.Mvc;
using MS.Models.RequestModel;
using MS.Services;
using MS.WebCore.Core;
using System.Threading.Tasks;

namespace MS.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoleController : AuthorizeV1Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<ExecuteResult> Post(RoleRequest request)
        {
            return await _roleService.Create(request);
        }

        [HttpPut]
        public async Task<ExecuteResult> Put(RoleRequest request)
        {
            return await _roleService.Update(request);
        }

        [HttpDelete]
        public async Task<ExecuteResult> Delete(long id)
        {
            return await _roleService.Delete(new RoleRequest { Id = id });
        }
    }
}