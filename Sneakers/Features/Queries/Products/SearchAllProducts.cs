using Domain.ViewModel.Product;
using MediatR;

namespace Sneakers.Features.Queries.Products
{
    public class SearchAllProducts : IRequest<IEnumerable<AllProductsDto>>
    {
        public string Keyword { get; set; } = String.Empty;
        public SearchAllProducts(string keyword)
        {
            Keyword = keyword;
        }
    }
}
