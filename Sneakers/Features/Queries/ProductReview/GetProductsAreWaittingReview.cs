using Domain.ViewModel.ProductReview;
using MediatR;

namespace Sneakers.Features.Queries.ProductReview
{
    public class GetProductsAreWaittingReview : IRequest<IEnumerable<ProductsAreWaitingReviewDto>>
    {
        public Guid UserId { get; set; }
        public GetProductsAreWaittingReview(Guid userId)
        {
            UserId = userId;
        }
    }
}
