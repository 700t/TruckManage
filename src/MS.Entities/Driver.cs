using MS.Entities.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static MS.Entities.Core.DriverEnums;

namespace MS.Entities
{
    /// <summary>
    /// 驾驶员
    /// </summary>
    public class Driver : BaseEntity
    {
        public string Name { get; set; }

        public GenderCode Gender { get; set; }

        public string Phone { get; set; }

        public string Photo { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdNumber { get; set; }

        /// <summary>
        /// 驾照
        /// </summary>
        public DrivingLicenseEnum License { get; set; }

        /// <summary>
        /// 颁发日期
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// 驾驶证照片
        /// </summary>
        public string LicensePhoto { get; set; }

        public string Remark { get; set; }

        public virtual List<RouteDriver> RouteDrivers { get; set; }
    }
}
