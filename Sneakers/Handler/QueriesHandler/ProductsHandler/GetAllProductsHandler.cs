using Dapper;
using DataAccess.DbContext;
using Domain.ViewModel;
using MediatR;
using Sneakers.Features.Queries.FeatureProducts;

namespace Sneakers.Handler.QueriesHandler.FeatureProductsHandler
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProducts, IEnumerable<ShowProductsDto>>
    {
        private SneakersDapperContext _context;
        public GetAllProductsHandler(SneakersDapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ShowProductsDto>> Handle(GetAllProducts request, CancellationToken cancellationToken)
        {
            var query = "SELECT p.*, i.Id AS ImageId, i.ImageUrl AS ThumbnailImage " +
                "FROM Product p " +
                "INNER JOIN ProductImage i ON p.Id = i.ProductId " +
                "WHERE i.IsThumbnail = 1";
            using (var connection = _context.CreateConnection())
            {
                var allProducts = await connection.QueryAsync<ShowProductsDto>(query);
                return allProducts.ToList();
            }
        }
    }
}
