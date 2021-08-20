using MS.Entities;
using MS.Entities.Core;
using System;
using System.Collections.Generic;
using System.Text;
using static MS.Entities.Core.RouteEnums;

namespace MS.Models.ViewModel
{
    public class RoutePageVM : BaseEntity
    {
        public string Name { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 行程状态：0未出发，1行驶中，2已完成
        /// </summary>
        public RunStatusEnum RunStatus { get; set; }

        public string StartAddress { get; set; }

        public string TargetAddress { get; set; }

        /// <summary>
        /// 车辆ID
        /// </summary>
        public long TruckId { get; set; }

        public bool IsRound { get; set; }

        public string Remark { get; set; }

        public string PlateNumber { get; set; }

        public string DriverNames { get; set; }
    }
}
