using Domain.ViewModel.Order;
using MediatR;

namespace Sneakers.Features.Queries.Order
{
    public record GetOrders (Guid? UserId, int Offset, int Limit, string? Status) : IRequest<IEnumerable<OrdersDto>>
    {

    }
}
