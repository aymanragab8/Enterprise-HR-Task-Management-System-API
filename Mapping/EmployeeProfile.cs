using AutoMapper;
using WebApplication2.Dtos.Employee;
using WebApplication2.Models.Entities;
namespace WebApplication2.Mapping
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<CreateEmployeeDto, Employee>().ReverseMap();
            CreateMap<ResponseEmployeeDto, Employee>().ReverseMap();
        }

    }
}
