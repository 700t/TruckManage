using AutoMapper;
using MS.Common.Extensions;
using MS.DbContexts;
using MS.Entities;
using MS.Entities.Core;
using MS.Models.ViewModel;
using MS.UnitOfWork;
using MS.UnitOfWork.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static MS.Entities.Core.RouteEnums;
using Microsoft.EntityFrameworkCore;

namespace MS.Models.RequestModel
{
    public class RoutePageRequest : PageModel
    {
        public string Name { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 行程状态：0未出发，1行驶中，2已完成
        /// </summary>
        public RunStatusEnum? RunStatus { get; set; }

        public string StartAddress { get; set; }

        public string TargetAddress { get; set; }

        /// <summary>
        /// 驾驶员姓名
        /// </summary>
        public string DriverName { get; set; }

        /// <summary>
        /// 是否往返
        /// </summary>
        public bool? IsRound { get; set; }

        public async Task<IPagedList<RoutePageVM>> PageListAsync(IUnitOfWork<MSDbContext> unitOfWork, IMapper mapper)
        {
            Expression<Func<Route, bool>> where = x => true;
            where = where.And(x => x.Status != StatusCode.Deleted);

            if (!string.IsNullOrEmpty(Key))
            {
                where = where.And(x => x.Name.Contains(Key) || x.StartAddress.Contains(Key) || x.TargetAddress.Contains(Key) || x.Truck.PlateNumber.Contains(Key) || x.RouteDrivers.Exists(r => r.Driver.Name.Contains(Key)));
            }
            else
            {
                if (!string.IsNullOrEmpty(Name))
                {
                    where = where.And(x => x.Name.Contains(Name));
                }
                if(StartTime != null && EndTime != null)
                {
                    where = where.And(x => (x.StartTime <= StartTime && x.EndTime > StartTime) || (x.StartTime <= EndTime && EndTime >= EndTime) || x.StartTime > StartTime && x.EndTime < EndTime);
                }
                if (!string.IsNullOrEmpty(StartAddress))
                {
                    where = where.And(x => x.StartAddress.Contains(StartAddress));
                }
                if (!string.IsNullOrEmpty(TargetAddress))
                {
                    where = where.And(x => x.TargetAddress.Contains(TargetAddress));
                }
                if(RunStatus != null)
                {
                    where = where.And(x => x.RunStatus == RunStatus);
                }
                if (!string.IsNullOrEmpty(DriverName))
                {
                    where = where.And(x => x.RouteDrivers.Exists(r => r.Driver.Name.Contains(DriverName)));
                }
                if(IsRound != null)
                {
                    where = where.And(x => x.IsRound == IsRound);
                }
            }
            Func<IQueryable<Route>, IOrderedQueryable<Route>> orderBy = x => x.OrderByDescending(r => r.Id);
            if(!string.IsNullOrEmpty(Order) && !string.IsNullOrEmpty(OrderField))
            {
                orderBy = x => (IOrderedQueryable<Route>)x.OrderBy(OrderField, Order == "descending");
            }
            //IPagedList<Route> pagedList = await unitOfWork.GetRepository<Route>().GetPagedListAsync(predicate: where, orderBy: orderBy, pageIndex: Page, pageSize: Limit);
            IPagedList<Route> pagedList = await unitOfWork.GetRepository<Route>().GetPagedListAsync(predicate: where, orderBy: orderBy, include: source=>source
            .Include(c=>c.Truck)
            .Include(c=>c.RouteDrivers)
            .ThenInclude(rd=>rd.Driver), pageIndex: Page, pageSize: Limit);

            var list = mapper.Map<PagedList<RoutePageVM>>(pagedList);

            return list;
        }
    }
}
