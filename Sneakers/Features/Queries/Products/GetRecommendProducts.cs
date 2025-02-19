using Domain.ViewModel;
using MediatR;

namespace Sneakers.Features.Queries.Products
{
    public class GetRecommendProducts : IRequest<IEnumerable<ShowProductsDto>>
    {
        public String UserId { get; set; }
        public GetRecommendProducts(String userId)
        {
            UserId = userId;
        }
    }
}
