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
