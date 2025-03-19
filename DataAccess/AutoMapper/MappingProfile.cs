using AutoMapper;
using Domain.Entities;
using Domain.ViewModel.Order;
using Domain.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegistration, User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
            CreateMap<OrderStatusHistoryDto, OrderStatusHistory>()
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.StatusNote));
        }
    }
}
