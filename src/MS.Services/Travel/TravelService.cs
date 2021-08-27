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
using static MS.Entities.Core.TravelEnums;
using MS.Entities.Core;
using MS.Entities;

namespace MS.Services.Travel
{
    public class TravelService : BaseService, ITravelService
    {
        private readonly JwtService _jwtService;
        private readonly SiteSetting _siteSetting;

        public TravelService(JwtService jwtService, IOptions<SiteSetting> options, IUnitOfWork<MSDbContext> unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claimsAccessor, IStringLocalizer localizer) : base(unitOfWork, mapper, idWorker, claimsAccessor, localizer)
        {
            _jwtService = jwtService;
            _siteSetting = options.Value;
        }

        public async Task<ExecuteResult<TravelPageVM>> GetAsync(long travelId)
        {
            var result = new ExecuteResult<TravelPageVM>();
            //var row = await _unitOfWork.GetRepository<MS.Entities.Travel>().FindAsync(travelId);

            var row = await _unitOfWork.GetRepository<MS.Entities.Travel>()
                .GetFirstOrDefaultAsync(predicate: x => x.Id == travelId, include: source => source
                .Include(n => n.Truck)
                .Include(t => t.TravelDrivers)
                .ThenInclude(m => m.Driver));

            if (row == null)
            {
                return result.SetFailMessage($"ID为 {travelId} 的线路不存在");
            }
            var viewModel = _mapper.Map<TravelPageVM>(row);

            return result.SetData(viewModel);
        }

        public async Task<ExecuteResult<IPagedList<TravelPageVM>>> PageListAsync(TravelPageRequest request)
        {
            var list = await request.PageListAsync(_unitOfWork, _mapper);
            return new ExecuteResult<IPagedList<TravelPageVM>>(list);
        }

        public async Task<ExecuteResult<MS.Entities.Travel>> CreateAsync(TravelRequest request)
        {
            var result = new ExecuteResult<MS.Entities.Travel>();
            if(request.CheckField(ExecuteType.Create, _unitOfWork) is ExecuteResult checkResult && !checkResult.IsSucceed)
            {
                return result.SetFailMessage(checkResult.Message);
            }
            using(var trans = _unitOfWork.BeginTransaction())
            {
                var newRow = _mapper.Map<MS.Entities.Travel>(request);
                newRow.Id = _idWorker.NextId();
                newRow.RunStatus = RunStatusEnum.NotYet;
                newRow.Status = StatusCode.Enable;
                newRow.Creator = _claimsAccessor.UserId;
                newRow.CreateTime = DateTime.Now;

                var travelDrivers = new List<TravelDriver>();
                foreach(var driverId in request.DriverIds)
                {
                    travelDrivers.Add(new TravelDriver {
                        DriverId = driverId,
                        Driver = null,
                        TravelId = newRow.Id,
                        Travel = null
                    });
                }
                newRow.TravelDrivers = travelDrivers;

                await _unitOfWork.GetRepository<MS.Entities.Travel>().InsertAsync(newRow);
                await _unitOfWork.SaveChangesAsync();
                await trans.CommitAsync();
                
                newRow.TravelDrivers = null;
                result.SetData(newRow);
            }
            return result;
        }

        public async Task<ExecuteResult> UpdateAsync(TravelRequest request)
        {
            var result = new ExecuteResult();
            if(request.CheckField(ExecuteType.Update, _unitOfWork) is ExecuteResult checkResult && !checkResult.IsSucceed)
            {
                return result.SetFailMessage(checkResult.Message);
            }
            var row = await _unitOfWork.GetRepository<MS.Entities.Travel>()
                .GetFirstOrDefaultAsync(predicate: x=>x.Id==request.Id, include: source=>source
                .Include(c=>c.TravelDrivers));
            _mapper.Map(request, row);
            row.Modifier = _claimsAccessor.UserId;
            row.ModifyTime = DateTime.Now;

            var travelDrivers = new List<TravelDriver>();
            foreach (var driverId in request.DriverIds)
            {
                travelDrivers.Add(new TravelDriver
                {
                    DriverId = driverId,
                    Driver = null,
                    TravelId = row.Id,
                    Travel = null
                });
            }
            _unitOfWork.GetRepository<MS.Entities.TravelDriver>().Delete(row.TravelDrivers);

            row.TravelDrivers = travelDrivers;
            _unitOfWork.GetRepository<MS.Entities.Travel>().Update(row);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<ExecuteResult> DeleteAsync(TravelRequest request)
        {
            var result = new ExecuteResult();
            if (request.CheckField(ExecuteType.Delete, _unitOfWork) is ExecuteResult checkResult && !checkResult.IsSucceed)
            {
                return result.SetFailMessage(checkResult.Message);
            }
            var row = await _unitOfWork.GetRepository<MS.Entities.Travel>().FindAsync(request.Id);
            row.Modifier = _claimsAccessor.UserId;
            row.ModifyTime = DateTime.Now;
            row.Status = StatusCode.Deleted;
            _unitOfWork.GetRepository<MS.Entities.Travel>().Update(row);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<ExecuteResult> UpdateStatusAsync(long travelId, bool isEnabled)
        {
            var result = new ExecuteResult();
            if (travelId == 0 || !_unitOfWork.GetRepository<Entities.Truck>().Exists(x => x.Id == travelId))
            {
                return result.SetFailMessage("无效的车辆ID");
            }
            var row = await _unitOfWork.GetRepository<MS.Entities.Travel>().FindAsync(travelId);
            row.Status = isEnabled ? StatusCode.Enable : StatusCode.Disable;
            row.Modifier = _claimsAccessor.UserId;
            row.ModifyTime = DateTime.Now;
            _unitOfWork.GetRepository<MS.Entities.Travel>().Update(row);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

    }
}
