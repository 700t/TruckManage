using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MS.Common.Extensions;
using MS.Models.RequestModel;
using MS.Services.Car;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MS.Entities.Core.TruckEnums;

namespace MS.WebApi.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class TruckController : AuthorizeV1Controller
    {
        private readonly ITruckService _truckService;

        public TruckController(ITruckService truckService)
        {
            _truckService = truckService;
        }

        [HttpGet("{truckId}")]
        public async Task<IActionResult> GetAsync(long truckId)
        {
            var res = await _truckService.GetAsync(truckId);
            if (!res.IsSucceed)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync([FromQuery] TruckPageRequest request)
        {
            var res = await _truckService.PageListAsync(request);
            if (!res.IsSucceed)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }


        [HttpPut]
        public async Task<IActionResult> ChangeStatus([FromQuery] long truckId, [FromBody] ChangeStatusRequest request)
        {
            var res = await _truckService.UpdateStatusAsync(truckId, request.IsEnabled);
            if (!res.IsSucceed)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }


        [HttpGet]
        public IActionResult OriginOptions()
        {
            Dictionary<string, int> pairs = EnumExtension.GetEnumDictionary<OriginEnum>();
            var options = new List<dynamic>();
            foreach (var item in pairs)
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
        public async Task<IActionResult> AddAsync([FromBody] TruckRequest request)
        {
            var res = await _truckService.CreateAsync(request);
            if (!res.IsSucceed)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpPut("{truckId}")]
        public async Task<IActionResult> UpdateAsync(long truckId, [FromBody]TruckRequest request)
        {
            request.Id = truckId;
            var res = await _truckService.UpdateAsync(request);
            if (!res.IsSucceed)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpDelete("{truckId}")]
        public async Task<IActionResult> DeleteAsync(long truckId)
        {
            var res = await _truckService.DeleteAsnyc(new TruckRequest { Id = truckId });
            if (!res.IsSucceed)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

    }
}
