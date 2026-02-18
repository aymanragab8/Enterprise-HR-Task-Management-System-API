using AutoMapper;
using WebApplication2.Dtos.Auth;
using WebApplication2.Models.Entities;

namespace WebApplication2.Mapping
{
    public class AuthProfile :Profile
    {
        public AuthProfile()
        {
            CreateMap<RegisterDto,ApplicationUser>();
            CreateMap<RegisterDto,Employee>();
        }
    }
}
