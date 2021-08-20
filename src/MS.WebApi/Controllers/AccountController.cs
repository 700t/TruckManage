using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MS.Entities;
using MS.Models.RequestModel;
using MS.Services;
using MS.WebCore.Core;
using System;
using System.Threading.Tasks;

namespace MS.WebApi.Controllers
{
    //[Route("/mp/v1_0/[controller]/[action]")]
    [ApiController]
    public class AccountController : AuthorizeV1Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [AllowAnonymous]
        //public async Task<ExecuteResult<UserData>> Login(LoginViewModel viewModel)
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var res = await _accountService.Login(request);
            if (!res.IsSucceed)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        /// <summary>
        /// 仅做测试用
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login2(LoginRequest request)
        {
            var res = await _accountService.Login(request);
            Console.WriteLine(new JsonResult(res));
            return Ok(res);
            //return Ok( await _accountService.Login(request));
        }

        [HttpGet]
        public async Task<ExecuteResult<User>> Profile()
        {
            return await _accountService.GetUserProfile();
        }


        //[HttpPost]
        //[Route("{userId}")]
        //public async Task<ExecuteResult<User>> GetUserProfile(int userId)
        //{
        //    return await _accountService.GetUserProfile(userId);
        //}

    }
}