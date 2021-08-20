using AutoMapper;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using MS.Common.IDCode;
using MS.Component.Jwt;
using MS.Component.Jwt.UserClaim;
using MS.DbContexts;
using MS.Entities;
using MS.Entities.Core;
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
using static MS.Entities.Core.TruckEnums;

namespace MS.Services.Car
{
    public class TruckService : BaseService, ITruckService
    {
        private readonly JwtService _jwtService;
        private readonly SiteSetting _siteSetting;

        public TruckService(JwtService jwtService, IOptions<SiteSetting> options, IUnitOfWork<MSDbContext> unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claimsAccessor, IStringLocalizer localizer) : base(unitOfWork, mapper, idWorker, claimsAccessor, localizer)
        {
            _jwtService = jwtService;
            _siteSetting = options.Value;
        }

        public async Task<ExecuteResult<TruckPageVM>> GetAsync(long truckId)
        {
            var result = new ExecuteResult<TruckPageVM>();
            var row = await _unitOfWork.GetRepository<Truck>().FindAsync(truckId);
            if (row == null)
            {
                return result.SetFailMessage($"ID为 {truckId} 的车辆不存在");
            }
            var viewModel = _mapper.Map<TruckPageVM>(row);

            return result.SetData(viewModel);
        }

        public async Task<ExecuteResult<IPagedList<TruckPageVM>>> PageListAsync(TruckPageRequest request)
        {
            var list = await request.PageListAsync(_unitOfWork, _mapper);

            return new ExecuteResult<IPagedList<TruckPageVM>>(list);
        }

        public async Task<ExecuteResult<Truck>> CreateAsync(TruckRequest request)
        {
            var result = new ExecuteResult<Truck>();
            if (request.CheckField(ExecuteType.Create, _unitOfWork) is ExecuteResult executeResult && !executeResult.IsSucceed)
            {
                return result.SetFailMessage(executeResult.Message);
            }

            using (var trans = _unitOfWork.BeginTransaction())
            {
                var newRow = _mapper.Map<Truck>(request);
                newRow.Id = _idWorker.NextId();
                newRow.Status = StatusCode.Enable;
                newRow.IsUsed = false;
                newRow.Creator = _claimsAccessor.UserId;
                newRow.CreateTime = DateTime.Now;
                await _unitOfWork.GetRepository<Truck>().InsertAsync(newRow);
                await _unitOfWork.SaveChangesAsync();

                await trans.CommitAsync();
                result.SetData(newRow);
            }
            return result;
        }

        public async Task<ExecuteResult> UpdateAsync(TruckRequest request)
        {
            var result = new ExecuteResult();
            if (request.CheckField(ExecuteType.Update, _unitOfWork) is ExecuteResult checkResult && !checkResult.IsSucceed)
            {
                return result.SetFailMessage(checkResult.Message);
            }

            var row = await _unitOfWork.GetRepository<Truck>().FindAsync(request.Id);

            _mapper.Map(request, row);
            row.Modifier = _claimsAccessor.UserId;
            row.ModifyTime = DateTime.Now;
            _unitOfWork.GetRepository<Truck>().Update(row);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<ExecuteResult> DeleteAsnyc(TruckRequest request)
        {
            var result = new ExecuteResult();
            if(request.CheckField(ExecuteType.Delete, _unitOfWork) is ExecuteResult checkResult && !checkResult.IsSucceed)
            {
                return result.SetFailMessage(checkResult.Message);
            }

            var row = await _unitOfWork.GetRepository<Truck>().FindAsync(request.Id);
            row.Status = StatusCode.Deleted;
            row.Modifier = _claimsAccessor.UserId;
            row.ModifyTime = DateTime.Now;
            _unitOfWork.GetRepository<Truck>().Update(row);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<ExecuteResult> UpdateStatusAsync(long truckId, bool isEnabled)
        {
            var result = new ExecuteResult();
            if (truckId == 0 || !_unitOfWork.GetRepository<Truck>().Exists(x => x.Id == truckId))
            {
                return result.SetFailMessage("无效的车辆ID");
            }
            var row = await _unitOfWork.GetRepository<Truck>().FindAsync(truckId);
            row.Status = isEnabled ? StatusCode.Enable : StatusCode.Disable;
            row.Modifier = _claimsAccessor.UserId;
            row.ModifyTime = DateTime.Now;
            _unitOfWork.GetRepository<Truck>().Update(row);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

    }
}
