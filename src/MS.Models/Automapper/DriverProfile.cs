using AutoMapper;
using MS.Entities;
using MS.Models.RequestModel;
using MS.Models.ViewModel;
using MS.UnitOfWork.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Models.Automapper
{
    public class DriverProfile : Profile
    {
        public DriverProfile()
        {
            CreateMap(typeof(IPagedList<>), typeof(PagedList<>));
            CreateMap<Driver, DriverPageVM>()
                .ForMember(t => t.DrivingAge, opt => opt.MapFrom(src => DateTime.Now.Year - src.IssueDate.Year))
                .ForMember(t => t.Age, opt => opt.MapFrom(src => CalculateAge(src.IdNumber)));

            CreateMap<DriverRequest, Driver>();
            CreateMap<Driver, DriverOptionsVM>()
                .ForMember(t => t.Disabled, opt => opt.MapFrom(src => src.Status != Entities.Core.StatusCode.Enable));
        }

        private int CalculateAge(string idNumber)
        {
            int age = 0;
            string yearStr = idNumber.Substring(6, 4);
            if(int.TryParse(yearStr, out int year))
            {
                age = DateTime.Now.Year - year;
            }
            return age;
        }
    }
}
