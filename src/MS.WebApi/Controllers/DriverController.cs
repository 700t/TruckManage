using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MS.Common.Extensions;
using MS.Entities;
using MS.Models.RequestModel;
using MS.Services.Driver;
using MS.WebCore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MS.Entities.Core.DriverEnums;

namespace MS.WebApi.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class DriverController : AuthorizeV1Controller
    {
        private readonly IDriverService _driverService;

        public DriverController(IDriverService driverService)
        {
            _driverService = driverService;
        }

        [HttpGet("{driverId}")]
        public async Task<IActionResult> GetAsync(long driverId)
        {
            var res = await _driverService.GetAsync(driverId);
            if (!res.IsSucceed)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync([FromQuery]DriverPageRequest request)
        {
            var res = await _driverService.PageListAsync(request);
            if (!res.IsSucceed)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }


        [HttpPut]
        public async Task<IActionResult> ChangeStatus([FromQuery] long driverId, [FromBody] ChangeStatusRequest request)
        {
            var res = await _driverService.UpdateStatusAsync(driverId, request.IsEnabled);
            if (!res.IsSucceed)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpGet]
        public IActionResult LicenseOptions()
        {
            Dictionary<string,int> pairs = EnumExtension.GetEnumDictionary<DrivingLicenseEnum>();
            var options = new List<dynamic>();
            foreach(var item in pairs)
            {
                options.Add(new
                {
                    item.Key,
                    item.Value
                });
            }
            return Ok(options);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody]DriverRequest request)
        {
            var res = await _driverService.Create(request);
            if(!res.IsSucceed)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpPut("{driverId}")]
        public async Task<IActionResult> UpdateAsync(long driverId, [FromBody]DriverRequest request)
        {
            request.Id = driverId;
            var res = await _driverService.Update(request);
            if (!res.IsSucceed)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpDelete("{driverId}")]
        public async Task<IActionResult> DeleteAsync(long driverId)
        {
            var res = await _driverService.Delete(new DriverRequest { Id = driverId });
            if (!res.IsSucceed)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }
    }
}
