using AutoMapper;
using MS.Entities;
using MS.Models.RequestModel;
using MS.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Models.Automapper
{
    public class RouteProfile : Profile
    {
        public RouteProfile()
        {
            CreateMap<Route, RoutePageVM>()
                .ForMember(t => t.PlateNumber, opt => opt.MapFrom(src => src.Truck.PlateNumber))
                .ForMember(t => t.DriverNames, opt => opt.MapFrom(src => GetDriverNames(src.RouteDrivers)));

            CreateMap<RouteRequest, Route>();

        }

        public string GetDriverNames(List<RouteDriver> routeDrivers)
        {
            string[] names = new string[routeDrivers.Count];
            for (int i = 0; i < routeDrivers.Count; i++)
            {
                names[i] = routeDrivers[i].Driver.Name;
            }
            return string.Join(",", names);
        }
    }
}
