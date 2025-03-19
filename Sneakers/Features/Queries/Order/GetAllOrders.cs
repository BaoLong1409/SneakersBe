using Domain.ViewModel.Order;
using MediatR;

namespace Sneakers.Features.Queries.Order
{
    public class GetAllOrders : IRequest<IEnumerable<AllOrdersDto>>
    {
        public Guid UserId { get; set; }
        public GetAllOrders(Guid userId)
        {
            UserId = userId;
        }
    }
}
