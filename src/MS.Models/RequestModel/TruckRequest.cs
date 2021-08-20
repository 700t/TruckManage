using MS.DbContexts;
using MS.Entities;
using MS.UnitOfWork;
using MS.WebCore.Core;
using System.ComponentModel.DataAnnotations;
using static MS.Entities.Core.TruckEnums;

namespace MS.Models.RequestModel
{
    public class TruckRequest
    {
        public long? Id { get; set; }


        [Required(ErrorMessage = "{0}不能为空")]
        [RegularExpression(@"^[京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领A-Z]{1}[A-Z]{1}[A-Z0-9]{4}[A-Z0-9挂学警港澳]{1}$", ErrorMessage = "{0}格式不正确")]
        public string PlateNumber { get; set; }


        [Required(ErrorMessage = "{0}不能为空")]
        public string ModelNumber { get; set; }


        [Required(ErrorMessage = "{0}不能为空")]
        public double BearWeight { get; set; }


        [Required(ErrorMessage = "{0}不能为空")]
        public string UseRegion { get; set; }


        [Required(ErrorMessage = "{0}不能为空")]
        public OriginEnum Origin { get; set; }


        [Required(ErrorMessage = "{0}不能为空")]
        public string Alias { get; set; }


        //[Required(ErrorMessage = "{0}不能为空")]
        //public bool? IsUsed { get; set; }


        [Required(ErrorMessage = "{0}不能为空")]
        public string GpsCode { get; set; }

        public string Remark { get; set; }

        public ExecuteResult CheckField(ExecuteType executeType, IUnitOfWork<MSDbContext> unitOfWork)
        {
            var result = new ExecuteResult();
            var repo = unitOfWork.GetRepository<Truck>();

            if(executeType != ExecuteType.Create && !repo.Exists(x => x.Id == Id))
            {
                return result.SetFailMessage("该车辆不存在");
            }

            switch (executeType)
            {
                case ExecuteType.Create:
                    if (repo.Exists(x => x.PlateNumber == PlateNumber))
                    {
                        result.SetFailMessage($"已存在相同的车牌号码：{PlateNumber}");
                    }
                    if (repo.Exists(x => x.Alias == Alias))
                    {
                        result.SetFailMessage($"已存在相同的别名：{Alias}");
                    }
                    if (repo.Exists(x => x.GpsCode == GpsCode))
                    {
                        result.SetFailMessage($"已存在相同的GPS编码：{GpsCode}");
                    }
                    break;
                case ExecuteType.Update:
                    if(repo.Exists(x => x.PlateNumber == PlateNumber && x.Id != Id))
                    {
                        return result.SetFailMessage($"已存在相同的车牌号码：{PlateNumber}");
                    }
                    if (repo.Exists(x => x.Alias == Alias && x.Id != Id))
                    {
                        result.SetFailMessage($"已存在相同的别名：{Alias}");
                    }
                    if (repo.Exists(x => x.GpsCode == GpsCode && x.Id != Id))
                    {
                        result.SetFailMessage($"已存在相同的GPS编码：{GpsCode}");
                    }
                    break;
                case ExecuteType.Delete:
                    if (unitOfWork.GetRepository<Route>().Exists(x => x.TruckId == Id))
                    {
                        result.SetFailMessage("当前车辆已产生过运输行程，无法删除");
                    }
                    break;
                default:
                    break;
            }
            return result;

        }

    }
}
