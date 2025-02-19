using Domain.ViewModel;
using MediatR;

namespace Sneakers.Features.Queries.FeatureProducts
{
    public class GetAllFeatureProducts : IRequest<List<FeatureProductModel>>
    {
    }
}
