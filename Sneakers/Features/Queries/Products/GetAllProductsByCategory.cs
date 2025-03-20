using Domain.ViewModel.Product;
using MediatR;

namespace Sneakers.Features.Queries.Products
{
    public class GetAllProductsByCategory : IRequest<IEnumerable<AllProductsDto>>
    {
        public string? CategoryName { get; set; }
        public string? BrandName { get; set; }
        public GetAllProductsByCategory(string? categoryName, string? brandName)
        {
            CategoryName = categoryName;
            BrandName = brandName;
        }
    }
}
