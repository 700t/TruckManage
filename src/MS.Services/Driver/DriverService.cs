using AutoMapper;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using MS.Common.IDCode;
using MS.Component.Jwt;
using MS.Component.Jwt.UserClaim;
using MS.DbContexts;
using MS.Entities.Core;
using MS.Models.RequestModel;
using MS.Models.ViewModel;
using MS.UnitOfWork;
using MS.UnitOfWork.Collections;
using MS.WebCore;
using MS.WebCore.Core;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MS.Services.Driver
{
    public class DriverService : BaseService, IDriverService
    {
        private readonly JwtService _jwtService;
        private readonly SiteSetting _siteSetting;

        public DriverService(JwtService jwtService, IOptions<SiteSetting> options, IUnitOfWork<MSDbContext> unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claimsAccessor, IStringLocalizer localizer) : base(unitOfWork, mapper, idWorker, claimsAccessor, localizer)
        {
            _jwtService = jwtService;
            _siteSetting = options.Value;
        }

        public async Task<ExecuteResult<DriverPageVM>> GetAsync(long id)
        {
            var result = new ExecuteResult<DriverPageVM>();
            var row = await _unitOfWork.GetRepository<MS.Entities.Driver>().FindAsync(id);
            if (row == null)
            {
                return result.SetFailMessage($"ID为 {id} 的驾驶员不存在");
            }
            var viewModel = _mapper.Map<DriverPageVM>(row);

            return result.SetData(viewModel);
        }

        public async Task<ExecuteResult<IPagedList<DriverPageVM>>> PageListAsync(DriverPageRequest request)
        {
            var list = await request.PageListAsync(_unitOfWork, _mapper);

            return new ExecuteResult<IPagedList<DriverPageVM>>(list);
        }

        public async Task<ExecuteResult<MS.Entities.Driver>> Create(DriverRequest request)
        {
            var result = new ExecuteResult<MS.Entities.Driver>();
            //检查字段
            if (request.CheckField(ExecuteType.Create, _unitOfWork) is ExecuteResult checkResult && !checkResult.IsSucceed)
            {
                return result.SetFailMessage(checkResult.Message);
            }
            using (var tran = _unitOfWork.BeginTransaction())//开启一个事务
            {
                var newRow = _mapper.Map<MS.Entities.Driver>(request);
                newRow.Id = _idWorker.NextId();
                newRow.Status = Entities.Core.StatusCode.Enable;
                newRow.Creator = _claimsAccessor.UserId;
                newRow.CreateTime = DateTime.Now;
                await _unitOfWork.GetRepository<MS.Entities.Driver>().InsertAsync(newRow);
                await _unitOfWork.SaveChangesAsync();
                await tran.CommitAsync();//提交事务

                result.SetData(newRow);//添加成功，把新的实体返回回去
            }
            return result;
        }

        public async Task<ExecuteResult> Update(DriverRequest request)
        {
            var result = new ExecuteResult();
            if (request.CheckField(ExecuteType.Update, _unitOfWork) is ExecuteResult checkResult && !checkResult.IsSucceed)
            {
                return checkResult;
            }

            var row = await _unitOfWork.GetRepository<MS.Entities.Driver>().FindAsync(request.Id);

            _mapper.Map(request, row);
            row.Modifier = _claimsAccessor.UserId;
            row.ModifyTime = DateTime.Now;
            _unitOfWork.GetRepository<MS.Entities.Driver>().Update(row);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<ExecuteResult> Delete(DriverRequest request)
        {
            var result = new ExecuteResult();
            if(request.CheckField(ExecuteType.Create, _unitOfWork) is ExecuteResult checkResult && !checkResult.IsSucceed)
            {
                return checkResult;
            }

            //_unitOfWork.GetRepository<MS.Entities.Driver>().Delete(request.Id);
            //await _unitOfWork.SaveChangesAsync();
            var row = await _unitOfWork.GetRepository<MS.Entities.Driver>().FindAsync(request.Id);
            row.Status = Entities.Core.StatusCode.Deleted;
            row.Modifier = _claimsAccessor.UserId;
            row.ModifyTime = DateTime.Now;
            _unitOfWork.GetRepository<MS.Entities.Driver>().Update(row);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<ExecuteResult> UpdateStatusAsync(long driverId, bool isEnabled)
        {
            var result = new ExecuteResult();
            if (driverId == 0 || !_unitOfWork.GetRepository<MS.Entities.Driver>().Exists(x => x.Id == driverId))
            {
                return result.SetFailMessage("无效的驾驶员ID");
            }
            var row = await _unitOfWork.GetRepository<MS.Entities.Driver>().FindAsync(driverId);
            row.Status = isEnabled ? StatusCode.Enable : StatusCode.Disable;
            row.Modifier = _claimsAccessor.UserId;
            row.ModifyTime = DateTime.Now;
            _unitOfWork.GetRepository<MS.Entities.Driver>().Update(row);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<ExecuteResult<IList<DriverOptionsVM>>> DriverOptions()
        {
            var result = new ExecuteResult<IList<DriverOptionsVM>>();

            Expression<Func<Entities.Driver, bool>> where = x => x.Status != StatusCode.Deleted;
            IList<Entities.Driver> drivers = await _unitOfWork.GetRepository<Entities.Driver>().GetAllAsync(predicate: where);

            var driverVms = _mapper.Map<IList<DriverOptionsVM>>(drivers);
            return result.SetData(driverVms);
        }

    }
}
