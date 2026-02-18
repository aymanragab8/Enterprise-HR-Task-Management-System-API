using AutoMapper;
using WebApplication2.Dtos.Department;
using WebApplication2.Dtos.LeaveRequest;
using WebApplication2.Models.Entities;

namespace WebApplication2.Mapping
{
    public class LeaveRequestProfile : Profile
    {
        public LeaveRequestProfile()
        {
            CreateMap<CreateLeaveRequestDto, LeaveRequest>()
                     .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcMember) => srcMember != null)); 
            CreateMap<UpdateLeaveRequestDto, LeaveRequest>()
                    .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<LeaveRequest, LeaveRequestDetalisDto>();
            CreateMap<LeaveRequest, AllLeaveRequestsDto>();

        }
    }
}
