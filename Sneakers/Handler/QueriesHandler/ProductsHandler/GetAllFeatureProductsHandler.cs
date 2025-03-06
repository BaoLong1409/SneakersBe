using Dapper;
using DataAccess.DbContext;
using Domain.ViewModel;
using MediatR;
using Sneakers.Features.Queries.FeatureProducts;

namespace Sneakers.Handler.QueriesHandler.FeatureProductsHandler
{
    public class GetAllFeatureProductsHandler : IRequestHandler<GetAllFeatureProducts, List<FeatureProductModel>>
    {
        private readonly SneakersDapperContext _context;
        public GetAllFeatureProductsHandler(SneakersDapperContext context)
        {
            _context = context;
        }
        public async Task<List<FeatureProductModel>> Handle(GetAllFeatureProducts request, CancellationToken cancellationToken)
        {
            var query = "SELECT fp.ImageUrl, p.Name, p.Price, p.Sale, fp.LeftColor, fp.MiddleColor, fp.RightColor " +
                "FROM FeatureProducts fp " +
                "INNER JOIN Product p ON fp.ProductId = p.Id ";
            using (var connection = _context.CreateConnection()) {
                var result = await connection.QueryAsync<FeatureProductModel>(query);
                return result.ToList();
            }
        }
    }
}
