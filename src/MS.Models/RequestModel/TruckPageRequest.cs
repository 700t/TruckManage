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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static MS.Entities.Core.TruckEnums;

namespace MS.Models.RequestModel
{
    public class TruckPageRequest : PageModel
    {
        [Display(Name ="车牌号")]
        public string PlateNumber { get; set; }


        [Display(Name = "型号")]
        public string ModelNumber { get; set; }


        [Display(Name = "载重")]
        public double? BearWeight { get; set; }


        [Display(Name = "用车区域")]
        public string UseRegion { get; set; }


        [Display(Name = "性质")]
        public OriginEnum? Origin { get; set; }


        [Display(Name = "是否占用")]
        public bool? IsUsed { get; set; }


        [Display(Name = "状态")]
        public StatusCode? Status { get; set; }


        [Display(Name = "GPS编码")]
        public string GpsCode { get; set; }


        public async Task<IPagedList<TruckPageVM>> PageListAsync(IUnitOfWork<MSDbContext> unitOfWork, IMapper mapper)
        {
            //var result = new ExecuteResult<Truck>();
            Expression<Func<Truck, bool>> where = x => true;
            where = where.And(x => x.Status != StatusCode.Deleted);

            if (!string.IsNullOrEmpty(Key))
            {
                where = where.And(x => x.Alias.Contains(Key) || x.GpsCode.Contains(Key) || x.PlateNumber.Contains(Key) || x.ModelNumber.Contains(Key));
            }
            else
            {
                if (!string.IsNullOrEmpty(PlateNumber))
                {
                    where = where.And(x => x.PlateNumber.Contains(PlateNumber));
                }
                if (!string.IsNullOrEmpty(ModelNumber))
                {
                    where = where.And(x => x.ModelNumber.Contains(ModelNumber));
                }
                if(BearWeight != null)
                {
                    where = where.And(x => (int)x.BearWeight == (int)BearWeight);
                }
                if (!string.IsNullOrEmpty(UseRegion))
                {
                    where = where.And(x => x.UseRegion.Contains(UseRegion));
                }
                if (Origin != null)
                {
                    where = where.And(x => x.Origin == Origin);
                }
                if (Status != null)
                {
                    where = where.And(x => x.Status == Status);
                }
                if (IsUsed != null)
                {
                    where = where.And(x => x.IsUsed == IsUsed);
                }
                if (!string.IsNullOrEmpty(GpsCode))
                {
                    where = where.And(x => x.GpsCode.Contains(GpsCode));
                }
            }
            Func<IQueryable<Truck>, IOrderedQueryable<Truck>> orderBy = q => q.OrderByDescending(c => c.Id);
            if(!string.IsNullOrEmpty(Order) && !string.IsNullOrEmpty(OrderField))
            {
                orderBy = x => (IOrderedQueryable<Truck>)x.OrderBy(OrderField, Order == "descending");
            }

            IPagedList<Truck> pagedList = await unitOfWork.GetRepository<Truck>()
                .GetPagedListAsync(predicate: where, orderBy: orderBy, pageIndex: Page, pageSize: Limit);

            var list = mapper.Map<PagedList<TruckPageVM>>(pagedList);
            return list;
        }

    }
}
