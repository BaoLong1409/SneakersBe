using Domain.ViewModel.Order;
using MediatR;

namespace Sneakers.Features.Queries.Order
{
    public class GetOrderInfo : IRequest<OrderInfoDto>
    {
        public Guid OrderId { get; set; }
        public GetOrderInfo(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
