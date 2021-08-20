using MS.Entities;
using MS.Models.RequestModel;
using MS.Models.ViewModel;
using MS.UnitOfWork.Collections;
using MS.WebCore.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MS.Services.Car
{
    public interface ITruckService : IBaseService
    {
        Task<ExecuteResult<TruckPageVM>> GetAsync(long truckId);
        Task<ExecuteResult<IPagedList<TruckPageVM>>> PageListAsync(TruckPageRequest request);
        Task<ExecuteResult<Truck>> CreateAsync(TruckRequest request);
        Task<ExecuteResult> UpdateAsync(TruckRequest request);
        Task<ExecuteResult> DeleteAsnyc(TruckRequest request);
        Task<ExecuteResult> UpdateStatusAsync(long truckId, bool isEnabled);
    }
}
