using AutoMapper;
using MS.DbContexts;
using MS.Entities;
using MS.Entities.Core;
using MS.UnitOfWork;
using MS.WebCore.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static MS.Entities.Core.DriverEnums;

namespace MS.Models.RequestModel
{
    public class DriverRequest
    {
        public long? Id { get; set; }

        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(10, ErrorMessage = "{0}不能超过{1}个字符")]
        public string Name { get; set; }

        public GenderCode Gender { get; set; }

        [Required(ErrorMessage = "{0}不能为空")]
        [RegularExpression(@"^1[3|5|7|8|9]\d{9}$", ErrorMessage = "{0}格式不正确")]
        public string Phone { get; set; }

        public string Photo { get; set; }

        [Required(ErrorMessage = "{0}不能为空")]
        [RegularExpression(@"^[1-8][1-7]\d{4}(?:19|20)\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])\d{3}[\dX]$", ErrorMessage = "{0}格式不正确")]
        public string IdNumber { get; set; }


        [Required(ErrorMessage = "{0}不能为空")]
        public DrivingLicenseEnum License { get; set; }


        [Required(ErrorMessage = "{0}不能为空")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// 驾驶证照片
        /// </summary>
        public string LicensePhoto { get; set; }

        public string Remark { get; set; }

        public StatusCode? Status { get; set; }

        public ExecuteResult CheckField(ExecuteType executeType, IUnitOfWork<MSDbContext> unitOfWork)
        {
            ExecuteResult result = new ExecuteResult();
            var repo = unitOfWork.GetRepository<Driver>();

            //如果不是新增角色，操作之前都要先检查角色是否存在
            if (executeType != ExecuteType.Create && !repo.Exists(a => a.Id == Id))
            {
                return result.SetFailMessage("该驾驶员不存在");
            }

            //针对不同的操作，检查逻辑不同
            switch (executeType)
            {
                case ExecuteType.Delete:                    
                    break;
                case ExecuteType.Update:
                    break;
                case ExecuteType.Create:
                    //如果存在相同的司机姓名+身份证号，则返回报错
                    if (repo.Exists(a => a.Name == Name && a.IdNumber == IdNumber))
                    {
                        return result.SetFailMessage($"添加司机姓名：{Name} 已存在");
                    }
                    break;
                default:                    
                    break;
            }
            return result;//没有错误，默认返回成功
        }


    }
}
