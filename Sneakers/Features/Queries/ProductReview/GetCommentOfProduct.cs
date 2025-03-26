using Domain.ViewModel.ProductReview;
using MediatR;

namespace Sneakers.Features.Queries.ProductReview
{
    public class GetCommentOfProduct : IRequest<IEnumerable<ProductReviewDto>>
    {
        public Guid ProductId { get; set; }
        public string ColorName { get; set; }
        public GetCommentOfProduct(Guid productId, string colorName)
        {
            ProductId = productId;
            ColorName = colorName;
        }
    }
}
