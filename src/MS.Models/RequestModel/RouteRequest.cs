using MS.DbContexts;
using MS.Entities;
using MS.UnitOfWork;
using MS.WebCore.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using static MS.Entities.Core.RouteEnums;

namespace MS.Models.RequestModel
{
    public class RouteRequest
    {
        public long? Id { get; set; }


        [Display(Name= "线路名称")]
        [Required(ErrorMessage ="{0}不能为空")]
        public string Name { get; set; }

        [Display(Name = "出发时间")]
        [Required(ErrorMessage = "{0}不能为空")]
        public DateTime StartTime { get; set; }

        [Display(Name = "到达时间")]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 行程状态：0未出发，1行驶中，2已完成
        /// </summary>
        [Display(Name = "行程状态")]
        public RunStatusEnum? RunStatus { get; set; }

        [Display(Name = "起始地点")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string StartAddress { get; set; }

        [Display(Name = "目标地点")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string TargetAddress { get; set; }

        [Display(Name = "车辆ID")]
        [Required(ErrorMessage = "{0}不能为空")]
        public long TruckId { get; set; }

        [Display(Name = "驾驶员ID")]
        [Required(ErrorMessage = "{0}不能为空")]
        public List<long> DriverIds { get; set; }

        [Display(Name = "是否往返")]
        public bool? IsRound { get; set; }

        public string Remark { get; set; }


        public ExecuteResult CheckField(ExecuteType executeType, IUnitOfWork<MSDbContext> unitOfWork)
        {
            var result = new ExecuteResult();
            var repo = unitOfWork.GetRepository<Route>();
            if(executeType != ExecuteType.Create && !repo.Exists(x => x.Id == Id))
            {
                return result.SetFailMessage("该线路不存在");
            }

            switch (executeType)
            {
                case ExecuteType.Create:
                    break;
                case ExecuteType.Update:
                    break;
                case ExecuteType.Delete:
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
