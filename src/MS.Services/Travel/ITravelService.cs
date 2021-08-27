using Microsoft.AspNetCore.Mvc;
using MS.Models.RequestModel;
using MS.Models.ViewModel;
using MS.UnitOfWork.Collections;
using MS.WebCore.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MS.Services.Travel
{
    public interface ITravelService : IBaseService
    {
        Task<ExecuteResult<TravelPageVM>> GetAsync(long travelId);
        Task<ExecuteResult<IPagedList<TravelPageVM>>> PageListAsync(TravelPageRequest request);
        Task<ExecuteResult<MS.Entities.Travel>> CreateAsync(TravelRequest request);
        Task<ExecuteResult> UpdateAsync(TravelRequest request);
        Task<ExecuteResult> DeleteAsync(TravelRequest request);
        Task<ExecuteResult> UpdateStatusAsync(long travelId, bool isEnabled);
    }
}
