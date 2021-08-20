using AutoMapper;
using Microsoft.Extensions.Localization;
using MS.Common.Extensions;
using MS.DbContexts;
using MS.Entities;
using MS.Entities.Core;
using MS.Models.ViewModel;
using MS.UnitOfWork;
using MS.UnitOfWork.Collections;
using MS.WebCore;
using MS.WebCore.Core;
using MS.WebCore.Expression;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MS.Models.RequestModel
{
    public class DriverPageRequest : PageModel
    {
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Display(Name = "性别")]
        public GenderCode? Gender { get; set; }

        [Display(Name = "身份证号")]
        public string IdNumber { get; set; }

        [Display(Name = "手机号码")]
        public string Phone { get; set; }

        [Display(Name = "状态")]
        public StatusCode? Status { get; set; }


        public async Task<IPagedList<DriverPageVM>> PageListAsync(IUnitOfWork<MSDbContext> unitOfWork, IMapper mapper)
        {
            var result = new ExecuteResult<Driver>();
            //方式一
            Expression<Func<Driver, bool>> where = x => true;
            where = where.And(x => x.Status != StatusCode.Deleted);

            //方式二
            //var where = PredicateBuilder.True<Driver>();
            if (!string.IsNullOrEmpty(Key))
            {
                where = where.And(x => x.Name.Contains(Key) || x.Phone.Contains(Key) || x.IdNumber.Contains(Key));
            }
            else
            {
                if (!string.IsNullOrEmpty(Name))
                {
                    where = where.And(x => x.Name.Contains(Name));
                }
                if (!string.IsNullOrEmpty(Phone))
                {
                    where = where.And(x => x.Phone.Contains(Phone));
                }
                if (!string.IsNullOrEmpty(IdNumber))
                {
                    where = where.And(x => x.IdNumber.Contains(IdNumber));
                }

                if (Gender != null)
                {
                    where = where.And(x => x.Gender == Gender);
                }
                if (Status != null)
                {
                    where = where.And(x => x.Status == Status);
                }
            }
            //Expression<Func<IQueryable<Driver>, IOrderedQueryable<Driver>>> orderBy = q => q.OrderByDescending(c => c.Id);
            Func<IQueryable<Driver>, IOrderedQueryable<Driver>> orderBy = q => q.OrderByDescending(c => c.Id);

            if (!string.IsNullOrEmpty(Order) && !string.IsNullOrEmpty(OrderField))
            {
                orderBy = x => (IOrderedQueryable<Driver>)x.OrderBy(OrderField, Order == "descending");
            }

            IPagedList<Driver> pagedList = await unitOfWork.GetRepository<Driver>()
                .GetPagedListAsync(predicate: where, orderBy: orderBy, pageIndex: Page, pageSize: Limit);

            var list = mapper.Map<PagedList<DriverPageVM>>(pagedList);
            return list;
        }



        private PropertyInfo GetPropertyInfo(Type t, string propertyName)
        {
            var properties = t.GetProperties();
            var prop = properties.FirstOrDefault(x => x.Name == propertyName);
            if (prop == null)
            {
                throw new ArgumentException("name");
            }
            return prop;
        }

    }
}
