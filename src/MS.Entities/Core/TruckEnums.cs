using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MS.Entities.Core
{
    public class TruckEnums
    {
        public enum OriginEnum
        {
            [Description("自购")]
            Buy,

            [Description("租凭")]
            Lease,

            [Description("借入")]
            Borrow
        }

        public enum UseStatusEnum
        {
            [Description("空闲")]
            Free,

            [Description("停止")]
            Using,

            //[Description("行驶中")]
            //Moving,
        }
    }
}
