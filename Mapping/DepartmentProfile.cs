using AutoMapper;
using WebApplication2.Dtos.Department;
using WebApplication2.Models.Entities;

namespace WebApplication2.Mapping
{
    public class DepartmentProfile : Profile 
    {
        public DepartmentProfile()
        {
            CreateMap<CreateDepartmentDto, Department>(); 
            CreateMap<UpdateDepartmentDto, Department>()
                    .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcMember) => srcMember != null)); 
            CreateMap<Department, DepartmentDetalisDto>()
                .ForMember(dest => dest.ManagerName,
                opt => opt.MapFrom(src => src.Manager != null ? src.Manager.Name : null));
            CreateMap<Department, AllDepartmentsDto>();
        }
    }
}
