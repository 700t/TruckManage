using MS.Entities.Core;
using System;
using System.Collections.Generic;
using static MS.Entities.Core.RouteEnums;

namespace MS.Entities
{
    /// <summary>
    /// 行程路线
    /// </summary>
    public class Route : BaseEntity
    {
        /// <summary>
        /// 线路名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 出发时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 到达时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 行程状态：0未出发，1行驶中，2已完成
        /// </summary>
        public RunStatusEnum RunStatus { get; set; }

        /// <summary>
        /// 起始地点
        /// </summary>
        public string StartAddress { get; set; }

        /// <summary>
        /// 目标地点
        /// </summary>
        public string TargetAddress { get; set; }

        /// <summary>
        /// 车辆ID
        /// </summary>
        public long TruckId { get; set; }

        public Truck Truck { get; set; }

        /// <summary>
        /// 是否往返
        /// </summary>
        public bool IsRound { get; set; }

        public string Remark { get; set; }


        public virtual List<RouteDriver> RouteDrivers { get; set; }
    }
}
