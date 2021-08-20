using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MS.Entities.Core
{
    public class DriverEnums
    {
        /// <summary>
        /// 驾照类型
        /// </summary>
        public enum DrivingLicenseEnum
        {    
            [Description("无")]
            None,

            //大型客车
            [Description("A1")]
            A1,

            //牵引车
            [Description("A2")]
            A2,

            //城市公交车
            [Description("A3")]
            A3,

            //中型客车
            [Description("B1")]
            B1,

            //大型货车
            [Description("B2")]
            B2,

            //小型汽车
            [Description("C1")]
            C1,

            //小型自动挡汽车
            [Description("C2")]
            C2,

            //低速载货汽车
            [Description("C3")]
            C3,
        }


    }
}
