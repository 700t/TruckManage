using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MS.Entities.Core
{
    public enum GenderCode
    {
        [Description("未知")]
        None,

        [Description("男")]
        Male,

        [Description("女")]
        Female,
    }
}
