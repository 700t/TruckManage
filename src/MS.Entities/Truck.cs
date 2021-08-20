using MS.Entities.Core;
using System;
using System.Collections.Generic;
using System.Text;
using static MS.Entities.Core.TruckEnums;

namespace MS.Entities
{
    /// <summary>
    /// 卡车
    /// </summary>
    public class Truck : BaseEntity
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        public string PlateNumber { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        public string ModelNumber { get; set; }

        /// <summary>
        /// 载重(吨)
        /// </summary>
        public double BearWeight { get; set; }

        /// <summary>
        /// 用车区域
        /// </summary>
        public string UseRegion { get; set; }

        /// <summary>
        /// 性质&来源
        /// </summary>
        public OriginEnum Origin { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 是否占用
        /// </summary>
        public bool IsUsed { get; set; }

        /// <summary>
        /// GPS编码
        /// </summary>
        public string GpsCode { get; set; }

        public string Remark { get; set; }
    }
}
