using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MS.Entities.Core
{
    public class RouteEnums
    {
        public enum RunStatusEnum
        {
            [Description("未出发")]
            NotYet,

            [Description("行驶中")]
            Running,

            [Description("已完成")]
            Done
        }
    }
}
