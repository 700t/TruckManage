using MS.Entities.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Entities
{
    /// <summary>
    /// 线路-驾驶员 关系表
    /// </summary>
    public class RouteDriver : IEntity
    {
        //public long Id { get; set; }

        public long RouteId { get; set; }

        public Route Route { get; set; }

        public long DriverId { get; set; }

        public Driver Driver { get; set; }
    }
}
