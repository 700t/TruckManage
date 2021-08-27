using MS.WebCore.Core;
using System;
using System.Collections.Generic;
using System.Text;
using MS.Entities;
using System.Threading.Tasks;
using MS.UnitOfWork.Collections;
using MS.Models.RequestModel;
using MS.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace MS.Services.Driver
{
    public interface IDriverService : IBaseService
    {
        Task<ExecuteResult<DriverPageVM>> GetAsync(long id);
        //分页
        Task<ExecuteResult<IPagedList<DriverPageVM>>> PageListAsync(DriverPageRequest request);
        Task<ExecuteResult<MS.Entities.Driver>> Create(DriverRequest request);
        Task<ExecuteResult> Update(DriverRequest request);
        Task<ExecuteResult> Delete(DriverRequest request);
        Task<ExecuteResult> UpdateStatusAsync(long driverId, bool isEnabled);
        Task<ExecuteResult<IList<DriverOptionsVM>>> DriverOptions();
    }
}
