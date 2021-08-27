using Microsoft.AspNetCore.Mvc;
using MS.Common.Extensions;
using MS.Models.RequestModel;
using MS.Services.Driver;
using MS.Services.Travel;
using MS.Services.Truck;
using System.Collections.Generic;
using System.Threading.Tasks;
using static MS.Entities.Core.TravelEnums;

namespace MS.WebApi.Controllers
{
    //[Route("api/[controller]")]
    /// <summary>
    /// 运输行程
    /// </summary>
    [ApiController]
    public class TravelController : AuthorizeV1Controller
    {
        private readonly ITravelService _travelService;
        private readonly ITruckService _truckService;
        private readonly IDriverService _driverService;

        public TravelController(ITravelService travelService, ITruckService truckService, IDriverService driverService)
        {
            _travelService = travelService;
            _truckService = truckService;
            _driverService = driverService;
        }

        [HttpGet("{travelId}")]
        public async Task<IActionResult> GetAsync(long travelId)
        {
            var res = await _travelService.GetAsync(travelId);
            if (!res.IsSucceed)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync([FromQuery] TravelPageRequest request)
        {
            var res = await _travelService.PageListAsync(request);
            return Ok(res);
        }

        [HttpPut]
        public async Task<IActionResult> ChangeStatus([FromQuery] long travelId, [FromBody] ChangeStatusRequest request)
        {
            var res = await _travelService.UpdateStatusAsync(travelId, request.IsEnabled);
            return Ok(res);
        }

        [HttpGet]
        public IActionResult RunOptions()
        {
            Dictionary<string, int> pairs = EnumExtension.GetEnumDictionary<RunStatusEnum>();
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
        public async Task<IActionResult> AddAsync([FromBody] TravelRequest request)
        {
            var res = await _travelService.CreateAsync(request);
            return Ok(res);
        }

        [HttpPut("{travelId}")]
        public async Task<IActionResult> UpdateAsync(long travelId, [FromBody] TravelRequest request)
        {
            request.Id = travelId;
            var res = await _travelService.UpdateAsync(request);
            return Ok(res);
        }

        [HttpDelete("{travelId}")]
        public async Task<IActionResult> DeleteAsync(long travelId)
        {
            var res = await _travelService.DeleteAsync(new TravelRequest { Id = travelId });
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> TruckOptions()
        {
            var res = await _truckService.TruckOptions();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> DriverOptions()
        {
            var res = await _driverService.DriverOptions();
            return Ok(res);
        }

    }
}
