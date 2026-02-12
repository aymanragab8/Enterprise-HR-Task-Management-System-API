using AutoMapper;
using WebApplication2.Dtos.Department;
using WebApplication2.Models.Entities;

namespace WebApplication2.Mapping
{
    public class DepartmentProfile : Profile 
    {
        public DepartmentProfile()
        {
            CreateMap<CreateDepartmentDto, Department>().ReverseMap(); 
            CreateMap<ResponseDepartmentDto, Department>().ReverseMap();
        }
    }
}
