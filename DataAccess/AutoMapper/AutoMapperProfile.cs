using AutoMapper;
using Domain.Entities;
using Domain.ViewModel.Cart;
using Domain.ViewModel.Order;
using Domain.ViewModel.User;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<IdentityRole<Guid>, Role>();
            CreateMap<User, UserDto>();
            CreateMap<ManageProductInCartDto, ProductCart>();
            CreateMap<OrderAddDto, Order>();
            CreateMap<OrderDetailDto, OrderDetail>();
            CreateMap<OrderStatusHistoryDto, OrderStatusHistory>();

        }
    }
}
