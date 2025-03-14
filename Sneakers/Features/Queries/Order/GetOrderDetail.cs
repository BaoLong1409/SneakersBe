using Domain.ViewModel.Order;
using MediatR;

namespace Sneakers.Features.Queries.Order
{
    public class GetOrderDetails : IRequest<IEnumerable<OrderDetailDto>>
    {
        public Guid OrderId { get; set; }
        public GetOrderDetails(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
