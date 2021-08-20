using Microsoft.AspNetCore.Mvc;
using MS.Models.RequestModel;
using MS.Models.ViewModel;
using MS.UnitOfWork.Collections;
using MS.WebCore.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MS.Services.Route
{
    public interface IRouteService : IBaseService
    {
        Task<ExecuteResult<RoutePageVM>> GetAsync(long routeId);
        Task<ExecuteResult<IPagedList<RoutePageVM>>> PageListAsync(RoutePageRequest request);
        Task<ExecuteResult<MS.Entities.Route>> CreateAsync(RouteRequest request);
        Task<ExecuteResult> UpdateAsync(RouteRequest request);
        Task<ExecuteResult> DeleteAsync(RouteRequest request);
    }
}
