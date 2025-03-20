using Domain.ViewModel.Product;
using MediatR;

namespace Sneakers.Features.Queries.Products
{
    public class GetAllProductsWithCondition : IRequest<IEnumerable<AllProductsDto>>
    {
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public GetAllProductsWithCondition(int[] priceFilter)
        {
            MinValue = priceFilter[0];
            MaxValue = priceFilter[1];
        }
    }
}
