using AutoMapper;
using WebApplication2.Dtos.Employee;
using WebApplication2.Models.Entities;
namespace WebApplication2.Mapping
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<UpdateEmployeeDto, Employee>()
                    .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Employee, EmployeeDetailsDto>();
            CreateMap<Employee, AllEmployeesDto>()
                            .ForMember(dest => dest.DepartmentName,
                       opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : ""));

        }

    }
}
