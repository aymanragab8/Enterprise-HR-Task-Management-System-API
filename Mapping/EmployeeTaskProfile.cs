using AutoMapper;
using WebApplication2.Dtos.LeaveRequest;
using WebApplication2.Dtos.Task;
using WebApplication2.Models.Entities;

namespace WebApplication2.Mapping
{
    public class EmployeeTaskProfile:Profile
    {
        public EmployeeTaskProfile()
        {
            CreateMap<AssignTaskDto, EmployeeTask>();
            CreateMap<UpdateTaskDetalisDto, EmployeeTask>()
                    .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<EmployeeTask, TaskDetalisDto>();
            CreateMap<EmployeeTask, AllLeaveRequestsDto>();
        }
    }
}
