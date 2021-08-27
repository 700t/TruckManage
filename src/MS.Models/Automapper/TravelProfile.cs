using AutoMapper;
using MS.Entities;
using MS.Models.RequestModel;
using MS.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Models.Automapper
{
    public class TravelProfile : Profile
    {
        public TravelProfile()
        {
            CreateMap<Travel, TravelPageVM>()
                .ForMember(t => t.PlateNumber, opt => opt.MapFrom(src => src.Truck.PlateNumber))
                .ForMember(t => t.DriverIds, opt => opt.MapFrom(src => GetIds(src.TravelDrivers)))
                .ForMember(t => t.DriverNames, opt => opt.MapFrom(src => GetDriverNames(src.TravelDrivers)));

            CreateMap<TravelRequest, Travel>();

        }

        public IList<long> GetIds(List<TravelDriver> travelDrivers)
        {
            long[] ids = new long[travelDrivers.Count];
            for (int i = 0; i < travelDrivers.Count; i++)
            {
                ids[i] = travelDrivers[i].DriverId;
            }
            return ids;
        }

        public string GetDriverNames(List<TravelDriver> travelDrivers)
        {
            string[] names = new string[travelDrivers.Count];
            for (int i = 0; i < travelDrivers.Count; i++)
            {
                names[i] = travelDrivers[i].Driver.Name;
            }
            return string.Join(",", names);
        }
    }
}
