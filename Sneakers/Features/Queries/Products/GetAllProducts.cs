using Domain.ViewModel;
using MediatR;

namespace Sneakers.Features.Queries.FeatureProducts
{
    public class GetAllProducts : IRequest<IEnumerable<AllProductsDto>>
    {
    }
}
