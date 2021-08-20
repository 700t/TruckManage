using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using MS.Common.Extensions;
using MS.Models.RequestModel;
using MS.Services.Route;
using System.Collections.Generic;
using System.Threading.Tasks;
using static MS.Entities.Core.RouteEnums;

namespace MS.WebApi.Controllers
{
    //[Route("api/[controller]")]
    /// <summary>
    /// 运输线路
    /// </summary>
    [ApiController]
    public class RouteController : AuthorizeV1Controller
    {
        private readonly IRouteService _routeService;

        public RouteController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        [HttpGet("{routeId}")]
        public async Task<IActionResult> GetAsync(long routeId)
        {
            var res = await _routeService.GetAsync(routeId);
            if (!res.IsSucceed)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync([FromQuery]RoutePageRequest request)
        {
            var res = await _routeService.PageListAsync(request);
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
        public async Task<IActionResult> AddAsync([FromBody] RouteRequest request)
        {
            var res = await _routeService.CreateAsync(request);
            return Ok(res);
        }

        [HttpPut("{routeId}")]
        public async Task<IActionResult> UpdateAsync(long routeId, [FromBody] RouteRequest request)
        {
            request.Id = routeId;
            var res = await _routeService.UpdateAsync(request);
            return Ok(res);
        }

        [HttpDelete("{routeId}")]
        public async Task<IActionResult> DeleteAsync(long routeId)
        {
            var res = await _routeService.DeleteAsync(new RouteRequest { Id = routeId });
            return Ok(res);
        }
    }
}
