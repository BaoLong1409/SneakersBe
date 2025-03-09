using Domain.ViewModel.Product;
using MediatR;

namespace Sneakers.Features.Queries.FeatureProducts
{
    public class GetAllFeatureProducts : IRequest<List<FeatureProductModel>>
    {
    }
}
