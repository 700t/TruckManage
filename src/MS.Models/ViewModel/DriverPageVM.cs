using MS.Entities;
using MS.Entities.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Models.ViewModel
{
    public class DriverPageVM : Driver, IEntity
    {
        public int Age { get; set; }

        public int DrivingAge { get; set; }

    }
}
