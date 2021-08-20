using AutoMapper;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using MS.Common.IDCode;
using MS.Component.Jwt;
using MS.Component.Jwt.UserClaim;
using MS.DbContexts;
using MS.Models.RequestModel;
using MS.Models.ViewModel;
using MS.UnitOfWork;
using MS.UnitOfWork.Collections;
using MS.WebCore;
using MS.WebCore.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static MS.Entities.Core.RouteEnums;
using MS.Entities.Core;
using MS.Entities;

namespace MS.Services.Route
{
    public class RouteService : BaseService, IRouteService
    {
        private readonly JwtService _jwtService;
        private readonly SiteSetting _siteSetting;

        public RouteService(JwtService jwtService, IOptions<SiteSetting> options, IUnitOfWork<MSDbContext> unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claimsAccessor, IStringLocalizer localizer) : base(unitOfWork, mapper, idWorker, claimsAccessor, localizer)
        {
            _jwtService = jwtService;
            _siteSetting = options.Value;
        }

        public async Task<ExecuteResult<RoutePageVM>> GetAsync(long routeId)
        {
            var result = new ExecuteResult<RoutePageVM>();
            //var row = await _unitOfWork.GetRepository<MS.Entities.Route>().FindAsync(routeId);

            var row = await _unitOfWork.GetRepository<MS.Entities.Route>()
                .GetFirstOrDefaultAsync(predicate: x => x.Id == routeId, include: source => source
                .Include(n => n.Truck)
                .Include(t => t.RouteDrivers)
                .ThenInclude(m => m.Driver));

            if (row == null)
            {
                return result.SetFailMessage($"ID为 {routeId} 的线路不存在");
            }
            var viewModel = _mapper.Map<RoutePageVM>(row);

            return result.SetData(viewModel);
        }

        public async Task<ExecuteResult<IPagedList<RoutePageVM>>> PageListAsync(RoutePageRequest request)
        {
            var list = await request.PageListAsync(_unitOfWork, _mapper);
            return new ExecuteResult<IPagedList<RoutePageVM>>(list);
        }

        public async Task<ExecuteResult<MS.Entities.Route>> CreateAsync(RouteRequest request)
        {
            var result = new ExecuteResult<MS.Entities.Route>();
            if(request.CheckField(ExecuteType.Create, _unitOfWork) is ExecuteResult checkResult && !checkResult.IsSucceed)
            {
                return result.SetFailMessage(checkResult.Message);
            }
            using(var trans = _unitOfWork.BeginTransaction())
            {
                var newRow = _mapper.Map<MS.Entities.Route>(request);
                newRow.Id = _idWorker.NextId();
                newRow.RunStatus = RunStatusEnum.NotYet;
                newRow.Status = StatusCode.Enable;
                newRow.Creator = _claimsAccessor.UserId;
                newRow.CreateTime = DateTime.Now;

                var routeDrivers = new List<RouteDriver>();
                foreach(var driverId in request.DriverIds)
                {
                    routeDrivers.Add(new RouteDriver {
                        DriverId = driverId,
                        Driver = null,
                        RouteId = newRow.Id,
                        Route = null
                    });
                }
                newRow.RouteDrivers = routeDrivers;

                await _unitOfWork.GetRepository<MS.Entities.Route>().InsertAsync(newRow);
                await _unitOfWork.SaveChangesAsync();
                await trans.CommitAsync();
                
                newRow.RouteDrivers = null;
                result.SetData(newRow);
            }
            return result;
        }

        public async Task<ExecuteResult> UpdateAsync(RouteRequest request)
        {
            var result = new ExecuteResult();
            if(request.CheckField(ExecuteType.Update, _unitOfWork) is ExecuteResult checkResult && !checkResult.IsSucceed)
            {
                return result.SetFailMessage(checkResult.Message);
            }
            var row = await _unitOfWork.GetRepository<MS.Entities.Route>()
                .GetFirstOrDefaultAsync(predicate: x=>x.Id==request.Id, include: source=>source
                .Include(c=>c.RouteDrivers));
            _mapper.Map(request, row);
            row.Modifier = _claimsAccessor.UserId;
            row.ModifyTime = DateTime.Now;

            var routeDrivers = new List<RouteDriver>();
            foreach (var driverId in request.DriverIds)
            {
                routeDrivers.Add(new RouteDriver
                {
                    DriverId = driverId,
                    Driver = null,
                    RouteId = row.Id,
                    Route = null
                });
            }
            _unitOfWork.GetRepository<MS.Entities.RouteDriver>().Delete(row.RouteDrivers);

            row.RouteDrivers = routeDrivers;
            _unitOfWork.GetRepository<MS.Entities.Route>().Update(row);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<ExecuteResult> DeleteAsync(RouteRequest request)
        {
            var result = new ExecuteResult();
            if (request.CheckField(ExecuteType.Delete, _unitOfWork) is ExecuteResult checkResult && !checkResult.IsSucceed)
            {
                return result.SetFailMessage(checkResult.Message);
            }
            var row = await _unitOfWork.GetRepository<MS.Entities.Route>().FindAsync(request.Id);
            row.Modifier = _claimsAccessor.UserId;
            row.ModifyTime = DateTime.Now;
            row.Status = StatusCode.Deleted;
            _unitOfWork.GetRepository<MS.Entities.Route>().Update(row);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

    }
}
