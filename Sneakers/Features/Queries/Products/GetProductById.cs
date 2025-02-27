using Domain.ViewModel;
using MediatR;

namespace Sneakers.Features.Queries.Products
{
    public class GetProductById : IRequest<DetailProductDto>
    {
        public Guid ProductId { get; set; }
        public String ColorName { get; set; }
        public GetProductById(Guid productId, string colorName)
        {
            this.ProductId = productId;
            ColorName = colorName;
        }
    }
}
