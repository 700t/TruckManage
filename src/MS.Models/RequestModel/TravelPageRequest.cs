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
using static MS.Entities.Core.TravelEnums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MS.Models.RequestModel
{
    public class TravelPageRequest : PageModel
    {
        [Display(Name = "行程名称")]
        public string Name { get; set; }


        [Display(Name = "出发时间")]
        public DateTime? StartTime { get; set; }


        [Display(Name = "结束时间")]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 行程状态：0未出发，1行驶中，2已完成
        /// </summary>
        [Display(Name = "行程状态")]
        public RunStatusEnum? RunStatus { get; set; }


        [Display(Name = "出发地")]
        public string StartAddress { get; set; }


        [Display(Name = "目的地")]
        public string TargetAddress { get; set; }


        [Display(Name = "驾驶员姓名")]
        public string DriverName { get; set; }


        [Display(Name = "是否往返")]
        public bool? IsRound { get; set; }


        [Display(Name = "状态")]
        public StatusCode? Status { get; set; }


        public async Task<IPagedList<TravelPageVM>> PageListAsync(IUnitOfWork<MSDbContext> unitOfWork, IMapper mapper)
        {
            Expression<Func<Travel, bool>> where = x => true;
            where = where.And(x => x.Status != StatusCode.Deleted);

            if (!string.IsNullOrEmpty(Key))
            {
                where = where.And(x => x.Name.Contains(Key) || x.StartAddress.Contains(Key) || x.TargetAddress.Contains(Key) || x.Truck.PlateNumber.Contains(Key) || x.TravelDrivers.Exists(r => r.Driver.Name.Contains(Key)));
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
                    where = where.And(x => x.TravelDrivers.Exists(r => r.Driver.Name.Contains(DriverName)));
                }
                if(IsRound != null)
                {
                    where = where.And(x => x.IsRound == IsRound);
                }
                if(Status != null)
                {
                    where = where.And(x => x.Status == Status);
                }
            }
            Func<IQueryable<Travel>, IOrderedQueryable<Travel>> orderBy = x => x.OrderByDescending(r => r.Id);
            if(!string.IsNullOrEmpty(Order) && !string.IsNullOrEmpty(OrderField))
            {
                orderBy = x => (IOrderedQueryable<Travel>)x.OrderBy(OrderField, Order == "descending");
            }
            //IPagedList<Travel> pagedList = await unitOfWork.GetRepository<Travel>().GetPagedListAsync(predicate: where, orderBy: orderBy, pageIndex: Page, pageSize: Limit);
            IPagedList<Travel> pagedList = await unitOfWork.GetRepository<Travel>().GetPagedListAsync(predicate: where, orderBy: orderBy, include: source=>source
            .Include(c=>c.Truck)
            .Include(c=>c.TravelDrivers)
            .ThenInclude(rd=>rd.Driver), pageIndex: Page, pageSize: Limit);

            var list = mapper.Map<PagedList<TravelPageVM>>(pagedList);

            return list;
        }
    }
}
