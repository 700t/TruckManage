using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Models.ViewModel
{
    public class TruckOptionsVM
    {
        public long Id { get; set; }

        public string PlateNumber { get; set; }

        public double BearWeight { get; set; }

        //是否停用
        public bool Disabled { get; set; }
    }
}
