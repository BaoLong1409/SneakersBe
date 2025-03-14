using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ViewModel.Order;
using MediatR;
using Sneakers.Features.Queries.Order;

namespace Sneakers.Services.OrderService
{
    public class OrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator; 
        public OrderDetailService(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<IEnumerable<OrderDetailDto>> GetAllOrderDetails (Guid orderId)
        {
            var orderDetails = await _mediator.Send(new GetOrderDetails(orderId));

            if (orderDetails == null)
            {
                return Enumerable.Empty<OrderDetailDto>();
            }

            return orderDetails;
        }
    }
}
