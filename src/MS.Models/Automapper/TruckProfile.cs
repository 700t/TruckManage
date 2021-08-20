using AutoMapper;
using MS.Entities;
using MS.Models.RequestModel;
using MS.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Models.Automapper
{
    public class TruckProfile : Profile
    {
        public TruckProfile()
        {
            CreateMap<TruckRequest, Truck>();
            CreateMap<Truck, TruckPageVM>();
        }
    }
}
