using AutoMapper;
using Domain.Entities;
using Domain.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<IdentityRole<Guid>, Role>();
            CreateMap<User, UserDto>();
        }
    }
}
